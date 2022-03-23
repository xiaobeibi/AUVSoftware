using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUVSoftware.ViewModels
{
    public class TransferGPSViewModel : ViewModelBase
    {
        public TransferGPSViewModel()
        {
            // 初始化经纬度
            Longitude = "";
            Latitude = "";

            //注册网口发过来的消息
            Messenger.Default.Register<string>(this, "NetworkReceiveMessage", NetworkReceiveGPSMessageTransfer);
        }

        //从串口那接收来的消息
        private void NetworkReceiveGPSMessageTransfer(string msg)
        {
            string[] CurrentData = msg.Split(',');

            if (CurrentData[0].Equals("@SS") && CurrentData[CurrentData.Length - 1].Equals("$"))
            {
                if (CurrentData[1].Equals("E") && CurrentData[3].Equals("N") && !string.IsNullOrEmpty(CurrentData[2]) && !string.IsNullOrEmpty(CurrentData[4]))
                {
                    Longitude = CurrentData[2];
                    Latitude = CurrentData[4];

                    string sendMsg = CurrentData[2] + "," + CurrentData[4];
                    Messenger.Default.Send<string>(sendMsg, "TransferGPSMessage"); //注意：token参数一致
                }
            }
        }

        //GPS经度
        private string longitude;
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; RaisePropertyChanged(() => Longitude); }
        }

        //GPS纬度
        private string latitude;
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; RaisePropertyChanged(() => Latitude); }
        }

    }
}
