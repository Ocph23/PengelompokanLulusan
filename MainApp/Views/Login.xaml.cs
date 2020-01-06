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
using System.Windows.Shapes;

namespace MainApp.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LoginViewModel vm;

        public Login()
        {
            InitializeComponent();
            this.DataContext=vm= new LoginViewModel() { WindowClose=this.Close};
        }

        private void btnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void onPasswordChange(object sender, RoutedEventArgs e)
        {
            var password = ((PasswordBox)sender).Password;
            if (!string.IsNullOrEmpty(password))
                vm.Password = password;
        }
    }


    class LoginViewModel:User
    {
        public Action WindowClose { get; internal set; }
        public CommandHandler LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new CommandHandler { CanExecuteAction = LoginValidate, ExecuteAction = LoginAction };
        }

        private bool LoginValidate(object obj)
        {
            if (string.IsNullOrEmpty(UserName)|| string.IsNullOrEmpty(Password))
                return false;
            return true;
            
        }

        private void LoginAction(object obj)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var result = db.User.Where(x => x.UserName == UserName).FirstOrDefault();
                    if (result == null)
                    {
                        throw new SystemException("Anda Tidak Memiliki Akses");
                    }
                    else if (result.Password != Password)
                        throw new SystemException("Password Anda Salah");
                    else
                    {
                        var form = new MainWindow();
                        form.Show();
                        WindowClose();
                    }
                }
            }
            catch (Exception ex)
            {
              //  Crashes.TrackError(ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
