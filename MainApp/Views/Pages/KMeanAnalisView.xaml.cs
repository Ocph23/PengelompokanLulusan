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
using MainApp.DataAccess.Models;
using MainApp.Reports;
using Microsoft.Reporting.WinForms;

namespace MainApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for KMeanAnalisView.xaml
    /// </summary>
    public partial class KMeanAnalisView : Page
    {
        public KMeanAnalisView(List<DataAccess.Models.Mahasiswa> dataSource, NavigationService navigation)
        {
            InitializeComponent();
            this.DataContext = new KMeanAnalisViewModel( dataSource,navigation);
        }
    }


    public class KMeanAnalisViewModel:BaseNotify
    {
        public NavigationService Navigation { get; }
        public CommandHandler AnalisaCommand { get; }
        public CommandHandler PrintPengelompokanCommand { get; }
        public CommandHandler PrintGrafikCommand { get; }
        public CommandHandler PrintStatisticCommand { get; }
        public CommandHandler PrintRekomendasiCommand { get; }
        public CommandHandler BackCommand { get; }
        public List<Mahasiswa> Source { get; set; }
        private double[][] rawData = null;
        public KMeanAnalisViewModel(List<Mahasiswa> dataSource, NavigationService navigation)
        {
            this.Navigation = navigation;
            AnalisaCommand = new CommandHandler { CanExecuteAction = AnalisaValidate, ExecuteAction = AnalisaActionAsync };
            PrintPengelompokanCommand = new CommandHandler { CanExecuteAction = CanPrint, ExecuteAction = PrintPengelompokanAction };
            PrintGrafikCommand = new CommandHandler { CanExecuteAction = CanPrint, ExecuteAction = PrintGrafikAction };
            PrintStatisticCommand = new CommandHandler { CanExecuteAction = CanPrint, ExecuteAction = PrintStatisticAction };
            PrintRekomendasiCommand = new CommandHandler { CanExecuteAction = CanPrint, ExecuteAction = PrintRekomendasiAction };
            BackCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = x => Navigation.GoBack() };
            this.Source = new List<Mahasiswa>();
            int number = 1;
            foreach (var item in dataSource)
            {
                item.OnChangeCentroid += Item_OnChangeCentroid;
                item.Nomor = number;
                this.Source.Add(item);
                number++;
            }
            using (var db = new DbContext())
            {
                Jurusans = db.Jurusan.Select().ToList();
                for(var i=0;i<Jurusans.Count;i++)
                {
                    Jurusans[i].Initial = i+1;
                }
            }
            JumlahData = Source.Count;
            MappingData(dataSource);
        }

        private void PrintStatisticAction(object obj)
        {
            var centroidAwal = Source.Where(x => x.IsCentroid).ToList();
            var group = Source.GroupBy(x => x.Clustering);
            List<StatistikModel> dataSource = new List<StatistikModel>();
            int n = 1;
            foreach (var item in centroidAwal)
            {
                dataSource.Add(new StatistikModel
                {
                    Clustering = n,
                    Gelombang = item.GelombangModel.Nama,
                    HasilTest = group.Where(x=>x.Key ==item.Clustering).FirstOrDefault().Average(x=>x.HasilTest),
                    IPK = group.Where(x => x.Key == item.Clustering).FirstOrDefault().Average(x=>x.IPK),
                    IsCentroid = item.IsCentroid,
                    Jurusan = item.Jurusan.Name,
                    MasaStudi = group.Where(x => x.Key == item.Clustering).FirstOrDefault().Average(x => x.MasaStudi),
                    Suku = item.SukuModel.Nama,
                    Jumlah = group.Where(x=>x.Key==item.Clustering).FirstOrDefault().Count()
                });
                n++;
            }

            HelperPrint.PrintPreviewWithFormAction("Judul", new ReportDataSource("DataSet1", dataSource),
               "MainApp.Reports.StatistikLayout.rdlc", null);
        }


        private void PrintRekomendasiAction(object obj)
        {
            var centroidAwal = Source.Where(x => x.IsCentroid).ToList();
            var group = Source.GroupBy(x => x.Clustering);
            List<StatistikModel> dataSource = new List<StatistikModel>();
            int n = 1;
            foreach (var item in centroidAwal)
            {
                dataSource.Add(new StatistikModel
                {
                    Clustering = n,
                    Gelombang = item.GelombangModel.Nama,
                    HasilTest = group.Where(x => x.Key == item.Clustering).FirstOrDefault().Average(x => x.HasilTest),
                    IPK = group.Where(x => x.Key == item.Clustering).FirstOrDefault().Average(x => x.IPK),
                    IsCentroid = item.IsCentroid,
                    Jurusan = item.Jurusan.Name,
                    MasaStudi = group.Where(x => x.Key == item.Clustering).FirstOrDefault().Average(x => x.MasaStudi),
                    Suku = item.SukuModel.Nama,
                    Jumlah = group.Where(x => x.Key == item.Clustering).FirstOrDefault().Count()
                });
                n++;
            }

            HelperPrint.PrintPreviewWithFormAction("Judul", new ReportDataSource("DataSet1", dataSource.OrderByDescending(x=>x.Jumlah).ToList()),
               "MainApp.Reports.RekomendasiLayout.rdlc", null);
        }

        private void PrintPengelompokanAction(object obj)  
        {
            
            HelperPrint.PrintPreviewWithFormAction("Judul", new ReportDataSource("DataSet1", HelperPrint.CreateReportModel(Source)),
                "MainApp.Reports.PengelompokanLayout.rdlc", null);
        }

        private void PrintGrafikAction(object obj)
        {
            List<GrafikModel> datas = new List<GrafikModel>();

            var sukus =Source.GroupBy(x => new {  x.Clustering , x.SukuModel.Nama});
            foreach(var item in sukus)
            {
                datas.Add(new GrafikModel { Cluster = item.Key.Clustering, Nama = item.Key.Nama, Kategori = "Suku", Nilai = item.Count() });
            }

            var data = Source.GroupBy(x => new {  x.Clustering , x.Jurusan.Name});
            foreach (var item in data)
            {
                datas.Add(new GrafikModel { Cluster = item.Key.Clustering, Nama = item.Key.Name, Kategori = "Jurusan", Nilai = item.Count() });
            }

            var ipks = Source.GroupBy(x => new { x.Clustering, x.IPKText });
            foreach (var items in ipks)
            {
                datas.Add(new GrafikModel { Cluster = items.Key.Clustering, Nama = items.Key.IPKText.ToString(), Kategori = "IPK", Nilai = items.Count() });
            }


            var gels = Source.GroupBy(x => new { x.Clustering, x.GelombangModel.Nama });
            foreach (var item in gels)
                datas.Add(new GrafikModel { Cluster = item.Key.Clustering, Nama = item.Key.Nama, Kategori = "Gelombang", Nilai = item.Count() });



            var tests = Source.GroupBy(x => new { x.Clustering, x.HasilTestText });
            foreach (var items in tests)
            {
                    datas.Add(new GrafikModel { Cluster = items.Key.Clustering, Nama = items.Key.HasilTestText, Kategori = "Hasil Test", Nilai = items.Count() });
            }

            var studis = Source.GroupBy(x => new { x.Clustering ,x.MasaStudiText });
            foreach (var items in studis)
            {
                    datas.Add(new GrafikModel { Cluster = items.Key.Clustering, Nama = items.Key.MasaStudiText, Kategori = "Masa Studi", Nilai = items.Count() });
            }



            HelperPrint.PrintPreviewWithFormAction("Judul", new ReportDataSource("DataSet1", datas),
                "MainApp.Reports.Grafik.rdlc", null);
        }

        private bool CanPrint(object obj)
        {
            if (Source != null && Source.Count > 0 && Source[0].Clustering != 0)
                return true;
            return false;
        }

        private bool AnalisaValidate(object obj)
        {
            if (JumlahData <= 0 || JumlahCentroid <= 0 || JumlahCluster != JumlahCentroid)
                return false;
            return true;
        }

        private async void AnalisaActionAsync(object obj)
        {
            await Task.Delay(1000);
           var centroidAwal = MappingData(Source.Where(x => x.IsCentroid).ToList());
            var datas = MappingData(Source);
            var kmean = new KMeans(datas,centroidAwal, JumlahCluster);
            for(var i=0;i< kmean.LastClustering.Count();i++)
            {
                Source[i].Clustering = kmean.LastClustering[i]+1;
            }
        }

        private  void Item_OnChangeCentroid(bool isCentorid)
        {
            if (isCentorid)
                JumlahCentroid++;
            else
                JumlahCentroid--;
        }

        public List<Jurusan> Jurusans { get; }

        private double[][] MappingData(List<Mahasiswa> dataSource)
        {
            rawData = new double[dataSource.Count][];
            for (var i=0;i<dataSource.Count; i++)
            {
                var data = dataSource[i];
                rawData[i] = new double[] {  data.Suku ,Jurusans.Where(x=>x.Id==data.IdJurusan).FirstOrDefault().Initial,
                    data.IPK, data.Gelombang, data.HasilTest, data.MasaStudi };
            }
            return rawData;
        }




        private int jlhCentoid;

        public int JumlahCentroid
        {
            get { return jlhCentoid; }
            set { SetProperty(ref jlhCentoid , value); }
        }


        private int jumlahData;

        public int JumlahData
        {
            get { return jumlahData; }
            set { SetProperty(ref jumlahData , value); }
        }


        private int jumlahCluster;

        public int JumlahCluster
        {
            get { return jumlahCluster; }
            set {SetProperty(ref jumlahCluster ,value); }
        }





    }
}

