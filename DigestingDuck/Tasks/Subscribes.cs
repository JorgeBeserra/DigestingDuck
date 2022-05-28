using System;
using System.Data;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using DigestingDuck.Models;
using IqOptionApiDotNet;
using IqOptionApiDotNet.Models;
using IqOptionApiDotNet.Models.DigitalOptions;

using Realms.Exceptions;

namespace DigestingDuck.Tasks
{
    public class Subscribes : Runner
    {
        private const string Message = "live-deal-digital-option";
        private readonly IqOptionApiDotNetClient IqClientApi;
        private readonly IProgress<string> progresso;
        readonly Database database = new Database();

        public Subscribes(IProgress<string> progress, IqOptionApiDotNetClient api)
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

                    var aDigital = database.realm.All<AtivoStatus>().Where(x => x.Aberto == true && x.Alvo == true);
                    foreach (var a in aDigital)
                    {

                        var requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                        IqClientApi.SubscribeLiveDeal(requestId, Message, (ActivePair)Convert.ToInt32(a.Ativo._id), DigitalOptionsExpiryType.PT1M);
                        requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                        IqClientApi.SubscribeLiveDeal(requestId, Message, (ActivePair)Convert.ToInt32(a.Ativo._id), DigitalOptionsExpiryType.PT5M);
                        requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                        IqClientApi.SubscribeLiveDeal(requestId, Message, (ActivePair)Convert.ToInt32(a.Ativo._id), DigitalOptionsExpiryType.PT15M);
                    }
                  
                    // Chamada observadora de recebimento de dados.
                    IqClientApi.WsClient.LiveDealObservable().Subscribe(x => {

                        Database db = new Database();
                        db.AbrirDB();

                        try
                        {
                            var trad = db.realm.All<Trader>().Where(q => q.Alvo == true && q.UserId == x.UserId);                          

                            var config = db.realm.All<UsuarioConfiguracao>().First();

                            TimeSpan difference;
                            difference = x.CreatedAt.Value.Subtract(DateTimeOffset.Now);
                            int segundos = difference.Seconds * -1;

                            var posicaoJaExiste = db.realm.All<TraderCopia>().Where(q => q.PosicaoIdOrigem == x.PositionId);
                            // Verificando Requisitos
                            // 1 - Trader tem que estar nos ALVOS
                            // 2 - O tempo de atraso tem que ser maior que o configurado
                            // 3 - O valor da entrada tem que ser maior que o configurado
                            // 4 - E a posicao nao pode já ter existido
                            if (trad.Count() > 0 && segundos < config.DelaySinal && x.AmountEnrolled >= config.ValorMinimo && posicaoJaExiste.Count() == 0)
                            {
                                    
                                    
                                        // values goes here
                                        _logger.Information(
                                            $"Realizar Posição: {x.PositionId} Entrada: {x.UserId} Created {x.CreatedAt} Amount: {x.AmountEnrolled} Direction: {x.InstrumentDir} Diferença de Tempo: {difference} Ativo: {x.ActiveId} Expiracao {x.ExpirationType}"
                                        );

                                        progresso.Report("---------- [TRADER ENTRADA DETECTADA] ----------");
                                        progresso.Report(DateTimeOffset.Now.ToString());
                                        progresso.Report("Posição: " + x.PositionId);
                                        progresso.Report("Usuário: " + x.Name);
                                        progresso.Report("Id: " + x.UserId);
                                        //progresso.Report("Nível Mundial: ");
                                        //progresso.Report("This weeks gross profit: ");
                                        progresso.Report("Montante: " + x.AmountEnrolled + " Direção: " + x.InstrumentDir);
                                        progresso.Report("Expiração: " + x.ExpirationType);
                                        progresso.Report("Atraso do Sinal: " + difference);
                                        //progresso.Report("Atraso do Sinal: " + difference.Seconds);
                                        //progresso.Report("Atraso do Sinal: " + segundos);
                                        progresso.Report("Ativo: " + x.ActiveId);
                                        progresso.Report("------------------------------------------------");

                                        OrderDirection direction = OrderDirection.Call;
                                        if (x.InstrumentDir.ToString() == "call")
                                        {
                                            direction = OrderDirection.Call;
                                        }
                                        else if (x.InstrumentDir.ToString() == "put")
                                        {
                                            direction = OrderDirection.Put;
                                        }

                                        DigitalOptionsExpiryDuration expiration = DigitalOptionsExpiryDuration.M1;

                                        if (x.ExpirationType.ToString() == "PT1M")
                                        {
                                            expiration = DigitalOptionsExpiryDuration.M1;
                                        }
                                        else if (x.ExpirationType.ToString() == "PT5M")
                                        {
                                            expiration = DigitalOptionsExpiryDuration.M5;
                                        }
                                        else if (x.ExpirationType.ToString() == "PT15M")
                                        {
                                            expiration = DigitalOptionsExpiryDuration.M15;
                                        }

                                        double ValorEntrada;

                                        if (config.ValorEntrada == 0)
                                        {
                                            ValorEntrada = x.AmountEnrolled;
                                        }
                                        else
                                        {
                                            ValorEntrada = config.ValorEntrada;
                                        }
                                        Console.WriteLine("Iniciando Task De entrada");
                                        Task.Run(() => new EntradaDigital(progresso, IqClientApi, x.ActiveId, x.PositionId, direction, expiration, ValorEntrada, x.UserId).RunSample());

                            }
                            
                        }
                        catch (RealmException ex)
                        {
                            clientBug.Notify(ex);
                            MessageBox.Show("Erro:" + ex);
                        }

                    });
                }

            }
            catch (Exception ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro:" + ex);
            }

            progresso.Report("Processo Iniciado!!!");

        }
    }
}