using Realms.Exceptions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;


namespace DigestingDuck.Pages
{
    /// <summary>
    /// Interação lógica para Gerenciamento.xam
    /// </summary>
    public partial class Gerenciamento : Page
    {
        private bool Load = false;
        public Gerenciamento()
        {
            InitializeComponent();
        }

        private void PageGerenciamento_Loaded(object sender, RoutedEventArgs e)
        {
            // Carregando Configurações da tela de GERENCIAMENTO (REALM)
            InputLucroAtingir.Text = ((MainWindow)Application.Current.MainWindow).config.StopLoss.ToString("N2", CultureInfo.CurrentCulture);
            InputPercasAtingir.Text = ((MainWindow)Application.Current.MainWindow).config.StopLoss.ToString("N2", CultureInfo.CurrentCulture);
            InputValorEntrada.Text = ((MainWindow)Application.Current.MainWindow).config.ValorEntrada.ToString("N2", CultureInfo.CurrentCulture);
            InputSorosNivel.Text = ((MainWindow)Application.Current.MainWindow).config.SorosNivel.ToString("N2", CultureInfo.CurrentCulture);
            InputMtgFatorEscala.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFator.ToString("N2", CultureInfo.CurrentCulture);
            InputMtgFatorNiveis.Text = ((MainWindow)Application.Current.MainWindow).config.MtgNivel.ToString("N2", CultureInfo.CurrentCulture);
            inputMtgEntrada01.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFixo01.ToString("N2", CultureInfo.CurrentCulture);
            inputMtgEntrada02.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFixo02.ToString("N2", CultureInfo.CurrentCulture);
            inputMtgEntrada03.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFixo03.ToString("N2", CultureInfo.CurrentCulture);
            inputMtgEntrada04.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFixo04.ToString("N2", CultureInfo.CurrentCulture);
            inputMtgEntrada05.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFixo05.ToString("N2", CultureInfo.CurrentCulture);
            inputMtgEntrada06.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFixo06.ToString("N2", CultureInfo.CurrentCulture);
            inputMtgEntrada07.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFixo07.ToString("N2", CultureInfo.CurrentCulture);
            inputMtgEntrada08.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFixo08.ToString("N2", CultureInfo.CurrentCulture);
            inputMtgEntrada09.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFixo09.ToString("N2", CultureInfo.CurrentCulture);
            inputMtgEntrada10.Text = ((MainWindow)Application.Current.MainWindow).config.MtgFixo10.ToString("N2", CultureInfo.CurrentCulture);
            Load = true;
        }

        private void InputValorEntrada_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.ValorEntrada = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor da entrada alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputLucroAtingir_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;

                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.StopWin = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor do STOP WIN alterado para " + tb.Text + ".");
                    //lblBarStopWin.Content = ((MainWindow)Application.Current.MainWindow).config.StopWin;
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }
            }
        }

        private void InputPercasAtingir_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.StopLoss = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor do STOP LOSS alterado para " + tb.Text + ".");
                    //lblBarStopLoss.Content = ((MainWindow)Application.Current.MainWindow).config.StopLoss;
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }
            }
        }

        private void InputSorosNivel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.SorosNivel = Convert.ToInt32(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor do nível de SOROS alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputMtgFatorEscala_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFator = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor do Fator de Martingale alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputMtgFatorNiveis_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgNivel = Convert.ToInt32(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor da quantidade de níveis do Martingale entrada alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputMtgEntrada01_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFixo01 = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor fixo para o Martingale 01 alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputMtgEntrada02_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFixo02 = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor fixo para o Martingale 02 alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }
            }
        }

        private void InputMtgEntrada03_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFixo03 = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor fixo para o Martingale 03 alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputMtgEntrada04_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFixo04 = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor fixo para o Martingale 04 alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputMtgEntrada05_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFixo05 = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor fixo para o Martingale 05 alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputMtgEntrada06_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFixo06 = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor fixo para o Martingale 06 alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputMtgEntrada07_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFixo07 = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor fixo para o Martingale 07 alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputMtgEntrada08_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFixo08 = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor fixo para o Martingale 08 alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        private void InputMtgEntrada09_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFixo09 = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor fixo para o Martingale 09 alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }
            }
        }

        private void InputMtgEntrada10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Load)
            {
                var tb = sender as TextBox;
                try
                {
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { ((MainWindow)Application.Current.MainWindow).config.MtgFixo10 = Convert.ToDouble(tb.Text); });
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Valor fixo para o Martingale 10 alterado para " + tb.Text + ".");
                }
                catch (RealmException ex)
                {
                    ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                    ((MainWindow)Application.Current.MainWindow).GerarTexto("Erro ao alterar configuração: " + ex);
                }

            }
        }

        
    }
}
