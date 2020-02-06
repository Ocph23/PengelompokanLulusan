using MainApp.DataAccess;
using MainApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MainApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for FilterDataView.xaml
    /// </summary>
    public partial class FilterDataView : Window
    {
        private FilterDataViewModel vm;

        public FilterDataView()
        {
            InitializeComponent();
            this.DataContext=vm = new FilterDataViewModel() { WindowClose=this.Close};
        }

        private void rangeSlider_LowerValueChanged(object sender, RoutedEventArgs e)
        {
            vm.AngkatanMinimum = (int)rangeSlider.LowerValue;
        }

        private void rangeSlider_HigherValueChanged(object sender, RoutedEventArgs e)
        {

        }
    }


   public class FilterDataViewModel : BaseNotify
    {

        private FakultasContext context = new FakultasContext();

        public List<Mahasiswa> Model { get; set; }
        public FilterDataViewModel()
        {
            MyTitle = "Filter Data Analisa";
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveValidate, ExecuteAction = SaveAction };
            Fakultases = context.GetFakultas();
     
            using (var db = new DbContext())
            {
                int number = 1;
                var items = (from m in db.Mahasiswa.Select()
                            join p in db.Progdi.Select() on m.IdProgdi equals p.Id
                            join f in db.Fakultas.Select() on p.ParentId equals f.Id
                            select new Mahasiswa
                            {
                                Gelombang = m.Gelombang,
                                HasilTest = m.HasilTest,
                                IdJurusan = m.IdJurusan,
                                IDMahasiswa = m.IDMahasiswa,
                                IdProgdi = m.IdProgdi,
                                IPK = m.IPK,
                                MasaStudi = m.MasaStudi,
                                Nama = m.Nama,
                                NPM = m.NPM,
                                Suku = m.Suku,
                                FakultasId = f.Id
                            }).ToList();
                foreach (var item in items)
                {
                    item.Jurusan = db.Jurusan.Where(x => x.Id == item.IdJurusan).FirstOrDefault();
                    item.SukuModel = DataHelpers.DataSuku.Where(x => x.Nilai == item.Suku).FirstOrDefault();
                    item.GelombangModel = DataHelpers.DataGelombang.Where(x => x.Nilai == item.Gelombang).FirstOrDefault();
                    DataSource.Add(item);
                    number++;
                }
                DataSourceView = (CollectionView)CollectionViewSource.GetDefaultView(DataSource);
                DataSourceView.Filter = filterData;

                SliderMaximum = DateTime.Now.Year;
                SliderMinimum = SliderMaximum - 10;
                AngkatanMinimum = SliderMinimum;
                AngkatanMaximum = SliderMaximum;
                DataSourceView.Refresh();
            }
        }

        private bool filterData(object obj)
        {
            var data = obj as Mahasiswa;
            if (data == null)
                return true;

            if(IncludeAngkatan)
            {
                int aouputAngkatan = 0;
                var isNumber =Int32.TryParse(data.Angkatan, out aouputAngkatan);
                if (!isNumber)
                    return false;

                if(SelectedFakultas==null)
                    return aouputAngkatan >= AngkatanMinimum && aouputAngkatan <= AngkatanMaximum;
                else
                {
                    if(ProgdiId==0)
                    {
                        return aouputAngkatan <= AngkatanMinimum && aouputAngkatan >= AngkatanMaximum && data.FakultasId==SelectedFakultas.Id;
                    }
                    else
                    {
                        return aouputAngkatan <= AngkatanMinimum && aouputAngkatan >= AngkatanMaximum 
                            && data.FakultasId == SelectedFakultas.Id && data.IdProgdi==ProgdiId;
                    }

                }
            }
            else
            {
                if (SelectedFakultas == null)
                    return true;
                else
                {
                    if (ProgdiId == 0)
                    {
                        return data.FakultasId == SelectedFakultas.Id;
                    }
                    else
                    {
                        return data.FakultasId == SelectedFakultas.Id && data.IdProgdi == ProgdiId;
                    }

                }
            }
            
        }

        private void SaveAction(object obj)
        {
           var results = DataSourceView.Cast<Mahasiswa>();
            Model = results.ToList();
            WindowClose();
        }

        private bool SaveValidate(object obj)
        {
            if(SelectedFakultas==null && !IncludeAngkatan)
                return false;
            return true;
        }

        private void CancelAction(object obj)
        {
            Model = null;
            WindowClose();
        }

        public Action WindowClose { get; internal set; }
        public CommandHandler CancelCommand { get; }
        public CommandHandler SaveCommand { get; }
        public List<Fakultas> Fakultases { get; }

        private Fakultas selectedFakultas;

        public Fakultas SelectedFakultas
        {
            get { return selectedFakultas; }
            set { SetProperty(ref selectedFakultas ,value); DataSourceView.Refresh(); }
        }



        private ProgramStudi progdi;

        public ProgramStudi SelectedProgdi
        {
            get { return progdi; }
            set { SetProperty(ref progdi, value); }
        }


        public List<Mahasiswa> DataSource { get; set; } = new List<Mahasiswa>();
        public CollectionView DataSourceView { get; }







        private int faklutasId;

        public int FakultasId
        {
            get { return faklutasId; }
            set { SetProperty(ref faklutasId, value); }
        }


        private int progdiId;

        public int ProgdiId
        {
            get { return progdiId; }
            set { SetProperty(ref progdiId ,value); DataSourceView.Refresh(); }
        }
         

        private int sliderMinimum;

        public int SliderMinimum
        {
            get { return sliderMinimum; }
            set { SetProperty(ref sliderMinimum, value); }
        }


        private int sliderMaximum;

        public int SliderMaximum
        {
            get { return sliderMaximum; }
            set { SetProperty(ref sliderMaximum, value); }
        }


        private int angkatanMinimum;

        public int AngkatanMinimum
        {
            get { return angkatanMinimum; }
            set { SetProperty(ref angkatanMinimum, value); DataSourceView.Refresh(); }
        }


        private int angkatanMaximum;

        public int AngkatanMaximum
        {
            get { return angkatanMaximum; }

            set { SetProperty(ref angkatanMaximum, value); DataSourceView.Refresh(); }
        }


        private bool includeAngkatan;

        public bool IncludeAngkatan
        {
            get { return includeAngkatan; }
            set { SetProperty(ref includeAngkatan, value);
                DataSourceView.Refresh();
            }
        }




    }

}