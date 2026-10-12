using System.Reactive.Disposables;
using System.Reactive.Linq;
using IqOptionApiDotNet;
using IqOptionApiDotNet.Models;
using IqOptionApiDotNet.Models.DigitalOptions;

namespace DigestingDuck.Core;

public interface ITradingBroker
{
    bool IsConnected { get; }
    event Action<LiveDeal>? Deal;
    event Action<PositionChanged>? Position;
    event Action<OptionClosed>? OptionClosed;
    Task SelectBalanceAsync(BalanceType balance);
    Task WatchAsync(IEnumerable<int> assetIds);
    void Unwatch();
    Task<DigitalOptionsPlacedResult> PlaceAsync(BalanceType balance, ActivePair pair, OrderDirection direction, DigitalOptionsExpiryDuration duration, decimal amount);
}

public sealed class BrokerSession : IDisposable, ITradingBroker
{
    private readonly SemaphoreSlim requests = new(1);
    private IqOptionApiDotNetClient? client;
    private CompositeDisposable subscriptions = new();
    private readonly HashSet<int> watched = [];
    public bool IsConnected => client?.IsConnected == true;
    public IReadOnlyList<Balance> Balances { get; private set; } = [];
    public Profile.ProfileResult? Profile { get; private set; }
    public event Action<string>? Message;
    public event Action<LiveDeal>? Deal;
    public event Action<PositionChanged>? Position;
    public event Action<OptionClosed>? OptionClosed;
    public event Action<Balance>? BalanceChanged;
    private static string RequestId() => Guid.NewGuid().ToString("N");

    public async Task ConnectAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Informe e-mail e senha.");
        await requests.WaitAsync();
        try
        {
            Disconnect();
            client = new(email, password);
            if (!await Task.Run(() => client.ConnectAsync()))
                throw new InvalidOperationException("A corretora recusou o login.");
            Profile = client.WsClient.Profile;
            Balances = client.WsClient.balances?.ToArray() ?? [];
            subscriptions.Add(client.WsClient.LiveDealObservable().Subscribe(x => Deal?.Invoke(x)));
            subscriptions.Add(client.WsClient.PositionChangedObservable().Subscribe(x => Position?.Invoke(x)));
            subscriptions.Add(client.WsClient.ClosedOptionObservable().Subscribe(x => OptionClosed?.Invoke(x)));
            subscriptions.Add(client.WsClient.BalanceChangedObservable.Subscribe(x =>
            {
                var updated = x.CurrentBalance;
                if (updated == null) return;
                Balances = Balances.Select(b => b.Id == updated.Id ? updated : b).ToArray();
                BalanceChanged?.Invoke(updated);
            }));
            client.SubscribeBalanceChanged(RequestId());
            Message?.Invoke("Conectado à corretora.");
        }
        catch { Disconnect(); throw; }
        finally { requests.Release(); }
    }
    private async Task<T> Query<T>(Func<IqOptionApiDotNetClient, Task<T>> call)
    {
        await requests.WaitAsync();
        try
        {
            if (!IsConnected) throw new InvalidOperationException("Conecte a corretora primeiro.");
            return await call(client!);
        }
        finally { requests.Release(); }
    }
    public async Task RefreshBalancesAsync()
    {
        Balances = (await Query(x => x.GetBalancesAsync(RequestId(), new[] { BalanceType.Practice, BalanceType.Real }))).ToArray();
    }
    public Task<LeaderBoardDealsClientResult> GetTradersAsync() => Query(x => x.RequestLeaderboardDealsClientAsync(RequestId(), CountryType.BR, 1, 50));
    public Task<Alerts> GetAlertsAsync() => Query(x => x.GetAlerts(RequestId()));
    public Task<Alert> CreateAlertAsync(ActivePair pair, double price) => Query(x => x.CreateAlert(RequestId(), pair, InstrumentType.DigitalOption, price, 0));
    public Task<Alert> DeleteAlertAsync(long id) => Query(x => x.DeleteAlert(RequestId(), id));
    public Task<CandleCollections> GetCandlesAsync(ActivePair pair) => Query(x => x.GetCandlesAsync(RequestId(), pair, TimeFrame.Min1, 60, DateTimeOffset.UtcNow));
    public async Task WatchAsync(IEnumerable<int> assetIds)
    {
        await requests.WaitAsync();
        try
        {
            if (!IsConnected) throw new InvalidOperationException("Conecte a corretora primeiro.");
            Unwatch();
            foreach (var id in assetIds.Distinct())
            {
                foreach (var period in new[] { DigitalOptionsExpiryType.PT1M, DigitalOptionsExpiryType.PT5M, DigitalOptionsExpiryType.PT15M })
                    client!.SubscribeLiveDeal(RequestId(), "live-deal-digital-option", (ActivePair)id, period);
                watched.Add(id);
            }
            Message?.Invoke($"Monitorando {watched.Count} ativos.");
        }
        finally { requests.Release(); }
    }
    public void Unwatch()
    {
        if (IsConnected)
            foreach (var id in watched)
                foreach (var period in new[] { DigitalOptionsExpiryType.PT1M, DigitalOptionsExpiryType.PT5M, DigitalOptionsExpiryType.PT15M })
                    client!.UnSubscribeLiveDeal(RequestId(), "live-deal-digital-option", (ActivePair)id, period);
        watched.Clear();
    }
    public async Task SelectBalanceAsync(BalanceType balance)
    {
        await requests.WaitAsync();
        try
        {
            if (!IsConnected || Profile == null) throw new InvalidOperationException("Conecte a corretora primeiro.");
            var selected = Balances.SingleOrDefault(x => x.Type == balance)
                ?? throw new InvalidOperationException("Saldo selecionado não disponível nesta conta.");
            await client!.WsClient.UnSubscribePositionChanged(RequestId(), InstrumentType.DigitalOption);
            Profile.BalanceId = selected.Id;
            Profile.BalanceType = selected.Type;
            Profile.Balance = selected.Amount;
            await client.WsClient.SubscribePositionChanged(RequestId(), InstrumentType.DigitalOption);
        }
        finally { requests.Release(); }
    }
    public Task<DigitalOptionsPlacedResult> PlaceAsync(BalanceType balance, ActivePair pair, OrderDirection direction, DigitalOptionsExpiryDuration duration, decimal amount) =>
        Query(x =>
        {
            var selected = Balances.SingleOrDefault(b => b.Type == balance)
                ?? throw new InvalidOperationException("Saldo selecionado não disponível.");
            if (x.WsClient.Profile.BalanceId != selected.Id)
                throw new InvalidOperationException("A conta selecionada mudou. Pare e inicie novamente.");
            return x.PlaceDigitalOptions(RequestId(), pair, direction, duration, (double)amount);
        });
    public void Disconnect()
    {
        subscriptions.Dispose();
        subscriptions = new();
        watched.Clear();
        client?.Dispose();
        client = null;
        Profile = null;
        Balances = [];
    }
    public void Dispose() => Disconnect();
}
