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

namespace AUVSoftware.UserControls
{
    /// <summary>
    /// SlidingPointerUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class SlidingPointerUserControl : UserControl
    {

        private List<int> ShortTicks;
        private List<int> MiddleTicks;
        private List<int> LongTicks;
        private List<string> TextLists;

        public SlidingPointerUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ShortTicks = new List<int>();
            this.MiddleTicks = new List<int>();
            this.LongTicks = new List<int>();
            this.TextLists = new List<string>();
            for (int i = 0; i < 360; i++)
            {
                ShortTicks.Add(i);
            }

            for (int i = 0; i < 72; i++)
            {

                MiddleTicks.Add(i);
            }

            for (int i = 0; i < 36; i++)
            {

                LongTicks.Add(i);
            }
            for (int i = -18; i < 18; i++)
            {
                if (i >= 9 && i < 18)
                {
                    TextLists.Add((270 - i * 10).ToString());
                }
                else
                {
                    TextLists.Add((i * -10 - 90).ToString());
                }
            }
            lblong.ItemsSource = LongTicks;
            lbmedium.ItemsSource = MiddleTicks;
            lbshort.ItemsSource = ShortTicks;
            lbtext.ItemsSource = TextLists;
        }

        public int IndicatorPointer
        {
            get { return (int)GetValue(IndicatorPointerProperty); }
            set { SetValue(IndicatorPointerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndicatorPointerProperty =
            DependencyProperty.Register("IndicatorPointer", typeof(int), typeof(SlidingPointerUserControl), new PropertyMetadata(0));

    }
}
