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

using System.Data;
using System.Globalization;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using IqOptionApiDotNet;
using IqOptionApiDotNet.Models;
using IqOptionApiDotNet.Models.DigitalOptions;

using Realms;
using Realms.Exceptions;


using DigestingDuck.Models;

namespace DigestingDuck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool Carregado = false;
        public IqOptionApiDotNetClient IqClientApiDotNet;
        public Profile profile;
        public IEnumerable<Balance> balance;

        public Database database = new Database();


        public DataTable saldosTbl = new DataTable();

        public DataTable tradersTopTbl = new DataTable();
        public DataTable tradersAlvosTbl = new DataTable();

        public IQueryable<Configuracao> configuracao;
        public Configuracao conf; // Configurações Gerais
        public IQueryable<Usuario> usuarios;
        public UsuarioConfiguracao config; // Configurações do Usuario

        public IQueryable<AtivoStatus> ativosBinaria;
        public IQueryable<AtivoStatus> ativosTurbo;
        public IQueryable<AtivoStatus> ativosDigital;
        public IQueryable<Trader> traderTop;
        public IQueryable<UsuarioSaldo> usuariosSaldos;

        public IQueryable<SinaisLista> sinaisListas;
        public IQueryable<SinaisSinal> sinaisSinais;

        public long strId = 1;
        public string strNome = "Sem Login";
        public string strEmail = "sabidos@sabidos.com.br";
        public string strPassword = "";
        public Bugsnag.Client clientBug;

        public string tipoAmbienteSelecionado = "Treinamento"; // Saldo Inicial
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

        public bool Som = true;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        public MainWindow()
        {
            database.AbrirDB();
            database.AbrirConexao();

            configuracao = database.realm.All<Configuracao>();

            pAlertas = (Pages.Alertas)this.DataContext;
            pAtivos = (Pages.Ativos)this.DataContext;
            pChat = (Pages.Chat)this.DataContext;
            pCorretora = (Pages.Corretora)this.DataContext;
            pDashboard = (Pages.Dashboard)this.DataContext;
            pGerenciamento = (Pages.Gerenciamento)this.DataContext;
            pPortifolio = (Pages.Portifolio)this.DataContext;
            pSinais = (Pages.Sinais)this.DataContext;
            pTopTraders = (Pages.TopTraders)this.DataContext;

            InitializeComponent();         
           
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath);

            xmlDoc.SelectSingleNode("//bugsnag").Attributes["appVersion"].Value = Versao;
            if (Debugger.IsAttached)
            {
                xmlDoc.SelectSingleNode("//bugsnag").Attributes["releaseStage"].Value = "development";
                xmlDoc.SelectSingleNode("//bugsnag").Attributes["notifyReleaseStages"].Value = "development";
            }
            else
            {
                xmlDoc.SelectSingleNode("//bugsnag").Attributes["releaseStage"].Value = "production";
                xmlDoc.SelectSingleNode("//bugsnag").Attributes["notifyReleaseStages"].Value = "production";
            }

            xmlDoc.Save(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath);

            ConfigurationManager.RefreshSection("//bugsnag");

            clientBug = new Bugsnag.Client(Bugsnag.ConfigurationSection.Configuration.Settings);

            SoundDealLoose.Source = new Uri("Sounds/sound_deal_loose.mp3", UriKind.Relative);
            SoundDealLooseNotification.Source = new Uri("Sounds/sound_deal_loose_notification.mp3", UriKind.Relative);
            SoundDealWin.Source = new Uri("Sounds/sound_deal_win.mp3", UriKind.Relative);
            SoundMakeDeal.Source = new Uri("Sounds/sound_make_deal.mp3", UriKind.Relative);



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
            MainWindowFrame.Content = new Pages.Corretora();

            /*try
            {
                

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
            }*/


            try
            {
                config = database.realm.All<UsuarioConfiguracao>().Where(p => p.Id == 1).First();
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex);
            }

            try
            {
                ativosBinaria = database.realm.All<AtivoStatus>().Filter("AtivoCategoria.Id = 1");
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex.Message);
            }

            try
            {
                ativosTurbo = database.realm.All<AtivoStatus>().Filter("AtivoCategoria.Id = 2");
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex);
            }

            try
            {
                ativosDigital = database.realm.All<AtivoStatus>().Filter("AtivoCategoria.Id = 3");
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex);
            }

            try
            {
                traderTop = database.realm.All<Trader>();
            }
            catch (RealmException ex)
            {
                clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex);
            }

            try
            {
                sinaisListas = database.realm.All<SinaisLista>();
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
                Task.Run(() => new Tasks.Subscribes(progress, IqClientApiDotNet).Run());
                Task.Run(() => new Tasks.Sinais(progress, IqClientApiDotNet).Run());
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
                    var real = profile.profileResult.Balances.FirstOrDefault(x => x.Type == BalanceType.Real);
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
                    var demo = profile.profileResult.Balances.FirstOrDefault(x => x.Type == BalanceType.Practice);
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
            LblSaldoAtual.Content = profile.profileResult.Balance;
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
            logTexto.AppendText("Montante:" + " Direção: " + "\n");
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

        private void tipoAmbiente_DropDownClosed(object sender, EventArgs e)
        {
            if (Som)
            {
                SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                SoundMakeDeal.Play();
            }

            if (tipoAmbiente.SelectedItem != null)
            {
                if (tipoAmbiente.Text == "Real")
                {
                    tipoAmbienteSelecionado = "Real";
                }
                else
                {
                    tipoAmbienteSelecionado = "Treinamento";
                }
            }

            BalanceType tipoSaldo;

            if (tipoAmbienteSelecionado == "Real")
            {
                tipoSaldo = BalanceType.Real;
                lblSaldo.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(44, 172, 64));
            }
            else
            {
                tipoSaldo = BalanceType.Practice;
                lblSaldo.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(225, 119, 0));
            }

            if ((balance != null) && (balance.Any()))
            {
                IEnumerable<Balance> query = balance.Where(x => x.Type == tipoSaldo);

                foreach (Balance balance in query)
                {
                    lblSaldo.Content = Convert.ToDouble(balance.Amount);
                }
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /* 
             * Rotina para Maximizar a Janela, 
             * porém tem que organizar o GRID 
             * pois nos primeiros teste ficou tudo fora de Ordem
             * 
            if(this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
            */
        }

        private void tipoAmbiente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
