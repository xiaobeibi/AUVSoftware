using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AUVSoftware.ViewModels
{
    public class MotionControlViewModel : ViewModelBase
    {
        //当前手柄类
        private Joystick curJoystick = null;



        public MotionControlViewModel()
        {
            // 注册开关消息
            Messenger.Default.Register<string>(this, "HandleMotionSwitch", HandleMotionSwitchReceiving);

            //初始化红色
            ConnectColor = "#FA234C";
            //初始化开关为Off
            HandleSwitch = false;
            //初始速度为0
            SliderValue = "1.5";
            AuvSpeedText = "0";

            //初始方位 0,0
            XAxisValue = "0";
            YAxisValue = "0";

            // 点击值初始化
            LeftMotorValue = "0";
            RightMotorValue = "0";

            // 左右舵板初始化
            LeftRudderBoardBindingText = "0";
            RightRudderBoardBindingText = "0";
        }

        private void HandleMotionSwitchReceiving(string msg)
        {
            HandleSwitch = msg.Equals("On");
        }

        //手柄颜色
        private string connectColor;
        public string ConnectColor
        {
            get { return connectColor; }
            set { connectColor = value; RaisePropertyChanged(() => ConnectColor); }
        }

        //速度条
        private string sliderValue;
        public string SliderValue
        {
            get { return sliderValue; }
            set { sliderValue = value; RaisePropertyChanged(() => SliderValue); }
        }

        //手柄开关
        private bool handleSwitch;
        public bool HandleSwitch
        {
            get { return handleSwitch; }
            set { handleSwitch = value; RaisePropertyChanged(() => HandleSwitch); }
        }

        //X轴进度
        private string xAxisValue;
        public string XAxisValue
        {
            get { return xAxisValue; }
            set { xAxisValue = value; RaisePropertyChanged(() => XAxisValue); }
        }

        //Y轴进度
        private string yAxisValue;
        public string YAxisValue
        {
            get { return yAxisValue; }
            set { yAxisValue = value; RaisePropertyChanged(() => YAxisValue); }
        }

        //AUV速度值
        private string auvSpeedText;
        public string AuvSpeedText
        {
            get { return auvSpeedText; }
            set { auvSpeedText = value; RaisePropertyChanged(() => AuvSpeedText); }
        }

        //左电机值
        private string leftMotorValue;
        public string LeftMotorValue
        {
            get { return leftMotorValue; }
            set { leftMotorValue = value; RaisePropertyChanged(() => LeftMotorValue); }
        }
        //右电机值
        private string rightMotorValue;
        public string RightMotorValue
        {
            get { return rightMotorValue; }
            set { rightMotorValue = value; RaisePropertyChanged(() => RightMotorValue); }
        }

        //连接发送
        private RelayCommand connectButton;
        public RelayCommand ConnectButton
        {
            get
            {
                if (connectButton == null) return new RelayCommand(() => ConnectButtonClicking());
                return connectButton;
            }
            set { connectButton = value; }
        }
        private void ConnectButtonClicking()
        {
            if (ConnectColor.Equals("#FA234C"))
            {
                try
                {
                    DirectInput dirInput = new DirectInput();
                    DeviceType typeJoystick = DeviceType.Joystick;
                    DeviceType typeGamepad = DeviceType.Gamepad;
                    IList<DeviceInstance> allDevices = dirInput.GetDevices();

                    foreach (DeviceInstance item in allDevices)
                    {
                        if (typeJoystick == item.Type || typeGamepad == item.Type)
                        {
                            curJoystick = new Joystick(dirInput, item.InstanceGuid);
                            curJoystick.Acquire();
                            // On
                            HandleSwitch = true;
                            Messenger.Default.Send<string>("On", "HandleMotionMessage"); //注意参数一致
                            // green
                            ConnectColor = "#A1FF00";
                            // 开启线程
                            Task.Run(new Action(JoyListening));
                        }
                    }
                }
                catch
                {
                    curJoystick = null;
                    // Off
                    HandleSwitch = false;
                    Messenger.Default.Send<string>("Error", "HandleMotionMessage"); //注意参数一致
                    // red
                    ConnectColor = "#FA234C";
                }

                if (ConnectColor.Equals("#FA234C"))
                {
                    curJoystick = null;
                    HandleSwitch = false;
                    Messenger.Default.Send<string>("Error", "HandleMotionMessage"); //注意参数一致
                }
            }
            else
            {
                curJoystick = null;
                HandleSwitch = false;
                Messenger.Default.Send<string>("Off", "HandleMotionMessage"); //注意参数一致
                ConnectColor = "#FA234C";
            }
        }

        // 手柄监听程序
        private void JoyListening()
        {
            int isJoysKeyNumber = -1;
            int isJoysKeyNumberdirection = -1;
            try
            {
                while (true)
                {
                    if (HandleSwitch)
                    {
                        JoystickState joys = curJoystick.GetCurrentState();

                        if (joys.Buttons.Count(B => B) > 0)
                        {
                            for (int Key = 0; Key < 10; Key++)
                            {
                                if (joys.Buttons[Key])
                                {
                                    if (isJoysKeyNumber != Key)
                                    {
                                        isJoysKeyNumber = Key;
                                        JoystickMoveEvent(Key);
                                        break;
                                    }
                                    else
                                    {
                                        isJoysKeyNumber = -1;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (joys.PointOfViewControllers[0] != -1)
                        {
                            int pointViewNumber = joys.PointOfViewControllers[0];
                            if (isJoysKeyNumberdirection != pointViewNumber)
                            {
                                isJoysKeyNumberdirection = pointViewNumber;
                                JoystickMoveEventDirection(pointViewNumber);
                            }
                            else
                            {
                                isJoysKeyNumberdirection = -1;
                            }
                        }
                        else
                        {
                            XYAxisHandle(joys.X + 1, joys.Y + 1);
                            JoystickMove(joys.X, joys.Y);
                            ZAxisHandle(joys.RotationZ);
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            catch
            {
                curJoystick = null;
                HandleSwitch = false;
                ConnectColor = "#FA234C";
                Messenger.Default.Send<string>("Error", "HandleMotionMessage"); //注意参数一致
            }
        }

        //手柄真实按钮
        private void JoystickMoveEvent(int keyNumber)
        {
            string msg = "";
            if (keyNumber == 6)
            {
                DisplayCommandMessageToView("上浮 Speed " + SliderValue + " 节");
                msg = "@MPU" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            }
            else if (keyNumber == 7)
            {
                DisplayCommandMessageToView("下潜 Speed " + SliderValue + " 节");
                msg = "@MPD" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            }
            else if (keyNumber == 4)
            {
                DisplayCommandMessageToView("定深 Speed " + SliderValue + " 节");
                //msg = "@MPU" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            }
            else if (keyNumber == 5)
            {
                DisplayCommandMessageToView("定速 Speed " + SliderValue + " 节");
                //msg = "@MPD" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            }
            else if (keyNumber == 0)
            {
                DisplayCommandMessageToView("左滚 Speed " + SliderValue + "节");
                msg = "@MRU" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            }
            else if (keyNumber == 1)
            {
                DisplayCommandMessageToView("后倾 Speed " + SliderValue + "节");
                msg = "@MRR" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            }
            else if (keyNumber == 2)
            {
                DisplayCommandMessageToView("右滚 Speed " + SliderValue + "节");
                msg = "@MRD" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            }
            else if (keyNumber == 3)
            {
                DisplayCommandMessageToView("前倾 Speed " + SliderValue + " 节");
                msg = "@MRL" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            }
            else if (keyNumber == 8)
            {
                DisplayCommandMessageToView("定高 Speed " + SliderValue + "节");
                //msg = "@MRF" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            }
            else if (keyNumber == 9)
            {
                DisplayCommandMessageToView("定艏 Speed " + SliderValue + "节");
                //msg = "@MRB" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            }

            SendData(msg);
        }
        //手柄方向键按钮
        private void JoystickMoveEventDirection(int keyNumber)
        {
            if (keyNumber == 0)
            {
                DisplayCommandMessageToView("前进 Speed " + SliderValue + " 节");
                string msg = "@MPF" + SpeedDataAnalysis(SliderValue) + "$\r\n";
                SendData(msg);
            }
            else if (keyNumber == 9000)
            {
                DisplayCommandMessageToView("右转 Speed " + SliderValue + " 节");
                string msg = "@MRB" + SpeedDataAnalysis(SliderValue) + "$\r\n";
                SendData(msg);
            }
            else if (keyNumber == 18000)
            {
                DisplayCommandMessageToView("后退 Speed " + SliderValue + " 节");
                string msg = "@MPB" + SpeedDataAnalysis(SliderValue) + "$\r\n";
                SendData(msg);
            }
            else if (keyNumber == 27000)
            {
                DisplayCommandMessageToView("左转 Speed " + SliderValue + " 节");
                string msg = "@MRF" + SpeedDataAnalysis(SliderValue) + "$\r\n";
                SendData(msg);
            }
        }

        //手柄左遥感方向 X Y 界面
        private void XYAxisHandle(int xValue, int yValue)
        {
            XAxisValue = (xValue / 65536.0 * 212.0).ToString();
            YAxisValue = (yValue / 65536.0 * 212.0).ToString();
        }

        // 手柄 X Y 控制程序
        private void JoystickMove(int keyX, int keyY)
        {
            double normalKeyX = Math.Round(keyX / 65535.0 * 200.0 - 100, 1);
            double normalKeyY = Math.Round(keyY / 65535.0 * 200.0 - 100, 1);

            string normalOperation = JoyOperation(normalKeyX, normalKeyY);
            string normalOperationAngle = JoyOperationAngle(normalKeyX, normalKeyY);

            if (normalKeyY < 0 && !normalOperation.Equals("0") && !normalOperationAngle.Equals("0"))
            {
                string msg = "@MY+" + normalOperation + normalOperationAngle + "$\r\n";
                DisplayCommandMessageToView(msg);
                SendData(msg);
                // 电机显示正
                SpeedPositiveMotor(normalOperation, normalOperationAngle);
            }
            else if (normalKeyY > 0 && !normalOperation.Equals("0") && !normalOperationAngle.Equals("0"))
            {
                string msg = "@MY-" + normalOperation + normalOperationAngle + "$\r\n";
                DisplayCommandMessageToView(msg);
                SendData(msg);
                // 电机显示负
                SpeedNegativeMotor(normalOperation, normalOperationAngle);
            }
            else
            {
                LeftMotorValue = "0";
                RightMotorValue = "0";
            }

            // 速度UI显示
            AuvSpeedText = normalOperation;
        }

        //手柄右遥感方向 Z
        private void ZAxisHandle(int zValue)
        {
            if (zValue < 2000)
            {
                if (double.Parse(SliderValue) < 6)
                {
                    SliderValue = (double.Parse(SliderValue) + 0.5).ToString();
                }
            }
            else if (zValue > 63000)
            {
                if (double.Parse(SliderValue) > 0)
                {
                    SliderValue = (double.Parse(SliderValue) - 0.5).ToString();
                }
            }
        }

        #region 运动控制按钮区
        //上浮按钮
        private RelayCommand spaceAButton;
        public RelayCommand SpaceAButton
        {
            get
            {
                if (spaceAButton == null) return new RelayCommand(() => SpaceAButtonClicking());
                return spaceAButton;
            }
            set { spaceAButton = value; }
        }
        private void SpaceAButtonClicking()
        {
            DisplayCommandMessageToView("上浮 Speed " + SliderValue + " 节");
            string msg = "@MPU" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            SendData(msg);
        }

        //下潜按钮
        private RelayCommand spaceBButton;
        public RelayCommand SpaceBButton
        {
            get
            {
                if (spaceBButton == null) return new RelayCommand(() => SpaceBButtonClicking());
                return spaceBButton;
            }
            set { spaceBButton = value; }
        }
        private void SpaceBButtonClicking()
        {
            DisplayCommandMessageToView("下潜 Speed " + SliderValue + " 节");
            string msg = "@MPD" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            SendData(msg);
        }

        //定深按钮
        private RelayCommand floatUpButton;
        public RelayCommand FloatUpButton
        {
            get
            {
                if (floatUpButton == null) return new RelayCommand(() => FloatUpButtonClicking());
                return floatUpButton;
            }
            set { floatUpButton = value; }
        }
        private void FloatUpButtonClicking()
        {
            DisplayCommandMessageToView("定深 Speed " + SliderValue + " 节");
            //string msg = "@MPU" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            //SendData(msg);
        }

        //定速按钮
        private RelayCommand diveDownButton;
        public RelayCommand DiveDownButton
        {
            get
            {
                if (diveDownButton == null) return new RelayCommand(() => DiveDownButtonClicking());
                return diveDownButton;
            }
            set { diveDownButton = value; }
        }
        private void DiveDownButtonClicking()
        {
            DisplayCommandMessageToView("定速 Speed " + SliderValue + " 节");
            //string msg = "@MPD" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            //SendData(msg);
        }

        //前进按钮
        private RelayCommand forwardButton;
        public RelayCommand ForwardButton
        {
            get
            {
                if (forwardButton == null) return new RelayCommand(() => ForwardButtonClicking());
                return forwardButton;
            }
            set { forwardButton = value; }
        }
        private void ForwardButtonClicking()
        {
            DisplayCommandMessageToView("前进 Speed " + SliderValue + " 节");
            string msg = "@MPF" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            SendData(msg);
        }

        //后退按钮
        private RelayCommand backOffButton;
        public RelayCommand BackOffButton
        {
            get
            {
                if (backOffButton == null) return new RelayCommand(() => BackOffButtonClicking());
                return backOffButton;
            }
            set { backOffButton = value; }
        }
        private void BackOffButtonClicking()
        {
            DisplayCommandMessageToView("后退 Speed " + SliderValue + " 节");
            string msg = "@MPB" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            SendData(msg);
        }

        //左转按钮
        private RelayCommand shiftLeftButton;
        public RelayCommand ShiftLeftButton
        {
            get
            {
                if (shiftLeftButton == null) return new RelayCommand(() => ShiftLeftButtonClicking());
                return shiftLeftButton;
            }
            set { shiftLeftButton = value; }
        }
        private void ShiftLeftButtonClicking()
        {
            DisplayCommandMessageToView("左转 Speed " + SliderValue + " 节");
            string msg = "@MRF" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            SendData(msg);
        }

        //右转按钮
        private RelayCommand shiftRightButton;
        public RelayCommand ShiftRightButton
        {
            get
            {
                if (shiftRightButton == null) return new RelayCommand(() => ShiftRightButtonClicking());
                return shiftRightButton;
            }
            set { shiftRightButton = value; }
        }
        private void ShiftRightButtonClicking()
        {
            DisplayCommandMessageToView("右转 Speed " + SliderValue + " 节");
            string msg = "@MRB" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            SendData(msg);
        }

        //前倾按钮
        private RelayCommand leanForwardButton;
        public RelayCommand LeanForwardButton
        {
            get
            {
                if (leanForwardButton == null) return new RelayCommand(() => LeanForwardButtonClicking());
                return leanForwardButton;
            }
            set { leanForwardButton = value; }
        }
        private void LeanForwardButtonClicking()
        {
            DisplayCommandMessageToView("前倾 Speed " + SliderValue + " 节");
            string msg = "@MRL" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            SendData(msg);
        }

        //后倾按钮
        private RelayCommand backwardTiltButton;
        public RelayCommand BackwardTiltButton
        {
            get
            {
                if (backwardTiltButton == null) return new RelayCommand(() => BackwardTiltButtonClicking());
                return backwardTiltButton;
            }
            set { backwardTiltButton = value; }
        }
        private void BackwardTiltButtonClicking()
        {
            DisplayCommandMessageToView("后倾 Speed " + SliderValue + "节");
            string msg = "@MRR" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            SendData(msg);
        }

        //左滚按钮
        private RelayCommand rollLeftButton;
        public RelayCommand RollLeftButton
        {
            get
            {
                if (rollLeftButton == null) return new RelayCommand(() => RollLeftButtonClicking());
                return rollLeftButton;
            }
            set { rollLeftButton = value; }
        }
        private void RollLeftButtonClicking()
        {
            DisplayCommandMessageToView("左滚 Speed " + SliderValue + "节");
            string msg = "@MRU" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            SendData(msg);
        }

        //右滚按钮
        private RelayCommand rollRightButton;
        public RelayCommand RollRightButton
        {
            get
            {
                if (rollRightButton == null) return new RelayCommand(() => RollRightButtonClicking());
                return rollRightButton;
            }
            set { rollRightButton = value; }
        }
        private void RollRightButtonClicking()
        {
            DisplayCommandMessageToView("右滚 Speed " + SliderValue + "节");
            string msg = "@MRD" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            SendData(msg);
        }

        //定高按钮
        private RelayCommand turnLeftButton;
        public RelayCommand TurnLeftButton
        {
            get
            {
                if (turnLeftButton == null) return new RelayCommand(() => TurnLeftButtonClicking());
                return turnLeftButton;
            }
            set { turnLeftButton = value; }
        }
        private void TurnLeftButtonClicking()
        {
            DisplayCommandMessageToView("定高 Speed " + SliderValue + "节");
            //string msg = "@MRF" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            //SendData(msg);
        }

        //定艏按钮
        private RelayCommand turnRightButton;
        public RelayCommand TurnRightButton
        {
            get
            {
                if (turnRightButton == null) return new RelayCommand(() => TurnRightButtonClicking());
                return turnRightButton;
            }
            set { turnRightButton = value; }
        }
        private void TurnRightButtonClicking()
        {
            DisplayCommandMessageToView("定艏 Speed " + SliderValue + "节");
            //string msg = "@MRB" + SpeedDataAnalysis(SliderValue) + "$\r\n";
            //SendData(msg);
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

        //解析速度数据——Z
        private string SpeedDataAnalysis(string speedData)
        {
            string ans = "0";
            if (!string.IsNullOrEmpty(speedData))
            {
                
                switch (speedData)
                {
                    case "0.5":
                        ans = "1";
                        break;
                    case "1":
                        ans = "2";
                        break;
                    case "1.5":
                        ans = "3";
                        break;
                    case "2":
                        ans = "4";
                        break;
                    case "2.5":
                        ans = "5";
                        break;
                    case "3":
                        ans = "6";
                        break;
                    case "3.5":
                        ans = "7";
                        break;
                    case "4":
                        ans = "8";
                        break;
                    case "4.5":
                        ans = "9";
                        break;
                    case "5":
                        ans = "A";
                        break;
                    case "5.5":
                        ans = "B";
                        break;
                    case "6":
                        ans = "C";
                        break;
                    default:
                        ans = "0";
                        break;
                }
            }
            return ans;
        }

        // 获取遥感角度
        private string JoyOperationAngle(double keyX, double keyY)
        {
            double hudu = Math.Atan2(-keyY, keyX);
            if (hudu > -Math.PI && hudu < 0)
            {
                hudu += 2 * Math.PI;
            }
            double jiaodu = hudu * 180 / Math.PI;

            if ((jiaodu >= 75 && jiaodu <= 105) || (jiaodu >= 255 && jiaodu <= 285))
            {
                return "+0";
            }
            else if ((jiaodu >= 105 && jiaodu <= 135) || (jiaodu >= 225 && jiaodu <= 255))
            {
                return "+1";
            }
            else if ((jiaodu >= 135 && jiaodu <= 165) || (jiaodu >= 195 && jiaodu <= 225))
            {
                return "+2";
            }
            else if ((jiaodu >= 165 && jiaodu <= 175) || (jiaodu >= 185 && jiaodu <= 195))
            {
                return "+3";
            }
            else if (jiaodu >= 175 && jiaodu <= 185)
            {
                return "+4";
            }
            else if ((jiaodu >= 45 && jiaodu <= 75) || (jiaodu >= 255 && jiaodu <= 315))
            {
                return "-1";
            }
            else if ((jiaodu >= 15 && jiaodu <= 45) || (jiaodu >= 315 && jiaodu <= 345))
            {
                return "-2";
            }
            else if ((jiaodu >= 5 && jiaodu <= 15) || (jiaodu >= 345 && jiaodu <= 355))
            {
                return "-3";
            }
            else if ((jiaodu >= 0 && jiaodu <= 5) || (jiaodu >= 355 && jiaodu <= 360))
            {
                return "-4";
            }
            else
            {
                return "+0";
            }
        }

        // 获取遥感速度大小——x y
        private string JoyOperation(double keyX, double keyY)
        {
            double keyOperation = Math.Round(Math.Sqrt(keyX * keyX + keyY * keyY), 1);
            if (keyOperation >= 0 && keyOperation < 30)
            {
                return "0";
            }
            else if (keyOperation >= 30 && keyOperation < 50)
            {
                return "1";
            }
            else if (keyOperation >= 50 && keyOperation < 70)
            {
                return "2";
            }
            else if (keyOperation >= 70 && keyOperation < 90)
            {
                return "3";
            }
            else if (keyOperation >= 90 && keyOperation < 100)
            {
                return "4";
            }
            else if (keyOperation >= 100)
            {
                return "5";
            }
            else
            {
                return "0";
            }
        }

        //速度正值点击处理函数
        private void SpeedPositiveMotor(string Operation, string OperationAngle)
        {
            LeftMotorValue = (Convert.ToInt32(Operation) - Convert.ToInt32(OperationAngle)).ToString();
            RightMotorValue = (Convert.ToInt32(Operation) + Convert.ToInt32(OperationAngle)).ToString();
        }
        //速度正值点击处理函数
        private void SpeedNegativeMotor(string Operation, string OperationAngle)
        {
            LeftMotorValue = (-Convert.ToInt32(Operation) + Convert.ToInt32(OperationAngle)).ToString();
            RightMotorValue = (-Convert.ToInt32(Operation) - Convert.ToInt32(OperationAngle)).ToString();
        }
    }
}
