using System;
using System.Collections.Generic;
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
using DropCoin.Model;
using Microsoft.Win32;

namespace DropCoin.View
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow_Event(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AccountAddress.Text != "" && AccountPassword.Password != "")
            {
                AccountAddress.IsEnabled = false;
                AccountPassword.IsEnabled = false;
                LoginButton.IsEnabled = false;

                string username = AccountAddress.Text;
                string password = AccountPassword.Password;

                if (await DropAccount.Login(username, password))
                {
                    DialogResult = true;
                }
                else
                {
                    AccountAddress.Clear();
                    AccountPassword.Clear();
                }

                AccountAddress.IsEnabled = true;
                AccountPassword.IsEnabled = true;
                LoginButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private async void CreateAccount_OnClick(object sender, RoutedEventArgs e)
        {
            CreateAccount.IsEnabled = false;
            RegistrationPassword.IsEnabled = false;
            RegistrationUserName.IsEnabled = false;

            if (RegistrationUserName.Text != "" && RegistrationPassword.Password != "")
            {
                string username = RegistrationUserName.Text;
                string password = RegistrationPassword.Password;
                await Task.Run(() => DropAccount.Registration(username, password));
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            RegistrationUserName.Clear();
            RegistrationPassword.Clear();
            CreateAccount.IsEnabled = true;
            RegistrationPassword.IsEnabled = true;
            RegistrationUserName.IsEnabled = true;
        }
    }
}
