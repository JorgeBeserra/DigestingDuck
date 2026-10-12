using System.Text.Json;
namespace DigestingDuck.Core;

public sealed record LegacySettingsBundle(int FormatVersion, AppSettings Settings, Dictionary<string, string?> LegacyRules)
{
    public static LegacySettingsBundle Read(string json)
    {
        var bundle = JsonSerializer.Deserialize<LegacySettingsBundle>(json) ?? throw new ArgumentException("Arquivo vazio.");
        if (bundle.FormatVersion != 1 || bundle.Settings is null || bundle.LegacyRules is null)
            throw new ArgumentException("Formato de migração inválido.");
        if (bundle.Settings.TargetTraderIds is null || bundle.Settings.WatchedAssetIds is null)
            throw new ArgumentException("IDs ausentes no arquivo de migração.");
        bundle.Settings.Validate();
        return bundle;
    }
}
