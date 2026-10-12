using System.Globalization;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DigestingDuck.Core;
using IqOptionApiDotNet.Models;

namespace DigestingDuck.Desktop;

public sealed class MainWindow : Window
{
    private readonly BrokerSession broker = new();
    private readonly TradingEngine engine;
    private readonly SettingsStore store = new();
    private readonly JournalStore journal;
    private bool loaded;
    private AppSettings settings = new();
    private readonly TextBlock status = new() { Text = "Desconectado", Foreground = Brushes.Orange };
    private readonly TextBlock identity = new() { Text = "Conecte sua corretora para começar.", FontSize = 22 };
    private readonly TextBlock balances = new() { Text = "Saldo indisponível", FontSize = 24 };
    private readonly TextBlock profit = new() { Text = "Resultado da sessão: 0,00", FontSize = 18 };
    private readonly TextBox email = new() { PlaceholderText = "E-mail da IQ Option" };
    private readonly TextBox password = new() { PlaceholderText = "Senha", PasswordChar = '●' };
    private readonly ComboBox environment = new() { ItemsSource = new[] { "Treinamento", "Real" }, SelectedIndex = 0 };
    private readonly CheckBox enableOrders = new() { Content = "Habilitar envio de ordens de cópia e sinais", IsChecked = false };
    private readonly TextBox amount = new() { Text = "1" };
    private readonly TextBox stopWin = new() { Text = "10" };
    private readonly TextBox stopLoss = new() { Text = "10" };
    private readonly TextBox delay = new() { Text = "5" };
    private readonly TextBox minimum = new() { Text = "1" };
    private readonly TextBox targetIds = new() { PlaceholderText = "IDs dos traders separados por vírgula" };
    private readonly TextBox assetIds = new() { Text = "1861,76" };
    private readonly ComboBox assetPicker = new() { ItemsSource = Enum.GetValues<ActivePair>().Where(x => (int)x > 0).ToArray(), SelectedItem = ActivePair.EURUSD_op };
    private readonly ListBox trades = new();
    private readonly ListBox deals = new();
    private readonly ListBox signalList = new();
    private readonly ListBox traderList = new();
    private readonly ListBox alertList = new();
    private readonly TextBox logs = new() { IsReadOnly = true, AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, MinHeight = 180 };
    private readonly TextBox candles = new() { IsReadOnly = true, AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, MinHeight = 200 };
    private readonly Queue<string> recentDeals = new();
    private readonly Queue<string> recentLogs = new();
    private bool closing;
    public MainWindow()
    {
        engine = new(broker);
        journal = new(store.DirectoryPath);
        Title = "DigestingDuck";
        Width = 1180; Height = 790; MinWidth = 800; MinHeight = 600;
        Background = new SolidColorBrush(Color.Parse("#121621"));
        var header = new Grid { ColumnDefinitions = new("*,Auto"), Margin = new(28, 20) };
        header.Children.Add(new TextBlock { Text = "DIGESTING DUCK", FontSize = 26, FontWeight = FontWeight.Bold });
        Grid.SetColumn(status, 1); header.Children.Add(status);
        var tabs = new TabControl { Margin = new(20, 0, 20, 20), ItemsSource = new[]
        {
            Tab("Painel", Dashboard()), Tab("Corretora", BrokerPage()), Tab("Ativos", AssetPage()),
            Tab("Gerenciamento", Management()), Tab("Traders", TradersPage()), Tab("Sinais", SignalsPage()),
            Tab("Portfólio", Portfolio()), Tab("Alertas", AlertsPage())
        }};
        var root = new DockPanel(); DockPanel.SetDock(header, Dock.Top); root.Children.Add(header); root.Children.Add(tabs); Content = root;
        broker.Message += Log;
        engine.Message += Log;
        engine.Changed += () => Dispatcher.UIThread.Post(async () => { UpdatePortfolio(); if (loaded) await Run(() => journal.SaveAsync(engine.Snapshot())); });
        broker.Deal += x => Dispatcher.UIThread.Post(() =>
        {
            recentDeals.Enqueue($"{x.CreatedAt?.LocalDateTime:HH:mm:ss} | {x.Name} [{x.UserId}] | {x.ActiveId} | {x.InstrumentDir} | {x.AmountEnrolled:N2}");
            while (recentDeals.Count > 200) recentDeals.Dequeue();
            deals.ItemsSource = recentDeals.Reverse().ToArray();
        });
        broker.BalanceChanged += _ => Dispatcher.UIThread.Post(UpdateBalances);
        Opened += async (_, _) => await Run(async () => { settings = await store.LoadAsync(); engine.Restore(await journal.LoadAsync()); loaded = true; ApplySettings(); UpdatePortfolio(); Log($"Dados locais: {store.DirectoryPath}"); });
        Closing += async (_, e) =>
        {
            if (closing) return;
            e.Cancel = true;
            await Run(async () => { await engine.DisposeAsync(); if (loaded) await journal.SaveAsync(engine.Snapshot()); broker.Dispose(); });
            closing = true; Close();
        };
    }
    private static TabItem Tab(string title, Control content) => new() { Header = title, Content = new ScrollViewer { Content = content, Margin = new(12) } };
    private static StackPanel Stack(params Control[] children)
    {
        var p = new StackPanel { Spacing = 14, Margin = new(8) };
        foreach (var c in children) p.Children.Add(c);
        return p;
    }
    private static Control Field(string label, Control control) => Stack(new TextBlock { Text = label, Foreground = Brushes.LightGray }, control);
    private static TextBlock Heading(string text) => new() { Text = text, FontSize = 24, FontWeight = FontWeight.SemiBold };
    private Button Button(string label, Func<Task> action)
    {
        var button = new Button { Content = label, Padding = new(18, 10), HorizontalAlignment = HorizontalAlignment.Left };
        button.Click += async (_, _) => { button.IsEnabled = false; try { await Run(action); } finally { button.IsEnabled = true; } };
        return button;
    }
    private async Task Run(Func<Task> action)
    {
        try { await action(); environment.IsEnabled = !engine.IsRunning; enableOrders.IsEnabled = !engine.IsRunning; status.Text = broker.IsConnected ? (engine.IsRunning ? "Monitorando" : "Conectado") : "Desconectado"; }
        catch (Exception ex) { Log(ex.Message); status.Text = "Verifique o registro"; }
    }
    private void Log(string text) => Dispatcher.UIThread.Post(() =>
    {
        recentLogs.Enqueue($"{DateTimeOffset.Now:HH:mm:ss}  {text}");
        while (recentLogs.Count > 250) recentLogs.Dequeue();
        logs.Text = string.Join(Environment.NewLine, recentLogs.Reverse());
    });
    private Control Dashboard() => Stack(Heading("Sua estação de acompanhamento"), identity, balances, profit,
        new TextBlock { Text = "Escolha os ativos e traders, configure os limites e acompanhe a sessão.", TextWrapping = TextWrapping.Wrap },
        Button("Atualizar saldos", async () => { await broker.RefreshBalancesAsync(); UpdateBalances(); }),
        Button("Iniciar monitoramento", Start), Button("Parar", () => engine.StopAsync()), Heading("Registro da sessão"), logs);
    private Control BrokerPage() => Stack(Heading("Conectar à IQ Option"),
        new TextBlock { Text = "A senha fica somente em memória durante a sessão.", TextWrapping = TextWrapping.Wrap },
        Field("E-mail", email), Field("Senha", password), Field("Conta para novas operações", environment), enableOrders,
        Button("Conectar", async () =>
        {
            if (engine.IsRunning) await engine.StopAsync();
            await broker.ConnectAsync(email.Text ?? "", password.Text ?? "");
            password.Text = "";
            identity.Text = broker.Profile?.Name ?? "Conectado";
            UpdateBalances();
            settings = settings with { Email = email.Text ?? "" }; await store.SaveAsync(settings);
        }), Button("Desconectar", async () => { await engine.StopAsync(); broker.Disconnect(); identity.Text = "Desconectado"; balances.Text = "Saldo indisponível"; }));
    private void UpdateBalances() => balances.Text = string.Join("  •  ", broker.Balances.Select(x => $"{(x.Type == BalanceType.Practice ? "Treinamento" : "Real")}: {x.Amount:N2} {x.Currency}"));
    private Control AssetPage() => Stack(Heading("Ativos e cotações"), Field("Ativo para consultar", assetPicker),
        Button("Consultar últimas 60 velas", async () => candles.Text = Newtonsoft.Json.JsonConvert.SerializeObject(await broker.GetCandlesAsync((ActivePair)assetPicker.SelectedItem!), Newtonsoft.Json.Formatting.Indented)), candles,
        Field("IDs dos ativos monitorados, separados por vírgula", assetIds),
        new TextBlock { Text = "O catálogo da biblioteca não garante que um ativo esteja aberto. Confira a disponibilidade na corretora.", TextWrapping = TextWrapping.Wrap },
        Button("Salvar ativos", SaveSettings));
    private Control Management() => Stack(Heading("Limites da sessão"), Field("Valor de cada entrada", amount), Field("Stop win", stopWin), Field("Stop loss", stopLoss),
        Field("Atraso máximo do sinal (segundos)", delay), Field("Valor mínimo da entrada do trader", minimum),
        new TextBlock { Text = "O stop loss considera o valor das ordens ainda abertas. As configurações são aplicadas ao iniciar uma nova sessão.", TextWrapping = TextWrapping.Wrap },
        Button("Salvar gerenciamento", SaveSettings));
    private Control TradersPage() => Stack(Heading("Traders acompanhados"),
        Button("Consultar ranking Brasil", async () =>
        {
            var result = await broker.GetTradersAsync();
            traderList.ItemsSource = result.Result?.Top?.Values.OrderByDescending(x => x.Score).Select(x => $"{x.UserName} | ID {x.UserId} | Resultado {x.Score:N2}").ToArray() ?? [];
        }), traderList, Field("IDs dos traders alvo, separados por vírgula", targetIds), Button("Salvar traders", SaveSettings), Heading("Operações recebidas"), deals);
    private Control SignalsPage() => Stack(Heading("Sinais programados"),
        new TextBlock { Text = "CSV: data ISO-8601 com fuso;ativo;CALL ou PUT;1, 5 ou 15\nExemplo: 2026-10-12T15:00:00-03:00;EURUSD_op;CALL;1", TextWrapping = TextWrapping.Wrap },
        Button("Importar arquivo CSV", async () =>
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions { Title = "Importar sinais", AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("CSV") { Patterns = new[] { "*.csv" } } } });
            if (files.Count == 0) return;
            await using var stream = await files[0].OpenReadAsync();
            using var reader = new StreamReader(stream);
            var imported = SignalCsv.Parse(await reader.ReadToEndAsync());
            engine.ImportSignals(imported); Log($"{imported.Count} sinais importados."); UpdatePortfolio();
        }), signalList);
    private Control Portfolio()
    {
        var orderId = new TextBox { PlaceholderText = "ID da ordem sem resultado" };
        var result = new TextBox { PlaceholderText = "Lucro líquido confirmado na corretora" };
        return Stack(Heading("Histórico de operações"), trades,
        new TextBlock { Text = "Se um resultado não chegar, confira a ordem na corretora e registre o lucro líquido antes de reiniciar a sessão.", TextWrapping = TextWrapping.Wrap },
        Field("ID da ordem para conciliar", orderId), Field("Resultado líquido (negativo para perda)", result),
        Button("Registrar resultado conferido", () => { engine.Reconcile(long.Parse(orderId.Text ?? ""), Number(result)); return Task.CompletedTask; }),
        Button("Exportar portfólio", async () =>
        {
            var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions { SuggestedFileName = "digestingduck-operacoes.csv", DefaultExtension = "csv" });
            if (file == null) return;
            await using var stream = await file.OpenWriteAsync(); using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync("id;data;ativo;direção;valor;status;resultado");
            foreach (var t in engine.History)
                await writer.WriteLineAsync(FormattableString.Invariant($"{t.Id};{t.At:O};{t.AssetId};{t.Direction};{t.Amount};{t.Status};{t.Profit}"));
        }));
    }
    private Control AlertsPage()
    {
        var id = new TextBox { PlaceholderText = "ID do alerta" };
        var pair = new ComboBox { ItemsSource = Enum.GetValues<ActivePair>().Where(x => (int)x > 0).ToArray(), SelectedItem = ActivePair.EURUSD_op };
        var price = new TextBox { PlaceholderText = "Preço" };
        return Stack(Heading("Alertas de preço"), Button("Consultar alertas", async () =>
        {
            var result = await broker.GetAlertsAsync();
            alertList.ItemsSource = result.Records?.Select(x => $"#{x.Id} | {x.AssetId} | {x.Value:N5}").ToArray() ?? [];
        }), alertList, Field("Ativo", pair), Field("Preço do alerta", price),
        Button("Criar alerta", async () => { var result = await broker.CreateAlertAsync((ActivePair)pair.SelectedItem!, (double)Number(price)); Log($"Alerta criado: {result.Id}"); }),
        Field("ID para remover", id), Button("Remover alerta", async () => { await broker.DeleteAlertAsync(long.Parse(id.Text ?? "")); Log("Alerta removido."); }));
    }
    private static decimal Number(TextBox box)
    {
        if (!decimal.TryParse((box.Text ?? "").Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
            throw new ArgumentException("Informe um valor numérico válido.");
        return value;
    }
    private static long[] Ids(TextBox box) => (box.Text ?? "").Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).Distinct().ToArray();
    private async Task SaveSettings()
    {
        settings = settings with { Email = email.Text ?? "", EntryAmount = Number(amount), StopWin = Number(stopWin), StopLoss = Number(stopLoss),
            MaxSignalDelaySeconds = int.Parse(delay.Text ?? ""), MinimumTraderAmount = Number(minimum), TargetTraderIds = Ids(targetIds), WatchedAssetIds = Ids(assetIds).Select(x => checked((int)x)).ToArray() };
        await store.SaveAsync(settings); Log("Configurações salvas.");
    }
    private void ApplySettings()
    {
        email.Text = settings.Email; amount.Text = settings.EntryAmount.ToString(); stopWin.Text = settings.StopWin.ToString(); stopLoss.Text = settings.StopLoss.ToString();
        delay.Text = settings.MaxSignalDelaySeconds.ToString(); minimum.Text = settings.MinimumTraderAmount.ToString();
        targetIds.Text = string.Join(',', settings.TargetTraderIds); assetIds.Text = string.Join(',', settings.WatchedAssetIds);
    }
    private async Task Start()
    {
        if (!loaded) throw new InvalidOperationException("Os dados locais não foram carregados. Confira o registro antes de iniciar.");
        await SaveSettings();
        var selected = environment.SelectedIndex == 0 ? BalanceType.Practice : BalanceType.Real;
        if (enableOrders.IsChecked == true)
        {
            var confirmation = new Window { Title = "Confirmar execução", Width = 440, Height = 240, CanResize = false, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            var accept = new Button { Content = "Confirmar envio de ordens", Padding = new(12) };
            var cancel = new Button { Content = "Cancelar", Padding = new(12) };
            confirmation.Content = Stack(new TextBlock { Text = $"Enviar ordens na conta {(selected == BalanceType.Practice ? "de treinamento" : "REAL")}, com entrada {settings.EntryAmount:N2}?", TextWrapping = TextWrapping.Wrap }, accept, cancel);
            accept.Click += (_, _) => confirmation.Close(true); cancel.Click += (_, _) => confirmation.Close(false);
            if (!await confirmation.ShowDialog<bool>(this)) return;
        }
        await engine.StartAsync(settings, selected, enableOrders.IsChecked == true);
    }
    private void UpdatePortfolio()
    {
        trades.ItemsSource = engine.History.Select(x => x.ToString()).ToArray();
        signalList.ItemsSource = engine.Signals.Select(x => $"{x.At.LocalDateTime:dd/MM HH:mm:ss} | ativo {x.AssetId} | {x.Direction} | M{(int)x.Duration}").ToArray();
        profit.Text = $"Resultado da sessão: {engine.Profit:N2}";
    }
}
