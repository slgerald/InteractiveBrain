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
using System.Threading;

namespace LEDPracticeAppWPFV1._0._1
{
    /// <summary>
    /// The middleSchoolUserControl is used to separate the top brain from the bottom brain and 
    /// provide information about the functions, healthy behaviors and unhealthy behaviors
    /// </summary>
    public partial class middleSchoolControl1 : UserControl
    {
        private static middleSchoolControl1 _instance; //render userControl based on button pressed
        //flags used to determine which radio button has been used 
        bool functionsFlag; 
        bool healthyBehaviorsFlag;
        bool unhealthyBehaviorsFlag;
        DoubleAnimation animation = new DoubleAnimation();//animation used for the glowing effect
        Storyboard sb;

        //This method allows the UserControl to be rendered when called
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
        //This function initializes the userControl with EventHandlers and other characteristics
        //EventHandlers routed to controls in xaml file
        public middleSchoolControl1()
        {
            InitializeComponent();
            functions.IsChecked = true;
            frontalLobeBox.MouseDown += new MouseButtonEventHandler(frontalLobeBox_MouseDown);
           // gotItButton.Click -= new RoutedEventHandler(gotItButton_Click);
            
           
            temporalLobeBox.MouseDown += new MouseButtonEventHandler(temporalLobeBox_MouseLeftClick);
            temporalLobeBox.MouseLeave += new MouseEventHandler(temporalLobeBox_MouseLeave);
            gotItButton.Click += new RoutedEventHandler(gotItButton_Click);
            sb = (Storyboard)this.Resources["scaling"];
            Console.WriteLine(" ReverseFlag: " + sb.AutoReverse);

           // reverseOn(); animation woul be already complete
        }

        //This functions determines what is down when the temporal lobe is left mouse clicked
        //Right now it begins a glowing animation based on make the image opaque to less opaque
        private void temporalLobeBox_MouseLeftClick(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            animation.From = 1.0;
            animation.To = 0.5;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            animation.AutoReverse = true;
            animation.RepeatBehavior = RepeatBehavior.Forever;
            img.BeginAnimation(OpacityProperty,animation);
             
        }
       //This functions determines what happens when the MouseLeaves the temporal Lobe
       //Now it ends the glowing animation
        private void temporalLobeBox_MouseLeave(object sender, MouseEventArgs e){
            Image img = (Image)sender;
            img.BeginAnimation(OpacityProperty, null);
        }
        //This functions determines what happens when the frontalLobe box is clicked once with any 
        //mouse button
        private void frontalLobeBox_MouseDown(object sender, MouseEventArgs e)
        {  //This causes the other parts of the brain to disappear
                if(sb.AutoReverse)
            { reverseOn(); }
            else { reverseOff(); }
                //without reverseOn every other time autoreversetrue
                Console.WriteLine(" In frontalLobeBox MouseDown ReverseFlag: " + sb.AutoReverse);
            
            parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
            occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            
            if (functionsFlag)//The functions Flag is used to ensure functions are listed when the facts message box appears
                {
                    factsMessageBox.Text = "planning, reasoning, speech, voluntary movement " +
                        "(motor cortex is in the frontal lobe), problem solving, regulating " +
                        " emotions (the frontal lobe doesn’t initiate the emotion, but it helps " +
                        "us control our emotions)";

                }
                //The healthyBehaviors Flag is used to ensure functions are listed when the facts message box appears
                if (healthyBehaviorsFlag)
                {
                    factsMessageBox.Text = "Reading, Problem-Solving games, choreography (like " +
                        "for ballet, Zumba), meditation";
                }
            

        }
      
        //This function sets flags to true and false
        private void functions_Checked(object sender, RoutedEventArgs e)
        {
            functionsFlag = true;
            healthyBehaviorsFlag = false;
            unhealthyBehaviorsFlag = false;
        }
        //This function sets flags as true and false 
        private void healthyBehaviors_Checked(object sender, RoutedEventArgs e)
        {
            healthyBehaviorsFlag = true;
            functionsFlag = false;
            unhealthyBehaviorsFlag = false;
        }
        //This function set flags as true and false 
        private void unhealthyBehaviors_Checked(object sender, RoutedEventArgs e)
        {
            unhealthyBehaviorsFlag = true;
            functionsFlag = false;
            healthyBehaviorsFlag = false;
        }

        private void gotItButton_Released(object sender, MouseEventArgs e)
        {
        

        }
        private void reverseOff()
       {       
                sb.AutoReverse = false;
              
              Console.WriteLine("In Reverse Off. ReverseFlag: "  + sb.AutoReverse);
           
      }
       private void reverseOn()
        {
            
          Console.WriteLine("In Reverse On. ReverseFlag: " + sb.AutoReverse);
           sb.Begin(this, true);
              sb.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
            sb.AutoReverse = true;
            
            Console.WriteLine("Leaving Reverse On. ReverseFlag: "  + sb.AutoReverse);
        }

        private void gotItButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Entering ButtonClick. ReverseFlag: "  + sb.AutoReverse);
           // if (sb.AutoReverse)
           // { reverseOn(); }
          //  else { reverseOff(); }
            if (sb.AutoReverse)
            { reverseOn(); }
            else { reverseOff(); }
            parietalLobeBox.Visibility = System.Windows.Visibility.Visible;
                temporalLobeBox.Visibility = System.Windows.Visibility.Visible;
                cerebellumBox.Visibility = System.Windows.Visibility.Visible;
                occipitalLobeBox.Visibility = System.Windows.Visibility.Visible;
                factsMessageBox.Visibility = System.Windows.Visibility.Hidden;
                gotItButton.Visibility = System.Windows.Visibility.Hidden;
            if (!sb.AutoReverse)
            { reverseOn(); }
            else { reverseOff(); }

            Console.WriteLine("Leaving ButtonClick . ReverseFlag: "  + sb.AutoReverse);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           

        }
    }
}
