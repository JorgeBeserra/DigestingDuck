using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IqOptionApiDotNet;
using IqOptionApiDotNet.Models;
using System.Data;
using IqOptionApiDotNet.Models.DigitalOptions;
using Serilog;
using System.Globalization;
using System.Diagnostics;
using Realms;
using DigestingDuck.models;
using Realms.Exceptions;
using System.Configuration;
using System.Xml;

namespace DigestingDuck
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private bool Carregado = false;
        public IqOptionApiDotNetClient IqClientApiDotNet;
        public Profile profile;
        public Database database = new Database();
        

        public DataTable saldosTbl = new DataTable();

        public DataTable tradersTopTbl = new DataTable();
        public DataTable tradersAlvosTbl = new DataTable();
        readonly MediaPlayer Sound1 = new MediaPlayer();

        public IQueryable<Configuracao> configuracao;
        public Configuracao conf; // Configurações Gerais
        public IQueryable<Usuarios> usuarios;
        public UsuariosConfiguracao config; // Configurações do Usuario

        public IQueryable<AtivosStatus> ativosBinaria;
        public IQueryable<AtivosStatus> ativosTurbo;
        public IQueryable<AtivosStatus> ativosDigital;
        public IQueryable<Traders> traderTop;
        public IQueryable<UsuariosSaldos> usuariosSaldos;

        public long strId = 1;
        public string strNome = "Sem Login";
        public string strEmail = "sabidos@sabidos.com.br";
        public string strPassword = "";
        public Bugsnag.Client clientBug;

        public double SaldoInicial = 0; // Saldo Inicial
        public double SaldoAtual = 0; // Saldo Total

        public double SaldoTotalMonetario = 0; // Soma de Lucros e Perdas
        public double SaldoTotalPercentual = 0; // Soma de Lucros e Perdas

        public string ResultadoUltimaEntrada = "";
        public int CountSoros = 0; // Quantas Entradas com Soros
        public int CountMartingale = 0; // Quantos Já foram

        readonly string Versao = GetAssemblyFileVersion();

        public Pages.Alertas pAlertas;
        public Pages.Ativos pAtivos;
        public Pages.Chat pChat;
        public Pages.Corretora pCorretora;
        public Pages.Dashboard pDashboard;
        public Pages.Gerenciamento pGerenciamento;
        public Pages.Portifolio pPortifolio;
        public Pages.Sinais pSinais;
        public Pages.TopTraders pTopTraders;

        bool Som = true;

        public MainWindow()
        {

            InitializeComponent();



            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            xmlDoc.SelectSingleNode("//bugsnag").Attributes["appVersion"].Value = Versao;
            if(Debugger.IsAttached)
            {
                xmlDoc.SelectSingleNode("//bugsnag").Attributes["releaseStage"].Value = "development";
                xmlDoc.SelectSingleNode("//bugsnag").Attributes["notifyReleaseStages"].Value = "development";
            }
            else
            {
                xmlDoc.SelectSingleNode("//bugsnag").Attributes["releaseStage"].Value = "production";
                xmlDoc.SelectSingleNode("//bugsnag").Attributes["notifyReleaseStages"].Value = "production";
            }

            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection("//bugsnag");

            clientBug = new Bugsnag.Client(Bugsnag.ConfigurationSection.Configuration.Settings);

            SoundDealLoose.Source = new Uri("Sounds/sound_deal_loose.mp3", UriKind.Relative);
            SoundDealLooseNotification.Source = new Uri("Sounds/sound_deal_loose_notification.mp3", UriKind.Relative);
            SoundDealWin.Source = new Uri("Sounds/sound_deal_win.mp3", UriKind.Relative);
            SoundMakeDeal.Source = new Uri("Sounds/sound_make_deal.mp3", UriKind.Relative);

            database.AbrirDB();
            database.AbrirConexao();

            this.Title = "Digesting Duck - " + Versao;

            clientBug.BeforeNotify((report) => {
                
                report.Event.User = new Bugsnag.Payload.User
                {
                    Id = strId.ToString(),
                    Name = strNome,
                    Email = strEmail
                };
            });

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pAlertas = (Pages.Alertas)this.DataContext;
            pAtivos = (Pages.Ativos)this.DataContext;
            pChat = (Pages.Chat)this.DataContext;
            pCorretora = (Pages.Corretora)this.DataContext;
            pDashboard = (Pages.Dashboard)this.DataContext;
            pGerenciamento = (Pages.Gerenciamento)this.DataContext;
            pPortifolio = (Pages.Portifolio)this.DataContext;
            pSinais = (Pages.Sinais)this.DataContext;
            pTopTraders = (Pages.TopTraders)this.DataContext;

            MainWindowFrame.Content = new Pages.Corretora();

            try
            {
                configuracao = database.realm.All<Configuracao>();

                if (!configuracao.Any())
                {
                    database.realm.Write(() => {
                        var a = new Configuracao
                        {
                            Id = 1,
                            UltimaExecucao = DateTimeOffset.Now,
                            UltimoUsuario = 0,
                            LembrarDados = false
                        };
                        database.realm.Add(a);
                    });
                }
                else
                {
                    conf = configuracao.First();
                    if (conf.LembrarDados == true)
                    {
                        pCorretora.ChkLembrarDados.IsChecked = true;
                        usuarios = database.realm.All<Usuarios>().Where(u => u.Id == conf.UltimoUsuario);
                        if (usuarios.Any())
                        {
                            Usuarios usuario = usuarios.First();
                            pCorretora.InputEmail.Text = usuario.Email;
                            pCorretora.InputPassword.Password = usuario.Senha;
                        }

                    }
                    else
                    {
                        //pCorretora.ChkLembrarDados.IsChecked = false;
                    }
                }

            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex);
            }


            try
            {
                config = database.realm.All<UsuariosConfiguracao>().Where(p => p.Id == 1).First();
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex);
            }

            try
            {
                ativosBinaria = database.realm.All<AtivosStatus>().Filter("AtivoCategoria.Id = 1");
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex.Message);
            }

            try
            {
                ativosTurbo = database.realm.All<AtivosStatus>().Filter("AtivoCategoria.Id = 2");
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex);
            }

            try
            {
                ativosDigital = database.realm.All<AtivosStatus>().Filter("AtivoCategoria.Id = 3");
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex);
            }

            try
            {
                traderTop = database.realm.All<Traders>();
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex);
            }

            Carregado = true;
        }

        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Play();
            }
            try
            {
                var progress = new Progress<string>(s => GerarTexto(s));
                // Falta criar as demais task....
                Task.Run(() => new Subscribes(progress, IqClientApiDotNet).Run());
                Console.ReadLine();
                BtnIniciar.IsEnabled = false;
                BtnParar.IsEnabled = true;
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro:" + ex);
            }
        }

        private async void RadioButtonAmbiente_Checked(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Play();
            }
            var button = sender as RadioButton;

            if (button.Content.ToString() == "Real")
            {
                try
                {
                    var real = profile.Balances.FirstOrDefault(x => x.Type == BalanceType.Real);
                    string requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    var result = await IqClientApiDotNet.ChangeBalanceAsync(requestId, real.Id);
                    System.Threading.Thread.Sleep(2000);
                    AtualizarProfile();
                }
                catch (RealmException ex)
                {
                    clientBug.Notify(ex);
                    MessageBox.Show("Erro:" + ex);
                }
            }

            if (button.Content.ToString() == "Treinamento")
            {
                try
                {
                    var demo = profile.Balances.FirstOrDefault(x => x.Type == BalanceType.Practice);
                    string requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    await IqClientApiDotNet.ChangeBalanceAsync(requestId, demo.Id);
                    System.Threading.Thread.Sleep(2000);
                    AtualizarProfile();
                }
                catch (RealmException ex)
                {
                    clientBug.Notify(ex);
                    MessageBox.Show("Erro:" + ex);
                }
            }
        }

        private async void AtualizarProfile()
        {
            string requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
            profile = await IqClientApiDotNet.GetProfileAsync(requestId);
            LblSaldoAtual.Content = profile.Balance;
        }

        

        private void BtnParar_Click(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Play();
            }
            try
            {
                //Task.Run(() => new UnsubscribeLiveDealSample(IqClientApiDotNet).RunSample());                
                BtnIniciar.IsEnabled = true;
                BtnParar.IsEnabled = false;
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro:" + ex);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //database.FecharConexao();
            /*
            if (IqClientApi.IsConnected)
            {
                //Task.Run(() => new UnsubscribeLiveDealSample(IqClientApi, ativosAlvosTbl).RunSample());
            }
            */
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var position = await IqClientApiDotNet.PlaceDigitalOptions(requestId, ActivePair.EURUSD, OrderDirection.Call, DigitalOptionsExpiryDuration.M1, 1);

            Console.WriteLine($"Placed position Id: {position.Id}");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            logTexto.AppendText(DateTimeOffset.Now + "\n");
            logTexto.AppendText("-------------- COMPRA -----------------" + "\n");
            logTexto.AppendText("Usuário: " + "\n");
            logTexto.AppendText("Id: " + "\n");
            //logTexto.AppendText("Nível Mundial: " + "\n");
            //logTexto.AppendText("This weeks gross profit:18231.2" + "\n");
            logTexto.AppendText("Montante:"  + " Direção: " + "\n");
            logTexto.AppendText("Expiração: " + "\n");
            logTexto.AppendText("Atraso do Sinal: " + "\n");
            logTexto.AppendText("Ativo: " + "\n");
            logTexto.AppendText("Sua Entrada" + "\n");
        }

        private void LogTexto_TextChanged(object sender, TextChangedEventArgs e)
        {
            logTexto.ScrollToEnd();
        }

        public void GerarTexto(string texto)
        {
            var dateFormat = "dd/MM/yyyy HH:mm:ss";
            logTexto.AppendText("[" + DateTimeOffset.Now.ToString(dateFormat) + "] - " + texto + "\n");
        }

        public static string GetAssemblyFileVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersion.FileVersion;
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var t = traderTop.Where(x => x.UserId == 1086525).First();
            MessageBox.Show("Agora 2 esta: " + t.Alvo);
            database.realm.Write(() => { t.Alvo = false; });
            MessageBox.Show("Agora 3 esta: " + t.Alvo);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }

        private void UserChip_DeleteClick(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                SoundMakeDeal.Play();
            }
            this.Close();
        }

        private void BtnCorretora_Click(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                SoundMakeDeal.Play();
            }
            MainWindowFrame.Content = new Pages.Corretora();
        }

        private void BtnPortifolio_Click(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                SoundMakeDeal.Play();
            }
            MainWindowFrame.Content = new Pages.Portifolio();
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                SoundMakeDeal.Play();
            }
            MainWindowFrame.Content = new Pages.Dashboard();
        }

        private void BtnAtivos_Click(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                SoundMakeDeal.Play();
            }
            MainWindowFrame.Content = new Pages.Ativos();
        }

        private void BtnGerenciamento_Click(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                SoundMakeDeal.Play();
            }
            MainWindowFrame.Content = new Pages.Gerenciamento();
        }

        private void BtnTopTraders_Click(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                SoundMakeDeal.Play();
            }
            MainWindowFrame.Content = new Pages.TopTraders();
        }

        private void BtnSinais_Click(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                SoundMakeDeal.Play();
            }
            MainWindowFrame.Content = new Pages.Sinais();
        }

        private void BtnAlerts_Click(object sender, RoutedEventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                SoundMakeDeal.Play();
            }
            MainWindowFrame.Content = new Pages.Alertas();
        }

        
    }
}