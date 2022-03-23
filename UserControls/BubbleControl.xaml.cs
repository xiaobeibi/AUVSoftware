using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AUVSoftware.UserControls
{
    /// <summary>
    /// BubbleControl.xaml 的交互逻辑
    /// </summary>
    public partial class BubbleControl : Window
    {
        public BubbleControl()
        {
            InitializeComponent();
        }

        public string NotifyMessage { get; set; }

        private void ThisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Text = NotifyMessage;
        }

        async private void Storyboard_Completed(object sender, EventArgs e)
        {
            await Task.Delay(2000);
            BeginStoryboard(FindResource("CloseStoryboard") as Storyboard);
        }

        private void Storyboard_Completed_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
