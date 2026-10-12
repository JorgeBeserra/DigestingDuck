using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using Realms;
using NUnit.Framework;
using DigestingDuck.Models;
namespace DigestingDuck.LegacyExport.Tests;
public class ExportTests
{
    [Test]
    public async Task ExportsSelectedProfileWithoutCredentialsOrModifyingSource()
    {
        var directory = Path.Combine(Path.GetTempPath(), "duck-fixture-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var database = Path.Combine(directory, "test.realm");
            using (var realm = Realm.GetInstance(new RealmConfiguration(database)))
                realm.Write(() => {
                    var user = realm.Add(new Usuario { Id = 123, Email = "fixture@example.com", Senha = "DO-NOT-EXPORT" });
                    realm.Add(new Usuario { Id = 789, Email = "other@example.com", Senha = "OTHER-SECRET" });
                    realm.Add(new UsuarioConfiguracao { Id = 1, Usuario = user, ValorEntrada = 2, StopLoss = 20, MtgNivel = 3 });
                    realm.Add(new TraderAlvo { Id = 1, UserId = 456 });
                    var asset = realm.Add(new Ativo { _id = 76 });
                    realm.Add(new AtivoStatus { Id = 1, Alvo = true, Ativo = asset });
                });
            var hash = SHA256.HashData(await File.ReadAllBytesAsync(database));
            var output = Path.Combine(directory, "export.json");
            var start = new ProcessStartInfo(Environment.GetEnvironmentVariable("LEGACY_DOTNET") ?? "dotnet") { RedirectStandardOutput = true, RedirectStandardError = true };
            start.ArgumentList.Add(Environment.GetEnvironmentVariable("LEGACY_EXPORT_DLL") ?? throw new InvalidOperationException("Defina LEGACY_EXPORT_DLL."));
            start.ArgumentList.Add(database); start.ArgumentList.Add(output); start.ArgumentList.Add("123");
            using var process = Process.Start(start)!;
            var stdout = process.StandardOutput.ReadToEndAsync(); var stderr = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            Assert.That(process.ExitCode, Is.Zero, await stdout + await stderr);
            var text = await File.ReadAllTextAsync(output);
            Assert.That(text, Does.Not.Contain("DO-NOT-EXPORT").And.Not.Contain("OTHER-SECRET").And.Not.Contain("Senha"));
            using var json = JsonDocument.Parse(text);
            Assert.That(json.RootElement.GetProperty("Settings").GetProperty("Email").GetString(), Is.EqualTo("fixture@example.com"));
            Assert.That(json.RootElement.GetProperty("Settings").GetProperty("TargetTraderIds")[0].GetInt64(), Is.EqualTo(456));
            Assert.That(json.RootElement.GetProperty("Settings").GetProperty("WatchedAssetIds")[0].GetInt32(), Is.EqualTo(76));
            Assert.That(json.RootElement.GetProperty("LegacyRules").GetProperty("MtgNivel").GetString(), Is.EqualTo("3"));
            Assert.That(SHA256.HashData(await File.ReadAllBytesAsync(database)), Is.EqualTo(hash));
        }
        finally { Directory.Delete(directory, true); }
    }
}
