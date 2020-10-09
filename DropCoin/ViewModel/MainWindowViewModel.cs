using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DropCoin.Annotations;
using DropCoin.Util;
using DropCoin.View;

namespace DropCoin.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private StartWindow startWindow;

        private RelayCommand showStartWindowCommand;

        public RelayCommand ShowStartWindowCommand
        {
            get
            {
                return showStartWindowCommand ??= new RelayCommand(async obj =>
                {
                    Start();
                });
            }
        }

        private void Start()
        {
            startWindow = new StartWindow()
            {
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow
            };

            if (startWindow.ShowDialog() == true)
            {

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
