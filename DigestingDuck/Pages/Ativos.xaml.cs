using DigestingDuck.models;
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
using System.Windows.Shapes;

namespace DigestingDuck.Pages
{
    /// <summary>
    /// Lógica interna para Ativos.xaml
    /// </summary>
    public partial class Ativos : Page
    {
        public Ativos()
        {
            InitializeComponent();
        }

        private void PageAtivos_Loaded(object sender, RoutedEventArgs e)
        {
            datagridAtivosBinaria.ItemsSource = ((MainWindow)Application.Current.MainWindow).ativosBinaria;
            datagridAtivosTurbo.ItemsSource = ((MainWindow)Application.Current.MainWindow).ativosTurbo;
            datagridAtivosDigital.ItemsSource = ((MainWindow)Application.Current.MainWindow).ativosDigital;
        }

        private void DatagridAtivosBinaria_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = (DataGrid)sender;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {

                        var value = ((AtivosStatus)grid.SelectedItem);

                        var t = ((MainWindow)Application.Current.MainWindow).ativosBinaria.Where(x => x.Id == value.Id).First();

                        if (value.Alvo == true)
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Alvo = false; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("O ativo " + value.Descricao + " [" + value.Id + "] da categoria BINARIA foi REMOVIDO dos alvo.");
                        }
                        else
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Alvo = true; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("O ativo " + value.Descricao + " [" + value.Id + "] da categoria BINARIA foi MARCADO como alvo.");
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

        private void DatagridAtivosTurbo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = (DataGrid)sender;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {

                        var value = ((AtivosStatus)grid.SelectedItem);

                        var t = ((MainWindow)Application.Current.MainWindow).ativosTurbo.Where(x => x.Id == value.Id).First();

                        if (value.Alvo == true)
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Alvo = false; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("O ativo " + value.Descricao + " [" + value.Id + "] da categoria TURBO foi REMOVIDO dos alvo.");
                        }
                        else
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Alvo = true; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("O ativo " + value.Descricao + " [" + value.Id + "] da categoria TURBO foi MARCADO como alvo.");
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

        private void DatagridAtivosBinaria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DatagridAtivosDigital_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = (DataGrid)sender;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {

                        var value = ((AtivosStatus)grid.SelectedItem);

                        var t = ((MainWindow)Application.Current.MainWindow).ativosDigital.Where(x => x.Id == value.Id).First();

                        if (value.Alvo == true)
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Alvo = false; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("O ativo " + value.Descricao + " [" + value.Id + "] da categoria DIGITAL foi REMOVIDO dos alvo.");
                        }
                        else
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Alvo = true; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("O ativo " + value.Descricao + " [" + value.Id + "] da categoria DIGITAL foi MARCADO como alvo.");
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


    }
}
