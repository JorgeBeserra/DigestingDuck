using System.Text.Json;

namespace DigestingDuck.Core;
public sealed record Journal(IReadOnlyList<Signal> Signals, IReadOnlyList<TradeRecord> Trades);
public sealed class JournalStore(string directory)
{
    private readonly SemaphoreSlim gate = new(1);
    private string FilePath => Path.Combine(directory, "journal.json");
    public async Task<Journal> LoadAsync()
    {
        if (!File.Exists(FilePath)) return new([], []);
        return JsonSerializer.Deserialize<Journal>(await File.ReadAllTextAsync(FilePath)) ?? new([], []);
    }
    public async Task SaveAsync(Journal journal)
    {
        await gate.WaitAsync();
        try
        {
            Directory.CreateDirectory(directory);
            var temp = FilePath + ".tmp";
            await File.WriteAllTextAsync(temp, JsonSerializer.Serialize(journal));
            File.Move(temp, FilePath, true);
        }
        finally { gate.Release(); }
    }
}
