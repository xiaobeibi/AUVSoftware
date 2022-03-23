/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:AUVSoftware"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using AUVSoftware.ViewModels;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;


namespace AUVSoftware.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            ///
            //系统自带
            SimpleIoc.Default.Register<MainViewModel>();

            //主窗口页面
            SimpleIoc.Default.Register<MainWindowViewModel>();

            //IP登录页面
            SimpleIoc.Default.Register<MainLoginViewModel>();

            //串口登录页面
            SimpleIoc.Default.Register<LoginSerialPortViewModel>();

            //网络调试界面
            SimpleIoc.Default.Register<NetworkDebugViewModel>();

            //串口调试界面
            SimpleIoc.Default.Register<SerialPortDebugViewModel>();

            //驱动器测试
            SimpleIoc.Default.Register<DriverFirstTestViewModel>();

            //GPS中转类
            SimpleIoc.Default.Register<TransferGPSViewModel>();

            //手柄控制
            SimpleIoc.Default.Register<MotionControlViewModel>();

            //命令显示窗口
            SimpleIoc.Default.Register<DisplayCommandViewModel>();

            //水下运行命令类
            SimpleIoc.Default.Register<UnderwaterOperationViewModel>();

            //侧扫声呐控制类
            SimpleIoc.Default.Register<SideScanSonarViewModel>();
        }

        //系统自带
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        //主窗口页面
        public MainWindowViewModel MainWindowViewModel => ServiceLocator.Current.GetInstance<MainWindowViewModel>();

        //IP登录页面
        public MainLoginViewModel MainLoginViewModel => ServiceLocator.Current.GetInstance<MainLoginViewModel>();

        //串口登录页面
        public LoginSerialPortViewModel LoginSerialPortViewModel => ServiceLocator.Current.GetInstance<LoginSerialPortViewModel>();

        //网络调试界面
        public NetworkDebugViewModel NetworkDebugViewModel => ServiceLocator.Current.GetInstance<NetworkDebugViewModel>();

        //串口调试界面
        public SerialPortDebugViewModel SerialPortDebugViewModel => ServiceLocator.Current.GetInstance<SerialPortDebugViewModel>();

        //驱动器测试
        public DriverFirstTestViewModel DriverFirstTestViewModel => ServiceLocator.Current.GetInstance<DriverFirstTestViewModel>();

        //GPS中转类
        public TransferGPSViewModel TransferGPSViewModel => ServiceLocator.Current.GetInstance<TransferGPSViewModel>();

        //手柄控制
        public MotionControlViewModel MotionControlViewModel => ServiceLocator.Current.GetInstance<MotionControlViewModel>();

        //命令显示窗口
        public DisplayCommandViewModel DisplayCommandViewModel => ServiceLocator.Current.GetInstance<DisplayCommandViewModel>();

        //水下运行命令类
        public UnderwaterOperationViewModel UnderwaterOperationViewModel => ServiceLocator.Current.GetInstance<UnderwaterOperationViewModel>();

        //侧扫声呐控制类
        public SideScanSonarViewModel SideScanSonarViewModel => ServiceLocator.Current.GetInstance<SideScanSonarViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}