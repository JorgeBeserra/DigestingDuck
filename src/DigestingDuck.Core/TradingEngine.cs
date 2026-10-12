using System.Collections.Concurrent;
using IqOptionApiDotNet.Models;
using IqOptionApiDotNet.Models.DigitalOptions;

namespace DigestingDuck.Core;

public sealed record TradeRecord(long Id, long? SourceId, DateTimeOffset At, int AssetId, string Direction, decimal Amount, string Status, decimal? Profit, BalanceType Balance, Guid Session, string RequestKey)
{
    public override string ToString() => $"{At.LocalDateTime:dd/MM HH:mm:ss} | #{Id} | ativo {AssetId} | {Direction} | {Amount:N2} | {Status} | {Profit:N2}";
}
public static class EntryPolicy
{
    public static bool Accept(LiveDeal deal, AppSettings settings, DateTimeOffset now)
    {
        if (!deal.CreatedAt.HasValue || !settings.TargetTraderIds.Contains(deal.UserId)) return false;
        var age = (now - deal.CreatedAt.Value).TotalSeconds;
        return age >= 0 && age <= settings.MaxSignalDelaySeconds &&
            deal.AmountEnrolled >= (double)settings.MinimumTraderAmount && settings.WatchedAssetIds.Contains((int)deal.ActiveId);
    }
}
public sealed class TradingEngine : IAsyncDisposable
{
    private readonly ITradingBroker broker;
    private readonly SemaphoreSlim orders = new(1);
    private readonly ConcurrentDictionary<long, TradeRecord> history = new();
    private readonly HashSet<string> submitted = [];
    private readonly object sync = new();
    private CancellationTokenSource? stop;
    private Task? loop;
    private AppSettings settings = new();
    private BalanceType balance = BalanceType.Practice;
    private Guid sessionId = Guid.NewGuid();
    public bool IsRunning => stop != null;
    public bool CopyEnabled { get; private set; }
    public decimal Profit => history.Values.Where(x => x.Session == sessionId).Sum(x => x.Profit ?? 0);
    public IReadOnlyList<TradeRecord> History => history.Values.OrderByDescending(x => x.At).ToArray();
    public List<Signal> Signals { get; } = [];
    public event Action<string>? Message;
    public event Action? Changed;
    public TradingEngine(ITradingBroker broker)
    {
        this.broker = broker;
        broker.Deal += OnDeal;
        broker.Position += OnPosition;
        broker.OptionClosed += OnClosed;
    }
    public async Task StartAsync(AppSettings current, BalanceType selectedBalance, bool enableOrders)
    {
        if (IsRunning) throw new InvalidOperationException("O monitoramento já está iniciado.");
        current.Validate();
        if (!broker.IsConnected) throw new InvalidOperationException("Conecte a corretora primeiro.");
        if (history.Values.Any(x => x.Status == "Aberta"))
            throw new InvalidOperationException("Há ordens sem resultado confirmado. Confira as posições na corretora antes de iniciar outra sessão.");
        sessionId = Guid.NewGuid();
        settings = current;
        balance = selectedBalance;
        CopyEnabled = enableOrders;
        await broker.SelectBalanceAsync(selectedBalance);
        await broker.WatchAsync(settings.WatchedAssetIds);
        stop = new();
        loop = ScheduleAsync(stop.Token);
        Message?.Invoke(enableOrders ? "Execução de ordens habilitada." : "Monitoramento iniciado, sem enviar ordens.");
    }
    public async Task StopAsync()
    {
        var cancellation = stop;
        stop = null;
        CopyEnabled = false;
        cancellation?.Cancel();
        if (loop != null) { try { await loop; } catch (OperationCanceledException) { } }
        await orders.WaitAsync();
        try { broker.Unwatch(); } finally { orders.Release(); }
        cancellation?.Dispose();
        Message?.Invoke("Monitoramento encerrado.");
    }
    private void OnDeal(LiveDeal deal)
    {
        if (!IsRunning || !CopyEnabled || !EntryPolicy.Accept(deal, settings, DateTimeOffset.UtcNow)) return;
        var duration = deal.ExpirationType switch
        {
            DigitalOptionsExpiryType.PT1M => DigitalOptionsExpiryDuration.M1,
            DigitalOptionsExpiryType.PT5M => DigitalOptionsExpiryDuration.M5,
            DigitalOptionsExpiryType.PT15M => DigitalOptionsExpiryDuration.M15,
            _ => (DigitalOptionsExpiryDuration)0
        };
        if ((int)duration == 0) return;
        _ = ExecuteAsync($"deal:{deal.PositionId}", deal.PositionId, (int)deal.ActiveId, deal.InstrumentDir, duration);
    }
    private async Task ScheduleAsync(CancellationToken cancellation)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));
        while (await timer.WaitForNextTickAsync(cancellation))
        {
            Signal[] ready;
            lock (sync) ready = Signals.Where(x => x.At <= DateTimeOffset.UtcNow &&
                (DateTimeOffset.UtcNow - x.At).TotalSeconds <= settings.MaxSignalDelaySeconds).ToArray();
            if (!CopyEnabled) continue;
            foreach (var signal in ready)
                await ExecuteAsync($"signal:{signal.Id}", null, signal.AssetId, signal.Direction, signal.Duration);
        }
    }
    public void Restore(Journal journal)
    {
        if (IsRunning) throw new InvalidOperationException("Pare a sessão antes de restaurar dados.");
        Signals.Clear();
        Signals.AddRange(journal.Signals);
        foreach (var trade in journal.Trades) { history[trade.Id] = trade; submitted.Add(trade.RequestKey); }
    }
    public Journal Snapshot() { lock (sync) return new(Signals.ToArray(), History); }
    public void ImportSignals(IEnumerable<Signal> signals)
    {
        if (IsRunning) throw new InvalidOperationException("Pare o monitoramento antes de importar sinais.");
        lock (sync) Signals.AddRange(signals);
        Changed?.Invoke();
    }
    private async Task ExecuteAsync(string key, long? sourceId, int asset, OrderDirection direction, DigitalOptionsExpiryDuration duration)
    {
        await orders.WaitAsync();
        try
        {
            if (!IsRunning || !CopyEnabled || !submitted.Add(key)) return;
            var pendingAmount = history.Values.Where(x => x.Status == "Aberta").Sum(x => x.Amount);
            if (Profit >= settings.StopWin || Profit - pendingAmount - settings.EntryAmount < -settings.StopLoss)
            {
                CopyEnabled = false;
                Message?.Invoke("Limite de gerenciamento atingido; novas ordens desabilitadas.");
                return;
            }
            var result = await broker.PlaceAsync(balance, (ActivePair)asset, direction, duration, settings.EntryAmount);
            if (result.IsError()) { Message?.Invoke($"Ordem recusada: {result.ErrorMessage}"); return; }
            history[result.Id!.Value] = new(result.Id.Value, sourceId, DateTimeOffset.UtcNow, asset, direction.ToString(), settings.EntryAmount, "Aberta", null, balance, sessionId, key);
            Changed?.Invoke();
        }
        catch (Exception ex) { Message?.Invoke($"Erro ao enviar ordem: {ex.Message}"); }
        finally { orders.Release(); }
    }
    public void Reconcile(long id, decimal netProfit)
    {
        if (!history.TryGetValue(id, out var trade)) throw new ArgumentException("Ordem não encontrada no histórico local.");
        if (trade.Profit.HasValue) throw new InvalidOperationException("Esta ordem já tem resultado confirmado.");
        Close(id, netProfit, netProfit > 0 ? "WIN (manual)" : netProfit < 0 ? "LOSS (manual)" : "EMPATE (manual)");
    }
    private void OnPosition(PositionChanged position)
    {
        if (!string.Equals(position.Status, "closed", StringComparison.OrdinalIgnoreCase)) return;
        foreach (var id in position.PortfolioChangedEventInfo?.OrderIds ?? [])
            Close(id, (decimal)position.Pnl, position.Pnl > 0 ? "WIN" : position.Pnl < 0 ? "LOSS" : "EMPATE");
    }
    private void OnClosed(OptionClosed option) => Close(option.OptionId, (decimal)(option.ProfitAmount - option.Amount), option.Result.ToString());
    private void Close(long id, decimal profit, string status)
    {
        if (!history.TryGetValue(id, out var item) || item.Profit.HasValue) return;
        history[id] = item with { Status = status, Profit = profit };
        Changed?.Invoke();
    }
    public async ValueTask DisposeAsync()
    {
        await StopAsync();
        broker.Deal -= OnDeal;
        broker.Position -= OnPosition;
        broker.OptionClosed -= OnClosed;
    }
}
