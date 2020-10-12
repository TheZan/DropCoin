using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DropCoin.Annotations;
using DropCoin.Model;
using DropCoin.Util;
using DropCoin.View;

namespace DropCoin.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string accountAddress;

        public string AccountAddress
        {
            get => accountAddress;
            set
            {
                accountAddress = value;
                OnPropertyChanged("AccountAddress");
            }
        }

        private string accountBalance;

        public string AccountBalance
        {
            get => accountBalance;
            set
            {
                accountBalance = value;
                OnPropertyChanged("AccountBalance");
            }
        }

        private string accountAddressTo;

        public string AccountAddressTo
        {
            get => accountAddressTo;
            set
            {
                accountAddressTo = value;
                OnPropertyChanged("AccountAddressTo");
            }
        }

        private string countSendToken;

        public string CountSendToken
        {
            get => countSendToken;
            set
            {
                countSendToken = value;
                OnPropertyChanged("CountSendToken");
            }
        }

        private StartWindow startWindow;

        private RelayCommand showStartWindowCommand;

        public RelayCommand ShowStartWindowCommand
        {
            get
            {
                return showStartWindowCommand ??= new RelayCommand(obj =>
                {
                    Start();
                });
            }
        }

        private RelayCommand copyAddressCommand;

        public RelayCommand CopyAddressCommand
        {
            get
            {
                return copyAddressCommand ??= new RelayCommand(obj =>
                {
                    Clipboard.SetText(AccountAddress);
                });
            }
        }

        private RelayCommand refreshBalanceCommand;

        public RelayCommand RefreshBalanceCommand
        {
            get
            {
                return refreshBalanceCommand ??= new RelayCommand(async obj =>
                {
                    await Task.Run(GetData);
                });
            }
        }

        private RelayCommand sendTransactionCommand;

        public RelayCommand SendTransactionCommand
        {
            get
            {
                return sendTransactionCommand ??= new RelayCommand(async obj =>
                {
                    var transactionConfirm = MessageBox.Show($"Вы действительно хотите превеести {CountSendToken} DRP на кошелек с адресом {AccountAddressTo}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (transactionConfirm == MessageBoxResult.Yes)
                    {
                        if (await Task.Run(() => DropAccount.SendTransaction(AccountAddressTo, CountSendToken)))
                        {
                            await Task.Run(GetData);

                            MessageBox.Show("Успех!");
                        }
                    }
                    

                });
            }
        }

        private async void Start()
        {
            startWindow = new StartWindow()
            {
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow
            };

            if (startWindow.ShowDialog() == true)
            {
                await Task.Run(GetData);
            }
        }

        private async void GetData()
        {
            AccountAddress = DropAccount.AccountAddress;
            AccountBalance = await DropAccount.GetBalance();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
