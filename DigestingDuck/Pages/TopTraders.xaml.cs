using DigestingDuck.Models;
using IqOptionApiDotNet.Models;
using Realms.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    /// Interação lógica para TopTraders.xam
    /// </summary>
    public partial class TopTraders : Page
    {
        private bool Load = false;
        public TopTraders()
        {
            InitializeComponent();
        }

        private void PageToptraders_Loaded(object sender, RoutedEventArgs e)
        {
            datagridTradersTop.ItemsSource = ((MainWindow)Application.Current.MainWindow).traderTop;
            BtnAdicionarID.IsEnabled = true;
            BtnAtualizarTraders.IsEnabled = true;

            // Carregando configuração Traders Alvo
            InputTotalTraders.Text = ((MainWindow)Application.Current.MainWindow).config.FiltroTopTraders.ToString();
            InputDelaySuportado.Text = ((MainWindow)Application.Current.MainWindow).config.DelaySinal.ToString();
            InputValorMinimoCopia.Text = ((MainWindow)Application.Current.MainWindow).config.ValorMinimo.ToString("N2", CultureInfo.CurrentCulture);
            Load = true;
        }

        private async void BtnAdicionarID_Click(object sender, RoutedEventArgs e)
        {
            if (!InputIdEspecifico.Text.IsNullOrEmpty())
            {
                long userid = Convert.ToInt32(InputIdEspecifico.Text);
                var requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                UserProfileClientResult UserProfile = await ((MainWindow)Application.Current.MainWindow).IqClientApiDotNet.GetUserProfileClientAsync(requestId, userid);
                Window1 w = new Window1(UserProfile);
                w.Show();
            }
            else
            {
                MessageBox.Show("Favor escrever um ID no campo [ID ESPECIFICO]");
            }
        }

        private void BtnAlvosLimpar_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).GerarTexto("Informação (Aguarde estamos removendo todos os TRADERS dos alvos)");
            var t = ((MainWindow)Application.Current.MainWindow).traderTop.Where(x => x.Alvo == true);
            foreach (var trader in t)
            {
                ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { trader.Alvo = false; });
            }
            ((MainWindow)Application.Current.MainWindow).GerarTexto("Todos os TRADERS foram removidos dos alvos.");
        }

        private void BtnAlvosTodos_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).GerarTexto("Informação (Aguarde estamos adicionando todos TRADERS como alvos)");
            var t = ((MainWindow)Application.Current.MainWindow).traderTop.Where(x => x.Alvo == false);
            foreach (var trader in t)
            {
                ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { trader.Alvo = true; });
            }
            ((MainWindow)Application.Current.MainWindow).GerarTexto("Todos os TRADERS foram marcados como alvos.");
        }

        private async void BtnAtualizarTraders_Click(object sender, RoutedEventArgs e)
        {
            var requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
            await ((MainWindow)Application.Current.MainWindow).IqClientApiDotNet.RequestLeaderboardDealsClientAsync(requestId, 0, 191, 100, CountryType.Worldwide, 10, 10, 10, 10, 2);
        }

        private void DatagridTradersTop_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = (DataGrid)sender;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {

                        var value = ((Trader)grid.SelectedItem);

                        var t = ((MainWindow)Application.Current.MainWindow).traderTop.Where(x => x.UserId == value.UserId).First();

                        if (value.Alvo == true)
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Alvo = false; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("O trader " + value.UserName + " [" + value.UserId + "] foi REMOVIDO dos alvo.");
                        }
                        else
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Alvo = true; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("O trader " + value.UserName + " [" + value.UserId + "] foi MARCADO como alvo.");
                        }

                        //grid.ItemsSource = null;

                        /*
                         * SQLITE
                         * 
                         *                         
                        int UserId = Convert.ToInt32(dr[7]);
                        String UserName = dr[3].ToString();

                        string sql = "SELECT user_id FROM traders_alvos WHERE user_id = @id;";
                        var cmd = new SQLiteCommand(database.meuSQLITE);
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", UserId);
                        cmd.Prepare();
                        SQLiteDataReader data = cmd.ExecuteReader();
                        */

                        //var traders = database.realm.All<DigestingDuck.models.Traders>().Where(x=>x.UserId == value.UserId).First();
                        //Console.WriteLine(value.UserName);
                        try
                        {



                            //if(value.Alvo == true)
                            //{
                            /*
                            using (var trans = database.realm.BeginWrite())
                            {
                                traders.Alvo = false;
                                trans.Commit();
                            }
                            */
                            //foreach
                            //database.realm.Write(() => { value.Alvo = false; });
                            //}
                            //else
                            //{
                            /*
                            using (var trans = database.realm.BeginWrite())
                            {
                                traders.Alvo = true;
                                trans.Commit();
                            }
                            */
                            //database.realm.Write(() => { value.Alvo = true; });
                            //}


                            /*
                            string sqlInserir = "INSERT INTO traders_alvos (user_id) VALUES (@id)";
                            var cmdInserir = new SQLiteCommand(database.meuSQLITE);
                            cmdInserir.CommandText = sqlInserir;
                            cmdInserir.Parameters.AddWithValue("@id", value.UserId);
                            cmdInserir.Prepare();
                            int linha = cmdInserir.ExecuteNonQuery();
                            */
                            /*
                            if(linha > 0)
                            {
                                gerarTexto("O trader " + value.UserName + " [" + value.UserId + "] foi marcado como alvo.");
                                AtualizarTabelaTradersTop();
                                AtualizarTabelaTradersAlvos();
                            }
                            else
                            {
                                MessageBox.Show("Erro ao Inserir");
                            }
                            */
                        }
                        catch (NullReferenceException ex)
                        {
                            ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                            Console.WriteLine(ex.Message);
                        }
                    }



                }

            }
            catch (RealmException ex)
            {
                ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void InputDelaySuportado_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    int valor = Convert.ToInt32(tb.Text);
                    string dica = "";
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.DelaySinal = valor; });
                    if (valor > 3)
                    {
                        dica = "(Lembre-se o ideal é entre 0 e 3, mas fique a vontade para testar outros valores)";
                    }
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Tempo de atraso do sinal permitido alterado para " + tb.Text + ". " + dica);
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputValorMinimoCopia_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.ValorMinimo = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor MÍNIMO para entrada alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void BtnImportarCSV_Click(object sender, RoutedEventArgs e)
        {
            using (var reader = new StreamReader(@"D:\TradersTop.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    Console.WriteLine(values[0]);
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => {
                        var a = new Trader
                        {
                            UserId = Convert.ToInt64(values[0]),
                            DataCadastro = DateTimeOffset.Now,
                            DataAtualizacao = DateTimeOffset.Now,
                            UserName = values[4],
                            Avatar = values[11],
                            Flag = values[7],
                            Win = 0,
                            Loss = 0
                        };
                        if (values[2] == "1")
                        {
                            a.Alvo = true;
                        }
                        else
                        {
                            a.Alvo = false;
                        }
                        a.RankingValor = Convert.ToDouble(values[5]);
                        a.RankingPosicao = Convert.ToInt32(values[3]);
                        ((MainWindow)Application.Current.MainWindow).database.realm.Add(a);
                    });
                }
            }
        }


    }

}
