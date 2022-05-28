using System;
using System.Data;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DigestingDuck.Models;
using IqOptionApiDotNet;
using IqOptionApiDotNet.Models;
using IqOptionApiDotNet.Models.DigitalOptions;

using Realms.Exceptions;
using MongoDB.Bson;

namespace DigestingDuck.Tasks
{
    public class Sinais : Runner
    {
        private const string Message = "sinais";
        private readonly IqOptionApiDotNetClient IqClientApi;
        private readonly IProgress<string> progresso;
        readonly Database database = new Database();

        public Sinais(IProgress<string> progress, IqOptionApiDotNetClient api)
        {
            IqClientApi = api;          
            progresso = progress;
        }

        public override async Task Run()
        {
            var clientBug = new Bugsnag.Client(Bugsnag.ConfigurationSection.Configuration.Settings);

            database.AbrirDB();

            progresso.Report("Iniciando!!!");

            try
            {
                
                if (IqClientApi.IsConnected)
                {
                    // Ouvindo o saldo...
                    IqClientApi.BalanceChangedObservable.Subscribe(x =>
                    {
                        //Console.WriteLine(x.CurrentBalance.NewAmount);
                    });

                    IqClientApi.WsClient.PlaceDigitalOptionResultObservable.Subscribe(x=>
                    {
                        Console.WriteLine("Chegou o Resultado!!: " + x.Id);
                    });

                    IqClientApi.WsClient.PositionChangedObservable().Subscribe(x =>
                    {
                        if(x.Status == "closed")
                        {
                            long OrderId = x.PortfolioChangedEventInfo.OrderIds[0]; // Aqui depois tem que fazer um COUNT dos IDS todos
                            string Resultado = "I";
                            if (x.Pnl > 0)
                            {
                                Resultado = "WIN";
                            }
                            else if (x.Pnl < 0)
                            {
                                Resultado = "LOSS";
                            }
                            else if (x.Pnl == 0)
                            {
                                Resultado = "EMPATE";
                            }
                            Database db2 = new Database();
                            db2.AbrirDB();
                            var tradersCopia = db2.realm.All<TraderCopia>().Where(q => q.PosicaoIdNovo == x.PortfolioChangedEventInfo.OrderIds[0]).First();
                            db2.realm.Write(() =>
                            {
                                tradersCopia.Resultado = Resultado;
                                tradersCopia.LucroTreinamento = x.Pnl;
                            });

                            Console.WriteLine("Dados Salvos");

                            progresso.Report("------------------ [RESULTADO] ----------------");
                            progresso.Report("Id: " + x.UserId);
                            progresso.Report("Usuário: " + tradersCopia.Trader.UserName);
                            progresso.Report("Resultado: " + Resultado);
                            progresso.Report("Lucro: " + x.Pnl);
                            progresso.Report("------------------------------------------------");
                            
                        }
                        
                        
                    });

                    var inicioData = new DateTimeOffset(2022, 5, 26,  0,  0,  0,  0, TimeSpan.Zero);
                    var fimData =    new DateTimeOffset(2022, 5, 26, 23, 59, 59, 0, TimeSpan.Zero);

                    var aDigital = database.realm.All<SinaisSinal>().Where(x => x.DataHora >= inicioData && x.DataHora <= fimData && x.Status == true);
                    progresso.Report("---------------[PRÓXIMOS SINAIS]--------------------");

                    string ativo;

                    SleepToTarget Temp;

                    foreach (SinaisSinal i in aDigital)
                    {
                        ativo = "";
                        foreach (var a in i.Ativos)
                        {
                            ativo =  a.Descricao;
                        }
                        progresso.Report("Ativo: " + ativo);
                        progresso.Report("Horário: " + i.DataHora);
                        progresso.Report("Expiração: " + i.ExpirationType);

                        Temp = new SleepToTarget(i.DataHora.DateTime,  Done, i.Id, progresso, IqClientApi);
                        Temp.Start();
                    };

                    progresso.Report("------------------------------------------------");

                }

            }
            catch (Exception ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro:" + ex);
            }

            progresso.Report("Processo de Sinais Iniciado!!!");

        }

        static void Done(IProgress<string> progresso, IqOptionApiDotNetClient iq, ObjectId s)
        {
            Database db = new Database();
            db.AbrirDB();
            var cfg = db.realm.All<UsuarioConfiguracao>().First();
            var e = db.realm.All<SinaisSinal>().Where(q => q.Id == s).Single();

            InstrumentType tipo = InstrumentType.DigitalOption;
            
            OrderDirection direction = OrderDirection.Call;
            if (e.InstrumentDir.ToString() == "call")
            {
                direction = OrderDirection.Call;
            }
            else if (e.InstrumentDir.ToString() == "put")
            {
                direction = OrderDirection.Put;
            }

            DigitalOptionsExpiryDuration expiration = DigitalOptionsExpiryDuration.M1;

            if (e.ExpirationType.ToString() == "PT1M")
            {
                expiration = DigitalOptionsExpiryDuration.M1;
            }
            else if (e.ExpirationType.ToString() == "PT5M")
            {
                expiration = DigitalOptionsExpiryDuration.M5;
            }
            else if (e.ExpirationType.ToString() == "PT15M")
            {
                expiration = DigitalOptionsExpiryDuration.M15;
            }

            double ValorEntrada;

            if (cfg.ValorEntrada == 0)
            {
                ValorEntrada = 2;
            }
            else
            {
                ValorEntrada = cfg.ValorEntrada;
            }

            Task.Run(() => new EntradaDigital(progresso, iq, ActivePair.EURUSD, 112, direction, expiration, ValorEntrada, 12).RunSample());
        }

    }

    class SleepToTarget
    {
        private DateTime TargetTime;
        private Action<IProgress<string>, IqOptionApiDotNetClient, ObjectId> MyAction;
        private IqOptionApiDotNetClient IqClientApi;
        private ObjectId MySinal;
        private IProgress<string> MyProgresso;
        private const int MinSleepMilliseconds = 250;

        public SleepToTarget(DateTime TargetTime, Action<IProgress<string>, IqOptionApiDotNetClient, ObjectId> MyAction, ObjectId s, IProgress<string> MyProgresso, IqOptionApiDotNetClient IqClientApi)
        {
            this.TargetTime = TargetTime;
            this.MyProgresso = MyProgresso;
            this.IqClientApi = IqClientApi;
            this.MyAction = MyAction;
            this.MySinal = s;
        }

        public void Start()
        {
            new Thread(new ThreadStart(ProcessTimer)).Start();
        }

        private void ProcessTimer()
        {
            DateTime Now = DateTime.Now;

            while (Now < TargetTime)
            {
                int SleepMilliseconds = (int)Math.Round((TargetTime - Now).TotalMilliseconds / 2);
                Console.WriteLine(SleepMilliseconds);
                Thread.Sleep(SleepMilliseconds > MinSleepMilliseconds ? SleepMilliseconds : MinSleepMilliseconds);
                Now = DateTime.Now;
            }

            MyAction(MyProgresso, IqClientApi, MySinal);
        }
    }
}