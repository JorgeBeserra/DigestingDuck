using DigestingDuck.Core;
using IqOptionApiDotNet.Models;
using IqOptionApiDotNet.Models.DigitalOptions;
using NUnit.Framework;

namespace DigestingDuck.Tests;
public class CoreTests
{
    [Test]
    public void CsvRequiresTimezoneAndValidDuration()
    {
        Assert.Throws<FormatException>(() => SignalCsv.Parse("2026-10-12T15:00:00;EURUSD_op;CALL;1"));
        Assert.Throws<FormatException>(() => SignalCsv.Parse("2026-10-12T15:00:00Z;EURUSD_op;CALL;2"));
        var signal = SignalCsv.Parse("data;ativo;direção;minutos\n2026-10-12T15:00:00-03:00;EURUSD_op;PUT;5").Single();
        Assert.That(signal.At.UtcDateTime.Hour, Is.EqualTo(18));
        Assert.That(signal.Direction, Is.EqualTo(OrderDirection.Put));
    }
    [Test]
    public void RejectsSignalsOlderThanOneMinuteEvenIfSecondsComponentIsSmall()
    {
        var now = DateTimeOffset.UtcNow;
        var settings = new AppSettings { TargetTraderIds = [7], WatchedAssetIds = [1861] };
        var deal = new LiveDeal { UserId = 7, ActiveId = (ActivePair)1861, AmountEnrolled = 10, CreatedAt = now.AddSeconds(-61) };
        Assert.That(EntryPolicy.Accept(deal, settings, now), Is.False);
        deal.CreatedAt = now.AddSeconds(-3);
        Assert.That(EntryPolicy.Accept(deal, settings, now), Is.True);
        deal.CreatedAt = now.AddSeconds(3);
        Assert.That(EntryPolicy.Accept(deal, settings, now), Is.False);
    }
    [Test]
    public async Task PersistsSettingsWithoutCredentialsAndRoundTripsJournal()
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        try
        {
            var store = new SettingsStore(directory);
            await store.SaveAsync(new AppSettings { Email = "test@example.invalid", EntryAmount = 2 });
            Assert.That((await store.LoadAsync()).EntryAmount, Is.EqualTo(2));
            Assert.That(await File.ReadAllTextAsync(Path.Combine(directory,"settings.json")), Does.Not.Contain("Password"));
            var journal = new JournalStore(directory);
            await journal.SaveAsync(new([], []));
            Assert.That((await journal.LoadAsync()).Trades, Is.Empty);
        }
        finally { if (Directory.Exists(directory)) Directory.Delete(directory,true); }
    }
    [Test]
    public async Task CopyIsDeduplicatedAndStopPreventsNewOrders()
    {
        var fake = new FakeBroker();
        await using var engine = new TradingEngine(fake);
        var changed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        engine.Changed += () => changed.TrySetResult();
        await engine.StartAsync(new AppSettings { TargetTraderIds = [7], WatchedAssetIds = [1861] }, BalanceType.Practice, true);
        var deal = new LiveDeal { UserId = 7, PositionId = 123, CreatedAt = DateTimeOffset.UtcNow, ActiveId = (ActivePair)1861, AmountEnrolled = 10, ExpirationType = DigitalOptionsExpiryType.PT1M };
        fake.Emit(deal); fake.Emit(deal);
        await changed.Task.WaitAsync(TimeSpan.FromSeconds(5));
        await engine.StopAsync(); fake.Emit(deal);
        Assert.That(fake.Calls, Is.EqualTo(1));
        Assert.That(engine.History.Count, Is.EqualTo(1));
    }
    private sealed class FakeBroker : ITradingBroker
    {
        public bool IsConnected => true;
        public event Action<LiveDeal>? Deal;
        public event Action<PositionChanged>? Position { add {} remove {} }
        public event Action<OptionClosed>? OptionClosed { add {} remove {} }
        public int Calls;
        public void Emit(LiveDeal deal) => Deal?.Invoke(deal);
        public Task SelectBalanceAsync(BalanceType balance) => Task.CompletedTask;
        public Task WatchAsync(IEnumerable<int> ids) => Task.CompletedTask;
        public void Unwatch() { }
        public Task<DigitalOptionsPlacedResult> PlaceAsync(BalanceType balance, ActivePair pair, OrderDirection direction, DigitalOptionsExpiryDuration duration, decimal amount)
        { Calls++; return Task.FromResult(new DigitalOptionsPlacedResult { Id = 42 }); }
    }
}
