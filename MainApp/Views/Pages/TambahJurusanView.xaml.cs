using MainApp.DataAccess.Models;
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
    /// Interaction logic for TambahJurusanView.xaml
    /// </summary>
    public partial class TambahJurusanView : Window
    {
        public TambahJurusanView()
        {
            InitializeComponent();
            this.DataContext = new TambahJurusanModel() { WindowClose = this.Close };
        }

        public TambahJurusanView(Jurusan selectedItem)
        {
            InitializeComponent();
            this.DataContext = new TambahJurusanModel(selectedItem) { WindowClose = this.Close };
        }
    }



    class TambahJurusanModel : Jurusan,IDataErrorInfo
    {
        private Jurusan selectedItem;

        public Jurusan Model { get; set; }
        public TambahJurusanModel()
        {
            MyTitle = "Tambah Jurusan";
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveValidate, ExecuteAction = SaveAction };
        }

        public TambahJurusanModel(Jurusan selectedItem)
        {
            MyTitle = "Tambah Edit";
            this.selectedItem = selectedItem;
           Id= selectedItem.Id;
            this.Kode= selectedItem.Kode;
            this.Name = selectedItem.Name;
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveValidate, ExecuteAction = SaveAction };
        }

        private void SaveAction(object obj)
        {
            try
            {
                var model = new Jurusan {  Id= this.Id, Kode=this.Kode, Name=Name};
                using (var db = new DbContext())
                {
                    if (selectedItem != null)
                    {
                        if (db.Jurusan.Update(x => new { x.Kode, x.Name}, model, x => x.Id == model.Id))
                        {
                            Model = model;
                            MessageBox.Show("Jurusan Perhasil Diubah", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            throw new SystemException("Data Tidak Tersimpan");
                    }
                    else
                    {
                        model.Id = db.Jurusan.InsertAndGetLastID(model);
                        if (model.Id <= 0)
                            throw new SystemException("Data Tidak Tersimpan");
                        Model = model;
                        MessageBox.Show("Jurusan Perhasil Ditambahkan", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    WindowClose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool SaveValidate(object obj)
        {


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


        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => Kode)] +
                    me[GetPropertyName(() => Name)] 
                    ;
                if (!string.IsNullOrEmpty(error))
                    return "Data Inputan Error : "+ error;
                return null;
            }
        }
        public string this[string columnName] => Validate(columnName);
        public string Validate(string name)
        {

            if (name == "KodeJurusan" && string.IsNullOrEmpty(Kode))
                return "Kode Jurusan Tidak Boleh Kosong";

            if (name == "NamaJurusan" && string.IsNullOrEmpty(Name))
                return "Nama Jurusan Tidak Boleh Kosong";

            return null;
        }

    }
}
