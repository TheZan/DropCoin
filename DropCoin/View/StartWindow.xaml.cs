using System;
using System.Collections.Generic;
using System.Text;
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

                DropAccount.AccountAddress = AccountAddress.Text;
                DropAccount.AccountPassword = AccountPassword.Password;

                if (await DropAccount.Login())
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

        private void CreateAccount_OnClick(object sender, RoutedEventArgs e)
        {
            if (RegistrationPassword.Password != "")
            {
                CreateAccount.IsEnabled = false;
                RegistrationPassword.IsEnabled = false;
                DropAccount.Registration(RegistrationPassword.Password);
                RegistrationPassword.Clear();
                CreateAccount.IsEnabled = true;
                RegistrationPassword.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}
