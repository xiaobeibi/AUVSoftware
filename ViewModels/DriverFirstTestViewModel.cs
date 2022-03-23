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
    public class DriverFirstTestViewModel : ViewModelBase
    {
        public DriverFirstTestViewModel()
        {
            // 初始化Value
            Motion1Value = "1500";
            Motion2Value = "1500";
            Motion3Value = "1500";
            Motion4Value = "1500";
            Motion5Value = "1500";

            // 注册网口发过来的消息
            Messenger.Default.Register<string>(this, "NetworkReceiveMessage", DriverFirstTestReceiving);
        }

        //从网口接收来的消息
        private void DriverFirstTestReceiving(string msg)
        {

        }

        //电机1Value
        private string motion1Value;
        public string Motion1Value
        {
            get { return motion1Value; }
            set { motion1Value = value; RaisePropertyChanged(() => Motion1Value); }
        }

        //电机2Value
        private string motion2Value;
        public string Motion2Value
        {
            get { return motion2Value; }
            set { motion2Value = value; RaisePropertyChanged(() => Motion2Value); }
        }
        //电机3Value
        private string motion3Value;
        public string Motion3Value
        {
            get { return motion3Value; }
            set { motion3Value = value; RaisePropertyChanged(() => Motion3Value); }
        }

        //电机4Value
        private string motion4Value;
        public string Motion4Value
        {
            get { return motion4Value; }
            set { motion4Value = value; RaisePropertyChanged(() => Motion4Value); }
        }

        //电机5Value
        private string motion5Value;
        public string Motion5Value
        {
            get { return motion5Value; }
            set { motion5Value = value; RaisePropertyChanged(() => Motion5Value); }
        }

        //电机1发送
        private RelayCommand motion1;
        public RelayCommand Motion1
        {
            get
            {
                if (motion1 == null) return new RelayCommand(() => Motion1Clicking());
                return motion1;
            }
            set { motion1 = value; }
        }
        private void Motion1Clicking()
        {
            string msg = "@MT1" + Motion1Value + "$\r\n";
            SendData(msg);
        }

        //电机1停止
        private RelayCommand motion1Stop;
        public RelayCommand Motion1Stop
        {
            get
            {
                if (motion1Stop == null) return new RelayCommand(() => Motion1StopClicking());
                return motion1Stop;
            }
            set { motion1Stop = value; }
        }
        private void Motion1StopClicking()
        {
            Motion1Value = "1500";
            string msg = "@MT11500$\r\n";
            SendData(msg);
        }

        //电机2发送
        private RelayCommand motion2;
        public RelayCommand Motion2
        {
            get
            {
                if (motion2 == null) return new RelayCommand(() => Motion2Clicking());
                return motion2;
            }
            set { motion2 = value; }
        }
        private void Motion2Clicking()
        {
            string msg = "@MT2" + Motion2Value + "$\r\n";
            SendData(msg);
        }

        //电机2停止
        private RelayCommand motion2Stop;
        public RelayCommand Motion2Stop
        {
            get
            {
                if (motion2Stop == null) return new RelayCommand(() => Motion2StopClicking());
                return motion2Stop;
            }
            set { motion2Stop = value; }
        }
        private void Motion2StopClicking()
        {
            Motion2Value = "1500";
            string msg = "@MT21500$\r\n";
            SendData(msg);
        }

        //电机3发送
        private RelayCommand motion3;
        public RelayCommand Motion3
        {
            get
            {
                if (motion3 == null) return new RelayCommand(() => Motion3Clicking());
                return motion3;
            }
            set { motion3 = value; }
        }
        private void Motion3Clicking()
        {
            string msg = "@MT3" + Motion3Value + "$\r\n";
            SendData(msg);
        }

        //电机3停止
        private RelayCommand motion3Stop;
        public RelayCommand Motion3Stop
        {
            get
            {
                if (motion3Stop == null) return new RelayCommand(() => Motion3StopClicking());
                return motion3Stop;
            }
            set { motion3Stop = value; }
        }
        private void Motion3StopClicking()
        {
            Motion3Value = "1500";
            string msg = "@MT31500$\r\n";
            SendData(msg);
        }

        //电机4发送
        private RelayCommand motion4;
        public RelayCommand Motion4
        {
            get
            {
                if (motion4 == null) return new RelayCommand(() => Motion4Clicking());
                return motion4;
            }
            set { motion4 = value; }
        }
        private void Motion4Clicking()
        {
            string msg = "@MT4" + Motion4Value + "$\r\n";
            SendData(msg);
        }

        //电机4停止
        private RelayCommand motion4Stop;
        public RelayCommand Motion4Stop
        {
            get
            {
                if (motion4Stop == null) return new RelayCommand(() => Motion4StopClicking());
                return motion4Stop;
            }
            set { motion4Stop = value; }
        }
        private void Motion4StopClicking()
        {
            Motion4Value = "1500";
            string msg = "@MT41500$\r\n";
            SendData(msg);
        }

        //电机5发送
        private RelayCommand motion5;
        public RelayCommand Motion5
        {
            get
            {
                if (motion5 == null) return new RelayCommand(() => Motion5Clicking());
                return motion5;
            }
            set { motion5 = value; }
        }
        private void Motion5Clicking()
        {
            string msg = "@MT5" + Motion5Value + "$\r\n";
            SendData(msg);
        }

        //电机5停止
        private RelayCommand motion5Stop;
        public RelayCommand Motion5Stop
        {
            get
            {
                if (motion5Stop == null) return new RelayCommand(() => Motion5StopClicking());
                return motion5Stop;
            }
            set { motion5Stop = value; }
        }
        private void Motion5StopClicking()
        {
            Motion5Value = "1500";
            string msg = "@MT51500$\r\n";
            SendData(msg);
        }

        //发送数据到串口
        private void SendData(string msg)
        {
            Messenger.Default.Send<string>(msg, "NetworkSendMessage"); //注意参数一致
        }
    }
}
