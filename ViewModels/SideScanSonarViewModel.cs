using AUVSoftware.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AUVSoftware.ViewModels
{
    public class SideScanSonarViewModel : ViewModelBase
    {

        public SideScanSonarViewModel()
        {
            // 注册开关消息
            Messenger.Default.Register<string>(this, "SideScanSonarMessage", SideScanSonarMessageReceiving);

            // 侧扫状态信息初始化
            SideScanStatusInformation = "white";

            // 侧扫发送信息初始化
            isSideScanLaunchStatus = "0";

            // 侧扫心跳信息
            isSideScanHeartbeatStatus = "";

            // 侧扫量程发送信息初始化
            SideScanSonarRangeValue = "15";

            // 侧扫增益信息初始化
            SideScanSonarGainInformation = "30";

            // 侧扫水质信息初始化
            SideScanSonarWaterQualityInformation = "1";

            // Radiobutton初始化
            RadioButtons = new List<CompBottonModel>()
            {
                new CompBottonModel(){ Content="15", IsCheck = true },
                new CompBottonModel(){ Content="30", IsCheck = false },
                new CompBottonModel(){ Content="45", IsCheck = false },
                new CompBottonModel(){ Content="60", IsCheck = false },
                new CompBottonModel(){ Content="75", IsCheck = false },
                new CompBottonModel(){ Content="90", IsCheck = false },
                new CompBottonModel(){ Content="120", IsCheck = false },
                new CompBottonModel(){ Content="150", IsCheck = false },
            };

            // 开启线程
            Task.Run(new Action(HeartbeatThread));

        }

        // 心跳线程
        private void HeartbeatThread()
        {
            while (true)
            {
                string currentHeartbeat = isSideScanHeartbeatStatus;
                Thread.Sleep(8000);
                if (isSideScanHeartbeatStatus.Equals(currentHeartbeat))
                {
                    SideScanStatusInformation = "white";
                }
            }
        }

        private void SideScanSonarMessageReceiving(string msg)
        {
            string[] CurrentData = msg.Split(',');

            if (CurrentData[0].Equals("$GPSTA") && (CurrentData.Length == 6))
            {
                if (CurrentData[3].Equals("0"))
                {
                    isSideScanStatus = false;
                    SideScanStatusInformation = "Red";
                }
                else if (CurrentData[3].Equals("1"))
                {
                    isSideScanStatus = true;
                    SideScanStatusInformation = "#FF00FF4C";
                }

                isSideScanHeartbeatStatus = CurrentData[2];
            }
        }

        #region 侧扫按钮信息

        // 侧扫发送信息
        private bool isSideScanStatus = false;

        // 侧扫发射信息
        private string isSideScanLaunchStatus;

        // 侧扫心跳信息
        private string isSideScanHeartbeatStatus;

        // 侧扫量程发送信息
        private string sideScanSonarRangeValue;
        public string SideScanSonarRangeValue
        {
            get { return sideScanSonarRangeValue; }
            set { sideScanSonarRangeValue = value; RaisePropertyChanged(() => SideScanSonarRangeValue); }
        }

        //侧扫状态数据
        private string sideScanStatusInformation;
        public string SideScanStatusInformation
        {
            get { return sideScanStatusInformation; }
            set { sideScanStatusInformation = value; RaisePropertyChanged(() => SideScanStatusInformation); }
        }

        //侧扫发送按钮
        private RelayCommand sideScanSonarSendButton;
        public RelayCommand SideScanSonarSendButton
        {
            get
            {
                if (sideScanSonarSendButton == null) return new RelayCommand(() => SideScanSonarSendButtonClicking(), IsSideScanSonarSendValue);
                return sideScanSonarSendButton;
            }
            set { sideScanSonarSendButton = value; }
        }
        private void SideScanSonarSendButtonClicking()
        {
            SendSideScanData("$GPOTH,256,*75\r\n");
            DisplayCommandMessageToView("开启侧扫！");
        }
        private bool IsSideScanSonarSendValue()
        {
            return !isSideScanStatus;
        }

        //侧扫发射按钮
        private RelayCommand sideScanSonarLaunchButton;
        public RelayCommand SideScanSonarLaunchButton
        {
            get
            {
                if (sideScanSonarLaunchButton == null) return new RelayCommand(() => SideScanSonarLaunchButtonClicking(), IsSideScanSonarLaunchValue);
                return sideScanSonarLaunchButton;
            }
            set { sideScanSonarLaunchButton = value; }
        }
        private void SideScanSonarLaunchButtonClicking()
        {
            SendSideScanData("$GPPAR,1,450,1,0,*79\r\n");
            //SendSideScanData("$GPPAR,1,450,1,0*55\r\n");
            isSideScanLaunchStatus = "1"; // 不可以发射
            DisplayCommandMessageToView("发射侧扫！");
        }
        private bool IsSideScanSonarLaunchValue()
        {
            return isSideScanStatus && isSideScanLaunchStatus.Equals("0");

        }

        //侧扫不发射按钮
        private RelayCommand sideScanSonarNoLaunchButton;
        public RelayCommand SideScanSonarNoLaunchButton
        {
            get
            {
                if (sideScanSonarNoLaunchButton == null) return new RelayCommand(() => SideScanSonarNoLaunchButtonClicking(), IsSideScanSonarNoLaunchValue);
                return sideScanSonarNoLaunchButton;
            }
            set { sideScanSonarNoLaunchButton = value; }
        }
        private void SideScanSonarNoLaunchButtonClicking()
        {
            SendSideScanData("$GPPAR,1,450,0,0,*78\r\n");
            //SendSideScanData("$GPPAR,1,450,0,0*54\r\n");
            isSideScanLaunchStatus = "0"; // 可以发射
            DisplayCommandMessageToView("关闭发射侧扫！");
        }
        private bool IsSideScanSonarNoLaunchValue()
        {
            return isSideScanLaunchStatus.Equals("1");
        }

        //侧扫停止按钮
        private RelayCommand sideScanSonarStopButton;
        public RelayCommand SideScanSonarStopButton
        {
            get
            {
                if (sideScanSonarStopButton == null) return new RelayCommand(() => SideScanSonarStopButtonClicking(), IsSideScanSonarStopValue);
                return sideScanSonarStopButton;
            }
            set { sideScanSonarStopButton = value; }
        }
        private void SideScanSonarStopButtonClicking()
        {
            SendSideScanData("$GPOTH,128,*7F\r\n");
            DisplayCommandMessageToView("停止侧扫！");
        }
        private bool IsSideScanSonarStopValue()
        {
            return isSideScanStatus;
        }

        #endregion

        #region 侧扫量程发送命令
        // RadioButtons
        private List<CompBottonModel> radioButtons;
        public List<CompBottonModel> RadioButtons
        {
            get { return radioButtons; }
            set
            {
                radioButtons = value; RaisePropertyChanged(() => RadioButtons);
            }
        }
        private CompBottonModel radioButton;
        public CompBottonModel RadioButton
        {
            get { return radioButton; }
            set { radioButton = value; RaisePropertyChanged(() => RadioButton); }
        }

        private RelayCommand radioCheckCommand;
        public RelayCommand RadioCheckCommand
        {
            get
            {
                if (radioCheckCommand == null)
                    radioCheckCommand = new RelayCommand(() => ExcuteRadioCommand());
                return radioCheckCommand;

            }
            set { radioCheckCommand = value; }
        }
        private void ExcuteRadioCommand()
        {
            SideScanSonarRangeValue = RadioButtons.Where(p => p.IsCheck).First().Content;
        }

        //侧扫量程发送命令
        private RelayCommand sideScanSonarRangeButton;
        public RelayCommand SideScanSonarRangeButton
        {
            get
            {
                if (sideScanSonarRangeButton == null) return new RelayCommand(() => SideScanSonarRangeButtonClicking(), IsSideScanSonarRangeValue);
                return sideScanSonarRangeButton;
            }
            set { sideScanSonarRangeButton = value; }
        }
        private void SideScanSonarRangeButtonClicking()
        {
            string hhValue = CheckHH("GPPAR,0,450," + SideScanSonarRangeValue + ",0,");
            SendSideScanData("$GPPAR,0,450," + SideScanSonarRangeValue + ",0,*" + hhValue + "\r\n");

            //string hhValue = CheckHH("GPPAR,0,450," + SideScanSonarRangeValue + ",0");
            //SendSideScanData("$GPPAR,0,450," + SideScanSonarRangeValue + ",0*" + hhValue + "\r\n");

            DisplayCommandMessageToView("侧扫量程 " + SideScanSonarRangeValue + " 发送！");
        }
        private bool IsSideScanSonarRangeValue()
        {
            return isSideScanStatus;
        }

        #endregion

        #region 侧扫增益发送命令

        //侧扫增益状态数据
        private string sideScanSonarGainInformation;
        public string SideScanSonarGainInformation
        {
            get { return sideScanSonarGainInformation; }
            set { sideScanSonarGainInformation = value; RaisePropertyChanged(() => SideScanSonarGainInformation); }
        }

        //侧扫增益发送按钮
        private RelayCommand sideScanSonarGainButton;
        public RelayCommand SideScanSonarGainButton
        {
            get
            {
                if (sideScanSonarGainButton == null) return new RelayCommand(() => SideScanSonarGainButtonClicking(), IsSideScanSonarGainValue);
                return sideScanSonarGainButton;
            }
            set { sideScanSonarGainButton = value; }
        }
        private void SideScanSonarGainButtonClicking()
        {
            string hhValue = CheckHH("GPPAR,2,450," + SideScanSonarGainInformation + ",0,");
            SendSideScanData("$GPPAR,2,450," + SideScanSonarGainInformation + ",0,*" + hhValue + "\r\n");

            //string hhValue = CheckHH("GPPAR,2,450," + SideScanSonarGainInformation + ",0");
            //SendSideScanData("$GPPAR,2,450," + SideScanSonarGainInformation + ",0*" + hhValue + "\r\n");

            DisplayCommandMessageToView("侧扫增益 " + SideScanSonarGainInformation + " 发送！");
        }
        private bool IsSideScanSonarGainValue()
        {
            return isSideScanStatus;
        }

        #endregion

        #region 侧扫水质发送命令

        //侧扫水质状态数据
        private string sideScanSonarWaterQualityInformation;
        public string SideScanSonarWaterQualityInformation
        {
            get { return sideScanSonarWaterQualityInformation; }
            set { sideScanSonarWaterQualityInformation = value; RaisePropertyChanged(() => SideScanSonarWaterQualityInformation); }
        }
        //侧扫水质发送按钮
        private RelayCommand sideScanSonarWaterQualityButton;
        public RelayCommand SideScanSonarWaterQualityButton
        {
            get
            {
                if (sideScanSonarWaterQualityButton == null) return new RelayCommand(() => SideScanSonarWaterQualityButtonClicking(), IsSideScanSonarWaterQualityValue);
                return sideScanSonarWaterQualityButton;
            }
            set { sideScanSonarWaterQualityButton = value; }
        }
        private void SideScanSonarWaterQualityButtonClicking()
        {
            //string hhValue = CheckHH("GPPAR,3,450," + SideScanSonarWaterQualityInformation + ",0,");
            //SendSideScanData("$GPPAR,3,450," + SideScanSonarWaterQualityInformation + ",0,*" + hhValue + "\r\n");

            string hhValue = CheckHH("GPPAR,3,450," + SideScanSonarWaterQualityInformation + ",0");
            SendSideScanData("$GPPAR,3,450," + SideScanSonarWaterQualityInformation + ",0*" + hhValue + "\r\n");

            DisplayCommandMessageToView("侧扫水质 " + SideScanSonarWaterQualityInformation + " 发送！");
        }
        private bool IsSideScanSonarWaterQualityValue()
        {
            return isSideScanStatus;
        }
        #endregion


        // 测试校验合
        private string CheckHH(string sentence)
        {
            byte checksumValue = 0;
            char[] checkChar = sentence.ToCharArray();
            foreach (char check in checkChar)
            {
                checksumValue ^= (byte)check;
            }
            return Convert.ToString(checksumValue, 16).ToUpper();
        }

        //发送命令给命令信息框展示
        private void DisplayCommandMessageToView(string msg)
        {
            Messenger.Default.Send<string>(msg, "DisplayCommandMessage"); //注意参数一致
        }

        //发送数据到网口
        private void SendSideScanData(string msg)
        {
            Messenger.Default.Send<string>(msg, "SideScanSonarSend"); //注意参数一致
        }
    }
}
