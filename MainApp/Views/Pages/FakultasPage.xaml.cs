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
    /// Interaction logic for FakultasPage.xaml
    /// </summary>
    public partial class FakultasPage : Page
    {
        private FakultasPageViewModel vm;

        public FakultasPage()
        {
            InitializeComponent();
            this.DataContext=vm = new FakultasPageViewModel();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(e.NewValue!=null)
            {
                vm.SelectedItem = e.NewValue as IItem;
            }
        }
    }


    public class FakultasPageViewModel:BaseNotify
    {

        private FakultasContext context = new FakultasContext();
        private IItem _selectedItem;

        public FakultasPageViewModel()
        {
            Source = context.GetFakultas();
            Children = (CollectionView)CollectionViewSource.GetDefaultView(Source);
            Children.Refresh();
            AddFacultyCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = AddFacultyAction };
            AddProgdiCommand = new CommandHandler { CanExecuteAction = AddProgdiValidate, ExecuteAction = AddProgdiAction};
            EditCommand = new CommandHandler { CanExecuteAction = x => SelectedItem != null, ExecuteAction = EditAction };
            DeleteCommand = new CommandHandler { CanExecuteAction =x=>SelectedItem!=null, ExecuteAction = DeleteAction };
            RefreshCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
        }

        private void RefreshAction(object obj)
        {
            Source.Clear();
            var result = context.GetFakultas();
            foreach(var item in result)
            {
                Source.Add(item);
            }

            Children.Refresh();
        }

        private void DeleteAction(object obj)
        {
            try
            {
                if (SelectedItem.type == typeof(Fakultas))
                {
                    context.DeleteFaculty(SelectedItem);
                    var result =Source.Where(x => x.Id == SelectedItem.Id).FirstOrDefault();
                    if(result!=null)
                    {
                        Source.Remove(result);
                    }
                }
                else
                {
                    context.DeleteProgdi(SelectedItem);
                    var result = (from gig in Source
                                  from c in gig.Children.Where(x => x.Id == SelectedItem.Id)
                                  select gig).FirstOrDefault();
                    if (result != null)
                    {
                        result.Children.Remove(SelectedItem);
                    }
                }
               
                Children.Refresh();
                MessageBox.Show("Data Berhasil Dihapus", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditAction(object obj)
        {
            try
            {
                if (SelectedItem.type == typeof(Fakultas))
                {
                    var form = new TambahFakultasView(new Fakultas { Id = SelectedItem.Id, Name = SelectedItem.Name, Kode = SelectedItem.Kode });
                    form.ShowDialog();
                    var viewmodel = form.DataContext as TambahFakultasViewModel;
                    var result = Source.Where(x => x.Id == SelectedItem.Id).FirstOrDefault();
                    if (result != null)
                    {
                        result.Kode = viewmodel.Model.Kode;
                        result.Name = viewmodel.Model.Name;
                    }
                   
                }
                else
                {
                    var form = new TambahProgdiView();
                    var viewmodel = new TambahProgdiViewModel(new ProgramStudi { ParentId = SelectedItem.ParentId, 
                        Id = SelectedItem.Id, Jenjang=SelectedItem.Jenjang, Name = SelectedItem.Name, Kode = SelectedItem.Kode }) { WindowClose = form.Close };
                    form.DataContext = viewmodel;
                    form.ShowDialog();
                    if (viewmodel != null && viewmodel.Model != null)
                    {
                        var result = (from gig in Source
                                     from c in gig.Children.Where(x=>x.Id==SelectedItem.Id)
                                     select c).FirstOrDefault();
                        if (result != null)
                        {
                            result.Jenjang = viewmodel.Model.Jenjang;
                            result.Kode = viewmodel.Model.Kode;
                            result.Name = viewmodel.Model.Name;
                        }
                    }
                }
                Children.Refresh();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool AddProgdiValidate(object obj)
        {
            if (SelectedItem != null && SelectedItem.type == typeof(Fakultas))
                return true;
            return false;
        }

        private void AddProgdiAction(object obj)
        {
            var form = new TambahProgdiView();
            var vm = new TambahProgdiViewModel { ParentId = SelectedItem.Id , WindowClose = form.Close };;
            form.DataContext = vm;
            form.ShowDialog();
            if(vm!=null && vm.Model!=null)
            {
                SelectedItem.Children.Add(vm.Model);
            }
            Children.Refresh();
        }

        private void AddFacultyAction(object obj)
        {
            var form = new TambahFakultasView();
            form.ShowDialog();
            var vm = form.DataContext as TambahFakultasViewModel;
            if (vm != null && vm.Model != null)
            {
                Source.Add(vm.Model);
            }

            Children.Refresh();
            SelectedItem = vm.Model;
        }

        public List<Fakultas> Source { get; private set; }
        public CollectionView Children { get; }
        public IItem SelectedItem {
            get
            {
                return _selectedItem;
            }
            set
            {
                SetProperty(ref _selectedItem,value);
            }
        }

        public CommandHandler AddFacultyCommand { get; }
        public CommandHandler AddProgdiCommand { get; }
        public CommandHandler EditCommand { get; }
        public CommandHandler DeleteCommand { get; }
        public CommandHandler RefreshCommand { get; }
    }
}
