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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AUVSoftware.Views
{
    /// <summary>
    /// MainLoginView.xaml 的交互逻辑
    /// </summary>
    public partial class MainLoginView : Window
    {
        private static MainLoginView Singleton;
        private static readonly object locker = new object();

        public MainLoginView()
        {
            InitializeComponent();

            //消息标志token，用于标识只阅读某个或者某些Sender发送的消息，并执行相应的处理，所以Sender那边的token要保持一致
            //执行方法Action，用来执行接收到消息后的后续工作，注意这边是支持泛型能力的，所以传递参数很方便。
            Messenger.Default.Register<string>(this, "LoginErrorMessage", ShowErrorInfo);
            //this.DataContext = new MainLoginViewModel();
            //卸载当前(this)对象注册的所有MVVMLight消息
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }
        /// <summary>
        /// 接收错误的消息
        /// </summary>
        /// <param name="msg">消息</param>
        private void ShowErrorInfo(string msg)
        {
            if (msg.Equals("连接已断开") || msg.Equals("已重新连接"))
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    JumpPage jumpPage = new JumpPage()
                    {
                        NotifyMessage = msg + "!!!"
                    };
                    jumpPage.WindowStartupLocation = WindowStartupLocation.Manual;
                    jumpPage.Owner = this;
                    jumpPage.Show();
                }));
            }
            else if (msg.Equals("连接成功"))
            {

            }
            else
            {
                BubbleControl bubbleControl = new BubbleControl()
                {
                    NotifyMessage = msg
                };
                bubbleControl.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                bubbleControl.Owner = this;
                bubbleControl.Show();
            }
        }

        public static MainLoginView GetInstance()
        {
            if (Singleton == null)
            {
                lock (locker)
                {
                    Singleton = new MainLoginView();
                }
            }
            return Singleton;
        }
        public void MainLoginShow()
        {
            Storyboard storyboard = this.FindResource("OnLoaded") as Storyboard;
            BeginStoryboard beginStoryboard = new BeginStoryboard();
            beginStoryboard.Storyboard = storyboard;
            storyboard.Begin();

            Singleton.Show();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = this.FindResource("OnUnloaded") as Storyboard;
            BeginStoryboard beginStoryboard = new BeginStoryboard();
            beginStoryboard.Storyboard = storyboard;
            storyboard.Begin();
        }
    }
}
