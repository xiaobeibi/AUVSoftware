using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUVSoftware.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            ImageSource = "LightBlue";

            // 注册提示消息
            Messenger.Default.Register<string>(this, "LoginErrorMessage", ButtonColorMessageReceiving);
        }

        private void ButtonColorMessageReceiving(string msg)
        {
            if (msg.Equals("连接已断开") || msg.Equals("服务器连接失败") || msg.Equals("服务器已断开"))
            {
                ImageSource = "LightBlue";
            }
            else if (msg.Equals("已重新连接") || msg.Equals("连接成功"))
            {
                ImageSource = "Lime";
            }
        }

        //图片栏
        private string imageSource;
        public string ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; RaisePropertyChanged(() => ImageSource); }
        }
    }
}
