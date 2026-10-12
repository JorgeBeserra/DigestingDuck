using Realms;
using System.Text.Json;
using DigestingDuck.Core;

if (args.Length is < 2 or > 3) { Console.Error.WriteLine("Uso: LegacyExport banco.realm arquivo.json [idUsuario]. Feche o programa antigo antes de exportar."); return 2; }
var workspace = Path.Combine(Path.GetTempPath(), "duck-legacy-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(workspace);
try
{
    var source = Path.GetFullPath(args[0]);
    var output = Path.GetFullPath(args[1]);
    if (source == output || File.Exists(output)) throw new IOException("Escolha um arquivo de saída novo.");
    var copy = Path.Combine(workspace, "legacy.realm");
    File.Copy(source, copy);
    using var realm = Realm.GetInstance(new RealmConfiguration(copy) { IsDynamic = true });
    var users = realm.DynamicApi.All("Usuario").ToArray();
    var user = args.Length == 3 ? users.Single(x => x.DynamicApi.Get<long>("Id") == long.Parse(args[2])) :
        users.Length == 1 ? users[0] : throw new ArgumentException("Há vários usuários. Informe o idUsuario: " + string.Join(", ", users.Select(x => x.DynamicApi.Get<long>("Id"))));
    var id = user.DynamicApi.Get<long>("Id");
    var config = realm.DynamicApi.All("UsuarioConfiguracao").ToArray().SingleOrDefault(x => x.DynamicApi.Get<IRealmObjectBase>("Usuario").DynamicApi.Get<long>("Id") == id);
    var legacy = new Dictionary<string, string?>();
    if (config != null)
        foreach (var property in config.ObjectSchema!)
            if (property.Name != "Usuario") legacy[property.Name] = config.DynamicApi.Get<RealmValue>(property.Name).ToString();
    var settings = new AppSettings { Email = user.DynamicApi.Get<string>("Email"),
        TargetTraderIds = realm.DynamicApi.All("TraderAlvo").ToArray().Select(x => x.DynamicApi.Get<long>("UserId")).Where(x => x > 0).Distinct().ToArray(),
        WatchedAssetIds = realm.DynamicApi.All("AtivoStatus").ToArray().Where(x => x.DynamicApi.Get<bool>("Alvo")).Select(x => x.DynamicApi.Get<IRealmObjectBase>("Ativo").DynamicApi.Get<int>("_id")).Where(x => x > 0).Distinct().ToArray() };
    // Risk rules are retained for review, never silently converted into different execution semantics.
    var bundle = new LegacySettingsBundle(1, settings, legacy);
    using var stream = new FileStream(output, FileMode.CreateNew, FileAccess.Write);
    JsonSerializer.Serialize(stream, bundle, new JsonSerializerOptions { WriteIndented = true });
    Console.WriteLine("Exportado sem senhas. Revise os limites na nova interface; histórico e sinais continuam no banco original.");
    return 0;
}
catch (Exception ex) { Console.Error.WriteLine($"Não foi possível exportar: {ex.Message}"); return 1; }
finally { Directory.Delete(workspace, true); }
