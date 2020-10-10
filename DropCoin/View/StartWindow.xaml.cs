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

        private void SelectPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == true)
            {
                KeyStorePath.Text = open.FileName;
            }
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            Account.AccountAddress = "0x29d569e77EA47600634d30e3A71a9e4fe63E60bb";
            Account.AccountPassword = "test";
            Account.Login();
            MessageBox.Show(await Account.GetBalance());
        }

        private void CreateAccount_OnClick(object sender, RoutedEventArgs e)
        {
            Account.Registration(RegistrationPassword.Password);
            RegistrationPassword.Clear();
        }
    }
}
