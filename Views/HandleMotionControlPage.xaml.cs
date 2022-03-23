using AUVSoftware.UserControls;
using GalaSoft.MvvmLight.Messaging;
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

namespace AUVSoftware.Views
{
    /// <summary>
    /// HandleMotionControlPage.xaml 的交互逻辑
    /// </summary>
    public partial class HandleMotionControlPage : Page
    {
        public HandleMotionControlPage()
        {
            InitializeComponent();

            //消息标志token，用于标识只阅读某个或者某些Sender发送的消息，并执行相应的处理，所以Sender那边的token要保持一致
            //执行方法Action，用来执行接收到消息后的后续工作，注意这边是支持泛型能力的，所以传递参数很方便。
            Messenger.Default.Register<string>(this, "HandleMotionMessage", HandleMotionErrorInfo);
            //this.DataContext = new MainLoginViewModel();
            //卸载当前(this)对象注册的所有MVVMLight消息
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }

        private void HandleMotionErrorInfo(string msg)
        {
            if (msg.Equals("On"))
            {
                HandleSwitchTY.Value = DSF.Net.Controllers.Switch.SwitchStatus.On;

                // 提示连接成功
                BubbleControl bubbleControl = new BubbleControl()
                {
                    NotifyMessage = "手柄连接成功!!!"
                };
                bubbleControl.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                bubbleControl.Owner = Window.GetWindow(this);
                bubbleControl.Show();
            }
            else if (msg.Equals("Off"))
            {
                HandleSwitchTY.Value = DSF.Net.Controllers.Switch.SwitchStatus.Off;

                // 提示连接成功
                BubbleControl bubbleControl = new BubbleControl()
                {
                    NotifyMessage = "手柄关闭!!!"
                };
                bubbleControl.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                bubbleControl.Owner = Window.GetWindow(this);
                bubbleControl.Show();
            }
            else
            {
                HandleSwitchTY.Value = DSF.Net.Controllers.Switch.SwitchStatus.Off;

                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "错误",
                    Message = "连接失败!请检查!"
                }.ShowDialog();
            }
        }

        private void HandleSwitchTY_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (HandleSwitchTY.Value == DSF.Net.Controllers.Switch.SwitchStatus.On)
            {
                Messenger.Default.Send<string>("On", "HandleMotionSwitch"); //注意参数一致
            }
            else
            {
                Messenger.Default.Send<string>("Off", "HandleMotionSwitch"); //注意参数一致
            }
        }

        // 舵板影藏
        private void RudderPlate_Checked(object sender, RoutedEventArgs e)
        {
            RudderPlateBorder.Visibility = Visibility.Hidden;
            RudderPlateGrid.Visibility = Visibility.Visible;
        }
        // 舵板开启
        private void RudderPlate_Unchecked(object sender, RoutedEventArgs e)
        {
            RudderPlateBorder.Visibility = Visibility.Visible;
            RudderPlateGrid.Visibility = Visibility.Hidden;
        }

        private void LeftRudderPanelMinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(LeftRudderPanelDisplay.Text) > -9)
            {
                LeftRudderPanelDisplay.Text = (Convert.ToInt32(LeftRudderPanelDisplay.Text) - 1).ToString();
            }
        }

        private void LeftRudderPanelPlusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(LeftRudderPanelDisplay.Text) < 9)
            {
                LeftRudderPanelDisplay.Text = (Convert.ToInt32(LeftRudderPanelDisplay.Text) + 1).ToString();
            }
        }

        private void RightRudderPanelMinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(RightRudderPanelDisplay.Text) > -9)
            {
                RightRudderPanelDisplay.Text = (Convert.ToInt32(RightRudderPanelDisplay.Text) - 1).ToString();
            }
        }

        private void RightRudderPanelPlusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(RightRudderPanelDisplay.Text) < 9)
            {
                RightRudderPanelDisplay.Text = (Convert.ToInt32(RightRudderPanelDisplay.Text) + 1).ToString();
            }
        }

        private void RudderPanelMinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(LeftRudderPanelDisplay.Text) < 9)
            {
                LeftRudderPanelDisplay.Text = (Convert.ToInt32(LeftRudderPanelDisplay.Text) + 1).ToString();
            }

            if (Convert.ToInt32(RightRudderPanelDisplay.Text) < 9)
            {
                RightRudderPanelDisplay.Text = (Convert.ToInt32(RightRudderPanelDisplay.Text) + 1).ToString();
            }
        }

        private void RudderPanelPlusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(LeftRudderPanelDisplay.Text) > -9)
            {
                LeftRudderPanelDisplay.Text = (Convert.ToInt32(LeftRudderPanelDisplay.Text) - 1).ToString();
            }

            if (Convert.ToInt32(RightRudderPanelDisplay.Text) > -9)
            {
                RightRudderPanelDisplay.Text = (Convert.ToInt32(RightRudderPanelDisplay.Text) - 1).ToString();
            }
        }
    }
}
