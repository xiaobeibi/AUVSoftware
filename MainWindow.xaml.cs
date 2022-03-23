using AUVSoftware.Views;
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
using System.Windows.Threading;

namespace AUVSoftware
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dateTimer = new DispatcherTimer();//显示当前时间线程
            dateTimer.Tick += new EventHandler(DateTimer_Tick);
            dateTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dateTimer.Start();
        }

        private void DateTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string timeDateString = string.Format("{0}-{1}-{2} {3}:{4}:{5}",
                now.Year,
                now.Month.ToString("00"),
                now.Day.ToString("00"),
                now.Hour.ToString("00"),
                now.Minute.ToString("00"),
                now.Second.ToString("00"));
            TimeDateTextBlock.Text = timeDateString;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                GlobalBody.Margin = new Thickness(0);
                this.WindowState = WindowState.Normal;
            }
            else
            {
                GlobalBody.Margin = new Thickness(5);
                this.WindowState = WindowState.Maximized;
            }
        }

        private void CloseTheExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NetworkLoginButton_Click(object sender, RoutedEventArgs e)
        {
            MainLoginView.GetInstance().MainLoginShow();
        }

        private void SerialPortLoginButton_CLick(object sender, RoutedEventArgs e)
        {
            LoginSerialPortView.GetInstance().LoginSerialPortShow();
        }

        private void MainPageRadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            MainPageFrame.Navigate(new Uri(radioButton.CommandParameter.ToString(), UriKind.Relative));
        }

        
    }
}
