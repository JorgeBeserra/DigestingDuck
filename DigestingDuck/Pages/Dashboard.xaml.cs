using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace DigestingDuck.Pages
{
    /// <summary>
    /// Interação lógica para Dashboard.xam
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            AtualizarBarra();
        }

        private void PageDashboard_Loaded(object sender, RoutedEventArgs e)
        {
            // Montando barras de Progresso
            lblBarStopWin.Content = ((MainWindow)Application.Current.MainWindow).config.StopLoss.ToString("N2", CultureInfo.CurrentCulture);
            lblBarStopLoss.Content = ((MainWindow)Application.Current.MainWindow).config.StopLoss.ToString("N2", CultureInfo.CurrentCulture);
            lblBarTotal.Content = ((MainWindow)Application.Current.MainWindow).SaldoTotalMonetario.ToString();
        }

        public void AtualizarBarra()
        {
            ((MainWindow)Application.Current.MainWindow).SaldoTotalMonetario = ((MainWindow)Application.Current.MainWindow).SaldoAtual - ((MainWindow)Application.Current.MainWindow).SaldoInicial;

            lblBarTotal.Content = ((MainWindow)Application.Current.MainWindow).SaldoTotalMonetario.ToString();

            if (((MainWindow)Application.Current.MainWindow).SaldoTotalMonetario < 0)
            {
                barWin.Value = 0;
                double saldoTotalMonetarioPositivo = ((MainWindow)Application.Current.MainWindow).SaldoTotalMonetario * -1;
                barLoss.Value = (saldoTotalMonetarioPositivo / ((MainWindow)Application.Current.MainWindow).config.StopLoss) * 100;
            }

            if (((MainWindow)Application.Current.MainWindow).SaldoTotalMonetario == 0)
            {
                barWin.Value = 0;
                barLoss.Value = 0;
            }

            if (((MainWindow)Application.Current.MainWindow).SaldoTotalMonetario > 0)
            {
                barLoss.Value = 0;
                barWin.Value = (((MainWindow)Application.Current.MainWindow).SaldoTotalMonetario / ((MainWindow)Application.Current.MainWindow).config.StopWin) * 100;
            }
        }
    }
}
