using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DropCoin.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TopPanel(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ToolWindowPanel(object sender, MouseButtonEventArgs e)
        {
            switch (((Image) sender).Name)
            {
                case "MinimizeWindowButton":
                    WindowState = WindowState.Minimized;
                    break;
                case "MaximizeWindowButton":
                    if (WindowState == WindowState.Normal)
                    {
                        WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        WindowState = WindowState.Normal;
                    }

                    break;
                case "CloseWindowButton":
                    Application.Current.Shutdown();
                    break;
            }
        }
    }
}
