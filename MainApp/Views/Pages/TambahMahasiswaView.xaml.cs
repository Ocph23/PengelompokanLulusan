using MainApp.DataAccess.Models;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MainApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for TambahMahasiswaView.xaml
    /// </summary>
    public partial class TambahMahasiswaView : Window
    {

        public TambahMahasiswaView(ProgramStudi selectedProgdi)
        {
            InitializeComponent();
            this.DataContext = new TambahMahasiswaViewModel(selectedProgdi) { WindowClose = this.Close };
        }

        public TambahMahasiswaView(Mahasiswa mahasiswa)
        {
            InitializeComponent();
            this.DataContext = new TambahMahasiswaViewModel(mahasiswa) { WindowClose = this.Close };
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox1 = (TextBox)sender;
            if(!Keyboard.IsKeyToggled(Key.Insert))
            {
                if (Keyboard.PrimaryDevice != null)
                {
                    if (Keyboard.PrimaryDevice.ActiveSource != null)
                    {
                        var e1 = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Insert) { RoutedEvent = Keyboard.KeyDownEvent };
                        InputManager.Current.ProcessInput(e1);
                    }
                }
            }

        }
    }

    class TambahMahasiswaViewModel : Mahasiswa, IDataErrorInfo
    {
        private Mahasiswa selectedItem;
        public Mahasiswa Model { get; set; }
        public TambahMahasiswaViewModel(ProgramStudi selectedProgdi)
        {
            MyTitle = "Tambah Mahasiswa";
            this.IdProgdi = selectedProgdi.Id;
            Load();
        }

        private void Load()
        {
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveValidate, ExecuteAction = SaveAction };
            Gelombangs = DataAccess.DataHelpers.DataGelombang;
            Sukus = DataAccess.DataHelpers.DataSuku;
            using (var db = new DbContext())
            {
                this.Jurusans = db.Jurusan.Select().ToList();
            }
        }

        public TambahMahasiswaViewModel(Mahasiswa selectedItem)
        {
            MyTitle = "Edit Mahasiswa";

            this.selectedItem = selectedItem;
            this.IdJurusan = selectedItem.IdJurusan;
            this.Gelombang = selectedItem.Gelombang;
            this.HasilTest = selectedItem.HasilTest;
            this.IDMahasiswa = selectedItem.IDMahasiswa;
            this.IdProgdi = selectedItem.IdProgdi;
            this.IPK = selectedItem.IPK;
            this.MasaStudi = selectedItem.MasaStudi;
            this.NPM = selectedItem.NPM;
            this.Suku = selectedItem.Suku;
            this.Nama = selectedItem.Nama;
            this.Jurusan = selectedItem.Jurusan;

            Load();
        }

        private void SaveAction(object obj)
        {
            try
            {
                var model = new Mahasiswa
                { 
                    IdJurusan = IdJurusan,
                    Gelombang = Gelombang,
                    HasilTest = HasilTest,
                    IDMahasiswa = IDMahasiswa,
                    IdProgdi = IdProgdi,
                    IPK = IPK,
                    MasaStudi = MasaStudi,
                    NPM = NPM,
                    Suku = Suku,
                    Nama = Nama,
                     GelombangModel= DataAccess.DataHelpers.DataGelombang.Where(x => x.Nilai == Gelombang).FirstOrDefault()
            };
                using (var db = new DbContext())
                {
                    if (selectedItem != null)
                    {
                        if (db.Mahasiswa.Update(x => new
                        {
                            x.IdJurusan,
                            x.Gelombang,
                            x.HasilTest,
                            x.IDMahasiswa,
                            x.IdProgdi,
                            x.IPK,
                            x.MasaStudi,
                            x.NPM,
                            x.Suku,
                            x.Nama,
                        }, model, x => x.IDMahasiswa == model.IDMahasiswa))
                        {
                            Model = model;
                            MessageBox.Show("Data Mahasiswa Perhasil Diubah", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            throw new SystemException("Data Tidak Tersimpan");
                    }
                    else
                    {
                        model.IDMahasiswa = db.Mahasiswa.InsertAndGetLastID(model);
                        if (model.IdJurusan <= 0)
                            throw new SystemException("Data Tidak Tersimpan");
                        Model = model;
                        MessageBox.Show("Data Mahasiswa Perhasil Ditambahkan", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    WindowClose();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool SaveValidate(object obj)
        {
            if (!string.IsNullOrEmpty(Error))
                return false;
            return true;
        }

        private void CancelAction(object obj)
        {
            Model = null;
            WindowClose();
        }


        public Action WindowClose { get; internal set; }
        public CommandHandler CancelCommand { get; set; }
        public CommandHandler SaveCommand { get; set; }
        public List<Gelombang> Gelombangs { get; private set; }
        public List<Suku> Sukus { get; private set; }
        public List<Jurusan> Jurusans { get; private set; }



        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => Nama)] + 
                    me[GetPropertyName(() => NPM)] + 
                    me[GetPropertyName(() => Jurusan)] + 
                     me[GetPropertyName(() => Gelombang)] + 
                    me[GetPropertyName(() => Suku)] + 
                    me[GetPropertyName(() => IdProgdi)] + 
                    me[GetPropertyName(() => IPK)] + 
                    me[GetPropertyName(() => HasilTest)] + 
                    me[GetPropertyName(() => MasaStudi)]
                    ;
                if (!string.IsNullOrEmpty(error))
                    return error;
                return null;
            }
        }
        public string this[string columnName] => Validate(columnName);
        public string Validate(string name)
        {

            if (name == "Nama" && string.IsNullOrEmpty(Nama))
                return "Nama Mahasiswa Tidak Boleh Kosong";

            if (name == "NPM" && string.IsNullOrEmpty(NPM))
                return "NPM Tidak Boleh Kosong";

            if (name == "Suku" && Suku <= 0)
                return "Suku Tidak Boleh Kosong";


            if (name == "Jurusan" && Jurusan==null)
                return "Jurusan Tidak Boleh Kosong";

            if (name == "IdJurusan" && IdJurusan <= 0)
                return "Jurusan Tidak Boleh Kosong";

            if (name == "Gelombang" && Gelombang <= 0)
                return Gelombang <= 0 ? "Gelombang Tidak Boeh Kosong" : null;

            if (name == "HasilTest")
                return HasilTest <= 0 ? "Hasil Test Harus > 0" : HasilTest > 10 ? "Hasil Test Harus <= 10" : null; 

            if (name == "IPK" )
                return IPK <= 0 ? "IPK Harus > 0 " : IPK>4? "IPK Harus <= 4 ":null;

            if (name == "MasaStudi")
                return MasaStudi < 3 ? "Masa Studi Harus > 3 Tahun":null;

            return null;
        }
    }
}
