using MainApp.DataAccess.Models;
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
    /// Interaction logic for ProdiPage.xaml
    /// </summary>
    public partial class ProdiPage : Page
    {
        public ProdiPage()
        {
            InitializeComponent();
            this.DataContext = new ProdiPageViewModel();
        }
    }

    class ProdiPageViewModel:BaseNotify
    {
        public ProdiPageViewModel()
        {
            AddNewItemCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = AddNewItemaction };
            using (var db = new DbContext())
            {
                DataSource = new List<ProgramStudi>();
                int number = 1;
                foreach(var item in db.Progdi.Select().ToList())
                {
                    item.Nomor = number;
                    DataSource.Add(item);
                    number++;
                }
            }

            Source = (CollectionView)CollectionViewSource.GetDefaultView(DataSource);
            EditCommand = new CommandHandler { CanExecuteAction = EditValidate, ExecuteAction = EditAction };
            DeleteCommand = new CommandHandler { CanExecuteAction = DeleteValidate, ExecuteAction = DeleteAction };


        }

        private bool DeleteValidate(object obj)
        {
            return SelectedItem == null ? false : true;
        }

        private void DeleteAction(object obj)
        {
            var dialogResult = MessageBox.Show("Yakin Hapus Data ? ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(dialogResult == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new DbContext())
                    {
                        if (db.Progdi.Delete(x => x.Id == SelectedItem.Id))
                        {
                            var result = DataSource.Where(x => x.Id == SelectedItem.Id).FirstOrDefault();
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
         
        }

        private void AddNewItemaction(object obj)
        {
            var form = new Views.Pages.TambahProgdiView();
            form.ShowDialog();
            var viewmodel = form.DataContext as TambahProgdiViewModel;
            if(viewmodel!=null && viewmodel.Model !=null)
            {
                viewmodel.Model.Nomor = DataSource.Count + 1;
                DataSource.Add(viewmodel.Model);
                Source.Refresh();
            }
        }

        private ProgramStudi selectedItem;

        public ProgramStudi SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem , value); }
        }


        public CommandHandler AddNewItemCommand { get; }
        public List<ProgramStudi> DataSource { get; }
        public CollectionView Source { get; }
        public CommandHandler EditCommand { get; }
        public CommandHandler DeleteCommand { get; }
    }
}
