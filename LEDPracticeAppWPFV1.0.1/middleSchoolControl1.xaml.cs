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
        bool functionsFlag;
        bool healthyBehaviorsFlag;
        bool unhealthyBehaviorsFlag;
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
            functions.IsChecked = true;
           frontalLobeBox.MouseEnter += new MouseEventHandler(frontalLobeBox_MouseEnter);
            frontalLobeBox.MouseLeave += new MouseEventHandler(frontalLobeBox_MouseLeave);
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
            myScaleTransform = new ScaleTransform(1.5, 1.5);
            img.RenderTransform = myScaleTransform;
            img.BeginAnimation(OpacityProperty,animation);
             
        }
        private void temporalLobeBox_MouseLeave(object sender, MouseEventArgs e){

            Image img = (Image)sender;
          //  animation.RepeatBehavior = new RepeatBehavior(1.0);
                img.BeginAnimation(OpacityProperty, null);
        }



        private void frontalLobeBox_MouseEnter(object sender, MouseEventArgs e)
        {
           

            parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
            occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;
            // Create a transform to scale the size of the button.
        //    oldCenterPoint = TranslatePoint(oldCenterPoint, frontalLobeBox);
        //   centerPoint = TranslatePoint(centerPoint, centerReference);
           // translateTransform.X = oldCenterPoint.X + centerPoint.X;
          //  translateTransform.Y = oldCenterPoint.Y + centerPoint.Y;
        //    frontalLobeBox.RenderTransform = myTranslateTransform;

            if (functionsFlag)
            {
                factsMessageBox.Text = "planning, reasoning, speech, voluntary movement " +
                    "(motor cortex is in the frontal lobe), problem solving, regulating " +
                    " emotions (the frontal lobe doesn’t initiate the emotion, but it helps " +
                    "us control our emotions)";
               
            }
            if (healthyBehaviorsFlag)
            {
                factsMessageBox.Text = "Reading, Problem-Solving games, choreography (like " +
                    "for ballet, Zumba), meditation";
            }

        }

        private void frontalLobeBox_MouseLeave(object sender, MouseEventArgs e)
        {
           
            parietalLobeBox.Visibility = System.Windows.Visibility.Visible;
            temporalLobeBox.Visibility = System.Windows.Visibility.Visible;
            cerebellumBox.Visibility = System.Windows.Visibility.Visible;
            occipitalLobeBox.Visibility = System.Windows.Visibility.Visible;
            // Create a transform to scale the size of the button.
            ScaleTransform myScaleTransform = new ScaleTransform();

            // Set the transform to triple the scale in the Y direction.
            myScaleTransform.ScaleY = (1/1.75);
            myScaleTransform.ScaleX = (1/1.75);

            // Create a transform to rotate the button
            TranslateTransform myTranslateTransform = new TranslateTransform();

            // Set the rotation of the transform to 45 degrees.
            myTranslateTransform.X = 40;
            myTranslateTransform.Y = -90;
            factsMessageBox.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void functions_Checked(object sender, RoutedEventArgs e)
        {
            functionsFlag = true;
            healthyBehaviorsFlag = false;
            unhealthyBehaviorsFlag = false;
        }

        private void healthyBehaviors_Checked(object sender, RoutedEventArgs e)
        {
            healthyBehaviorsFlag = true;
            functionsFlag = false;
            unhealthyBehaviorsFlag = false;
        }

        private void unhealthyBehaviors_Checked(object sender, RoutedEventArgs e)
        {
            unhealthyBehaviorsFlag = true;
            functionsFlag = false;
            healthyBehaviorsFlag = false;
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {

        }

        private void gotItButton_Click(object sender, RoutedEventArgs e)
        {
           
          
        }
    }
}
