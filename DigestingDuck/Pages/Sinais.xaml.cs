using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

using DigestingDuck.Models;

using Realms.Exceptions;
using MongoDB.Bson;

namespace DigestingDuck.Pages
{
    /// <summary>
    /// Interação lógica para Sinais.xam
    /// </summary>
    public partial class Sinais : Page
    {
        private readonly IProgress<string> progresso;
        public Sinais()
        {
            InitializeComponent();
        }

        private void PageSinais_Loaded(object sender, RoutedEventArgs e)
        {
            datagridSinaisListas.ItemsSource = ((MainWindow)Application.Current.MainWindow).sinaisListas;
            datagridSinaisSinais.ItemsSource = ((MainWindow)Application.Current.MainWindow).sinaisSinais;
            //BtnAdicionarID.IsEnabled = true;
            //BtnAtualizarTraders.IsEnabled = true;
        }

        private void datagridSinaisListas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = (DataGrid)sender;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {

                        var value = ((SinaisLista)grid.SelectedItem);

                        var t = ((MainWindow)Application.Current.MainWindow).sinaisListas.Where(x => x.Id == value.Id).First();

                        if (value.Status == true)
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Status = false; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("A lista " + value.Descricao + " [" + value.Id + "] foi DESATIVADA.");
                        }
                        else
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Status = true; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("A lista " + value.Descricao + " [" + value.Id + "] foi ATIVADA.");
                        }

                    }

                }

                datagridSinaisListas.ItemsSource = null;
                datagridSinaisListas.ItemsSource = ((MainWindow)Application.Current.MainWindow).sinaisListas;

            }
            catch (RealmException ex)
            {
                ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void datagridSinaisListas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = (DataGrid)sender;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {

                        var value = ((SinaisLista)grid.SelectedItem);

                        var lista = ((MainWindow)Application.Current.MainWindow).database.realm.All<SinaisLista>().Single(l => l.Id == value.Id);

                        if (lista != null)
                        {                           
                            datagridSinaisSinais.ItemsSource = lista.ListOfSinais.ToList();
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

        private void datagridSinaisSinais_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = (DataGrid)sender;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {

                        var value = ((SinaisSinal)grid.SelectedItem);

                        ((MainWindow)Application.Current.MainWindow).sinaisSinais = ((MainWindow)Application.Current.MainWindow).database.realm.All<SinaisSinal>();

                        var t = ((MainWindow)Application.Current.MainWindow).sinaisSinais.Where(x => x.Id == value.Id).First();

                        if (value.Status == true)
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Status = false; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("O sinal " + value.InstrumentDir + " [" + value.Id + "] foi DESATIVADO.");
                        }
                        else
                        {
                            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => { t.Status = true; });
                            ((MainWindow)Application.Current.MainWindow).GerarTexto("A sinal " + value.InstrumentDir + " [" + value.Id + "] foi ATIVADO.");
                        }

                        datagridSinaisSinais.ItemsSource = null;
                        datagridSinaisSinais.ItemsSource = t.SinaisListas.Single().ListOfSinais.ToList();

                    }

                }



            }
            catch (RealmException ex)
            {
                ((MainWindow)Application.Current.MainWindow).clientBug.Notify(ex);
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).database.realm.Write(() =>
            {
                Ativo a;
                SinaisLista l;
                SinaisSinal s;
                a = ((MainWindow)Application.Current.MainWindow).database.realm.All<Ativo>().Single(a => a.Descricao == "EURUSD");
                l = ((MainWindow)Application.Current.MainWindow).database.realm.Add(new SinaisLista { Descricao = "Jorge", Status = false });

                var hour = 22;
                var mins = 22;
                var secs = 0;
                var ms = 0;

                DateTimeOffset turingsBirthday;

                turingsBirthday  = new DateTimeOffset(2022, 5, 26, hour, 5, secs, ms, TimeSpan.Zero);

                s = ((MainWindow)Application.Current.MainWindow).database.realm.Add(new SinaisSinal { DataHora = turingsBirthday, InstrumentDir = "PUT", ExpirationType = "M5" });
                
                l.ListOfSinais.Add(s);
                a.ListOfSinais.Add(s);

                turingsBirthday = new DateTimeOffset(2022, 5, 26, hour, 6, secs, ms, TimeSpan.Zero);

                s = ((MainWindow)Application.Current.MainWindow).database.realm.Add(new SinaisSinal {  DataHora = turingsBirthday, InstrumentDir = "PUT", ExpirationType = "M5" });
                l.ListOfSinais.Add(s);
                a.ListOfSinais.Add(s);

                turingsBirthday = new DateTimeOffset(2022, 5, 26, hour, 7, secs, ms, TimeSpan.Zero);

                s = ((MainWindow)Application.Current.MainWindow).database.realm.Add(new SinaisSinal {  DataHora = turingsBirthday, InstrumentDir = "CALL", ExpirationType = "M5" });
                l.ListOfSinais.Add(s);
                a.ListOfSinais.Add(s);


                a = ((MainWindow)Application.Current.MainWindow).database.realm.All<Ativo>().Single(a => a.Descricao == "EURGBP");
                l = ((MainWindow)Application.Current.MainWindow).database.realm.Add(new SinaisLista { Descricao = "Thiago", Status = false });

                s = ((MainWindow)Application.Current.MainWindow).database.realm.Add(new SinaisSinal { DataHora = DateTimeOffset.Now, InstrumentDir = "PUT", ExpirationType = "M5" });
                l.ListOfSinais.Add(s);
                a.ListOfSinais.Add(s);

                a = ((MainWindow)Application.Current.MainWindow).database.realm.All<Ativo>().Single(a => a.Descricao == "GBPJPY");
                s = ((MainWindow)Application.Current.MainWindow).database.realm.Add(new SinaisSinal { DataHora = DateTimeOffset.Now, InstrumentDir = "CALL", ExpirationType = "M5" });
                l.ListOfSinais.Add(s);
                a.ListOfSinais.Add(s);

                s = ((MainWindow)Application.Current.MainWindow).database.realm.Add(new SinaisSinal { DataHora = DateTimeOffset.Now, InstrumentDir = "CALL", ExpirationType = "M5" });
                l.ListOfSinais.Add(s);
                a.ListOfSinais.Add(s);
            });
        }

        private void BtnImportarCSV_Click(object sender, RoutedEventArgs e)
        {
            using (var reader = new StreamReader(@"D:\sinais.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    Console.WriteLine(values[0]);
                    ((MainWindow)Application.Current.MainWindow).database.realm.Write(() => {
                        var s = new SinaisSinal
                        {
                            /*UserId = Convert.ToInt64(values[0]),
                            DataCadastro = DateTimeOffset.Now,
                            DataAtualizacao = DateTimeOffset.Now,
                            UserName = values[4],
                            Avatar = values[11],
                            Flag = values[7],
                            Win = 0,
                            Loss = 0*/
                        };
                        if (values[2] == "1")
                        {
                            s.Status= true;
                        }
                        else
                        {
                            s.Status = false;
                        }
                        /*s.RankingValor = Convert.ToDouble(values[5]);
                        s.RankingPosicao = Convert.ToInt32(values[3]);*/
                        ((MainWindow)Application.Current.MainWindow).database.realm.Add(s);
                    });
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var inicioData = new DateTimeOffset(2022, 5, 26, 22, 0, 0, 0, TimeSpan.Zero);
            var fimData = new DateTimeOffset(2022, 5, 26, 23, 59, 59, 0, TimeSpan.Zero);

            var aDigital = ((MainWindow)Application.Current.MainWindow).database.realm.All<SinaisSinal>().Where(x => x.DataHora >= inicioData && x.DataHora <= fimData && x.Status == true);

            SleepToTarget Temp;

            foreach (SinaisSinal i in aDigital)
            {
                foreach (var a in i.Ativos)
                {
                    MessageBox.Show("Sinais" + a.Descricao);
                }

                MessageBox.Show("Sinais" + i.DataHora.DateTime);
                Temp = new SleepToTarget(i.DataHora.DateTime, Done, i.Id);
                Temp.Start();
            };

        }

        static void Done(ObjectId s)
        {
            Database db = new Database();
            db.AbrirDB();
            var sinal = db.realm.All<SinaisSinal>().Where(q => q.Id == s).Single();
            MessageBox.Show("Done: " + sinal.InstrumentDir);
        }

    }

    class SleepToTarget
    {
        private DateTime TargetTime;
        private Action<ObjectId> MyAction;
        private ObjectId MyId;
        private const int MinSleepMilliseconds = 250;

        public SleepToTarget(DateTime TargetTime, Action<ObjectId> MyAction, ObjectId MySinal)
        {
            this.TargetTime = TargetTime;
            this.MyAction = MyAction;
            this.MyId =  MySinal;
        }

        public void Start()
        {
            new Thread(new ThreadStart(ProcessTimer)).Start();
        }

        private void ProcessTimer()
        {
            DateTime Now = DateTime.Now;

            while (Now < TargetTime)
            {
                int SleepMilliseconds = (int)Math.Round((TargetTime - Now).TotalMilliseconds / 2);
                Thread.Sleep(SleepMilliseconds > MinSleepMilliseconds ? SleepMilliseconds : MinSleepMilliseconds);
                Now = DateTime.Now;
            }

            MyAction(MyId);
        }
    }
}
