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

namespace InteractiveBrain
{
    /// <summary>
    /// The middleSchoolUserControl is used to separate the top brain from the bottom brain and 
    /// provide information about the functions, healthy behaviors and unhealthy behaviors
    /// </summary>
    public partial class middleSchoolControl1 : UserControl
    {
        private static middleSchoolControl1 _instance; //render userControl based on button pressed
        //flags used to determine which radio button has been used 
        bool functionsFlag; //The functions flag used to ensure functions are listed in the facts message box
        bool healthyBehaviorsFlag; //The healthyBehaviorsFlag is used to ensure healthy behaviors are listed in the facts message box
        bool unhealthyBehaviorsFlag; //The unhealthybehaviorsflag is used to ensure unhealthy behaviors are listed in the facts message box
        bool editFunctionsFlag;
        bool editHealthyBehaviorsFlag;
        bool editUnhealthyBehaviorsFlag;
        DoubleAnimation animation = new DoubleAnimation();//animation used for the glowing effect
        Storyboard sbFL; //Storyboard for the frontal lobe
        Storyboard sbTL; //Storyboard for the temporal lobe
        Storyboard sbPL; // storyboard for the parietal lobe
        Storyboard sbOL; //storyboard for the occipital lobe 
        Storyboard sbC;  //storyboard for the cerebellum
        //1-5 are used to determine which storyboard should be affected by the action
        //sbFL = 1, sbTL = 2, sbPL = 3, sbOL = 4, sbC = 5
        int storyboardFlag;

        string brainPart;


        bool defaultFlag; //used to determine if the default settings have changed for functions, healthy behaviors or unhealthy behaviors of body parts
        string changedFactsMessageBoxText;

        string defaultFunctionsFrontalLobe = Properties.Settings.Default.defaultFunctionsFrontalLobe;
        string changedFunctionsFrontalLobe = null;
        bool defaultFunctionsFLFlag = true;
        string defaultHealthyBehaviorsFrontalLobe = Properties.Settings.Default.defaultHealthyBehaviorsFrontalLobe;
        string changedHealthyBehaviorsFrontalLobe = null;
        bool defaultHBFLFlag = true;
        string changedUnhealthyBehaviorsFrontalLobe = null;
        string defaultUnhealthyBehaviorsFrontalLobe = Properties.Settings.Default.defaultUnhealthyBehaviorsFrontalLobe;
        bool defaultUHBFLFlag = true;

        string changedFunctionsTemporalLobe = null;
            string defaultFunctionsTemporalLobe = Properties.Settings.Default.defaultFunctionsTemporalLobe;
        bool defaultFunctionsTLFlag=true;
        string changedHealthyBehaviorsTemporalLobe = null;
        string defaultHealthyBehaviorsTemporalLobe = Properties.Settings.Default.defaultHealthyBehaviorsTemporalLobe;
        bool defaultHBTLFlag = true;
        string changedUnhealthyBehaviorsTemporalLobe = null;
        string defaultUnhealthyBehaviorsTemporalLobe = Properties.Settings.Default.defaultUnhealthyBehaviorsTemporalLobe;
        bool defaultUHBTLFlag = true;

        string changedFunctionsParietalLobe = null;
        string defaultFunctionsParietalLobe = Properties.Settings.Default.defaultFunctionsParietalLobe;
        bool defaultFunctionsPLFlag = true;
        string changedHealthyBehaviorsParietalLobe = null;
        string defaultHealthyBehaviorsParietalLobe = Properties.Settings.Default.defaultHealthyBehaviorsParietalLobe;
        bool defaultHBPLFlag = true;
        string changedUnhealthyBehaviorsParietalLobe = null;
        string defaultUnhealthyBehaviorsParietalLobe = Properties.Settings.Default.defaultUnhealthyBehaviorsParietalLobe;
        bool defaultUHBPLFlag = true;

        string changedFunctionsOccipitalLobe = null;
        string defaultFunctionsOccipitalLobe = Properties.Settings.Default.defaultFunctionsOccipitalLobe;
        bool defaultFunctionsOLFlag = true;
        string changedHealthyBehaviorsOccipitalLobe = null;
        string defaultHealthyBehaviorsOccipitalLobe = Properties.Settings.Default.defaultHealthyBehaviorsOccipitalLobe;
        bool defaultHBOLFlag = true;
        string changedUnhealthyBehaviorsOccipitalLobe = null;
        string defaultUnhealthyBehaviorsOccipitalLobe = Properties.Settings.Default.defaultUnhealthyBehaviorsOccipitalLobe;
        bool defaultUHBOLFlag = true;

        string changedFunctionsCerebellum = null;
        string defaultFunctionsCerebellum = Properties.Settings.Default.defaultFunctionsCerebellum;
        bool defaultFunctionsCFlag = true;
        string changedHealthyBehaviorsCerebellum = null;
        string defaultHealthyBehaviorsCerebellum = Properties.Settings.Default.defaultHealthyBehaviorsCerebellum;
        bool defaultHBCFlag = true;
        string changedUnhealthyBehaviorsCerebellum = null;
        string defaultUnhealthyBehaviorsCerebellum = Properties.Settings.Default.defaultUnhealthyBehaviorsCerebellum;
        bool defaultUHBCFlag = true;



        //This method allows the middle school UserControl to be rendered when called
        public static middleSchoolControl1 Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new middleSchoolControl1();
                }
                _instance = new middleSchoolControl1();
                return _instance;

            }
        }

        //This function initializes the userControl with EventHandlers and other characteristics
        //EventHandlers routed to controls in xaml file
        //Bind animations for brain parts to instatiated storyboards in the xaml file 
        public middleSchoolControl1()
        {
            InitializeComponent();
            functions.IsChecked = true;
            frontalLobeBox.MouseDown += new MouseButtonEventHandler(frontalLobeBox_MouseDown);
            temporalLobeBox.MouseDown += new MouseButtonEventHandler(temporalLobeBox_MouseDown);
            parietalLobeBox.MouseDown += new MouseButtonEventHandler(parietalLobeBox_MouseDown);
            occipitalLobeBox.MouseDown += new MouseButtonEventHandler(occipitalLobeBox_MouseDown);
            cerebellumBox.MouseDown += new MouseButtonEventHandler(cerebellumBox_MouseDown);
            gotItButton.Click += new RoutedEventHandler(gotItButton_Click);
            sbFL = (Storyboard)this.Resources["zoomingFL"];
            sbTL = (Storyboard)this.Resources["zoomingTL"];
            sbPL = (Storyboard)this.Resources["zoomingPL"];
            sbOL = (Storyboard)this.Resources["zoomingOL"];
            sbC = (Storyboard)this.Resources["zoomingC"];
            storyboardFlag = 0; // Initiates storyboard flag at zero
            defaultFlag = true;
            //  functionsIntro = "Functions are: ";
            // healthyBehaviorsIntro = "These healthy behaviors affect the ";
            // unhealthyBehaviorsIntro = "These unhealthy behaviors affect the ";
        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 

            if (editPopup.IsOpen) { editPopup.IsOpen = false; }
            editingTextBox.Text = "";
            editFunctionsRadioButton.IsChecked = false;
            editHealthyBehaviorsRadioButton.IsChecked = false;
            editUnhealthyBehaviorsRadioButton.IsChecked = false;
            brainPartComboBox.SelectedItem = null;
            editingMessageBlock.Text = "";
            //Finish Reset PopUp
        }

        //The following region has code that wasn't used for RenderTransform animation
        #region
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
        #endregion

        //This functions determines what happens when the frontalLobe box is clicked once with any 
        //mouse button
        #region
        private void frontalLobeBox_MouseDown(object sender, MouseEventArgs e)
        {
            brainPartsLabel.Text = "Frontal Lobe";
            storyboardFlag = 1;
            
            //without reverseOn every other time autoreverse = true
            if (sbFL.AutoReverse)
                reverseOn(1);
            else { reverseOff(1); }

            //Hide other parts and make facts box and go button visible
            parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
            occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;

            /*This was used to get location of the frontalLobeBox
             * double objectX = Canvas.GetLeft(frontalLobeBox);
            double objectY = Canvas.GetTop(frontalLobeBox);
            Console.WriteLine("objectX" + objectX + " , objectY" + objectY);
            */
            Console.WriteLine("In the frontal lobe function" + defaultFlag);
            if (functionsFlag)
            {
                if (defaultFunctionsFLFlag)
                {
                    factsMessageBox.Text = "Functions of the Frontal Lobe: " + defaultFunctionsFrontalLobe;
                }
                else
                {
                    Console.WriteLine(changedFunctionsFrontalLobe);
                    factsMessageBox.Text = "Functions of the Frontal Lobe: " + changedFunctionsFrontalLobe;
                    defaultFunctionsFrontalLobe = changedFunctionsFrontalLobe;
                    Console.WriteLine(Properties.Settings.Default.defaultFunctionsFrontalLobe);

                    Properties.Settings.Default.defaultFunctionsFrontalLobe = changedFunctionsFrontalLobe;
                    Properties.Settings.Default.Save();
                    defaultFunctionsFLFlag = true;
                }
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
            }
            if (healthyBehaviorsFlag)
            {
                if (defaultHBFLFlag)
                {
                    factsMessageBox.Text = "Healthy Behaviors that affect the Frontal Lobe: " + defaultHealthyBehaviorsFrontalLobe;
                }
                else
                {
                    factsMessageBox.Text = "Healthy Behaviors that affect the Frontal Lobe: " + changedHealthyBehaviorsFrontalLobe;
                    Properties.Settings.Default.defaultHealthyBehaviorsFrontalLobe = changedHealthyBehaviorsFrontalLobe;
                    Properties.Settings.Default.Save();
                    defaultHBFLFlag = true;
                }
                functions.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
            }
            if (unhealthyBehaviorsFlag)
            {
                if (defaultUHBFLFlag)
                {
                    factsMessageBox.Text = "Unhealthy Behaviors that affect the Frontal Lobe : " + Properties.Settings.Default.defaultUnhealthyBehaviorsFrontalLobe;

                }
                else
                {
                    factsMessageBox.Text = "Unhealthy Behaviors that affect the Frontal Lobe: " + changedUnhealthyBehaviorsFrontalLobe;
                    Properties.Settings.Default.defaultUnhealthyBehaviorsFrontalLobe = changedUnhealthyBehaviorsFrontalLobe;
                    Properties.Settings.Default.Save();
                    defaultUHBFLFlag = true;
                }
                
                healthyBehaviors.IsEnabled = false;
                functions.IsEnabled = false;
            }
        }
        #endregion

        //This functions determines what happens when the temporalLobe box is clicked once with any 
        //mouse button
        #region
        private void temporalLobeBox_MouseDown(object sender, MouseEventArgs e)
        {
            brainPartsLabel.Text = "Temporal Lobe";
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
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;

            if (functionsFlag)
            {
                if (defaultFunctionsTLFlag)
                {
                    factsMessageBox.Text = "Functions of the Temporal Lobe: " + Properties.Settings. Default.defaultFunctionsTemporalLobe;

                }
                else
                {
                    factsMessageBox.Text = "Functions of Temporal Lobe: " + changedFunctionsTemporalLobe;
                    Properties.Settings.Default.defaultFunctionsTemporalLobe = changedFunctionsTemporalLobe;
                    Properties.Settings.Default.Save();
                    defaultFunctionsTLFlag = true;
                }
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
            }
            if (healthyBehaviorsFlag)
            {
                if (defaultHBTLFlag)
                {
                    factsMessageBox.Text = "Healthy Behaviors that affect the Temporal Lobe: " + Properties.Settings.Default.defaultHealthyBehaviorsTemporalLobe;
                }
                else
                {
                    factsMessageBox.Text = "Healthy Behaviors that affect the Temporal Lobe: " + changedHealthyBehaviorsTemporalLobe;
                    Properties.Settings.Default.defaultHealthyBehaviorsTemporalLobe = changedHealthyBehaviorsTemporalLobe;
                    Properties.Settings.Default.Save();
                    defaultHBTLFlag = true;
                }
                functions.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
            }
            if (unhealthyBehaviorsFlag)
            {
                if (defaultUHBTLFlag)
                {
                    factsMessageBox.Text = "Unhealthy Behaviors that affect the Temporal Lobe: " + Properties.Settings.Default.defaultUnhealthyBehaviorsTemporalLobe;
                }
                else
                {
                    factsMessageBox.Text = "Unhealthy Behaviors that affect the Temporal Lobe: " + changedUnhealthyBehaviorsTemporalLobe;
                    Properties.Settings.Default.defaultUnhealthyBehaviorsTemporalLobe = changedUnhealthyBehaviorsTemporalLobe;
                    Properties.Settings.Default.Save();
                    defaultUHBTLFlag = true;
                }
                functions.IsEnabled = false;
                healthyBehaviors.IsEnabled = false;
            }
        }
        #endregion

        //This functions determines what happens when the parietalLobe box is clicked once with any 
        //mouse button
        #region
        private void parietalLobeBox_MouseDown(object sender, MouseEventArgs e)
        {
            brainPartsLabel.Text = "Parietal Lobe";

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
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;

            if (functionsFlag)
            {
                if (defaultFunctionsPLFlag)
                {
                    factsMessageBox.Text = "Functions of the Parietal Lobe: " + Properties.Settings.Default.defaultFunctionsParietalLobe;
                }
                else
                {
                    factsMessageBox.Text = "Functions of the Parietal Lobe: " + changedFunctionsParietalLobe;
                    Properties.Settings.Default.defaultFunctionsParietalLobe = changedFunctionsParietalLobe;
                    Properties.Settings.Default.Save();
                    defaultFunctionsPLFlag = true;
                }
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
            }
            if (healthyBehaviorsFlag)
            {
                if (defaultHBPLFlag)
                {
                    factsMessageBox.Text = "Healthy Behaviors that affect the Parietal Lobe: " + Properties.Settings.Default.defaultHealthyBehaviorsParietalLobe;
                }
                else
                {
                    factsMessageBox.Text = "Healthy Behaviors that affect the Parietal Lobe: " + changedHealthyBehaviorsParietalLobe;
                    Properties.Settings.Default.defaultHealthyBehaviorsParietalLobe = changedHealthyBehaviorsParietalLobe;
                    Properties.Settings.Default.Save();
                    defaultHBPLFlag = true;
                }
                functions.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;

            }
            if (unhealthyBehaviorsFlag)
            {
                if (defaultUHBPLFlag)
                {
                    factsMessageBox.Text = "Unhealthy Behaviors that affect the Parietal Lobe: " + Properties.Settings.Default.defaultUnhealthyBehaviorsParietalLobe;

                }
                else
                {
                    factsMessageBox.Text = "Unhealthy Behaviors that affect the Parietal Lobe: " + changedUnhealthyBehaviorsParietalLobe;
                    Properties.Settings.Default.defaultUnhealthyBehaviorsParietalLobe = changedUnhealthyBehaviorsParietalLobe;
                    Properties.Settings.Default.Save();
                    defaultUHBPLFlag = true;
                }
                functions.IsEnabled = false;
                healthyBehaviors.IsEnabled = false;

            }
        }
        #endregion
        //This functions determines what happens when the occipitalLobe box is clicked once with any 
        //mouse button
        #region
        private void occipitalLobeBox_MouseDown(object sender, MouseEventArgs e)
        {
            brainPartsLabel.Text = "Occipital Lobe";
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
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;

            if (functionsFlag)
            {
                if (defaultFunctionsOLFlag)
                {
                    factsMessageBox.Text = "Functions of the Occipital Lobe: " + Properties.Settings.Default.defaultFunctionsOccipitalLobe;
                }
                else
                {
                    factsMessageBox.Text = "Functions of the Occipital Lobe: " + changedFunctionsOccipitalLobe;
                    Properties.Settings.Default.defaultFunctionsOccipitalLobe = changedFunctionsOccipitalLobe;
                    Properties.Settings.Default.Save();
                    defaultFunctionsOLFlag = true;
                }
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
            }
            if (healthyBehaviorsFlag)
            {
                if (defaultHBOLFlag)
                {
                    factsMessageBox.Text = "Healthy Behaviors that affect the Occipital Lobe: " + Properties.Settings.Default.defaultHealthyBehaviorsOccipitalLobe;
                }
                else
                {
                    factsMessageBox.Text = "Healthy Behaviors that affect the Occipital Lobe: " + changedHealthyBehaviorsOccipitalLobe;
                    Properties.Settings.Default.defaultHealthyBehaviorsOccipitalLobe = changedHealthyBehaviorsOccipitalLobe;
                    Properties.Settings.Default.Save();
                    defaultHBOLFlag = true;
                }
                functions.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
            }
            if (unhealthyBehaviorsFlag)
            {
                if (defaultUHBOLFlag)
                {
                    factsMessageBox.Text = "Unhealthy Behaviors that affect the Occipital Lobe: " + Properties.Settings.Default.defaultUnhealthyBehaviorsOccipitalLobe;
                }
                else
                {
                    factsMessageBox.Text = "Unhealthy Behaviors that affect the Occipital Lobe: " + changedUnhealthyBehaviorsOccipitalLobe;
                    Properties.Settings.Default.defaultUnhealthyBehaviorsOccipitalLobe = changedUnhealthyBehaviorsOccipitalLobe;
                    Properties.Settings.Default.Save();
                    defaultUHBOLFlag = true;
                }
                functions.IsEnabled = false;
                healthyBehaviors.IsEnabled = false;
            }
        }
        #endregion

        //This functions determines what happens when the cerebellum box is clicked once with any 
        //mouse button
        #region
        private void cerebellumBox_MouseDown(object sender, MouseEventArgs e)
        {
            brainPartsLabel.Text = "Cerebellum Lobe";
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
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;

            if (functionsFlag)
            {
                if (defaultFunctionsCFlag)
                {
                    factsMessageBox.Text = "Functions of the Cerebellum: " + Properties.Settings.Default.defaultFunctionsCerebellum;
                }
                else
                {
                    factsMessageBox.Text = "Functions of the Cerebellum: " + changedFunctionsCerebellum;
                    Properties.Settings.Default.defaultFunctionsCerebellum = changedFunctionsCerebellum;
                    Properties.Settings.Default.Save();
                    defaultFunctionsCFlag = true;
                }
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
            }
            if (healthyBehaviorsFlag)
            {
                if (defaultHBCFlag)
                {
                    factsMessageBox.Text = "Healthy Behaviors that affect the Cerebellum: " + Properties.Settings.Default.defaultHealthyBehaviorsCerebellum;
                }
                else
                {
                    factsMessageBox.Text = "Healthy Behaviors that affect the Cerebellum: " + changedHealthyBehaviorsCerebellum;
                    Properties.Settings.Default.defaultHealthyBehaviorsCerebellum = changedHealthyBehaviorsCerebellum;
                    Properties.Settings.Default.Save();
                    defaultHBCFlag = true;
                }
                unhealthyBehaviors.IsEnabled = false;
                functions.IsEnabled = false; 

            }
            if (unhealthyBehaviorsFlag)
            {
                if (defaultUHBCFlag)
                {
                    factsMessageBox.Text = "Unhealthy Behaviors that affect the Cerebellum: " + Properties.Settings.Default.defaultUnhealthyBehaviorsCerebellum;
                }
                else
                {
                    factsMessageBox.Text = "Unhealthy Behaviors that affect the Cerebellum: " + changedUnhealthyBehaviorsCerebellum;
                    Properties.Settings.Default.defaultUnhealthyBehaviorsCerebellum = changedUnhealthyBehaviorsCerebellum;
                    Properties.Settings.Default.Save();
                    defaultUHBCFlag = true;
                }
                functions.IsEnabled = false;
                healthyBehaviors.IsEnabled = false;
            }
        }
        #endregion

        //When functions radio button is chosen
        //This function sets functions flag to 
        //true and healthyBehaviors and unhealthybehaviors to false
        private void functions_Checked(object sender, RoutedEventArgs e)
        {
            functionsFlag = true;
            healthyBehaviorsFlag = false;
            unhealthyBehaviorsFlag = false;
        }
        //When healthy behaviors radio button is chosen 
        //This function sets healthyBehaviors flags to true 
        //and unhealthyBehaviors and functions to false 
        private void healthyBehaviors_Checked(object sender, RoutedEventArgs e)
        {
            healthyBehaviorsFlag = true;
            functionsFlag = false;
            unhealthyBehaviorsFlag = false;
        }
        //When the unhealthybehaviors radio button is chosen
        //This function set unhealthyBehaviors flags to true 
        //and healthyBehaviors and functions flag to false 
        private void unhealthyBehaviors_Checked(object sender, RoutedEventArgs e)
        {
            unhealthyBehaviorsFlag = true;
            functionsFlag = false;
            healthyBehaviorsFlag = false;
        }

        //Depending on the brain part
        //This functions sets the autoreverse attribute 
        //of that brain part's storyboard false
        private void reverseOff(int storyboardFlag)
        {
            switch (storyboardFlag)
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
        //Depending on the brain part
        //This functions sets the autoreverse attribute 
        //of that brain part's storyboard true
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
        //This function helps ensure the correct AutoReverse attribute of the selected brain part
        //Which brain parts to make visible based on brain part selection
        //Make the gotItbutton, facts message box, and brain parts label hidden
        #region
        private void gotItButton_Click(object sender, RoutedEventArgs e)
        {     //sbFL = 1, sbTL = 2, sbPL = 3, sbOL = 4, sbC = 5
            //Safeguard against incorrect AutoREverse attribute of selected brain part 
            if (sbFL.AutoReverse)
            { reverseOn(1); }
            else { reverseOff(1); }
            if (sbTL.AutoReverse)
            { reverseOn(2); }
            else { reverseOff(2); }
            if (sbPL.AutoReverse)
            { reverseOn(3); }
            else { reverseOff(3); }
            if (sbOL.AutoReverse)
            { reverseOn(4); }
            else { reverseOff(4); }
            if (sbC.AutoReverse)
            { reverseOn(5); }
            else { reverseOff(5); }

            //Based on the brain part selected, determines which parts
            //should become visible again
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
            brainPartsLabel.Visibility = System.Windows.Visibility.Hidden;

            //Safegaurd against incorrect AutoReverse attribute of selected brain part 
            if (!sbFL.AutoReverse && storyboardFlag == 1)
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


            storyboardFlag = 0; //reset storyboard flag

        }
        #endregion
        //Purposely Empty Function
        //Means nothing is triggered by this event
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {


        }
        //Purposely Empty Function
        //Means nothing is triggered by this event
        private void brainPartsLabel_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!editPopup.IsOpen)
            {
                editPopup.IsOpen = true;
            }
            
            
        }

        private void editingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            changedFactsMessageBoxText = editingTextBox.Text;


        }
        #region
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                brainPart = ((ComboBoxItem)brainPartComboBox.SelectedItem).Content.ToString();
            }
            catch
            {
                if(brainPart == null && !(editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
                {
                    editingMessageBlock.Text = "Choose Brain Part and either functions, healthy behaviors and unhealthy behaviors";
                }
                if (brainPart == null && (editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag)) {
                    editingMessageBlock.Text = "Choose Brain Part";
                }

                if (brainPart != null && !(editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
                { editingMessageBlock.Text = "Choose edither functions, healthy behaviors, and unhealthy behaviors"; }
                if (brainPart != null && (editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
                {
                    if (editFunctionsFlag)
                        switch (brainPart)
                        {
                            case "Cerebellum":
                                changedFunctionsCerebellum = changedFactsMessageBoxText;
                                defaultFunctionsCFlag = false;
                                break;
                            case "Occipital Lobe":
                                changedFunctionsOccipitalLobe = changedFactsMessageBoxText;
                                defaultFunctionsOLFlag = false;
                                break;
                            case "Parietal Lobe":
                                changedFunctionsParietalLobe = changedFactsMessageBoxText;
                                defaultFunctionsPLFlag = false;
                                break;
                            case "Temporal Lobe":
                                changedFunctionsTemporalLobe = changedFactsMessageBoxText;
                                defaultFunctionsTLFlag = false;
                                break;
                            case "Frontal Lobe":
                                //editingTextBox.Text = defaultFunctionsFrontalLobe;
                                changedFunctionsFrontalLobe = changedFactsMessageBoxText;
                                defaultFunctionsFLFlag = false;
                                Console.WriteLine("In the editing button function " + changedFunctionsFrontalLobe);
                                break;

                        }


                    if (editHealthyBehaviorsFlag)
                    {
                        switch (brainPart)
                        {
                            case "Cerebellum":

                                changedHealthyBehaviorsCerebellum = changedFactsMessageBoxText;
                                defaultHBCFlag = false;
                                break;
                            case "Occipital Lobe":
                                changedHealthyBehaviorsOccipitalLobe = changedFactsMessageBoxText;
                                defaultHBOLFlag = false;
                                break;
                            case "Parietal Lobe":
                                changedHealthyBehaviorsParietalLobe = changedFactsMessageBoxText;
                                defaultHBPLFlag = false;
                                break;
                            case "Temporal Lobe":
                                changedHealthyBehaviorsTemporalLobe = changedFactsMessageBoxText;
                                defaultHBTLFlag = false;
                                break;
                            case "Frontal Lobe":

                                changedHealthyBehaviorsFrontalLobe = changedFactsMessageBoxText;
                                defaultHBFLFlag = false;
                                break;
                        }
                    }
                    if (editUnhealthyBehaviorsFlag)
                    {
                        switch (brainPart)
                        {
                            case "Cerebellum":
                                editingTextBox.Text = defaultUnhealthyBehaviorsCerebellum;
                                changedUnhealthyBehaviorsCerebellum = changedFactsMessageBoxText;
                                defaultUHBCFlag = false;

                                break;
                            case "Occipital Lobe":
                                changedUnhealthyBehaviorsOccipitalLobe = changedFactsMessageBoxText;
                                defaultUHBOLFlag = false;
                                break;
                            case "Parietal Lobe":
                                changedUnhealthyBehaviorsParietalLobe = changedFactsMessageBoxText;
                                defaultUHBPLFlag = false;
                                break;
                            case "Temporal Lobe":
                                changedUnhealthyBehaviorsTemporalLobe = changedFactsMessageBoxText;
                                defaultUHBTLFlag = false;
                                break;
                            case "Frontal Lobe":
                                changedUnhealthyBehaviorsFrontalLobe = changedFactsMessageBoxText;
                                defaultUHBFLFlag = false;
                                break;
                        }

                    }

                    editingTextBox.Text = "";
                    editingMessageBlock.Text = "Saved.";
                }
            }
            
        }
#endregion
        private void functionsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            editFunctionsFlag = true;
            editHealthyBehaviorsFlag = false;
            editUnhealthyBehaviorsFlag = false;
        }

        private void healthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            editFunctionsFlag = false;
            editHealthyBehaviorsFlag = true;
            editUnhealthyBehaviorsFlag = false;
        }

        private void unhealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            editFunctionsFlag = false;
            editHealthyBehaviorsFlag = false;
            editUnhealthyBehaviorsFlag = true;
        }

        private void brainPartComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                brainPart = ((ComboBoxItem)brainPartComboBox.SelectedItem).Content.ToString();
            }
            catch
            {
                if (brainPart == null && !(editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
                {
                    editingMessageBlock.Text = "Choose Brain Part and either functions, healthy behaviors and unhealthy behaviors";
                }
                if (brainPart == null && (editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
                {
                    editingMessageBlock.Text = "Choose Brain Part";
                }

                if (brainPart != null && !(editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
                { editingMessageBlock.Text = "Choose edither functions, healthy behaviors, and unhealthy behaviors"; }
                if (brainPart != null && (editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
                {
                    if (editFunctionsFlag)
                        switch (brainPart)
                        {
                            case "Cerebellum":
                                changedFunctionsCerebellum = changedFactsMessageBoxText;
                                defaultFunctionsCFlag = false;
                                break;
                            case "Occipital Lobe":
                                changedFunctionsOccipitalLobe = changedFactsMessageBoxText;
                                defaultFunctionsOLFlag = false;
                                break;
                            case "Parietal Lobe":
                                changedFunctionsParietalLobe = changedFactsMessageBoxText;
                                defaultFunctionsPLFlag = false;
                                break;
                            case "Temporal Lobe":
                                changedFunctionsTemporalLobe = changedFactsMessageBoxText;
                                defaultFunctionsTLFlag = false;
                                break;
                            case "Frontal Lobe":
                                //editingTextBox.Text = defaultFunctionsFrontalLobe;
                                changedFunctionsFrontalLobe = changedFactsMessageBoxText;
                                defaultFunctionsFLFlag = false;
                                Console.WriteLine("In the editing button function " + changedFunctionsFrontalLobe);
                                break;

                        }


                    if (editHealthyBehaviorsFlag)
                    {
                        switch (brainPart)
                        {
                            case "Cerebellum":

                                changedHealthyBehaviorsCerebellum = changedFactsMessageBoxText;
                                defaultHBCFlag = false;
                                break;
                            case "Occipital Lobe":
                                changedHealthyBehaviorsOccipitalLobe = changedFactsMessageBoxText;
                                defaultHBOLFlag = false;
                                break;
                            case "Parietal Lobe":
                                changedHealthyBehaviorsParietalLobe = changedFactsMessageBoxText;
                                defaultHBPLFlag = false;
                                break;
                            case "Temporal Lobe":
                                changedHealthyBehaviorsTemporalLobe = changedFactsMessageBoxText;
                                defaultHBTLFlag = false;
                                break;
                            case "Frontal Lobe":

                                changedHealthyBehaviorsFrontalLobe = changedFactsMessageBoxText;
                                defaultHBFLFlag = false;
                                break;
                        }
                    }
                    if (editUnhealthyBehaviorsFlag)
                    {
                        switch (brainPart)
                        {
                            case "Cerebellum":
                                editingTextBox.Text = defaultUnhealthyBehaviorsCerebellum;
                                changedUnhealthyBehaviorsCerebellum = changedFactsMessageBoxText;
                                defaultUHBCFlag = false;

                                break;
                            case "Occipital Lobe":
                                changedUnhealthyBehaviorsOccipitalLobe = changedFactsMessageBoxText;
                                defaultUHBOLFlag = false;
                                break;
                            case "Parietal Lobe":
                                changedUnhealthyBehaviorsParietalLobe = changedFactsMessageBoxText;
                                defaultUHBPLFlag = false;
                                break;
                            case "Temporal Lobe":
                                changedUnhealthyBehaviorsTemporalLobe = changedFactsMessageBoxText;
                                defaultUHBTLFlag = false;
                                break;
                            case "Frontal Lobe":
                                changedUnhealthyBehaviorsFrontalLobe = changedFactsMessageBoxText;
                                defaultUHBFLFlag = false;
                                break;
                        }

                    }

                    //  editingTextBox.Text = "";
                    editingMessageBlock.Text = "Saved.";
                }
            }
        }
    }
}

