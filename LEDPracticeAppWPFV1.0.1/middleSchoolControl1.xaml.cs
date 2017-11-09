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
using System.Windows.Media.Animation;

namespace LEDPracticeAppWPFV1._0._1
{
    /// <summary>
    /// Interaction logic for middleSchoolControl1.xaml
    /// </summary>
    public partial class middleSchoolControl1 : UserControl
    {
        private static middleSchoolControl1 _instance;
        ScaleTransform myScaleTransform;
        
        DoubleAnimation animation = new DoubleAnimation();
        public static middleSchoolControl1 Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new middleSchoolControl1();
                }
                return _instance;

            }
        }
        public middleSchoolControl1()
        {
            InitializeComponent();
            //   foreach (var img in middleSchoolControl1.OfType<Image>())
            //   {
        //    frontalLobeBox.MouseEnter += new MouseEventHandler(frontalLobeBox_MouseEnter);
         //   frontalLobeBox.MouseLeave += new MouseEventHandler(frontalLobeBox_MouseLeave);
            temporalLobeBox.MouseDown += new MouseButtonEventHandler(temporalLobeBox_MouseLeftClick);
            temporalLobeBox.MouseLeave += new MouseEventHandler(temporalLobeBox_MouseLeave);

            //  }

        }

        private void temporalLobeBox_MouseLeftClick(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            animation.From = 1.0;
            animation.To = 0.0;
            animation.Duration = new Duration(TimeSpan.FromSeconds(5));
            animation.AutoReverse = true;
            animation.RepeatBehavior = RepeatBehavior.Forever;
          //  myScaleTransform = new ScaleTransform(1.5, 1.5);
          //  img.RenderTransform = myScaleTransform;
            img.BeginAnimation(OpacityProperty,animation);
             
        }
        private void temporalLobeBox_MouseLeave(object sender, MouseEventArgs e){

            Image img = (Image)sender;
          //  animation.RepeatBehavior = new RepeatBehavior(1.0);
                img.BeginAnimation(OpacityProperty, null);
        }



        private void frontalLobeBox_MouseEnter(object sender, MouseEventArgs e)
        {
            factsMessageBox.Text = "This is your Frontal Lobe";

        }

        private void frontalLobeBox_MouseLeave(object sender, MouseEventArgs e)
        {
            factsMessageBox.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
