using MainApp.DataAccess;
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
    /// Interaction logic for TambahFakultasView.xaml
    /// </summary>
    public partial class TambahFakultasView : Window
    {
        public TambahFakultasView()
        {
            InitializeComponent();
            this.DataContext = new TambahFakultasViewModel() { WindowClose=this.Close};
        }
        public TambahFakultasView(Fakultas selectedItem)
        {
            InitializeComponent();
            this.DataContext = new TambahFakultasViewModel(selectedItem) { WindowClose=this.Close};
        }
    }



    class TambahFakultasViewModel : Fakultas, IDataErrorInfo
    {
        private Fakultas selectedItem;

        public Fakultas Model { get; set; }
        public TambahFakultasViewModel()
        {
            MyTitle = "Tambah Fakultas";
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveValidate, ExecuteAction = SaveAction };
        }

        public TambahFakultasViewModel(Fakultas selectedItem)
        {
            MyTitle = "Edit Fakultas";
            this.selectedItem = selectedItem;
            Id = selectedItem.Id;
            this.Kode = selectedItem.Kode;
            this.Name = selectedItem.Name;
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveValidate, ExecuteAction = SaveAction };
        }

        private void SaveAction(object obj)
        {
            try
            {
                var model = new Fakultas { Id = Id, Kode = this.Kode, Name = this.Name };
                using (var db = new DbContext())
                {
                    if (selectedItem != null)
                    {
                        if (db.Fakultas.Update(x => new {x.Kode, x.Name }, model, x => x.Id == model.Id))
                        {
                            Model = model;
                            MessageBox.Show("Fakultas Perhasil Diubah", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            throw new SystemException("Data Tidak Tersimpan");
                    }
                    else
                    {
                        model.Id = db.Fakultas.InsertAndGetLastID(model);
                        if (model.Id <= 0)
                            throw new SystemException("Data Tidak Tersimpan");
                        Model = model;
                        MessageBox.Show("Fakultas Berhasil Ditambahkan", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    me[GetPropertyName(() => Kode)] + "," +
                    me[GetPropertyName(() => Name)]
                    ;
                if (!string.IsNullOrEmpty(error))
                    return "Data Inputan Error : " + error;
                return null;
            }
        }
        public string this[string columnName] => Validate(columnName);
        public string Validate(string name)
        {

            if (name == "Kode" && string.IsNullOrEmpty(Kode))
                return "Kode Fakultas Tidak Boleh Kosong";

            if (name == "Name" && string.IsNullOrEmpty(Name))
                return "Nama Fakultas Tidak Boleh Kosong";

            return null;
        }
    }
}
