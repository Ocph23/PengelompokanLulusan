using MainApp.Views.Pages;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(_mainFrame) { WindowClose=this.Close};
            _mainFrame.Navigate(new Views.Pages.HomePage());
        }

        private void btnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMinimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }
    }



    public class MainWindowViewModel:BaseNotify
    {
        private int leftSideWidth =150;
        private int menuHeaderTitleWidth=140;

        public MainWindowViewModel(Frame _mainFrame)
        {
            this.MainFrame = _mainFrame;
            GoPageCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = GoPageAction };
        }

        private void GoPageAction(object obj)
        {
            var param = obj as string;
            switch (param)
            {
                case "home":
                    MainFrame.Navigate(new Views.Pages.HomePage());
                    break;
                case "master-fakultas":
                    MainFrame.Navigate(new Views.Pages.FakultasPage());
                    break;
                case "master-progdi":
                    MainFrame.Navigate(new Views.Pages.ProdiPage());
                    break;
                case "master-jurusan":
                    MainFrame.Navigate(new Views.Pages.JurusanPage());
                    break;
                case "mahasiswa":
                    MainFrame.Navigate(new Views.Pages.MahasiswaPage());
                    break;
                case "analisa":
                    var form1 = new FilterDataView();
                    form1.ShowDialog();
                    var vm = form1.DataContext as FilterDataViewModel;
                    if (vm.Model != null)
                        MainFrame.Navigate(new KMeanAnalisView(vm, MainFrame.NavigationService));
                    break;
                case "logout":
                    var form = new Views.Login();
                    form.Show();
                    WindowClose();
                    break;

                default:
                    break;
            }
        }

        public int LeftSideWidth
        {
            get { return leftSideWidth; }
            set { SetProperty(ref leftSideWidth, value);
                MenuHeaderTitleWidth = value<leftSideWidth?0:value-20;
            }
        }

        

              public int MenuHeaderTitleWidth
        {
            get { return menuHeaderTitleWidth; }
            set { SetProperty(ref menuHeaderTitleWidth, value); }
        }

        public Frame MainFrame { get; }
        public CommandHandler GoPageCommand { get; }
        public Action WindowClose { get; internal set; }
    }
}
