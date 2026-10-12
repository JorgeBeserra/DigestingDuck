using System.Text.Json;

namespace DigestingDuck.Core;

public sealed record AppSettings
{
    public string Email { get; init; } = "";
    public bool DarkTheme { get; init; } = true;
    public decimal EntryAmount { get; init; } = 1;
    public decimal StopWin { get; init; } = 10;
    public decimal StopLoss { get; init; } = 10;
    public int MaxSignalDelaySeconds { get; init; } = 5;
    public decimal MinimumTraderAmount { get; init; } = 1;
    public long[] TargetTraderIds { get; init; } = [];
    public int[] WatchedAssetIds { get; init; } = [1861, 76];
    public void Validate()
    {
        if (EntryAmount <= 0 || StopWin <= 0 || StopLoss <= 0)
            throw new ArgumentException("Valores de entrada e limites devem ser positivos.");
        if (MaxSignalDelaySeconds < 1 || MinimumTraderAmount < 0)
            throw new ArgumentException("Verifique o atraso máximo e o valor mínimo do sinal.");
        if (TargetTraderIds.Any(x => x <= 0) || WatchedAssetIds.Any(x => x <= 0))
            throw new ArgumentException("IDs devem ser positivos.");
    }
}

public sealed class SettingsStore
{
    private readonly SemaphoreSlim gate = new(1);
    public string DirectoryPath { get; }
    public SettingsStore(string? directory = null)
    {
        DirectoryPath = directory ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DigestingDuck", "desktop");
    }
    public async Task<AppSettings> LoadAsync()
    {
        var file = Path.Combine(DirectoryPath, "settings.json");
        if (!File.Exists(file)) return new();
        var settings = JsonSerializer.Deserialize<AppSettings>(await File.ReadAllTextAsync(file)) ?? new();
        settings.Validate();
        return settings;
    }
    public async Task SaveAsync(AppSettings settings)
    {
        settings.Validate();
        await gate.WaitAsync();
        try
        {
            Directory.CreateDirectory(DirectoryPath);
            var file = Path.Combine(DirectoryPath, "settings.json");
            var temp = file + ".tmp";
            await File.WriteAllTextAsync(temp, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
            File.Move(temp, file, true);
        }
        finally { gate.Release(); }
    }
}
