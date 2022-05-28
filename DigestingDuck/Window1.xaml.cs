using DigestingDuck.Models;
using IqOptionApiDotNet.Models;
using Realms.Exceptions;
using System;
using System.Windows;

namespace DigestingDuck
{
    /// <summary>
    /// Lógica interna para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        readonly Database database = new Database();
        private readonly UserProfileClientResult UserProfile;
        public Window1(UserProfileClientResult user)
        {
            InitializeComponent();
            UserProfile = user;
            InputUserId.Text = UserProfile.UserId.ToString();
            InputUserName.Text = UserProfile.UserName.ToString();
            InputPais.Text = UserProfile.Flag;
            LblStatus.Content = UserProfile.Status;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            database.AbrirDB();

            try
            {
                database.realm.Write(() => {
                    var a = new Trader();
                    a.UserId = Convert.ToInt64(UserProfile.UserId);
                    a.DataCadastro = DateTimeOffset.Now;
                    a.DataAtualizacao = DateTimeOffset.Now;
                    a.UserName = UserProfile.UserName;
                    a.Avatar = UserProfile.ImgUrl;
                    a.Flag = UserProfile.Flag;
                    a.Win = 0;
                    a.Loss = 0;
                    a.Alvo = false;
                    a.RankingValor = 0;
                    a.RankingPosicao = 0;
                    database.realm.Add(a);
                });
                MessageBox.Show("Inserido com sucesso!");
            }
            catch(RealmException ex)
            {
                MessageBox.Show("Erro: " + ex);
            }

            this.Close();
            
            
        }

    }
}
