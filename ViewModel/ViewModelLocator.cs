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
            //ϵͳ�Դ�
            SimpleIoc.Default.Register<MainViewModel>();

            //������ҳ��
            SimpleIoc.Default.Register<MainWindowViewModel>();

            //IP��¼ҳ��
            SimpleIoc.Default.Register<MainLoginViewModel>();

            //���ڵ�¼ҳ��
            SimpleIoc.Default.Register<LoginSerialPortViewModel>();

            //������Խ���
            SimpleIoc.Default.Register<NetworkDebugViewModel>();

            //���ڵ��Խ���
            SimpleIoc.Default.Register<SerialPortDebugViewModel>();

            //����������
            SimpleIoc.Default.Register<DriverFirstTestViewModel>();

            //GPS��ת��
            SimpleIoc.Default.Register<TransferGPSViewModel>();

            //�ֱ�����
            SimpleIoc.Default.Register<MotionControlViewModel>();

            //������ʾ����
            SimpleIoc.Default.Register<DisplayCommandViewModel>();

            //ˮ������������
            SimpleIoc.Default.Register<UnderwaterOperationViewModel>();

            //��ɨ���ſ�����
            SimpleIoc.Default.Register<SideScanSonarViewModel>();
        }

        //ϵͳ�Դ�
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        //������ҳ��
        public MainWindowViewModel MainWindowViewModel => ServiceLocator.Current.GetInstance<MainWindowViewModel>();

        //IP��¼ҳ��
        public MainLoginViewModel MainLoginViewModel => ServiceLocator.Current.GetInstance<MainLoginViewModel>();

        //���ڵ�¼ҳ��
        public LoginSerialPortViewModel LoginSerialPortViewModel => ServiceLocator.Current.GetInstance<LoginSerialPortViewModel>();

        //������Խ���
        public NetworkDebugViewModel NetworkDebugViewModel => ServiceLocator.Current.GetInstance<NetworkDebugViewModel>();

        //���ڵ��Խ���
        public SerialPortDebugViewModel SerialPortDebugViewModel => ServiceLocator.Current.GetInstance<SerialPortDebugViewModel>();

        //����������
        public DriverFirstTestViewModel DriverFirstTestViewModel => ServiceLocator.Current.GetInstance<DriverFirstTestViewModel>();

        //GPS��ת��
        public TransferGPSViewModel TransferGPSViewModel => ServiceLocator.Current.GetInstance<TransferGPSViewModel>();

        //�ֱ�����
        public MotionControlViewModel MotionControlViewModel => ServiceLocator.Current.GetInstance<MotionControlViewModel>();

        //������ʾ����
        public DisplayCommandViewModel DisplayCommandViewModel => ServiceLocator.Current.GetInstance<DisplayCommandViewModel>();

        //ˮ������������
        public UnderwaterOperationViewModel UnderwaterOperationViewModel => ServiceLocator.Current.GetInstance<UnderwaterOperationViewModel>();

        //��ɨ���ſ�����
        public SideScanSonarViewModel SideScanSonarViewModel => ServiceLocator.Current.GetInstance<SideScanSonarViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}