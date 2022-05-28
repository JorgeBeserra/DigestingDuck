using DigestingDuck.Models;
using IqOptionApiDotNet;
using IqOptionApiDotNet.Models;
using Realms.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
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
using WebSocketSharp;

namespace DigestingDuck.Pages
{
    /// <summary>
    /// Interação lógica para Corretora.xam
    /// </summary>
    public partial class Corretora : Page
    {
        public Corretora()
        {
            InitializeComponent();
            
            try
            {
                if ((((MainWindow)Application.Current.MainWindow).configuracao != null) && (!((MainWindow)Application.Current.MainWindow).configuracao.Any()))
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => {
                        var a = new Configuracao
                        {
                            Id = 1,
                            UltimaExecucao = DateTimeOffset.Now,
                            UltimoUsuario = 0,
                            LembrarDados = false
                        };
                        ((MainWindow)Application.Current.MainWindow).database.realm.Add(a);
                    });
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).conf = ((MainWindow)Application.Current.MainWindow).configuracao.First();
                    if (((MainWindow)Application.Current.MainWindow).conf.LembrarDados == true)
                    {
                        ChkLembrarDados.IsChecked = true;
                        ((MainWindow)Application.Current.MainWindow).usuarios = ((MainWindow)Application.Current.MainWindow).database.realm.All<Usuario>().Where(u => u.Id == ((MainWindow)Application.Current.MainWindow).conf.UltimoUsuario);
                        if (((MainWindow)Application.Current.MainWindow).usuarios.Any())
                        {
                            Usuario usuario = ((MainWindow)Application.Current.MainWindow).usuarios.First();
                            InputEmail.Text = usuario.Email;
                            InputPassword.Password = usuario.Senha;
                        }

                    }
                    else
                    {
                        ChkLembrarDados.IsChecked = false;
                    }
                }

            }
            catch (RealmException ex)
            {
                ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                MessageBox.Show("Erro: " + ex);
            }
            
        }

        private async void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            if (((MainWindow)Application.Current.MainWindow).Som)
            {
                ((MainWindow)Application.Current.MainWindow).SoundMakeDeal.Position = TimeSpan.FromMilliseconds(0);
                ((MainWindow)Application.Current.MainWindow).SoundMakeDeal.Play();
            }

            if (InputEmail.Text.IsNullOrEmpty() || InputPassword.Password.IsNullOrEmpty())
            {
                MessageBox.Show("Favor preencher USUARIO e SENHA");
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).strNome = InputEmail.Text;
                ((MainWindow)Application.Current.MainWindow).strEmail = InputEmail.Text;
                ((MainWindow)Application.Current.MainWindow).strPassword = InputPassword.Password;

                ((MainWindow)Application.Current.MainWindow).IqClientApiDotNet = new IqOptionApiDotNetClient(InputEmail.Text, InputPassword.Password);

                await ((MainWindow)Application.Current.MainWindow).IqClientApiDotNet.ConnectAsync();

                if (((MainWindow)Application.Current.MainWindow).IqClientApiDotNet.IsConnected)
                {

                    if (ChkLembrarDados.IsChecked == true)
                    {
                        ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).conf.LembrarDados = true; });
                    }
                    else
                    {
                        ((MainWindow)Application.Current.MainWindow).conf.LembrarDados = true;
                    }
                    
                    ((MainWindow)Application.Current.MainWindow).LblSituacao.Content = "Conectado";
                    BtnEntrar.IsEnabled = false;
                    ((MainWindow)Application.Current.MainWindow).BtnIniciar.IsEnabled = true;
                    
                    string requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    ((MainWindow)Application.Current.MainWindow).profile = await ((MainWindow)Application.Current.MainWindow).IqClientApiDotNet.GetProfileAsync(requestId);
                    requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    UserProfileClientResult profileCompleto = await ((MainWindow)Application.Current.MainWindow).IqClientApiDotNet.GetUserProfileClientAsync(requestId, ((MainWindow)Application.Current.MainWindow).profile.profileResult.UserId);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(profileCompleto.ImgUrl, UriKind.Absolute);
                    bitmap.EndInit();

                    requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);

                    BalanceType[] balanceType = new BalanceType[] { BalanceType.Real, BalanceType.Practice };

                    ((MainWindow)Application.Current.MainWindow).balance = await ((MainWindow)Application.Current.MainWindow).IqClientApiDotNet.GetBalancesAsync(requestId, balanceType);

                    BalanceType tipoSaldo;

                    if(((MainWindow)Application.Current.MainWindow).tipoAmbienteSelecionado == "Treinamento")
                    {
                        tipoSaldo = BalanceType.Practice;
                    }
                    else
                    {
                        tipoSaldo = BalanceType.Real;
                    }

                    IEnumerable<Balance> query = ((MainWindow)Application.Current.MainWindow).balance.Where(x => x.Type == tipoSaldo);

                    foreach(Balance balance in query)
                    {
                        ((MainWindow)Application.Current.MainWindow).lblSaldo.Content = Convert.ToDouble(balance.Amount);
                    }

                    ((MainWindow)Application.Current.MainWindow).tipoAmbiente.IsEnabled = true;

                    ((MainWindow)Application.Current.MainWindow).ChipImage.Source = bitmap;

                    ((MainWindow)Application.Current.MainWindow).strId = ((MainWindow)Application.Current.MainWindow).profile.profileResult.Id;

                    ((MainWindow)Application.Current.MainWindow).UserChip.Content = ((MainWindow)Application.Current.MainWindow).profile.profileResult.Name;
                                        
                    ((MainWindow)Application.Current.MainWindow).LblSaldoAtual.Content = ((MainWindow)Application.Current.MainWindow).profile.profileResult.Balance;

                    ((MainWindow)Application.Current.MainWindow).SaldoAtual = Convert.ToDouble(((MainWindow)Application.Current.MainWindow).profile.profileResult.Balance);

                    ((MainWindow)Application.Current.MainWindow).usuarios = ((MainWindow)Application.Current.MainWindow).database.realm.All<Usuario>().Where(u => u.Email == ((MainWindow)Application.Current.MainWindow).strEmail);
                    
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).conf.UltimoUsuario = ((MainWindow)Application.Current.MainWindow).profile.profileResult.Id; });

                    if (((MainWindow)Application.Current.MainWindow).usuarios.Count() > 0)
                    {
                        if (ChkLembrarDados.IsChecked == true)
                        {
                            Usuario usuario = ((MainWindow)Application.Current.MainWindow).usuarios.First();
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).conf.LembrarDados = true; });
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => {
                                usuario.Senha = ((MainWindow)Application.Current.MainWindow).strPassword;
                                usuario.Avatar = ((MainWindow)Application.Current.MainWindow).profile.profileResult.Avatar;
                                usuario.UltimoAcessoDataHora = DateTimeOffset.Now;
                            });
                        }
                        else
                        {
                            Usuario usuario = ((MainWindow)Application.Current.MainWindow).usuarios.First();
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).conf.LembrarDados = false; });
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => {
                                usuario.Senha = "";
                                usuario.Avatar = "";
                                usuario.UltimoAcessoDataHora = DateTimeOffset.Now;
                            });
                        }
                    }
                    else
                    {
                        if (ChkLembrarDados.IsChecked == true)
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).conf.LembrarDados = true; });
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => {
                                var a = new Usuario
                                {
                                    Id = ((MainWindow)Application.Current.MainWindow).profile.profileResult.Id,
                                    Email = ((MainWindow)Application.Current.MainWindow).strEmail,
                                    Senha = ((MainWindow)Application.Current.MainWindow).strPassword,
                                    Avatar = ((MainWindow)Application.Current.MainWindow).profile.profileResult.Avatar,
                                    UltimoAcessoDataHora = DateTimeOffset.Now
                                };
                                ((MainWindow)Application.Current.MainWindow).database.realm.Add(a);
                            });
                        }
                        else
                        {
                            try
                            {
                                ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).conf.LembrarDados = false; });
                                ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => {
                                    var a = new Usuario
                                    {
                                        Id = ((MainWindow)Application.Current.MainWindow).profile.profileResult.Id,
                                        Email = "",
                                        Senha = "",
                                        Avatar = "",
                                        UltimoAcessoDataHora = DateTimeOffset.Now
                                    };
                                    ((MainWindow)Application.Current.MainWindow).database.realm.Add(a);
                                });
                            }
                            catch (RealmException ex)
                            {
                                ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                                MessageBox.Show("Erro: " + ex);
                            }
                        }
                    }

                    Usuario user = ((MainWindow)Application.Current.MainWindow).usuarios.First();

                    var dayStart = DateTimeOffset.UtcNow.Date;

                    ((MainWindow)Application.Current.MainWindow).usuariosSaldos = ((MainWindow)Application.Current.MainWindow).database.realm.All<UsuarioSaldo>().Where(u => u.Usuario == user && u.Data >= dayStart);

                    ((MainWindow)Application.Current.MainWindow).usuariosSaldos = ((MainWindow)Application.Current.MainWindow).database.realm.All<UsuarioSaldo>().Where(u => u.Usuario == user && u.Data >= dayStart);

                    

                    if (((MainWindow)Application.Current.MainWindow).usuariosSaldos.Count() > 0)
                    {

                    }
                    else
                    {

                        ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => {

                            var a = new UsuarioSaldo
                            {
                                Id = Guid.NewGuid().ToString().Replace("-", string.Empty),
                                Usuario = user,
                                Data = DateTimeOffset.Now
                            };
                            if (((MainWindow)Application.Current.MainWindow).profile.profileResult.BalanceType == BalanceType.Practice)
                            {
                                a.SaldoTreinamento = Convert.ToDouble(((MainWindow)Application.Current.MainWindow).profile.profileResult.Balance);
                            }
                            else
                            {
                                a.SaldoTreinamento = 0;
                            }

                            if (((MainWindow)Application.Current.MainWindow).profile.profileResult.BalanceType == BalanceType.Real)
                            {
                                a.SaldoReal = Convert.ToDouble(((MainWindow)Application.Current.MainWindow).profile.profileResult.Balance);
                            }
                            else
                            {
                                a.SaldoReal = 0;
                            }

                            a.SaldoReal = 0;
                            ((MainWindow)Application.Current.MainWindow).database.realm.Add(a);
                        });
                    }

                    //((MainWindow)Application.Current.MainWindow).LblSaldoInicial.Content = ((MainWindow)Application.Current.MainWindow).usuariosSaldos.First().SaldoTreinamento;                    
                    //((MainWindow)Application.Current.MainWindow).SaldoInicial = ((MainWindow)Application.Current.MainWindow).usuariosSaldos.First().SaldoTreinamento;
                }

            }
        }

        private void PageCorretora_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
