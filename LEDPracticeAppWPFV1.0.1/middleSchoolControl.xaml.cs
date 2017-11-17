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
        Storyboard sbFL;
        Storyboard sbTL;
        Storyboard sbPL;
        Storyboard sbOL;
        Storyboard sbC;
        //sbFL = 1, sbTL = 2, sbPL = 3, sbOL = 4, sbC = 5
        int storyboardFlag;

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
            temporalLobeBox.MouseDown += new MouseButtonEventHandler(temporalLobeBox_MouseDown);
            parietalLobeBox.MouseDown += new MouseButtonEventHandler(parietalLobeBox_MouseDown);
            occipitalLobeBox.MouseDown += new MouseButtonEventHandler(occipitalLobeBox_MouseDown);
            cerebellumBox.MouseDown += new MouseButtonEventHandler(cerebellumBox_MouseDown);
           // temporalLobeBox.MouseLeave += new MouseEventHandler(temporalLobeBox_MouseLeave);
            gotItButton.Click += new RoutedEventHandler(gotItButton_Click);
            sbFL = (Storyboard)this.Resources["zoomingFL"];
            sbTL = (Storyboard)this.Resources["zoomingTL"];
            sbPL = (Storyboard)this.Resources["zoomingPL"];
            sbOL = (Storyboard)this.Resources["zoomingOL"];
            sbC = (Storyboard)this.Resources["zoomingC"];
            storyboardFlag = 0;

        }

        
        /*
        private void imageToCenter(object sender,double objectX, double objectY)
        {
            Image img = (Image)sender;

            double centerX = Canvas.GetLeft(centerReference);
            double centerY = Canvas.GetTop(centerReference);
            Console.WriteLine("centerX" + centerX + " , centerY" + centerY);
            double xshift = -(objectX - centerX)/5;
            double yshift = (objectY - centerY)/5;
            Console.WriteLine("xshift" + xshift + " , yshift" + yshift);
            TranslateTransform trans = new TranslateTransform();
            ScaleTransform scale = new ScaleTransform();
            img.RenderTransform = trans;
            img.RenderTransform = scale;
            DoubleAnimation animX = new DoubleAnimation(0, xshift, TimeSpan.FromSeconds(2));
            DoubleAnimation animY = new DoubleAnimation(0,yshift/5, TimeSpan.FromSeconds(2));
            DoubleAnimation animSize = new DoubleAnimation(0, 1.5, TimeSpan.FromSeconds(2));
            img.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animSize);
            img.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animSize);
            img.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animX);
           img.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animY);
        }

        //This functions determines what happens when the MouseLeaves the temporal Lobe
        //Now it ends the glowing animation
            private void temporalLobeBox_MouseLeave(object sender, MouseEventArgs e){
            Image img = (Image)sender;
            img.BeginAnimation(OpacityProperty, null);
            }
        */

        //This functions determines what happens when the frontalLobe box is clicked once with any 
        //mouse button
        private void frontalLobeBox_MouseDown(object sender, MouseEventArgs e)
        {
            storyboardFlag = 1;
            //without reverseOn every other time autoreversetrue
            if(sbFL.AutoReverse)
            reverseOn(1);
            else { reverseOff(1); }



            //Hide other parts and make facts box and go button visible
            parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
            occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;

            /*This was used to get location of the frontalLobeBox
             * double objectX = Canvas.GetLeft(frontalLobeBox);
            double objectY = Canvas.GetTop(frontalLobeBox);
            Console.WriteLine("objectX" + objectX + " , objectY" + objectY);
            */

            if (functionsFlag)//The functions Flag is used to ensure functions are listed when the facts message box appears
                {
                    factsMessageBox.Text = "planning, reasoning, speech, voluntary movement " +
                        "(motor cortex is in the frontal lobe), problem solving, regulating " +
                        " emotions (the frontal lobe doesn’t initiate the emotion, but it helps " +
                        "us control our emotions)";
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                }
                //The healthyBehaviors Flag is used to ensure functions are listed when the facts message box appears
                if (healthyBehaviorsFlag)
                {
                    factsMessageBox.Text = "Reading, Problem-Solving games, choreography (like " +
                        "for ballet, Zumba), meditation";
                functions.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                }
                if(unhealthyBehaviorsFlag)
                 {
                factsMessageBox.Text = "unhealthy frontal";
                healthyBehaviors.IsEnabled = false;
                functions.IsEnabled = false;
                    }
          //  Thread.Sleep(1000);
           // imageToCenter(sender, objectX, objectY);
        }
        //This functions determines what is down when the temporal lobe is left mouse clicked
        //Right now it begins a glowing animation based on make the image opaque to less opaque
        private void temporalLobeBox_MouseDown(object sender, MouseEventArgs e)
        {
            storyboardFlag = 2;
            // animation.From = 1.0;
            // animation.To = 0.5;
            // animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            //// animation.AutoReverse = true;
            // animation.RepeatBehavior = RepeatBehavior.Forever;
            // img.BeginAnimation(OpacityProperty,animation);
            // From = "175" To = "250" Yfor frontalLobeBox 
            // From="190" To="340" X for frontalLobeBox
            // double objectX = Canvas.GetLeft(temporalLobeBox);
            // double objectY = Canvas.GetTop(temporalLobeBox);
            // Console.WriteLine("objectX" + objectX + " , objectY" + objectY);
            //  imageToCenter(sender,objectX,objectY);
            if (sbTL.AutoReverse)
            {
                reverseOn(2);
            }
            else
            {
                reverseOff(2);
            }

            //Hide other parts and make facts box and go button visible
            parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            frontalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
            occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;

            if (functionsFlag)//The functions Flag is used to ensure functions are listed when the facts message box appears
            {
                factsMessageBox.Text = "Hearing, processing short term memory (hippocampus located in this lobe)";
            }
        }


        private void parietalLobeBox_MouseDown(object sender, MouseEventArgs e)
        {
            storyboardFlag = 3;
            if (sbPL.AutoReverse)
            {
                reverseOn(3);
            }
            else
            {
                reverseOff(3);
            }
            //Hide other parts and make facts box and go button visible
            temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            frontalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
            occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;

            if (functionsFlag)//The functions Flag is used to ensure functions are listed when the facts message box appears
            {
                factsMessageBox.Text = "sensory processing such as temperature, pressure, touch, & pain";
            }
        }
        private void occipitalLobeBox_MouseDown(object sender, MouseEventArgs e)
        {
            storyboardFlag = 4;
            if (sbOL.AutoReverse)
            {
                reverseOn(4);
            }
            else
            {
                reverseOff(4);
            }
            //Hide other parts and make facts box and go button visible
            temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            frontalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
            parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;

            if (functionsFlag)//The functions Flag is used to ensure functions are listed when the facts message box appears
            {
                factsMessageBox.Text = "visual processing";
            }
        }
        private void cerebellumBox_MouseDown(object sender, MouseEventArgs e)
        {
            storyboardFlag = 5;
            if (sbC.AutoReverse)
            {
                reverseOn(5);
            }
            else
            {
                reverseOff(5);
            }
            //Hide other parts and make facts box and go button visible
            temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            frontalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;

            if (functionsFlag)//The functions Flag is used to ensure functions are listed when the facts message box appears
            {
                factsMessageBox.Text = "visual processing";
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

        private void reverseOff(int storyboardFlag)
        {
            switch(storyboardFlag)
            {
                case 1:
                    sbFL.AutoReverse = false;
                    break;
                case 2:
                    sbTL.AutoReverse = false;
                    break;
                case 3:
                    sbPL.AutoReverse = false;
                    break;
                case 4:
                    sbOL.AutoReverse = false;
                    break;
                case 5:
                    sbC.AutoReverse = false;
                    break;
            }    
        }

        private void reverseOn(int storyboardFlag)
        {
            switch (storyboardFlag)
            {
                case 1:
                    sbFL.Begin(this, true);
                    sbFL.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbFL.AutoReverse = true;
                    break;
                case 2:
                    sbTL.Begin(this, true);
                    sbTL.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbTL.AutoReverse = true;
                    break;
                case 3:
                    sbPL.Begin(this, true);
                    sbPL.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbPL.AutoReverse = true;
                    break;
                case 4:
                    sbOL.Begin(this, true);
                    sbOL.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbOL.AutoReverse = true;
                    break;
                case 5:
                    sbC.Begin(this, true);
                    sbC.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbC.AutoReverse = true;
                    break;
            } 
       }

        private void gotItButton_Click(object sender, RoutedEventArgs e)
        {     //sbFL = 1, sbTL = 2, sbPL = 3, sbOL = 4, sbC = 5
       
                        if (sbFL.AutoReverse)
                        { reverseOn(1); }
                        else { reverseOff(1); }
            if (sbTL.AutoReverse)
            { reverseOn(2); }
            else { reverseOff(2); }
            if (sbPL.AutoReverse && storyboardFlag == 3)
            { reverseOn(3); }
            else { reverseOff(3); }
            if (sbOL.AutoReverse && storyboardFlag == 4)
            { reverseOn(4); }
            else { reverseOff(4); }
            if (sbC.AutoReverse && storyboardFlag == 5)
            { reverseOn(5); }
            else { reverseOff(5); }

            switch (storyboardFlag)
                {
                case 1:
                       parietalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      temporalLobeBox.Visibility = System.Windows.Visibility.Visible;
                     cerebellumBox.Visibility = System.Windows.Visibility.Visible;
                     occipitalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 2:
                       parietalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      frontalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      cerebellumBox.Visibility = System.Windows.Visibility.Visible;
                      occipitalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 3:
                    frontalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      temporalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      cerebellumBox.Visibility = System.Windows.Visibility.Visible;
                      occipitalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 4:
                       parietalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      temporalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      frontalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      cerebellumBox.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 5:
                       parietalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      temporalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      frontalLobeBox.Visibility = System.Windows.Visibility.Visible;
                      occipitalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
            factsMessageBox.Visibility = System.Windows.Visibility.Hidden;
            gotItButton.Visibility = System.Windows.Visibility.Hidden;


            if (!sbFL.AutoReverse && storyboardFlag==1)
            { reverseOn(1); }
            else { reverseOff(1); }
            if (!sbTL.AutoReverse && storyboardFlag == 2)
            { reverseOn(2); }
            else { reverseOff(2); }
            if (!sbPL.AutoReverse && storyboardFlag == 3)
            { reverseOn(3); }
            else { reverseOff(3); }
            if (!sbOL.AutoReverse && storyboardFlag == 4)
            { reverseOn(4); }
            else { reverseOff(4); }
            if (!sbC.AutoReverse && storyboardFlag == 5)
            { reverseOn(5); }
            else { reverseOff(5); }

            //re-enables the other radio buttons
            if (functionsFlag)
            {
                healthyBehaviors.IsEnabled = true;
                unhealthyBehaviors.IsEnabled = true;
            }
           
            if (healthyBehaviorsFlag)
            {
                functions.IsEnabled = true;
                unhealthyBehaviors.IsEnabled = true;
            }
            if (unhealthyBehaviorsFlag)
            {
                
                healthyBehaviors.IsEnabled = true;
                functions.IsEnabled = true;
            }


            storyboardFlag = 0;
            
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           

        }
    }
}
