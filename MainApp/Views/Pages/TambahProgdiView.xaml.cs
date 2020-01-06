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
    /// Interaction logic for TambahProgdiView.xaml
    /// </summary>
    public partial class TambahProgdiView : Window
    {
        public TambahProgdiView()
        {
            InitializeComponent();
        }

    }


    class TambahProgdiViewModel:ProgramStudi, IDataErrorInfo
    {
        private ProgramStudi selectedItem;

        public ProgramStudi Model { get; set; }
        public TambahProgdiViewModel()
        {
            MyTitle = "Tambah Program Studi";
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveValidate, ExecuteAction = SaveAction };
        }

        public TambahProgdiViewModel(ProgramStudi selectedItem)
        {
            MyTitle = "Edit Program Studi";
            this.selectedItem = selectedItem;
            Id = selectedItem.Id;
            this.Jenjang = selectedItem.Jenjang;
            this.Kode = selectedItem.Kode;
            this.Name = selectedItem.Name;
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveValidate, ExecuteAction = SaveAction };
        }

        private void SaveAction(object obj)
        {
            try
            {
                var model = new ProgramStudi { Id=Id, ParentId=this.ParentId, Jenjang = this.Jenjang, Kode = this.Kode, Name = this.Name };
                using (var db = new DbContext())
                {
                   if(selectedItem!=null)
                    {
                        if(db.Progdi.Update(x=>new { x.Jenjang,x.Kode,x.Name}, model, x=>x.Id==model.Id))
                        {
                            Model = model;
                            MessageBox.Show("Program Studi Perhasil Diubah", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            throw new SystemException("Data Tidak Tersimpan");
                    }
                    else
                    {
                        model.Id = db.Progdi.InsertAndGetLastID(model);
                        if (model.Id <= 0)
                            throw new SystemException("Data Tidak Tersimpan");
                        Model = model;
                        MessageBox.Show("Program Studi Perhasil Ditambahkan", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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


        public List<string> Jenjangs { get { return new List<string>() { "S1", "DIII", "DII", "DI" }; } }
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
                       me[GetPropertyName(() => Jenjang)] + "," +
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
                return "Kode Progdi Mahasiswa Tidak Boleh Kosong";

            if (name == "Name" && string.IsNullOrEmpty(Name))
                return "NamaProgdi Tidak Boleh Kosong";


            if (name == "Jenjang" && string.IsNullOrEmpty(Jenjang))
                return "Jenjang Tidak Boleh Kosong";


            return null;
        }
    }
}
