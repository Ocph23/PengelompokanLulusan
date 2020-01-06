using MainApp.DataAccess;
using MainApp.DataAccess.Models;
using Microsoft.AppCenter.Crashes;
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

namespace MainApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for MahasiswaPage.xaml
    /// </summary>
    public partial class MahasiswaPage : Page
    {
        public MahasiswaPage()
        {
            InitializeComponent();
            this.Loaded += MahasiswaPage_Loaded;
        }

        private void MahasiswaPage_Loaded(object sender, RoutedEventArgs e)
        {
            var Navigation = this.NavigationService;
            DataContext = new MahasiswaViewModel() { Navigation = this.NavigationService };
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }


    class MahasiswaViewModel : BaseNotify
    {
        private FakultasContext context = new FakultasContext();
        public MahasiswaViewModel()
        {
            AddNewItemCommand = new CommandHandler { CanExecuteAction = x => SelectedProgdi!=null, ExecuteAction = AddNewItemaction };
            DataSource = new List<Mahasiswa>();
            using (var db = new DbContext())
            {
                Fakultases = context.GetFakultas();
            }

            Source = (CollectionView)CollectionViewSource.GetDefaultView(DataSource);
            EditCommand = new CommandHandler { CanExecuteAction = EditValidate, ExecuteAction = EditAction };
            DeleteCommand = new CommandHandler { CanExecuteAction = DeleteValidate, ExecuteAction = DeleteAction };
            KMeansCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = KMeansAction };
        }

        private void KMeansAction(object obj)
        {
            
            var form = new FilterDataView();
            form.ShowDialog();

            var vm = form.DataContext as FilterDataViewModel;
            if(vm.Model!=null)
                this.Navigation.Navigate(new KMeanAnalisView(vm.Model,Navigation));
        }

        private bool DeleteValidate(object obj)
        {
            return SelectedItem == null ? false : true;
        }

        private void DeleteAction(object obj)
        {
            var dialogResult = MessageBox.Show("Yakin Hapus Data ? ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new DbContext())
                    {
                        if (db.Mahasiswa.Delete(x => x.IDMahasiswa == SelectedItem.IDMahasiswa))
                        {
                            var result = DataSource.Where(x => x.IDMahasiswa == SelectedItem.IDMahasiswa).FirstOrDefault();
                            if (result != null)
                            {
                                DataSource.Remove(result);
                            }

                            MessageBox.Show("Berhasil Dihapus", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            throw new SystemException("Data Tidak Terhapus");
                    }

                    Source.Refresh();
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private bool EditValidate(object obj)
        {
            return SelectedItem == null ? false : true;
        }

        private void EditAction(object obj)
        {
            var form = new Views.Pages.TambahMahasiswaView(SelectedItem.Clone() as Mahasiswa);
            form.ShowDialog();
            var viewmodel = form.DataContext as TambahMahasiswaViewModel;
            if (viewmodel != null && viewmodel.Model != null)
            {
                var result = DataSource.Where(x => x.IDMahasiswa == SelectedItem.IDMahasiswa).FirstOrDefault();
                if (result != null)
                {
                    result.IdJurusan = viewmodel.Model.IdJurusan;
                    result.Jurusan = viewmodel.Jurusan;
                    result.Gelombang = viewmodel.Model.Gelombang;
                    result.HasilTest = viewmodel.Model.HasilTest;
                    result.IdProgdi = viewmodel.Model.IdProgdi;
                    result.IPK = viewmodel.Model.IPK;
                    result.MasaStudi = viewmodel.Model.MasaStudi;
                    result.NPM = viewmodel.Model.NPM;
                    result.Suku = viewmodel.Model.Suku;
                    result.Nama = viewmodel.Model.Nama;
                    result.GelombangModel = viewmodel.Model.GelombangModel;
                }
                Source.Refresh();
            }
        }

        private void AddNewItemaction(object obj)
        {
            var form = new Views.Pages.TambahMahasiswaView(SelectedProgdi);
            form.ShowDialog();
            var viewmodel = form.DataContext as TambahMahasiswaViewModel;
            if (viewmodel != null && viewmodel.Model != null)
            {
                viewmodel.Model.Nomor = DataSource.Count + 1;
                DataSource.Add(viewmodel.Model);
                Source.Refresh();
            }
        }

        private Mahasiswa selectedItem;

        public Mahasiswa SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }


        public CommandHandler AddNewItemCommand { get; }
        public List<Mahasiswa> DataSource { get; }
        public CollectionView Source { get; }
        public CommandHandler EditCommand { get; }
        public CommandHandler DeleteCommand { get; }
        public CommandHandler KMeansCommand { get; }
        public List<Fakultas> Fakultases { get; }
        public List<ProgramStudi> Progdis { get; }

        private ProgramStudi selectedProgdi;

        public ProgramStudi SelectedProgdi
        {
            get { return selectedProgdi; }
            set {SetProperty(ref selectedProgdi ,value);
                if (value != null)
                {
                    CollectMahassiswa(value);
                }   
            }
        }


        private Fakultas selectedFakultas;

        public Fakultas SelectedFakultas
        {
            get { return selectedFakultas; }
            set
            {
                SetProperty(ref selectedFakultas, value);
            }
        }


        public NavigationService Navigation { get; internal set; }

        private void CollectMahassiswa(ProgramStudi value)
        {
           if(value==null)
            {
                this.DataSource.Clear();
            }
            else
            {
                using (var db = new DbContext())
                {
                    int number = 1;
                    DataSource.Clear();
                    foreach (var item in db.Mahasiswa.Where(x => x.IdProgdi == value.Id).ToList())
                    {
                        item.Nomor = number;
                        item.Jurusan = db.Jurusan.Where(x => x.Id == item.IdJurusan).FirstOrDefault();
                        item.SukuModel = DataHelpers.DataSuku.Where(x => x.Nilai == item.Suku).FirstOrDefault();
                        item.GelombangModel = DataHelpers.DataGelombang.Where(x => x.Nilai == item.Gelombang).FirstOrDefault();
                        DataSource.Add(item);
                        number++;
                    }

                }
            }

            Source.Refresh();
        }
    }
}
