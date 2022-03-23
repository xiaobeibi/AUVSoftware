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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AUVSoftware.Views
{
    /// <summary>
    /// UnderwaterOperationPage.xaml 的交互逻辑
    /// </summary>
    public partial class UnderwaterOperationPage : Page
    {
        public UnderwaterOperationPage()
        {
            InitializeComponent();
        }

        private void RudderPanelMinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(LeftRudderPanelDisplay.Text) < 9)
            {
                LeftRudderPanelDisplay.Text = (Convert.ToInt32(LeftRudderPanelDisplay.Text) + 1).ToString();
            }

            if (Convert.ToInt32(RightRudderPanelDisplay.Text) < 9)
            {
                RightRudderPanelDisplay.Text = (Convert.ToInt32(RightRudderPanelDisplay.Text) + 1).ToString();
            }
        }

        private void RudderPanelPlusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(LeftRudderPanelDisplay.Text) > -9)
            {
                LeftRudderPanelDisplay.Text = (Convert.ToInt32(LeftRudderPanelDisplay.Text) - 1).ToString();
            }

            if (Convert.ToInt32(RightRudderPanelDisplay.Text) > -9)
            {
                RightRudderPanelDisplay.Text = (Convert.ToInt32(RightRudderPanelDisplay.Text) - 1).ToString();
            }
        }

        private void LeftRudderPanelMinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(LeftRudderPanelDisplay.Text) > -9)
            {
                LeftRudderPanelDisplay.Text = (Convert.ToInt32(LeftRudderPanelDisplay.Text) - 1).ToString();
            }
        }

        private void LeftRudderPanelPlusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(LeftRudderPanelDisplay.Text) < 9)
            {
                LeftRudderPanelDisplay.Text = (Convert.ToInt32(LeftRudderPanelDisplay.Text) + 1).ToString();
            }
        }

        private void RightRudderPanelMinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(RightRudderPanelDisplay.Text) > -9)
            {
                RightRudderPanelDisplay.Text = (Convert.ToInt32(RightRudderPanelDisplay.Text) - 1).ToString();
            }
        }

        private void RightRudderPanelPlusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(RightRudderPanelDisplay.Text) < 9)
            {
                RightRudderPanelDisplay.Text = (Convert.ToInt32(RightRudderPanelDisplay.Text) + 1).ToString();
            }
        }
    }
}
