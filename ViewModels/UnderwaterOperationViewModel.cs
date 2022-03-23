using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AUVSoftware.ViewModels
{
    public class UnderwaterOperationViewModel : ViewModelBase
    {
        public UnderwaterOperationViewModel()
        {
            // 左右舵板初始化
            LeftRudderBoardBindingText = "0";
            RightRudderBoardBindingText = "0";

            // 定艏值初始化
            FixedBowValue = "0";
            // 定姿Pitch初始化
            FixedPosturePitchValue = "0";
            // 定姿Roll初始化
            FixedPostureRollValue = "0";
            // 定速值初始化
            ConstantSpeedValue = "0";
            // 定高值初始化
            FixedHeightValue = "0";
            // 定深初始化
            FixedDepthValue = "0";
        }

        #region 自动控制命令
        //定艏值
        private string fixedBowValue;
        public string FixedBowValue
        {
            get { return fixedBowValue; }
            set { fixedBowValue = value; RaisePropertyChanged(() => FixedBowValue); }
        }
        //定艏按钮
        private RelayCommand fixedBowButton;
        public RelayCommand FixedBowButton
        {
            get
            {
                if (fixedBowButton == null) return new RelayCommand(() => FixedBowButtonClicking(), CanSendFixedBowValue);
                return fixedBowButton;
            }
            set { fixedBowButton = value; }
        }
        private void FixedBowButtonClicking()
        {
            DisplayCommandMessageToView("定艏: " + FixedBowValue + " 度\r\n");
            string msg = "@MAH" + FixedBowValue + "$\r\n";
            SendData(msg);
        }
        private bool CanSendFixedBowValue()
        {
            if (int.TryParse(FixedBowValue, out int result))
            {
                if (result <= 180 && result >= -180)
                {
                    return true;
                }
            }
            return false;
        }

        // 定姿Pitch
        private string fixedPosturePitchValue;
        public string FixedPosturePitchValue
        {
            get { return fixedPosturePitchValue; }
            set { fixedPosturePitchValue = value; RaisePropertyChanged(() => FixedPosturePitchValue); }
        }
        // 定姿Roll
        private string fixedPostureRollValue;
        public string FixedPostureRollValue
        {
            get { return fixedPostureRollValue; }
            set { fixedPostureRollValue = value; RaisePropertyChanged(() => FixedPostureRollValue); }
        }
        //定艏按钮
        private RelayCommand fixedPostureButton;
        public RelayCommand FixedPostureButton
        {
            get
            {
                if (fixedPostureButton == null) return new RelayCommand(() => FixedPostureButtonClicking(), CanSendFixedPostureValue);
                return fixedPostureButton;
            }
            set { fixedPostureButton = value; }
        }
        private void FixedPostureButtonClicking()
        {
            DisplayCommandMessageToView("定姿: " + FixedPosturePitchValue + " , " + FixedPostureRollValue + " 度\r\n");
            string msg = "@MAA" + FixedPosturePitchValue + "," + FixedPostureRollValue + "$\r\n";
            SendData(msg);
        }
        private bool CanSendFixedPostureValue()
        {
            if (int.TryParse(FixedPosturePitchValue, out int resultPitch) && int.TryParse(FixedPostureRollValue, out int resultRoll))
            {
                if (resultPitch <= 80 && resultPitch >= -80 && resultRoll <= 80 && resultRoll >= -80)
                {
                    return true;
                }
            }
            return false;
        }

        //定速值
        private string constantSpeedValue;
        public string ConstantSpeedValue
        {
            get { return constantSpeedValue; }
            set { constantSpeedValue = value; RaisePropertyChanged(() => ConstantSpeedValue); }
        }
        //定速按钮
        private RelayCommand constantSpeedButton;
        public RelayCommand ConstantSpeedButton
        {
            get
            {
                if (constantSpeedButton == null) return new RelayCommand(() => ConstantSpeedButtonClicking());
                return constantSpeedButton;
            }
            set { constantSpeedButton = value; }
        }
        private void ConstantSpeedButtonClicking()
        {
            DisplayCommandMessageToView("定速: " + ConstantSpeedValue + " m/s\r\n");
            string msg = "@MAV" + ConstantSpeedValue + "$\r\n";
            SendData(msg);
        }

        //定高值
        private string fixedHeightValue;
        public string FixedHeightValue
        {
            get { return fixedHeightValue; }
            set { fixedHeightValue = value; RaisePropertyChanged(() => FixedHeightValue); }
        }
        //定高按钮
        private RelayCommand fixedHeightButton;
        public RelayCommand FixedHeightButton
        {
            get
            {
                if (fixedHeightButton == null) return new RelayCommand(() => FixedHeightButtonClicking(), CanSendFixedHeightValue);
                return fixedHeightButton;
            }
            set { fixedHeightButton = value; }
        }
        private void FixedHeightButtonClicking()
        {
            DisplayCommandMessageToView("定高: " + Convert.ToSingle(FixedHeightValue) + " m\r\n");
            string msg = "@MAE" + Convert.ToSingle(FixedHeightValue) + "$\r\n";
            SendData(msg);
        }
        private bool CanSendFixedHeightValue()
        {
            if (float.TryParse(FixedHeightValue, out float resultHeight))
            {
                if (resultHeight <= 30 && resultHeight >= 0 )
                {
                    return true;
                }
            }
            return false;
        }

        //定深值
        private string fixedDepthValue;
        public string FixedDepthValue
        {
            get { return fixedDepthValue; }
            set { fixedDepthValue = value; RaisePropertyChanged(() => FixedDepthValue); }
        }
        //定深按钮
        private RelayCommand fixedDepthButton;
        public RelayCommand FixedDepthButton
        {
            get
            {
                if (fixedDepthButton == null) return new RelayCommand(() => FixedDepthButtonClicking(), CanSendFixedDepthValue);
                return fixedDepthButton;
            }
            set { fixedDepthButton = value; }
        }
        private void FixedDepthButtonClicking()
        {
            DisplayCommandMessageToView("定深: " + Convert.ToSingle(FixedDepthValue) + " m\r\n");
            string msg = "@MAD" + Convert.ToSingle(FixedDepthValue) + "$\r\n";
            SendData(msg);
        }
        private bool CanSendFixedDepthValue()
        {
            if (float.TryParse(FixedDepthValue, out float resultDepth))
            {
                if (resultDepth <= 10 && resultDepth >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        //总控发送按钮
        private RelayCommand masterControlButton;
        public RelayCommand MasterControlButton
        {
            get
            {
                if (masterControlButton == null) return new RelayCommand(() => MasterControlButtonClicking());
                return masterControlButton;
            }
            set { masterControlButton = value; }
        }
        private void MasterControlButtonClicking()
        {
            //DisplayCommandMessageToView("总命令: " + Convert.ToSingle(FixedDepthValue) + " \r\n");
            //string msg = "@MAD" + Convert.ToSingle(FixedDepthValue) + "$\r\n";
            //SendData(msg);
        }

        //总控停止按钮
        private RelayCommand masterStopButton;
        public RelayCommand MasterStopButton
        {
            get
            {
                if (masterStopButton == null) return new RelayCommand(() => MasterStopButtonClicking());
                return masterStopButton;
            }
            set { masterStopButton = value; }
        }
        private void MasterStopButtonClicking()
        {
            DisplayCommandMessageToView(" 制动 \r\n");
            string msg = "@MAS$\r\n";
            SendData(msg);
        }

        #endregion


        #region 舵板控制按钮区
        //左舵板绑定文本
        private string leftRudderBoardBindingText;
        public string LeftRudderBoardBindingText
        {
            get { return leftRudderBoardBindingText; }
            set { leftRudderBoardBindingText = value; RaisePropertyChanged(() => LeftRudderBoardBindingText); }
        }
        //右舵板绑定文本
        private string rightRudderBoardBindingText;
        public string RightRudderBoardBindingText
        {
            get { return rightRudderBoardBindingText; }
            set { rightRudderBoardBindingText = value; RaisePropertyChanged(() => RightRudderBoardBindingText); }
        }
        //左舵板发送命令
        private RelayCommand leftRudderBoardSendCommand;
        public RelayCommand LeftRudderBoardSendCommand
        {
            get
            {
                if (leftRudderBoardSendCommand == null) return new RelayCommand(() => LeftRudderBoardSending());
                return leftRudderBoardSendCommand;
            }
            set { leftRudderBoardSendCommand = value; }
        }
        private void LeftRudderBoardSending()
        {
            string msg = Convert.ToInt32(LeftRudderBoardBindingText) >= 0
                ? "@MDL+" + LeftRudderBoardBindingText + "$\r\n"
                : "@MDL" + LeftRudderBoardBindingText + "$\r\n";
            DisplayCommandMessageToView(msg);
            SendData(msg);
        }
        //右舵板发送命令
        private RelayCommand rightRudderBoardSendCommand;
        public RelayCommand RightRudderBoardSendCommand
        {
            get
            {
                if (rightRudderBoardSendCommand == null) return new RelayCommand(() => RightRudderBoardSending());
                return rightRudderBoardSendCommand;
            }
            set { rightRudderBoardSendCommand = value; }
        }
        private void RightRudderBoardSending()
        {
            string msg = Convert.ToInt32(RightRudderBoardBindingText) >= 0
                ? "@MDR+" + RightRudderBoardBindingText + "$\r\n"
                : "@MDR" + RightRudderBoardBindingText + "$\r\n";
            DisplayCommandMessageToView(msg);
            SendData(msg);
        }
        //全部舵板发送命令
        private RelayCommand rudderBoardSendCommand;
        public RelayCommand RudderBoardSendCommand
        {
            get
            {
                if (rudderBoardSendCommand == null) return new RelayCommand(() => RudderBoardSending());
                return rudderBoardSendCommand;
            }
            set { rudderBoardSendCommand = value; }
        }
        private void RudderBoardSending()
        {
            string msg = Convert.ToInt32(LeftRudderBoardBindingText) >= 0
                ? "@MDL+" + LeftRudderBoardBindingText + "$\r\n"
                : "@MDL" + LeftRudderBoardBindingText + "$\r\n";
            msg += Convert.ToInt32(RightRudderBoardBindingText) >= 0
                ? "@MDR+" + RightRudderBoardBindingText + "$\r\n"
                : "@MDR" + RightRudderBoardBindingText + "$\r\n";
            DisplayCommandMessageToView(msg);
            SendData(msg);
        }
        #endregion

        //发送命令给命令信息框展示
        private void DisplayCommandMessageToView(string msg)
        {
            Messenger.Default.Send<string>(msg, "DisplayCommandMessage"); //注意参数一致
        }

        //发送数据到网口
        private void SendData(string msg)
        {
            Messenger.Default.Send<string>(msg, "NetworkSendMessage"); //注意参数一致
        }
    }
}
