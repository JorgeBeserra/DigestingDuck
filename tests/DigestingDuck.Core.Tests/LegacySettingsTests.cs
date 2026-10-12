using System.Text.Json;
using NUnit.Framework;
using DigestingDuck.Core;
namespace DigestingDuck.Core.Tests;
public class LegacySettingsTests
{
    [Test]
    public void ValidatesPortableSettingsAndPreservesLegacyRules()
    {
        var source = new LegacySettingsBundle(1, new AppSettings { Email = "user@example.com", TargetTraderIds = [123], WatchedAssetIds = [76] }, new() { ["MtgNivel"] = "3" });
        var imported = LegacySettingsBundle.Read(JsonSerializer.Serialize(source));
        Assert.That(imported.Settings.TargetTraderIds, Is.EqualTo(new long[] { 123 }));
        Assert.That(imported.LegacyRules["MtgNivel"], Is.EqualTo("3"));
    }
    [TestCase("null")]
    [TestCase("{\"FormatVersion\":2,\"Settings\":{},\"LegacyRules\":{}}")]
    [TestCase("{\"FormatVersion\":1,\"Settings\":{\"TargetTraderIds\":null},\"LegacyRules\":{}}")]
    public void RejectsInvalidBundles(string json) => Assert.Throws<ArgumentException>(() => LegacySettingsBundle.Read(json));
}
