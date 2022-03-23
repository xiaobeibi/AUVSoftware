using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUVSoftware.ViewModels
{
    public class NetworkDebugViewModel : ViewModelBase
    {
        public NetworkDebugViewModel()
        {
            //注册IP发过来的消息
            Messenger.Default.Register<string>(this, "NetworkReceiveMessage", NetworkMessageReceiving);

            NetWorkReceiveBit = "0";
            ReceiveCount = 0;
            SendCount = 0;
            NetWorkSendBit = "0";
            NetWorkButtonText = "开启接收";
        }

        //从IP那接收来的消息
        private void NetworkMessageReceiving(string msg)
        {
            if (NetWorkButtonText.Equals("关闭接收"))
            {
                //时间戳
                DateTime NetWorknow = DateTime.Now;
                string NetWorkTimeDateString = string.Format("{0}:{1}:{2}-->",
                    NetWorknow.Hour.ToString("00"),
                    NetWorknow.Minute.ToString("00"),
                    NetWorknow.Second.ToString("00"));

                //前端界面显示
                ReceiveCount += msg.Length;
                NetWorkReceiveBit = ReceiveCount.ToString();
                NetWorkReceiveTextBlock += NetWorkTimeDateString + msg + "\n";
            }

        }

        //网络接收数据显示
        private string netWorkReceiveTextBlock;
        public string NetWorkReceiveTextBlock
        {
            get { return netWorkReceiveTextBlock; }
            set { netWorkReceiveTextBlock = value; RaisePropertyChanged(() => NetWorkReceiveTextBlock); }
        }
        //网络发送数据显示
        private string netWorkSendTextBox;
        public string NetWorkSendTextBox
        {
            get { return netWorkSendTextBox; }
            set { netWorkSendTextBox = value; RaisePropertyChanged(() => NetWorkSendTextBox); }
        }
        //接收字符数
        public int ReceiveCount;
        private string netWorkReceiveBit;
        public string NetWorkReceiveBit
        {
            get { return netWorkReceiveBit; }
            set { netWorkReceiveBit = value; RaisePropertyChanged(() => NetWorkReceiveBit); }
        }
        //发送字符数
        public int SendCount;
        private string netWorkSendBit;
        public string NetWorkSendBit
        {
            get { return netWorkSendBit; }
            set { netWorkSendBit = value; RaisePropertyChanged(() => NetWorkSendBit); }
        }
        //开启关闭接收功能按钮文本
        private string netWorkButtonText;
        public string NetWorkButtonText
        {
            get { return netWorkButtonText; }
            set { netWorkButtonText = value; RaisePropertyChanged(() => NetWorkButtonText); }
        }

        //按钮区
        //网络接收关闭开启按钮
        private RelayCommand netWorkReceiveOpenButton;
        public RelayCommand NetWorkReceiveOpenButton
        {
            get
            {
                if (netWorkReceiveOpenButton == null) return new RelayCommand(() => NetWorkOpenClicking());
                return netWorkReceiveOpenButton;
            }
            set { netWorkReceiveOpenButton = value; }
        }
        private void NetWorkOpenClicking()
        {
            if (NetWorkButtonText.Equals("开启接收"))
            {
                NetWorkButtonText = "关闭接收";
            }
            else
            {
                NetWorkButtonText = "开启接收";
            }
        }

        //网络清空接收按钮
        private RelayCommand netWorkEmptyReceiveButton;
        public RelayCommand NetWorkEmptyReceiveButton
        {
            get
            {
                if (netWorkEmptyReceiveButton == null) return new RelayCommand(() => NetWorkEmptyReceiveClicking());
                return netWorkEmptyReceiveButton;
            }
            set { netWorkEmptyReceiveButton = value; }
        }
        private void NetWorkEmptyReceiveClicking()
        {
            NetWorkReceiveTextBlock = "";
        }

        //网络清空发送按钮
        private RelayCommand netWorkEmptySendButton;
        public RelayCommand NetWorkEmptySendButton
        {
            get
            {
                if (netWorkEmptySendButton == null) return new RelayCommand(() => NetWorkEmptySendClicking());
                return netWorkEmptySendButton;
            }
            set { netWorkEmptySendButton = value; }
        }
        private void NetWorkEmptySendClicking()
        {
            NetWorkSendTextBox = "";
        }

        //网络清空计数按钮
        private RelayCommand netWorkEmptyCountButton;
        public RelayCommand NetWorkEmptyCountButton
        {
            get
            {
                if (netWorkEmptyCountButton == null) return new RelayCommand(() => NetWorkEmptyCountClicking());
                return netWorkEmptyCountButton;
            }
            set { netWorkEmptyCountButton = value; }
        }
        private void NetWorkEmptyCountClicking()
        {
            NetWorkReceiveBit = "0";
            ReceiveCount = 0;
            NetWorkSendBit = "0";
            SendCount = 0;
        }

        //网络发送按钮
        private RelayCommand netWorkSendButton;
        public RelayCommand NetWorkSendButton
        {
            get
            {
                if (netWorkSendButton == null) return new RelayCommand(() => NetWorkSendClicking(), CanSend);
                return netWorkSendButton;
            }
            set { netWorkSendButton = value; }
        }
        private void NetWorkSendClicking()
        {
            Messenger.Default.Send<string>(NetWorkSendTextBox, "NetworkSendMessage"); //注意：token参数一致
            SendCount += NetWorkSendTextBox.Length;
            NetWorkSendBit = SendCount.ToString();
        }
        private bool CanSend()
        {
            return !string.IsNullOrEmpty(NetWorkSendTextBox);
        }


    }
}
