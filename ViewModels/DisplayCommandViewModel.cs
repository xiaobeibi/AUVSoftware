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
    public class DisplayCommandViewModel : ViewModelBase
    {
        public DisplayCommandViewModel()
        {
            // 注册开关消息
            Messenger.Default.Register<string>(this, "DisplayCommandMessage", DisplayCommandMessageReceiving);

            // 初始化文本
            DisplayText = "";
        }

        private void DisplayCommandMessageReceiving(string msg)
        {
            //时间戳
            DateTime NetWorknow = DateTime.Now;
            string NetWorkTimeDateString = string.Format("{0}-{1}-{2}: ",
                NetWorknow.Hour.ToString("00"),
                NetWorknow.Minute.ToString("00"),
                NetWorknow.Second.ToString("00"));

            //前端界面显示
            DisplayText += NetWorkTimeDateString + msg + "\n" + "\r\n";

            if (DisplayText.Length > 1000)
            {
                DisplayText = "";
            }
        }

        // 信息框文本
        private string displayText;
        public string DisplayText
        {
            get { return displayText; }
            set { displayText = value; RaisePropertyChanged(() => DisplayText); }
        }

        //清空发送
        private RelayCommand displayCommandButton;
        public RelayCommand DisplayCommandButton
        {
            get
            {
                if (displayCommandButton == null) return new RelayCommand(() => DisplayCommandButtonClicking());
                return displayCommandButton;
            }
            set { displayCommandButton = value; }
        }
        private void DisplayCommandButtonClicking()
        {
            DisplayText = "";
        }


    }
}
