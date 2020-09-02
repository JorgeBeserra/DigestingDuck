using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DigestingDuck.models;
using IqOptionApiDotNet;
using IqOptionApiDotNet.Models;
using IqOptionApiDotNet.Models.DigitalOptions;

namespace DigestingDuck
{
    public class EntradaDigital //: SampleRunner
    {
        private IqOptionApiDotNetClient IqClientApi;
        private ActivePair ActiveId;
        private long PositionIdOrigem;
        private OrderDirection InstrumentDir;
        private DigitalOptionsExpiryDuration ExpirationType;
        private double AmountEnrolled;
        private IProgress<string> progresso;
        private long TraderIdCopia;

        public EntradaDigital(IProgress<string> progress, IqOptionApiDotNetClient api, ActivePair activeId, long positionIdOrigem,OrderDirection instrumentDir, DigitalOptionsExpiryDuration expirationType, Double amountEnrolled, long traderIdCopia)
        {
            IqClientApi = api;
            ActiveId = activeId;
            PositionIdOrigem = positionIdOrigem;
            InstrumentDir = instrumentDir;
            ExpirationType = expirationType;
            AmountEnrolled = amountEnrolled;
            progresso = progress;
            TraderIdCopia = traderIdCopia;
        }

        public async Task RunSample()
        {
            var clientBug = new Bugsnag.Client(Bugsnag.ConfigurationSection.Configuration.Settings);

            var requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);

            // Realizando Entrada
            Debug.WriteLine("Enviando Entrada - pt1");
            var position = await IqClientApi.PlaceDigitalOptions(requestId, ActiveId, InstrumentDir, ExpirationType, AmountEnrolled);

            Debug.WriteLine("Enviando Entrada - pt2");
            progresso.Report("----------- [Sua Entrada] ----------");
            progresso.Report("Ativo: " + ActiveId );
            Debug.WriteLine("Enviando Entrada - pt3");
            progresso.Report("Direção: " + InstrumentDir);
            Debug.WriteLine("Enviando Entrada - pt4");
            progresso.Report("Posição ID Origem: " + position.Id );
            Debug.WriteLine("Enviando Entrada - pt5");
            progresso.Report("Expiração: " + ExpirationType );
            Debug.WriteLine("Enviando Entrada - pt6");
            progresso.Report("Valor da Entrada: " + AmountEnrolled);
            Debug.WriteLine("Enviando Entrada - pt7");
            progresso.Report("---------------------------");

            Debug.WriteLine($"Entrada posição Id: {position.Id}");
            Debug.WriteLine("Enviando Entrada - pt8");
            try
            {
                Database db1 = new Database();
                db1.AbrirDB();
                var trader = db1.realm.All<Traders>().First(d => d.UserId == TraderIdCopia);

                db1.realm.Write(() =>
                {
                    var a = new TradersCopias();
                    a.Id = requestId;
                    // a.UserId = Convert.ToInt64(UserProfile.UserId);
                    a.Trader = trader;
                    a.DataHora = DateTimeOffset.Now;
                    // a.Ativo = DateTimeOffset.Now;
                    a.PosicaoIdOrigem = PositionIdOrigem;
                    a.PosicaoIdNovo = Convert.ToInt64(position.Id);
                    a.InstrumentDir = InstrumentDir.ToString();
                    a.AmountEnrolled = AmountEnrolled;
                    a.ExpirationType = ExpirationType.ToString();
                    a.Resultado = "I";
                    a.LucroReal = 0;
                    a.LucroTreinamento = 0;
                    db1.realm.Add(a);
                });
            }
            catch(Realms.Exceptions.RealmException ex)
            {
                clientBug.Notify(ex);
                Console.WriteLine("Erro: " + ex);
            }
        }

    }
}