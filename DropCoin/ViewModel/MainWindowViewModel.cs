using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        public MainWindowViewModel()
        {
            IsEnabledControl = true;
        }

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

        private bool isEnabledControl;

        public bool IsEnabledControl
        {
            get => isEnabledControl;
            set
            {
                isEnabledControl = value;
                OnPropertyChanged("IsEnabledControl");
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
                    if (AccountAddressTo != null && CountSendToken != null && long.TryParse(CountSendToken, out _))
                    {
                        var transactionConfirm =
                            MessageBox.Show(
                                $"Вы действительно хотите перевести {CountSendToken} DRP на кошелек с адресом {AccountAddressTo}?",
                                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (transactionConfirm == MessageBoxResult.Yes)
                        {
                            IsEnabledControl = false;

                            if (await Task.Run(() => DropAccount.SendTransaction(AccountAddressTo, CountSendToken)))
                            {
                                await Task.Run(GetData);

                                AccountAddressTo = null;
                                CountSendToken = null;

                                MessageBox.Show("Транзакция завершена успешно!", "Информация", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            }

                            IsEnabledControl = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
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
