using AUVSoftware.Models;
using AUVSoftware.UserControls;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AUVSoftware.Views
{
    /// <summary>
    /// SideScanSonarPage.xaml 的交互逻辑
    /// </summary>
    public partial class SideScanSonarPage : Page
    {

        // UDP服务
        private static Socket UDPServer;

        // 文件信息
        public List<FilesName> filesName = null;

        // sftp文件服务
        private SFTPUtils sftpClient = null;

        // 本地文件路径
        private string localNowFile = null;

        // 常量
        private static readonly string RemoteDirectory = "/C:/ProgramData/OceanTechSideScanSonarPro/OceanTechSideScanSonarData/DefaultPrj/LogData";
        // 远程树莓派IP
        private static readonly string RaspberryPieIP = "192.168.1.31";
        // 本地主机IP
        private static readonly string LocalHostIP = "192.168.1.12";
        // 树莓派用户名
        private static readonly string RaspberryPieUserName = "tuyong";
        // 树莓派密码
        private static readonly string RaspberryPiePassword = "tuyong";

        public SideScanSonarPage()
        {
            InitializeComponent();

            Messenger.Default.Register<string>(this, "SideScanSonarSend", SideScanSonarSending);
            //this.DataContext = new MainLoginViewModel();
            //卸载当前(this)对象注册的所有MVVMLight消息
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);


            // 启动UDP，命令
            TurnOnUDPServer();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string currentPath = Environment.CurrentDirectory;

            //初始化目录
            if (!Directory.Exists(currentPath + @"\AUVData"))
            {
                Directory.CreateDirectory(currentPath + @"\AUVData");
            }

            localNowFile = currentPath + @"\AUVData\";
        }

        // 发送信息
        private void SideScanSonarSending(string msg)
        {
            try
            {
                if (UDPServer != null)
                {
                    EndPoint point = new IPEndPoint(IPAddress.Parse(RaspberryPieIP), 10110);
                    UDPServer.SendTo(Encoding.UTF8.GetBytes(msg), point);
                }
            }
            catch(Exception)
            {

            }
        }

        // UDP启动函数
        private void TurnOnUDPServer()
        {
            try
            {
                UDPServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(LocalHostIP), 10111);
                UDPServer.Bind(endPoint);

                // 开启线程
                Task.Run(new Action(ReceiveUDPMessage));
            }
            catch (Exception)
            {
                UDPServer.Close();
                UDPServer.Dispose();

                // 报错提示
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "错误",
                    Message = "请设置IP:192.168.1.12"
                }.ShowDialog();
            }
        }

        // UDP线程
        private void ReceiveUDPMessage()
        {
            try
            {
                byte[] buffer;

                while (true)
                {
                    EndPoint point = new IPEndPoint(IPAddress.Parse(LocalHostIP), 10111);//用来保存发送方的ip和端口号
                    buffer = new byte[64];
                    int length = UDPServer.ReceiveFrom(buffer, ref point);//接收数据报
                    if (length > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, length);
                        Messenger.Default.Send<string>(message, "SideScanSonarMessage"); //注意参数一致
                    }
                }
            }
            catch (Exception)
            {
                UDPServer.Close();
                UDPServer.Dispose();
            }
        }

        // 页面退出时退出UDP通信
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            UDPServer.Close();
            UDPServer.Dispose();
            sftpClient = null;
        }

        // 偷懒写法，未绑定
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null) 
            {
                return;
            }
            if (rb.IsChecked == true)
            {
                if (((string)rb.Content).Equals("清水"))
                {
                    WaterQualityVirtualText.Text = "0";
                }
                else if (((string)rb.Content).Equals("一般"))
                {
                    WaterQualityVirtualText.Text = "1";
                }
                else if (((string)rb.Content).Equals("浑浊"))
                {
                    WaterQualityVirtualText.Text = "2";
                }
            }
        }

        // 显示页面
        private void FolderTreeList(List<string> files)
        {
            //string currentPath = Environment.CurrentDirectory;
            //string nowFile = currentPath + @"\AUVData";

            filesName = new List<FilesName>();
            int count = 0;
            foreach (string file in files)
            {
                FilesName excel = new FilesName();
                excel.FileId = ++count;
                excel.FileName = file;
                excel.FileImagePath = "/AUVSoftware;component/images/xtfico.ico";
                filesName.Add(excel);
            }

            FileTree.ItemsSource = filesName;
        }

        /// <summary>
        /// 查询侧扫文件数据文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueryDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (sftpClient == null)
            {
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "警告",
                    Message = "请连接远程主机！"
                }.ShowDialog();
                return;
            }

            try
            {
                // 影藏贴图
                if (ShowImageBiliBili.Visibility == Visibility.Visible)
                {
                    ShowImageBiliBili.Visibility = Visibility.Collapsed;
                }

                List<string> files = sftpClient.GetFileList(RemoteDirectory);
                FolderTreeList(files);
            }
            catch (Exception)
            {
                sftpClient = null;

                // 报错提示
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "错误",
                    Message = "查询失败!请重试!"
                }.ShowDialog();

                SideScanConnectionButton.Content = "连接";
            }

        }

        /// <summary>
        /// 下载侧扫文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownLoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (FileTree.SelectedItems.Count == 0)
            {
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "警告",
                    Message = "请选择要下载的文件"
                }.ShowDialog();
                return;
            }
            if (sftpClient == null)
            {
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "警告",
                    Message = "请连接远程主机！"
                }.ShowDialog();
                return;
            }
            // 处理文件
            string filesname = (FileTree.SelectedItem as FilesName).FileName;
            if (sftpClient.Get(RemoteDirectory + "/" + filesname, localNowFile + filesname))
            {
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "成功",
                    Message = "下载完成!"
                }.ShowDialog();
            }
            else
            {
                // 报错提示
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "错误",
                    Message = "下载失败!请连接!"
                }.ShowDialog();

                SideScanConnectionButton.Content = "连接";
            }
        }

        /// <summary>
        /// 删除侧扫文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (FileTree.SelectedItems.Count == 0)
            {
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "警告",
                    Message = "请选择要删除的文件"
                }.ShowDialog();
                return;
            }
            if (sftpClient == null)
            {
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "警告",
                    Message = "请连接远程主机！"
                }.ShowDialog();
                return;
            }

            // 处理文件
            string filesname = (FileTree.SelectedItem as FilesName).FileName;
            if (sftpClient.Delete(RemoteDirectory + "/" + filesname))
            {
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "成功",
                    Message = "删除完成!"
                }.ShowDialog();

                List<string> files = sftpClient.GetFileList(RemoteDirectory);
                FolderTreeList(files);
            }
            else
            {
                // 报错提示
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "错误",
                    Message = "删除失败!请连接!"
                }.ShowDialog();

                SideScanConnectionButton.Content = "连接";
            }
        }

        /// <summary>
        /// 连接侧扫远程主机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SideScanConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sftpClient = new SFTPUtils(RaspberryPieIP, 22, RaspberryPieUserName, RaspberryPiePassword);
                if ((string)SideScanConnectionButton.Content == "连接")
                {
                    SideScanConnectionButton.Content = "已连接";
                }
            }
            catch (Exception)
            {
                sftpClient = null;

                // 报错提示
                _ = new MessageWindow()
                {
                    Owner = Window.GetWindow(this),
                    Header = "错误",
                    Message = "连接失败!请重试!"
                }.ShowDialog();
            }
        }

        // 打开本地数据目录
        private void OpenTheLocalPath_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(localNowFile);
        }

        // 打开侧扫数据处理软件
        private void OpenSideScanSonarImageSoftware_Click(object sender, RoutedEventArgs e)
        {
            ctnTest.StartAndEmbedProcess(localNowFile + @"ImageJforXTF\ImageJforXTF.exe");
        }
    }
}
