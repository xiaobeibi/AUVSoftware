using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AUVSoftware.ViewModels
{
    public class MainLoginViewModel : ViewModelBase
    {
        public MainLoginViewModel()
        {
            TCPthread = new Thread(ListenerServer);
            TCPthread.IsBackground = true;

            //Messenger传递要发送的消息
            Messenger.Default.Register<string>(this, "NetworkSendMessage", NetworkMessageSending);

            ImageSource = "Red";
            ConnectButton = "登录";
            IpTextBox = "192.168.1.119";
            PortTextBox = "8888";
        }
        /// <summary>
        /// 网络接收要发送的消息
        /// </summary>
        /// <param name="msg"></param>
        private void NetworkMessageSending(string msg)
        {
            try
            {
                if (client.Connected)
                {
                    byte[] buffer = Encoding.Default.GetBytes(msg);
                    sendStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {

            }
        }

        //TCP服务
        private TcpClient client;
        private NetworkStream sendStream;
        private const int bufferSize = 1024;
        //TCP通信线程
        private Thread TCPthread;

        //ip栏
        private string ipTextBox;
        public string IpTextBox
        {
            get { return ipTextBox; }
            set { ipTextBox = value; RaisePropertyChanged(() => IpTextBox); }
        }
        //端口栏
        private string portTextBox;
        public string PortTextBox
        {
            get { return portTextBox; }
            set { portTextBox = value; RaisePropertyChanged(() => PortTextBox); }
        }
        //图片栏
        private string imageSource;
        public string ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; RaisePropertyChanged(() => ImageSource); }
        }
        //连接按钮
        private string connectButton;
        public string ConnectButton
        {
            get { return connectButton; }
            set { connectButton = value; RaisePropertyChanged(() => ConnectButton); }
        }

        //连接命令
        private RelayCommand submitCmd;
        public RelayCommand SubmitCmd
        {
            get
            {
                if (submitCmd == null)
                    return new RelayCommand(() => ExcuteValidForm(), CanExcute);
                return submitCmd;
            }
            set { submitCmd = value; }
        }
        private void ExcuteValidForm()
        {
            if (ConnectButton.Equals("登录"))
            {
                if (Regex.Matches(IpTextBox.Trim(), Regex.Escape(new string('.', 1))).Count == 3)
                {
                    string[] sArray = IpTextBox.Split('.');
                    foreach (string i in sArray)
                    {
                        if ((!Regex.IsMatch(i.Trim(), @"^[+-]?\d*[.]?\d*$")) || (Convert.ToDouble(i.Trim()) > 255))
                        {
                            Messenger.Default.Send<string>("IP地址无效", "LoginErrorMessage"); //注意：token参数一致
                            return;
                        }
                    }
                }

                try
                {
                    client = new TcpClient();
                    if (!client.ConnectAsync(IPAddress.Parse(IpTextBox), int.Parse(PortTextBox)).Wait(2000))
                    {
                        throw new Exception("Failed to connect.");
                    }

                    sendStream = client.GetStream();

                    if (TCPthread.ThreadState == ThreadState.Aborted)
                    {
                        TCPthread = new Thread(ListenerServer);
                        TCPthread.IsBackground = true;
                    }
                    if (TCPthread.ThreadState == (ThreadState.Unstarted | ThreadState.Background))
                    {
                        TCPthread.Start();
                    }

                    ConnectButton = "断开";
                    ImageSource = "#FF2BFF00";
                    Messenger.Default.Send<string>("连接成功", "LoginErrorMessage");
                }
                catch
                {
                    //弹出提示异常
                    Messenger.Default.Send<string>("服务器连接失败", "LoginErrorMessage"); //注意：token参数一致
                    ConnectButton = "登录";
                    ImageSource = "Red";
                    return;
                }
            }
            else
            {
                TCPthread.Abort();
                sendStream.Close();
                client.Close();
                client = null;
                ConnectButton = "登录";
                ImageSource = "Red";
                //提示断开成功
                Messenger.Default.Send<string>("服务器已断开", "LoginErrorMessage"); //注意：token参数一致
            }
        }
        private bool CanExcute()
        {
            if (string.IsNullOrEmpty(IpTextBox) || string.IsNullOrEmpty(PortTextBox))
            {
                return false;
            }
            else if (Regex.Matches(IpTextBox.Trim(), Regex.Escape(new string('.', 1))).Count != 3)
            {
                return false;
            }

            if (!Regex.IsMatch(PortTextBox.Trim(), @"^[+-]?\d*[.]?\d*$"))
            {
                return false;
            }
            else if (Convert.ToDouble(PortTextBox.Trim()) > 65535)
            {
                return false;
            }

            return true;
        }
        //网络监听
        private void ListenerServer()
        {
            while (true)
            {
                try
                {
                    if (!client.Connected)
                    {
                        Thread.Sleep(5000);
                        TcpReconnect();
                    }
                    else
                    {
                        int readSize;
                        byte[] buffer = new byte[bufferSize];
                        lock (sendStream)
                        {
                            readSize = sendStream.Read(buffer, 0, bufferSize);
                        }

                        if (readSize > 0)
                        {
                            string msg = Encoding.Default.GetString(buffer, 0, readSize);
                            Messenger.Default.Send<string>(msg, "NetworkReceiveMessage"); //注意：token参数一致
                        }
                        else
                        {
                            sendStream.Close();
                            client.Close();
                            ConnectButton = "登录";
                            ImageSource = "Red";
                            // 断开弹窗提示
                            Messenger.Default.Send<string>("连接已断开", "LoginErrorMessage"); //注意：token参数一致
                        }
                    }
                }
                catch
                {

                }
            }
        }

        // 判断重连
        void TcpReconnect()
        {
            try
            {
                client = new TcpClient();
                if (!client.ConnectAsync(IPAddress.Parse(IpTextBox), int.Parse(PortTextBox)).Wait(2000))
                {
                    throw new Exception("Failed to connect.");
                }

                sendStream = client.GetStream();

                if (TCPthread.ThreadState == ThreadState.Aborted)
                {
                    TCPthread = new Thread(ListenerServer);
                    TCPthread.IsBackground = true;
                }
                if (TCPthread.ThreadState == (ThreadState.Unstarted | ThreadState.Background))
                {
                    TCPthread.Start();
                }

                ConnectButton = "断开";
                ImageSource = "#FF2BFF00";
                Messenger.Default.Send<string>("已重新连接", "LoginErrorMessage");
            }
            catch
            {
                //弹出提示异常
                ConnectButton = "登录";
                ImageSource = "Red";
            }
        }



    }
}
