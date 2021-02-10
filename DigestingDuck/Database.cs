using DigestingDuck.models;
using Realms;
using Realms.Exceptions;
using System;
using System.IO;
using System.Linq;

using RealmType = Realms.Realm;

namespace DigestingDuck
{
    public class Database
    {

        // Adicionar +1 quando tiver alguma alteração na base duvidas.. consultem a documentação
        // https://realm.io/docs/dotnet/latest/#migrations

        const int SCHEMA_VERSION = 0; 

        public RealmType realm;

        public string strSettingRealm;

        public Database()
        {

        }

        public RealmType AbrirDB()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DigestingDuck");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            strSettingRealm = Path.Combine(dir, "Realm.realm");

            var config = new RealmConfiguration(strSettingRealm)
            {
                SchemaVersion = SCHEMA_VERSION,
                MigrationCallback = (migration, oldSchemaVersion) =>
                {
                    if (oldSchemaVersion < 0)
                    {
                        // Colocar aqui o que precisa alterar quando for fazer migração!!!
                    }

                    if (oldSchemaVersion < 0)
                    {
                        // Colocar aqui o que precisa alterar quando for fazer migração!!!
                    }
                }
            };

            return this.realm = RealmType.GetInstance(config);
            
        }
    
        public void AbrirConexao()
        {
            /*
             * Migração desativada até que seja liberado alguma versão
             * 
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DigestingDuck");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            strSettingRealm = Path.Combine(dir, "Realm.realm");

            var config = new RealmConfiguration("Verificar com Calma Caminho para o Realm")
            {
                SchemaVersion = SCHEMA_VERSION,
                MigrationCallback = (migration, oldSchemaVersion) =>
                {
                    if (oldSchemaVersion < 0)
                    {
                        // Colocar aqui o que precisa alterar quando for fazer migração!!!
                    }

                    if (oldSchemaVersion < 0)
                    {
                        // Colocar aqui o que precisa alterar quando for fazer migração!!!
                    }
                }
            };
            var realm = Realms.Realm.GetInstance(config);
            */

            BasePadrao(this.realm);
        }

        public void BasePadrao(Realm realm)
        {
            // Possivelmente irei remover algumas dessas inserções pois ainda estou analisando como pegar tudo via API
            // Ainda tem uma request para ser Criada a GET-INITIALIZATION-DATA porém não tive tempo
            var ativos = realm.All<Ativos>();
            var ativoscategorias = realm.All<AtivosCategorias>();
            var ativosstatus = realm.All<AtivosStatus>();
            var usuariosconfiguracao = realm.All<UsuariosConfiguracao>();
            
            if (ativos.Count() == 0)
            {
                realm.Write(() => {
                    var a = new Ativos
                    {
                        Id = 1,
                        Status = true,
                        Descricao = "EURUSD"
                    }; realm.Add(a); });
                realm.Write(() => {
                    var a = new Ativos
                    {
                        Id = 2,
                        Status = true,
                        Descricao = "EURGBP"
                    }; realm.Add(a); });
                realm.Write(() => {
                    var a = new Ativos
                    {
                        Id = 3,
                        Status = true,
                        Descricao = "GBPJPY"
                    }; realm.Add(a); });
                realm.Write(() => {
                    var a = new Ativos
                    {
                        Id = 4,
                        Status = true,
                        Descricao = "EURJPY"
                    }; realm.Add(a); });
                realm.Write(() => {
                    var a = new Ativos
                    {
                        Id = 5,
                        Status = true,
                        Descricao = "GBPUSD"
                    }; realm.Add(a); });
                realm.Write(() => {
                    var a = new Ativos
                    {
                        Id = 6,
                        Status = true,
                        Descricao = "USDJPY"
                    }; realm.Add(a); });

                realm.Write(() => { var a = new Ativos(); a.Id = 7; a.Status = true; a.Descricao = "AUDCAD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 8; a.Status = true; a.Descricao = "NZDUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 10; a.Status = true; a.Descricao = "USDRUB"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 31; a.Status = true; a.Descricao = "AMAZON"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 32; a.Status = true; a.Descricao = "APPLE"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 33; a.Status = true; a.Descricao = "BAIDU"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 34; a.Status = true; a.Descricao = "CISCO"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 35; a.Status = true; a.Descricao = "FACEBOOK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 36; a.Status = true; a.Descricao = "GOOGLE"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 37; a.Status = true; a.Descricao = "INTEL"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 38; a.Status = true; a.Descricao = "MSFT"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 40; a.Status = true; a.Descricao = "YAHOO"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 41; a.Status = true; a.Descricao = "AIG"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 45; a.Status = true; a.Descricao = "CITI"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 46; a.Status = true; a.Descricao = "COKE"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 48; a.Status = true; a.Descricao = "GE"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 49; a.Status = true; a.Descricao = "GM"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 50; a.Status = true; a.Descricao = "GS"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 51; a.Status = true; a.Descricao = "JPM"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 52; a.Status = true; a.Descricao = "MCDON"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 53; a.Status = true; a.Descricao = "MORSTAN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 54; a.Status = true; a.Descricao = "NIKE"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 72; a.Status = true; a.Descricao = "USDCHF"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 74; a.Status = true; a.Descricao = "XAUUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 75; a.Status = true; a.Descricao = "XAGUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 76; a.Status = true; a.Descricao = "EURUSD-OTC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 77; a.Status = true; a.Descricao = "EURGBP-OTC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 78; a.Status = true; a.Descricao = "USDCHF-OTC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 79; a.Status = true; a.Descricao = "EURJPY-OTC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 80; a.Status = true; a.Descricao = "NZDUSD-OTC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 81; a.Status = true; a.Descricao = "GBPUSD-OTC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 84; a.Status = true; a.Descricao = "GBPJPY-OTC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 85; a.Status = true; a.Descricao = "USDJPY-OTC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 86; a.Status = true; a.Descricao = "AUDCAD-OTC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 87; a.Status = true; a.Descricao = "ALIBABA"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 95; a.Status = true; a.Descricao = "YANDEX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 99; a.Status = true; a.Descricao = "AUDUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 100; a.Status = true; a.Descricao = "USDCAD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 101; a.Status = true; a.Descricao = "AUDJPY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 102; a.Status = true; a.Descricao = "GBPCAD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 103; a.Status = true; a.Descricao = "GBPCHF"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 104; a.Status = true; a.Descricao = "GBPAUD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 105; a.Status = true; a.Descricao = "EURCAD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 106; a.Status = true; a.Descricao = "CHFJPY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 107; a.Status = true; a.Descricao = "CADCHF"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 108; a.Status = true; a.Descricao = "EURAUD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 113; a.Status = true; a.Descricao = "TWITTER"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 133; a.Status = true; a.Descricao = "FERRARI"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 167; a.Status = true; a.Descricao = "TESLA"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 168; a.Status = true; a.Descricao = "USDNOK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 212; a.Status = true; a.Descricao = "EURNZD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 219; a.Status = true; a.Descricao = "USDSEK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 220; a.Status = true; a.Descricao = "USDTRY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 252; a.Status = true; a.Descricao = "MMM:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 253; a.Status = true; a.Descricao = "ABT:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 254; a.Status = true; a.Descricao = "ABBV:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 255; a.Status = true; a.Descricao = "ACN:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 256; a.Status = true; a.Descricao = "ATVI:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 258; a.Status = true; a.Descricao = "ADBE:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 259; a.Status = true; a.Descricao = "AAP:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 269; a.Status = true; a.Descricao = "AA:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 272; a.Status = true; a.Descricao = "AGN:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 278; a.Status = true; a.Descricao = "MO:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 290; a.Status = true; a.Descricao = "AMGN:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 303; a.Status = true; a.Descricao = "T:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 304; a.Status = true; a.Descricao = "ADSK:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 313; a.Status = true; a.Descricao = "BAC:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 320; a.Status = true; a.Descricao = "BBY:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 324; a.Status = true; a.Descricao = "BA:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 328; a.Status = true; a.Descricao = "BMY:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 338; a.Status = true; a.Descricao = "CAT:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 344; a.Status = true; a.Descricao = "CTL:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 349; a.Status = true; a.Descricao = "CVX:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 356; a.Status = true; a.Descricao = "CTAS:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 360; a.Status = true; a.Descricao = "CTXS:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 365; a.Status = true; a.Descricao = "CL:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 366; a.Status = true; a.Descricao = "CMCSA:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 369; a.Status = true; a.Descricao = "CXO:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 370; a.Status = true; a.Descricao = "COP:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 371; a.Status = true; a.Descricao = "ED:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 374; a.Status = true; a.Descricao = "COST:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 379; a.Status = true; a.Descricao = "CVS:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 380; a.Status = true; a.Descricao = "DHI:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 381; a.Status = true; a.Descricao = "DHR:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 382; a.Status = true; a.Descricao = "DRI:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 383; a.Status = true; a.Descricao = "DVA:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 386; a.Status = true; a.Descricao = "DAL:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 388; a.Status = true; a.Descricao = "DVN:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 389; a.Status = true; a.Descricao = "DO:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 390; a.Status = true; a.Descricao = "DLR:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 391; a.Status = true; a.Descricao = "DFS:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 392; a.Status = true; a.Descricao = "DISCA:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 397; a.Status = true; a.Descricao = "DOV:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 400; a.Status = true; a.Descricao = "DTE:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 403; a.Status = true; a.Descricao = "DNB:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 404; a.Status = true; a.Descricao = "ETFC:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 405; a.Status = true; a.Descricao = "EMN:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 407; a.Status = true; a.Descricao = "EBAY:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 408; a.Status = true; a.Descricao = "ECL:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 409; a.Status = true; a.Descricao = "EIX:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 413; a.Status = true; a.Descricao = "EMR:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 415; a.Status = true; a.Descricao = "ETR:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 417; a.Status = true; a.Descricao = "EQT:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 418; a.Status = true; a.Descricao = "EFX:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 420; a.Status = true; a.Descricao = "EQR:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 421; a.Status = true; a.Descricao = "ESS:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 426; a.Status = true; a.Descricao = "EXPD:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 428; a.Status = true; a.Descricao = "EXR:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 429; a.Status = true; a.Descricao = "XOM:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 430; a.Status = true; a.Descricao = "FFIV:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 432; a.Status = true; a.Descricao = "FAST:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 433; a.Status = true; a.Descricao = "FRT:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 434; a.Status = true; a.Descricao = "FDX:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 435; a.Status = true; a.Descricao = "FIS:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 436; a.Status = true; a.Descricao = "FITB:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 437; a.Status = true; a.Descricao = "FSLR:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 438; a.Status = true; a.Descricao = "FE:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 439; a.Status = true; a.Descricao = "FISV:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 441; a.Status = true; a.Descricao = "FLS:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 443; a.Status = true; a.Descricao = "FMC:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 448; a.Status = true; a.Descricao = "FBHS:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 450; a.Status = true; a.Descricao = "FCX:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 451; a.Status = true; a.Descricao = "FTR:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 460; a.Status = true; a.Descricao = "GILD:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 471; a.Status = true; a.Descricao = "HAS:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 480; a.Status = true; a.Descricao = "HON:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 491; a.Status = true; a.Descricao = "IBM:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 513; a.Status = true; a.Descricao = "KHC:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 528; a.Status = true; a.Descricao = "LMT:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 542; a.Status = true; a.Descricao = "MA:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 548; a.Status = true; a.Descricao = "MDT:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 553; a.Status = true; a.Descricao = "MU:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 569; a.Status = true; a.Descricao = "NFLX:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 575; a.Status = true; a.Descricao = "NEE:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 586; a.Status = true; a.Descricao = "NVDA:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 597; a.Status = true; a.Descricao = "PYPL:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 603; a.Status = true; a.Descricao = "PFE:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 605; a.Status = true; a.Descricao = "PM:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 617; a.Status = true; a.Descricao = "PG:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 626; a.Status = true; a.Descricao = "QCOM:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 628; a.Status = true; a.Descricao = "DGX:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 630; a.Status = true; a.Descricao = "RTN:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 645; a.Status = true; a.Descricao = "CRM:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 647; a.Status = true; a.Descricao = "SLB:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 666; a.Status = true; a.Descricao = "SBUX:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 670; a.Status = true; a.Descricao = "SYK:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 689; a.Status = true; a.Descricao = "DIS:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 692; a.Status = true; a.Descricao = "TWX:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 723; a.Status = true; a.Descricao = "VZ:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 726; a.Status = true; a.Descricao = "V:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 729; a.Status = true; a.Descricao = "WMT:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 730; a.Status = true; a.Descricao = "WBA:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 733; a.Status = true; a.Descricao = "WFC:US"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 756; a.Status = true; a.Descricao = "SNAP"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 757; a.Status = true; a.Descricao = "DUBAI"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 758; a.Status = true; a.Descricao = "TA25"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 760; a.Status = true; a.Descricao = "AMD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 761; a.Status = true; a.Descricao = "ALGN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 762; a.Status = true; a.Descricao = "ANSS"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 772; a.Status = true; a.Descricao = "DRE"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 775; a.Status = true; a.Descricao = "IDXX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 781; a.Status = true; a.Descricao = "RMD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 783; a.Status = true; a.Descricao = "SU"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 784; a.Status = true; a.Descricao = "TFX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 785; a.Status = true; a.Descricao = "TMUS"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 796; a.Status = true; a.Descricao = "QQQ"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 808; a.Status = true; a.Descricao = "SPY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 816; a.Status = true; a.Descricao = "BTCUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 817; a.Status = true; a.Descricao = "XRPUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 818; a.Status = true; a.Descricao = "ETHUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 819; a.Status = true; a.Descricao = "LTCUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 821; a.Status = true; a.Descricao = "DSHUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 824; a.Status = true; a.Descricao = "BCHUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 825; a.Status = true; a.Descricao = "OMGUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 826; a.Status = true; a.Descricao = "ZECUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 829; a.Status = true; a.Descricao = "ETCUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 830; a.Status = true; a.Descricao = "BTCUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 831; a.Status = true; a.Descricao = "ETHUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 834; a.Status = true; a.Descricao = "LTCUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 836; a.Status = true; a.Descricao = "BCHUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 837; a.Status = true; a.Descricao = "BTGUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 845; a.Status = true; a.Descricao = "QTMUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 847; a.Status = true; a.Descricao = "XLMUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 858; a.Status = true; a.Descricao = "TRXUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 864; a.Status = true; a.Descricao = "EOSUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 865; a.Status = true; a.Descricao = "USDINR"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 866; a.Status = true; a.Descricao = "USDPLN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 867; a.Status = true; a.Descricao = "USDBRL"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 868; a.Status = true; a.Descricao = "USDZAR"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 889; a.Status = true; a.Descricao = "DBX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 891; a.Status = true; a.Descricao = "SPOT"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 892; a.Status = true; a.Descricao = "USDSGD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 893; a.Status = true; a.Descricao = "USDHKD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 894; a.Status = true; a.Descricao = "LLOYL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 895; a.Status = true; a.Descricao = "VODL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 896; a.Status = true; a.Descricao = "BARCL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 897; a.Status = true; a.Descricao = "TSCOL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 898; a.Status = true; a.Descricao = "BPL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 899; a.Status = true; a.Descricao = "HSBAL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 900; a.Status = true; a.Descricao = "RBSL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 901; a.Status = true; a.Descricao = "BLTL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 902; a.Status = true; a.Descricao = "MRWL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 903; a.Status = true; a.Descricao = "STANL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 904; a.Status = true; a.Descricao = "RRL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 905; a.Status = true; a.Descricao = "MKSL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 906; a.Status = true; a.Descricao = "BATSL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 908; a.Status = true; a.Descricao = "ULVRL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 909; a.Status = true; a.Descricao = "EZJL-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 910; a.Status = true; a.Descricao = "ADSD-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 911; a.Status = true; a.Descricao = "ALVD-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 912; a.Status = true; a.Descricao = "BAYND-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 913; a.Status = true; a.Descricao = "BMWD-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 914; a.Status = true; a.Descricao = "CBKD-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 915; a.Status = true; a.Descricao = "COND-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 916; a.Status = true; a.Descricao = "DAID-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 917; a.Status = true; a.Descricao = "DBKD-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 919; a.Status = true; a.Descricao = "DPWD-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 920; a.Status = true; a.Descricao = "DTED-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 921; a.Status = true; a.Descricao = "EOAND-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 922; a.Status = true; a.Descricao = "MRKD-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 923; a.Status = true; a.Descricao = "SIED-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 924; a.Status = true; a.Descricao = "TKAD-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 925; a.Status = true; a.Descricao = "VOW3D-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 926; a.Status = true; a.Descricao = "ENELM-CHIX"; realm.Add(a); });  
                realm.Write(() => { var a = new Ativos(); a.Id = 927; a.Status = true; a.Descricao = "ENIM-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 928; a.Status = true; a.Descricao = "FCAM-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 929; a.Status = true; a.Descricao = "PIRCM-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 930; a.Status = true; a.Descricao = "PSTM-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 931; a.Status = true; a.Descricao = "TITM-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 932; a.Status = true; a.Descricao = "UCGM-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 933; a.Status = true; a.Descricao = "CSGNZ-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 934; a.Status = true; a.Descricao = "NESNZ-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 935; a.Status = true; a.Descricao = "ROGZ-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 936; a.Status = true; a.Descricao = "UBSGZ-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 937; a.Status = true; a.Descricao = "SANE-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 938; a.Status = true; a.Descricao = "BBVAE-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 939; a.Status = true; a.Descricao = "TEFE-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 940; a.Status = true; a.Descricao = "AIRP-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 941; a.Status = true; a.Descricao = "HEIOA-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 942; a.Status = true; a.Descricao = "ORP-CHIX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 943; a.Status = true; a.Descricao = "AUDCHF"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 944; a.Status = true; a.Descricao = "AUDNZD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 945; a.Status = true; a.Descricao = "CADJPY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 946; a.Status = true; a.Descricao = "EURCHF"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 947; a.Status = true; a.Descricao = "GBPNZD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 948; a.Status = true; a.Descricao = "NZDCAD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 949; a.Status = true; a.Descricao = "NZDJPY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 951; a.Status = true; a.Descricao = "EURNOK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 952; a.Status = true; a.Descricao = "CHFSGD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 955; a.Status = true; a.Descricao = "EURSGD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 957; a.Status = true; a.Descricao = "USDMXN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 958; a.Status = true; a.Descricao = "JUVEM"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 959; a.Status = true; a.Descricao = "ASRM"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 966; a.Status = true; a.Descricao = "MANU"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 969; a.Status = true; a.Descricao = "UKOUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 970; a.Status = true; a.Descricao = "XPTUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 971; a.Status = true; a.Descricao = "USOUSD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 977; a.Status = true; a.Descricao = "W1"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 983; a.Status = true; a.Descricao = "AUDDKK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 985; a.Status = true; a.Descricao = "AUDMXN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 986; a.Status = true; a.Descricao = "AUDNOK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 988; a.Status = true; a.Descricao = "AUDSEK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 989; a.Status = true; a.Descricao = "AUDSGD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 990; a.Status = true; a.Descricao = "AUDTRY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 992; a.Status = true; a.Descricao = "CADMXN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 993; a.Status = true; a.Descricao = "CADNOK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 994; a.Status = true; a.Descricao = "CADPLN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 995; a.Status = true; a.Descricao = "CADTRY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 996; a.Status = true; a.Descricao = "CHFDKK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 998; a.Status = true; a.Descricao = "CHFNOK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1000; a.Status = true; a.Descricao = "CHFSEK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1001; a.Status = true; a.Descricao = "CHFTRY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1004; a.Status = true; a.Descricao = "DKKPLN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1005; a.Status = true; a.Descricao = "DKKSGD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1007; a.Status = true; a.Descricao = "EURDKK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1008; a.Status = true; a.Descricao = "EURMXN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1010; a.Status = true; a.Descricao = "EURTRY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1011; a.Status = true; a.Descricao = "EURZAR"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1013; a.Status = true; a.Descricao = "GBPILS"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1014; a.Status = true; a.Descricao = "GBPMXN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1015; a.Status = true; a.Descricao = "GBPNOK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1016; a.Status = true; a.Descricao = "GBPPLN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1017; a.Status = true; a.Descricao = "GBPSEK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1018; a.Status = true; a.Descricao = "GBPSGD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1019; a.Status = true; a.Descricao = "GBPTRY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1023; a.Status = true; a.Descricao = "NOKDKK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1024; a.Status = true; a.Descricao = "NOKJPY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1025; a.Status = true; a.Descricao = "NOKSEK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1026; a.Status = true; a.Descricao = "NZDDKK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1027; a.Status = true; a.Descricao = "NZDMXN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1028; a.Status = true; a.Descricao = "NZDNOK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1030; a.Status = true; a.Descricao = "NZDSEK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1031; a.Status = true; a.Descricao = "NZDSGD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1032; a.Status = true; a.Descricao = "NZDTRY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1033; a.Status = true; a.Descricao = "NZDZAR"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1036; a.Status = true; a.Descricao = "PLNSEK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1037; a.Status = true; a.Descricao = "SEKDKK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1038; a.Status = true; a.Descricao = "SEKJPY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1041; a.Status = true; a.Descricao = "SGDJPY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1045; a.Status = true; a.Descricao = "USDDKK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1048; a.Status = true; a.Descricao = "NZDCHF"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1049; a.Status = true; a.Descricao = "GBPHUF"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1050; a.Status = true; a.Descricao = "USDCZK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1051; a.Status = true; a.Descricao = "USDHUF"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1054; a.Status = true; a.Descricao = "CADSGD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1056; a.Status = true; a.Descricao = "EURCZK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1057; a.Status = true; a.Descricao = "EURHUF"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1062; a.Status = true; a.Descricao = "USDTHB"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1116; a.Status = true; a.Descricao = "IOTUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1117; a.Status = true; a.Descricao = "XLMUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1118; a.Status = true; a.Descricao = "NEOUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1119; a.Status = true; a.Descricao = "ADAUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1120; a.Status = true; a.Descricao = "XEMUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1122; a.Status = true; a.Descricao = "XRPUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1203; a.Status = true; a.Descricao = "EEM"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1204; a.Status = true; a.Descricao = "FXI"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1205; a.Status = true; a.Descricao = "IWM"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1206; a.Status = true; a.Descricao = "GDX"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1209; a.Status = true; a.Descricao = "XOP"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1210; a.Status = true; a.Descricao = "XLK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1211; a.Status = true; a.Descricao = "XLE"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1212; a.Status = true; a.Descricao = "XLU"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1213; a.Status = true; a.Descricao = "IEMG"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1214; a.Status = true; a.Descricao = "XLY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1215; a.Status = true; a.Descricao = "IYR"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1216; a.Status = true; a.Descricao = "SQQQ"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1217; a.Status = true; a.Descricao = "OIH"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1218; a.Status = true; a.Descricao = "SMH"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1219; a.Status = true; a.Descricao = "EWJ"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1221; a.Status = true; a.Descricao = "XLB"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1222; a.Status = true; a.Descricao = "DIA"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1223; a.Status = true; a.Descricao = "TLT"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1224; a.Status = true; a.Descricao = "SDS"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1225; a.Status = true; a.Descricao = "EWW"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1227; a.Status = true; a.Descricao = "XME"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1229; a.Status = true; a.Descricao = "QID"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1230; a.Status = true; a.Descricao = "AUS200"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1231; a.Status = true; a.Descricao = "FRANCE40"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1232; a.Status = true; a.Descricao = "GERMANY30"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1233; a.Status = true; a.Descricao = "HONGKONG50"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1234; a.Status = true; a.Descricao = "SPAIN35"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1235; a.Status = true; a.Descricao = "US30"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1236; a.Status = true; a.Descricao = "USNDAQ100"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1237; a.Status = true; a.Descricao = "JAPAN225"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1239; a.Status = true; a.Descricao = "USSPX500"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1241; a.Status = true; a.Descricao = "UK100"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1242; a.Status = true; a.Descricao = "TRXUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1244; a.Status = true; a.Descricao = "EOSUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1279; a.Status = true; a.Descricao = "BNBUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1288; a.Status = true; a.Descricao = "ACB"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1289; a.Status = true; a.Descricao = "CGC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1290; a.Status = true; a.Descricao = "CRON"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1291; a.Status = true; a.Descricao = "GWPH"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1292; a.Status = true; a.Descricao = "MJ"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1293; a.Status = true; a.Descricao = "TLRY"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1294; a.Status = true; a.Descricao = "BUD"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1313; a.Status = true; a.Descricao = "LYFT"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1315; a.Status = true; a.Descricao = "PINS"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1316; a.Status = true; a.Descricao = "ZM"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1334; a.Status = true; a.Descricao = "UBER"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1335; a.Status = true; a.Descricao = "MELI"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1336; a.Status = true; a.Descricao = "BYND"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1338; a.Status = true; a.Descricao = "BSVUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1339; a.Status = true; a.Descricao = "ONTUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1340; a.Status = true; a.Descricao = "ATOMUSD-L"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1343; a.Status = true; a.Descricao = "WORK"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1350; a.Status = true; a.Descricao = "FDJP"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1351; a.Status = true; a.Descricao = "CAN"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1352; a.Status = true; a.Descricao = "VIAC"; realm.Add(a); });
                realm.Write(() => { var a = new Ativos(); a.Id = 1353; a.Status = true; a.Descricao = "TFC"; realm.Add(a); });
            }
            if (ativoscategorias.Count() == 0)
            {
                realm.Write(() => { var a = new AtivosCategorias(); a.Id = 1; a.Descricao = "Binaria"; realm.Add(a); });
                realm.Write(() => { var a = new AtivosCategorias(); a.Id = 2; a.Descricao = "Turbo"; realm.Add(a); });
                realm.Write(() => { var a = new AtivosCategorias(); a.Id = 3; a.Descricao = "Digital"; realm.Add(a); });
                realm.Write(() => { var a = new AtivosCategorias(); a.Id = 4; a.Descricao = "Cfd"; realm.Add(a); });
                realm.Write(() => { var a = new AtivosCategorias(); a.Id = 5; a.Descricao = "Forex"; realm.Add(a); });
                realm.Write(() => { var a = new AtivosCategorias(); a.Id = 6; a.Descricao = "Crypto"; realm.Add(a); });
            }
            if (ativosstatus.Count() == 0)
            {
                var aBinarias = realm.All<AtivosCategorias>().First(d => d.Id == 1);
                var aTurbo = realm.All<AtivosCategorias>().First(d => d.Id == 2);
                var aDigital = realm.All<AtivosCategorias>().First(d => d.Id == 3);
                var aCfd = realm.All<AtivosCategorias>().First(d => d.Id == 4);
                var aForex = realm.All<AtivosCategorias>().First(d => d.Id == 5);
                var aCrypto = realm.All<AtivosCategorias>().First(d => d.Id == 6);
                
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 1; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDPLN"); a.Descricao = "USDPLN"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 2; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDBRL"); a.Descricao = "USDBRL"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 3; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDZAR"); a.Descricao = "USDZAR"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 4; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURGBP-OTC"); a.Descricao = "EURGBP-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 5; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURJPY"); a.Descricao = "EURJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 6; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NZDUSD-OTC"); a.Descricao = "NZDUSD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 7; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURCHF"); a.Descricao = "EURCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 8; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURGBP"); a.Descricao = "EURGBP"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 9; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCHF"); a.Descricao = "USDCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 10; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDCAD-OTC"); a.Descricao = "AUDCAD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 11; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPNZD"); a.Descricao = "GBPNZD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 12; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDHKD"); a.Descricao = "USDHKD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 13; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCHF-OTC"); a.Descricao = "USDCHF-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 14; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDUSD"); a.Descricao = "AUDUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 15; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDSGD"); a.Descricao = "USDSGD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 16; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDINR"); a.Descricao = "USDINR"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 17; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPCAD"); a.Descricao = "GBPCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 18; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPCHF"); a.Descricao = "GBPCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 19; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDCHF"); a.Descricao = "AUDCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 20; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPAUD"); a.Descricao = "GBPAUD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 21; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURCAD"); a.Descricao = "EURCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 22; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CADCHF"); a.Descricao = "CADCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 23; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURAUD"); a.Descricao = "EURAUD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 24; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDNZD"); a.Descricao = "AUDNZD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 25; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURNZD"); a.Descricao = "EURNZD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 26; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDRUB"); a.Descricao = "USDRUB"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 27; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPUSD-OTC"); a.Descricao = "GBPUSD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 28; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCAD"); a.Descricao = "USDCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 29; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDJPY"); a.Descricao = "AUDJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 30; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURUSD-OTC"); a.Descricao = "EURUSD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 31; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURUSD"); a.Descricao = "EURUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 32; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPJPY"); a.Descricao = "GBPJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 33; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPUSD"); a.Descricao = "GBPUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 34; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDCAD"); a.Descricao = "AUDCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 35; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDNOK"); a.Descricao = "USDNOK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 36; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDTRY"); a.Descricao = "USDTRY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 37; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NZDUSD"); a.Descricao = "NZDUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 38; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AMAZON"); a.Descricao = "AMAZON"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 39; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "APPLE"); a.Descricao = "APPLE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 40; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GOOGLE"); a.Descricao = "GOOGLE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 41; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MSFT"); a.Descricao = "MSFT"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 42; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GS"); a.Descricao = "GS"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 43; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ALIBABA"); a.Descricao = "ALIBABA"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 44; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TESLA"); a.Descricao = "TESLA"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 45; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BAIDU"); a.Descricao = "BAIDU"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 46; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FACEBOOK"); a.Descricao = "FACEBOOK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 47; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "INTEL"); a.Descricao = "INTEL"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 48; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CITI"); a.Descricao = "CITI"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 49; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "COKE"); a.Descricao = "COKE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 50; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "JPM"); a.Descricao = "JPM"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 51; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MCDON"); a.Descricao = "MCDON"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 52; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MORSTAN"); a.Descricao = "MORSTAN"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 53; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NIKE"); a.Descricao = "NIKE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 54; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "YANDEX"); a.Descricao = "YANDEX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 55; a.AtivoCategoria = aBinarias; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TWITTER"); a.Descricao = "TWITTER"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });


                realm.Write(() => { var a = new AtivosStatus(); a.Id = 56; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURGBP-OTC"); a.Descricao = "EURGBP-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 57; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURJPY"); a.Descricao = "EURJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 58; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NZDUSD-OTC"); a.Descricao = "NZDUSD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 59; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURCHF"); a.Descricao = "EURCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 60; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURGBP"); a.Descricao = "EURGBP"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 61; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCHF"); a.Descricao = "USDCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 62; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDCAD-OTC"); a.Descricao = "AUDCAD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 63; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPNZD"); a.Descricao = "GBPNZD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 64; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCHF-OTC"); a.Descricao = "USDCHF-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 65; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDUSD"); a.Descricao = "AUDUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 66; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPCAD"); a.Descricao = "GBPCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 67; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPCHF"); a.Descricao = "GBPCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 68; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CHFJPY"); a.Descricao = "CHFJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 69; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPAUD"); a.Descricao = "GBPAUD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 70; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURCAD"); a.Descricao = "EURCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 71; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CADCHF"); a.Descricao = "CADCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 72; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURAUD"); a.Descricao = "EURAUD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 73; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURNZD"); a.Descricao = "EURNZD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 74; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPUSD-OTC"); a.Descricao = "GBPUSD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 75; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCAD"); a.Descricao = "USDCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 76; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDJPY"); a.Descricao = "AUDJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 77; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURUSD-OTC"); a.Descricao = "EURUSD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 78; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CADJPY"); a.Descricao = "CADJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 79; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURUSD"); a.Descricao = "EURUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 80; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPJPY"); a.Descricao = "GBPJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 81; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPUSD"); a.Descricao = "GBPUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 82; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDJPY"); a.Descricao = "USDJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 83; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDCAD"); a.Descricao = "AUDCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 84; a.AtivoCategoria = aTurbo; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NZDUSD"); a.Descricao = "NZDUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });

                realm.Write(() => { var a = new AtivosStatus(); a.Id = 85; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURUSD"); a.Descricao = "EURUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 86; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURGBP"); a.Descricao = "EURGBP"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 87; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPJPY"); a.Descricao = "GBPJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 88; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURJPY"); a.Descricao = "EURJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 89; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPUSD"); a.Descricao = "GBPUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 90; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDJPY"); a.Descricao = "USDJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 91; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDCAD"); a.Descricao = "AUDCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 92; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURUSD-OTC"); a.Descricao = "EURUSD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 93; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURGBP-OTC"); a.Descricao = "EURGBP-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 94; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCHF-OTC"); a.Descricao = "USDCHF-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 95; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURJPY-OTC"); a.Descricao = "EURJPY-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 96; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NZDUSD-OTC"); a.Descricao = "NZDUSD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 97; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPUSD-OTC"); a.Descricao = "GBPUSD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 98; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPJPY-OTC"); a.Descricao = "GBPJPY-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 99; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDJPY-OTC"); a.Descricao = "USDJPY-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 100; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDCAD-OTC"); a.Descricao = "AUDCAD-OTC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 101; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDUSD"); a.Descricao = "AUDUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 102; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCAD"); a.Descricao = "USDCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 103; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDJPY"); a.Descricao = "AUDJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 104; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPCAD"); a.Descricao = "GBPCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 105; a.AtivoCategoria = aDigital; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPAUD"); a.Descricao = "GBPAUD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });

                realm.Write(() => { var a = new AtivosStatus(); a.Id = 106; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MELI"); a.Descricao = "MELI"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 107; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FSLR:US"); a.Descricao = "FSLR:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 108; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "PFE:US"); a.Descricao = "PFE:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 109; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EMN:US"); a.Descricao = "EMN:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 110; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "JPM"); a.Descricao = "JPM"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 111; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "QID"); a.Descricao = "QID"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 112; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GOOGLE"); a.Descricao = "GOOGLE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 113; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "HSBAL-CHIX"); a.Descricao = "HSBAL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 114; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DVA:US"); a.Descricao = "DVA:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 115; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XLU"); a.Descricao = "XLU"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 116; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "PIRCM-CHIX"); a.Descricao = "PIRCM-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 117; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BUD"); a.Descricao = "BUD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 118; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FRT:US"); a.Descricao = "FRT:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 119; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CBKD-CHIX"); a.Descricao = "CBKD-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 120; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AMGN:US"); a.Descricao = "AMGN:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 121; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XAGUSD"); a.Descricao = "XAGUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 122; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "RMD"); a.Descricao = "RMD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 123; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EFX:US"); a.Descricao = "EFX:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 124; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DLR:US"); a.Descricao = "DLR:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 125; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ADBE:US"); a.Descricao = "ADBE:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 126; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ADSK:US"); a.Descricao = "ADSK:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 127; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BARCL-CHIX"); a.Descricao = "BARCL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 128; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CGC"); a.Descricao = "CGC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 129; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SPOT"); a.Descricao = "SPOT"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 130; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SQQQ"); a.Descricao = "SQQQ"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 131; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "STANL-CHIX"); a.Descricao = "STANL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 132; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ALIBABA"); a.Descricao = "ALIBABA"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 133; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TITM-CHIX"); a.Descricao = "TITM-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 134; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FCX:US"); a.Descricao = "FCX:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 135; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ZM"); a.Descricao = "ZM"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 136; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DAL:US"); a.Descricao = "DAL:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 137; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TFC"); a.Descricao = "TFC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 138; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XLB"); a.Descricao = "XLB"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 139; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AMAZON"); a.Descricao = "AMAZON"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 140; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BYND"); a.Descricao = "BYND"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 141; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SMH"); a.Descricao = "SMH"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 142; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SPY"); a.Descricao = "SPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 143; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CAT:US"); a.Descricao = "CAT:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 144; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "COP:US"); a.Descricao = "COP:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 145; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FLS:US"); a.Descricao = "FLS:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 146; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GS"); a.Descricao = "GS"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 147; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EIX:US"); a.Descricao = "EIX:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 148; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USOUSD"); a.Descricao = "USOUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 149; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EWW"); a.Descricao = "EWW"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 150; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GE"); a.Descricao = "GE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 151; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MU:US"); a.Descricao = "MU:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 152; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TEFE-CHIX"); a.Descricao = "TEFE-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 153; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "PG:US"); a.Descricao = "PG:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 154; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ACN:US"); a.Descricao = "ACN:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 155; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "COST:US"); a.Descricao = "COST:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 156; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ENIM-CHIX"); a.Descricao = "ENIM-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 157; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MDT:US"); a.Descricao = "MDT:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 158; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EEM"); a.Descricao = "EEM"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 159; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EXPD:US"); a.Descricao = "EXPD:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 160; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FACEBOOK"); a.Descricao = "FACEBOOK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 161; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TESLA"); a.Descricao = "TESLA"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 162; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MRKD-CHIX"); a.Descricao = "MRKD-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 163; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FE:US"); a.Descricao = "FE:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 164; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MO:US"); a.Descricao = "MO:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 165; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "IEMG"); a.Descricao = "IEMG"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 166; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BAYND-CHIX"); a.Descricao = "BAYND-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 167; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "UCGM-CHIX"); a.Descricao = "UCGM-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 168; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TLRY"); a.Descricao = "TLRY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 169; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "COND-CHIX"); a.Descricao = "COND-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 170; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "QCOM:US"); a.Descricao = "QCOM:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 171; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "VODL-CHIX"); a.Descricao = "VODL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 172; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "V:US"); a.Descricao = "V:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 173; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "T:US"); a.Descricao = "T:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 174; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TFX"); a.Descricao = "TFX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 175; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TWITTER"); a.Descricao = "TWITTER"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 176; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "RBSL-CHIX"); a.Descricao = "RBSL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 177; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XOP"); a.Descricao = "XOP"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 178; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SDS"); a.Descricao = "SDS"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 179; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DPWD-CHIX"); a.Descricao = "DPWD-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 180; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GM"); a.Descricao = "GM"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 181; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BAC:US"); a.Descricao = "BAC:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 182; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NIKE"); a.Descricao = "NIKE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 183; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DRI:US"); a.Descricao = "DRI:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 184; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DFS:US"); a.Descricao = "DFS:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 185; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EMR:US"); a.Descricao = "EMR:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 186; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AAP:US"); a.Descricao = "AAP:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 187; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "JUVEM"); a.Descricao = "JUVEM"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 188; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DRE"); a.Descricao = "DRE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 189; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "RRL-CHIX"); a.Descricao = "RRL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 190; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DIS:US"); a.Descricao = "DIS:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 191; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CITI"); a.Descricao = "CITI"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 192; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CISCO"); a.Descricao = "CISCO"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 193; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "APPLE"); a.Descricao = "APPLE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 194; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EWJ"); a.Descricao = "EWJ"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 195; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FFIV:US"); a.Descricao = "FFIV:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 196; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "KHC:US"); a.Descricao = "KHC:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 197; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BATSL-CHIX"); a.Descricao = "BATSL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 198; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ENELM-CHIX"); a.Descricao = "ENELM-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 199; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NVDA:US"); a.Descricao = "NVDA:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 200; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EOAND-CHIX"); a.Descricao = "EOAND-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 201; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CTXS:US"); a.Descricao = "CTXS:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 202; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BBVAE-CHIX"); a.Descricao = "BBVAE-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 203; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "PM:US"); a.Descricao = "PM:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 204; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ED:US"); a.Descricao = "ED:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 205; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NFLX:US"); a.Descricao = "NFLX:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 206; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ALVD-CHIX"); a.Descricao = "ALVD-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 207; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SLB:US"); a.Descricao = "SLB:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 208; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XLY"); a.Descricao = "XLY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 210; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MJ"); a.Descricao = "MJ"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 211; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ATVI:US"); a.Descricao = "ATVI:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 212; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "HAS:US"); a.Descricao = "HAS:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 213; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BMY:US"); a.Descricao = "BMY:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 214; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EQR:US"); a.Descricao = "EQR:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 215; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SU"); a.Descricao = "SU"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 216; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CTAS:US"); a.Descricao = "CTAS:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 217; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MORSTAN"); a.Descricao = "MORSTAN"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 218; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DVN:US"); a.Descricao = "DVN:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 219; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XME"); a.Descricao = "XME"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 220; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FDJP"); a.Descricao = "FDJP"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 221; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BA:US"); a.Descricao = "BA:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 222; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GWPH"); a.Descricao = "GWPH"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 223; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XLK"); a.Descricao = "XLK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 224; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DBX"); a.Descricao = "DBX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 225; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FIS:US"); a.Descricao = "FIS:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 226; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MMM:US"); a.Descricao = "MMM:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 227; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DAID-CHIX"); a.Descricao = "DAID-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 228; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BAIDU"); a.Descricao = "BAIDU"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 229; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "WMT:US"); a.Descricao = "WMT:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 230; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XOM:US"); a.Descricao = "XOM:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 231; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "RTN:US"); a.Descricao = "RTN:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 232; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EBAY:US"); a.Descricao = "EBAY:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 233; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DISCA:US"); a.Descricao = "DISCA:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 234; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MSFT"); a.Descricao = "MSFT"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 235; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CVX:US"); a.Descricao = "CVX:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 236; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "WORK"); a.Descricao = "WORK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 237; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "LLOYL-CHIX"); a.Descricao = "LLOYL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 238; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TLT"); a.Descricao = "TLT"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 239; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ADSD-CHIX"); a.Descricao = "ADSD-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 240; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FXI"); a.Descricao = "FXI"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 241; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "IBM:US"); a.Descricao = "IBM:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 242; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "IDXX"); a.Descricao = "IDXX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 243; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AIRP-CHIX"); a.Descricao = "AIRP-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 244; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ABBV:US"); a.Descricao = "ABBV:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 245; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CXO:US"); a.Descricao = "CXO:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 246; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "VZ:US"); a.Descricao = "VZ:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 247; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ULVRL-CHIX"); a.Descricao = "ULVRL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 248; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "LMT:US"); a.Descricao = "LMT:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 249; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NEE:US"); a.Descricao = "NEE:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 250; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "VIAC"); a.Descricao = "VIAC"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 251; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ETR:US"); a.Descricao = "ETR:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 252; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FCAM-CHIX"); a.Descricao = "FCAM-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 253; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "QQQ"); a.Descricao = "QQQ"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 254; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DTE:US"); a.Descricao = "DTE:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 255; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ACB"); a.Descricao = "ACB"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 256; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TSCOL-CHIX"); a.Descricao = "TSCOL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 257; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CMCSA:US"); a.Descricao = "CMCSA:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 258; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DHR:US"); a.Descricao = "DHR:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 259; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ALGN"); a.Descricao = "ALGN"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 260; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "VOW3D-CHIX"); a.Descricao = "VOW3D-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 261; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FISV:US"); a.Descricao = "FISV:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 262; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ORP-CHIX"); a.Descricao = "ORP-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 263; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DHI:US"); a.Descricao = "DHI:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 264; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DTED-CHIX"); a.Descricao = "DTED-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 265; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GDX"); a.Descricao = "GDX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 266; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XAUUSD"); a.Descricao = "XAUUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 267; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EXR:US"); a.Descricao = "EXR:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 268; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FERRARI"); a.Descricao = "FERRARI"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 269; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "IYR"); a.Descricao = "IYR"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 270; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DOV:US"); a.Descricao = "DOV:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 271; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "HON:US"); a.Descricao = "HON:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 272; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MKSL-CHIX"); a.Descricao = "MKSL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 273; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MANU"); a.Descricao = "MANU"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 274; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SANE-CHIX"); a.Descricao = "SANE-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 275; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ABT:US"); a.Descricao = "ABT:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 276; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "UBER"); a.Descricao = "UBER"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 277; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FMC:US"); a.Descricao = "FMC:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 278; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SNAP"); a.Descricao = "SNAP"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 279; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TKAD-CHIX"); a.Descricao = "TKAD-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 280; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FBHS:US"); a.Descricao = "FBHS:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 281; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "UKOUSD"); a.Descricao = "UKOUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 282; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "COKE"); a.Descricao = "COKE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 283; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ESS:US"); a.Descricao = "ESS:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 284; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MRWL-CHIX"); a.Descricao = "MRWL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 285; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GILD:US"); a.Descricao = "GILD:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 286; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MA:US"); a.Descricao = "MA:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 287; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "LYFT"); a.Descricao = "LYFT"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 288; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CTL:US"); a.Descricao = "CTL:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 289; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DBKD-CHIX"); a.Descricao = "DBKD-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 290; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TMUS"); a.Descricao = "TMUS"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 291; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "MCDON"); a.Descricao = "MCDON"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 292; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "INTEL"); a.Descricao = "INTEL"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 293; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "FITB:US"); a.Descricao = "FITB:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 294; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BBY:US"); a.Descricao = "BBY:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 295; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EQT:US"); a.Descricao = "EQT:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 296; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BPL-CHIX"); a.Descricao = "BPL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 297; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ETFC:US"); a.Descricao = "ETFC:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 298; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SYK:US"); a.Descricao = "SYK:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 299; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "PSTM-CHIX"); a.Descricao = "PSTM-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 300; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DIA"); a.Descricao = "DIA"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 301; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BMWD-CHIX"); a.Descricao = "BMWD-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 302; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "PYPL:US"); a.Descricao = "PYPL:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 303; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AA:US"); a.Descricao = "AA:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 304; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CVS:US"); a.Descricao = "CVS:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 305; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "IWM"); a.Descricao = "IWM"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 306; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CRON"); a.Descricao = "CRON"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 307; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "WBA:US"); a.Descricao = "WBA:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 308; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ANSS"); a.Descricao = "ANSS"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 309; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XLE"); a.Descricao = "XLE"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 310; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ECL:US"); a.Descricao = "ECL:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 311; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SBUX:US"); a.Descricao = "SBUX:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 312; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "HEIOA-CHIX"); a.Descricao = "HEIOA-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 313; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "PINS"); a.Descricao = "PINS"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 314; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DGX:US"); a.Descricao = "DGX:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 315; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EZJL-CHIX"); a.Descricao = "EZJL-CHIX"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 316; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AMD"); a.Descricao = "AMD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 317; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CL:US"); a.Descricao = "CL:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 318; a.AtivoCategoria = aCfd; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CRM:US"); a.Descricao = "CRM:US"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });

                realm.Write(() => { var a = new AtivosStatus(); a.Id = 319; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCAD"); a.Descricao = "USDCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 320; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURSGD"); a.Descricao = "EURSGD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 321; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURJPY"); a.Descricao = "EURJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 322; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CADCHF"); a.Descricao = "CADCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 323; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDNOK"); a.Descricao = "USDNOK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                //realm.Write(() => { var a = new AtivosStatus(); a.Id = 324; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURSEK"); a.Descricao = "EURSEK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 325; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDDKK"); a.Descricao = "USDDKK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 326; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NZDUSD"); a.Descricao = "NZDUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 327; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURCHF"); a.Descricao = "EURCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 328; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDTHB"); a.Descricao = "USDTHB"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 329; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPAUD"); a.Descricao = "GBPAUD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 330; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDUSD"); a.Descricao = "AUDUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 331; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURMXN"); a.Descricao = "EURMXN"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 332; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCHF"); a.Descricao = "USDCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 333; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURNOK"); a.Descricao = "EURNOK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 334; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "SEKJPY"); a.Descricao = "SEKJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 335; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CADNOK"); a.Descricao = "CADNOK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 336; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NOKSEK"); a.Descricao = "NOKSEK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 337; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDNZD"); a.Descricao = "AUDNZD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 338; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPJPY"); a.Descricao = "GBPJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 339; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDNOK"); a.Descricao = "AUDNOK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 340; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPNOK"); a.Descricao = "GBPNOK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 341; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURAUD"); a.Descricao = "EURAUD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 342; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDCHF"); a.Descricao = "AUDCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 343; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPCHF"); a.Descricao = "GBPCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 344; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDCAD"); a.Descricao = "AUDCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 345; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NZDCHF"); a.Descricao = "NZDCHF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 346; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURZAR"); a.Descricao = "EURZAR"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 347; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDSEK"); a.Descricao = "USDSEK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 348; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURDKK"); a.Descricao = "EURDKK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 349; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURGBP"); a.Descricao = "EURGBP"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 350; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURCAD"); a.Descricao = "EURCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 351; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCZK"); a.Descricao = "USDCZK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 352; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURNZD"); a.Descricao = "EURNZD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 353; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDSGD"); a.Descricao = "AUDSGD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 354; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NZDCAD"); a.Descricao = "NZDCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 355; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CHFSGD"); a.Descricao = "CHFSGD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 356; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDMXN"); a.Descricao = "USDMXN"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 357; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURHUF"); a.Descricao = "EURHUF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 358; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPCAD"); a.Descricao = "GBPCAD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 359; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDTRY"); a.Descricao = "USDTRY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 360; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDJPY"); a.Descricao = "USDJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 361; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EURUSD"); a.Descricao = "EURUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 362; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDSEK"); a.Descricao = "AUDSEK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 363; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CHFNOK"); a.Descricao = "CHFNOK"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 364; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDPLN"); a.Descricao = "USDPLN"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                //realm.Write(() => { var a = new AtivosStatus(); a.Id = 365; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDCNH"); a.Descricao = "USDCNH"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 366; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDHUF"); a.Descricao = "USDHUF"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 367; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CHFJPY"); a.Descricao = "CHFJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 368; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPILS"); a.Descricao = "GBPILS"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 369; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NZDJPY"); a.Descricao = "NZDJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 370; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CADJPY"); a.Descricao = "CADJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 371; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "USDRUB"); a.Descricao = "USDRUB"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 372; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "GBPUSD"); a.Descricao = "GBPUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 373; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "CADPLN"); a.Descricao = "CADPLN"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 374; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NZDSGD"); a.Descricao = "NZDSGD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 375; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "AUDJPY"); a.Descricao = "AUDJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 376; a.AtivoCategoria = aForex; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NOKJPY"); a.Descricao = "NOKJPY"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });

                realm.Write(() => { var a = new AtivosStatus(); a.Id = 377; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XEMUSD-L"); a.Descricao = "XEMUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 378; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ETHUSD"); a.Descricao = "ETHUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 379; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "IOTUSD-L"); a.Descricao = "IOTUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 380; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EOSUSD-L"); a.Descricao = "EOSUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 381; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "QTMUSD"); a.Descricao = "QTMUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 382; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "EOSUSD"); a.Descricao = "EOSUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 383; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BCHUSD-L"); a.Descricao = "BCHUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 384; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ATOMUSD-L"); a.Descricao = "ATOMUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 385; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BTCUSD"); a.Descricao = "BTCUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 386; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TRXUSD"); a.Descricao = "TRXUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 387; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ONTUSD-L"); a.Descricao = "ONTUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 388; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XRPUSD-L"); a.Descricao = "XRPUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 389; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XLMUSD-L"); a.Descricao = "XLMUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 390; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ETHUSD-L"); a.Descricao = "ETHUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 391; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ZECUSD"); a.Descricao = "ZECUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 392; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "LTCUSD"); a.Descricao = "LTCUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 393; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BCHUSD"); a.Descricao = "BCHUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 394; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "NEOUSD-L"); a.Descricao = "NEOUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 395; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BTCUSD-L"); a.Descricao = "BTCUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 396; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "OMGUSD"); a.Descricao = "OMGUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 397; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BSVUSD-L"); a.Descricao = "BSVUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 398; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "TRXUSD-L"); a.Descricao = "TRXUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 399; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ADAUSD-L"); a.Descricao = "ADAUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 400; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "DSHUSD"); a.Descricao = "DSHUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 401; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "LTCUSD-L"); a.Descricao = "LTCUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 402; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "XRPUSD"); a.Descricao = "XRPUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 403; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "BNBUSD-L"); a.Descricao = "BNBUSD-L"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });
                realm.Write(() => { var a = new AtivosStatus(); a.Id = 404; a.AtivoCategoria = aCrypto; a.Ativo = realm.All<Ativos>().First(d => d.Descricao == "ETCUSD"); a.Descricao = "ETCUSD"; a.Aberto = true; a.Alvo = false; a.Payout = 0; realm.Add(a); });

            }
            if (usuariosconfiguracao.Count() == 0)
            {
                realm.Write(() => { 
                    var a = new UsuariosConfiguracao(); 
                    a.Id = 1; 
                    a.ValorEntrada = 2;
                    a.SorosNivel = 0;
                    a.StopWinRegra = "Percentual";
                    a.StopWin = 10;
                    a.StopLossRegra = "Percentual";
                    a.StopLoss = 10;
                    a.FiltroTopTraders = 20;
                    a.ValorMinimo = 50;
                    a.DelaySinal = 3;
                    a.PayoyutMinimo = 80;
                    a.HorarioInicio = "00:00:00";
                    a.HorarioFim = "00:00:00";
                    a.Ambiente = "Treinamento";
                    a.MtgRegra = "Fator";
                    a.MtgNivel = 0;
                    a.MtgFator = 0;
                    a.MtgFixo01 = 0;
                    a.MtgFixo02 = 0;
                    a.MtgFixo03 = 0;
                    a.MtgFixo04 = 0;
                    a.MtgFixo05 = 0;
                    a.MtgFixo06 = 0;
                    a.MtgFixo07 = 0;
                    a.MtgFixo08 = 0;
                    a.MtgFixo09 = 0;
                    a.MtgFixo10 = 0;
                    realm.Add(a); 
                });
            }

        }
    }
}
