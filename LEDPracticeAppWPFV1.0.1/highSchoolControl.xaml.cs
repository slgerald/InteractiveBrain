using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace InteractiveBrain
{
    /// <summary>
    /// The High School User control is used for interaction with the downstairs brain
    /// </summary>
    /// 

    public partial class highSchoolControl : UserControl
    {   //These flags determine what text  to display when different radio button selections 
        bool functionsFlag;
        bool healthyBehaviorsFlag;
        bool unhealthyBehaviorsFlag;
       // System.Windows.Point MouseDownLocation;
       // double originalImageHeight;
       // double originalImageWidth;
        private object movingObject; //The image being dragged and dropped 
        private double firstXPos, firstYPos;
        private static highSchoolControl _instance; //render userControl based on button pressed


        //This is the entry point for the userControl page 
        public highSchoolControl()
        {
            InitializeComponent();
             
            functions.IsChecked = true; //The page begins with the functions Radio Button being checked first 

            //The following lines ensure that different Mouse Events have an effect 
            pictureCanvasScaler.MouseDoubleClick += new MouseButtonEventHandler(RestoreScalingFactor);

            hippocampusBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(hippocampusBox_PreviewMouseLeftButtonDown);
            hippocampusBox.PreviewMouseMove += new MouseEventHandler(img_PreviewMouseMove);
            hippocampusBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(img_PreviewMouseLeftButtonUp);

            pituitaryGlandBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(pituitaryGlandBox_PreviewMouseLeftButtonDown);
            pituitaryGlandBox.PreviewMouseMove += new MouseEventHandler(img_PreviewMouseMove);
            pituitaryGlandBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(img_PreviewMouseLeftButtonUp);

            brainstemBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(brainstemBox_PreviewMouseLeftButtonDown);
            brainstemBox.PreviewMouseMove += new MouseEventHandler(img_PreviewMouseMove);
            brainstemBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(img_PreviewMouseLeftButtonUp);

            amygdalaBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(amygdalaBox_PreviewMouseLeftButtonDown);
            amygdalaBox.PreviewMouseMove += new MouseEventHandler(img_PreviewMouseMove);
            amygdalaBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(img_PreviewMouseLeftButtonUp);
        }
        public static highSchoolControl Instance //This allows a new highSchool User Control to be instantiated 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new highSchoolControl();
                }
                _instance = new highSchoolControl();
                return _instance;

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
        //This is what happens whem the scaler is clicked twice 
        void RestoreScalingFactor(object sender, MouseButtonEventArgs args)
        {
            ((Slider)sender).Value = 1.0;
            brainstemBox.Visibility = System.Windows.Visibility.Visible;
            amygdalaBox.Visibility = System.Windows.Visibility.Visible;
            pituitaryGlandBox.Visibility = System.Windows.Visibility.Visible;
            hippocampusBox.Visibility = System.Windows.Visibility.Visible;
            cutoutLowerBrain.Visibility = System.Windows.Visibility.Visible;
        }
        //This function determines what happens when the hippocampus is clicked on with the mouse's left button
        private void hippocampusBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            brainPartsLabel.Text = "Hippocampus"; //Label for the brain part 
            // In this event, we get the current mouse position on the control to use it in the MouseMove event.
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
         
            Canvas canvas = img.Parent as Canvas;

            firstXPos = e.GetPosition(img).X;
            firstYPos = e.GetPosition(img).Y;
           
            movingObject = sender;

            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            foreach (System.Windows.Controls.Image child in canvas.Children)
                if (top < Canvas.GetZIndex(child))
                    top = Canvas.GetZIndex(child);
            Canvas.SetZIndex(img, top + 1); 
            Mouse.Capture(img); //So the mouse pointer doesn't "slip" off of image while dragging and dropping 
            if (functionsFlag) 
            {
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "Functions of the Hippocampus include: processing short-term memory to convert into long term memory, in limbic system(note long term memories are stored all over the brain)";
            }

            if (healthyBehaviorsFlag)
            {
                unhealthyBehaviors.IsEnabled = false;
                functions.IsEnabled = false;
                factsMessageBox.Text = "Healthy Behaviors that affect the hippocampus: Matching games, doing mental math problems(without using paper)";
            }
            if (unhealthyBehaviorsFlag)
            {
                healthyBehaviors.IsEnabled = false;
                functions.IsEnabled = false;
                factsMessageBox.Text = "Unhealthy Behaviors that affect the hippocampus";
            }
            //Makes the text box, "got it" button, and brain parts label visible 
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
            //Determines what happens when the image is clicked twice 
            if (e.ClickCount == 2)
            {
                brainstemBox.Visibility = System.Windows.Visibility.Hidden;
               amygdalaBox.Visibility = System.Windows.Visibility.Hidden;
                pituitaryGlandBox.Visibility = System.Windows.Visibility.Hidden;
                cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                pictureCanvasScaler.Visibility = System.Windows.Visibility.Visible;
            }

        }

        //Determines what happens when the amygdala is clicked on 
        private void amygdalaBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            brainPartsLabel.Text = "Amygdala";
            // In this event, we get the current mouse position on the control to use it in the MouseMove event.
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            Console.WriteLine(sender);


            Canvas canvas = img.Parent as Canvas;

            firstXPos = e.GetPosition(img).X;
            firstYPos = e.GetPosition(img).Y;

            movingObject = sender;

            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            foreach (System.Windows.Controls.Image child in canvas.Children)
                if (top < Canvas.GetZIndex(child))
                    top = Canvas.GetZIndex(child);
            Canvas.SetZIndex(img, top + 1);

            Mouse.Capture(img); //So the mouse pointer doesn't "slip" off image when being dragged and dropped 

            //These if statements determine what to display in the textbox and disables whichever radio buttons are not being used 
            if (functionsFlag)
            {
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "Functions of the Amygdala include: emotional processing, particularly survival instincts(fear & aggression), hypersensitive to stress, role in storing emotional memories, in Limbic System";

            }
            if (healthyBehaviorsFlag)
            {
                functions.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "Healthy Behaviors that affect the amygdala";

            }
            if (unhealthyBehaviorsFlag)
            {
                functions.IsEnabled = false;
                healthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "Unhealthy Behaviors that affect the amygdala";
            }

            //makes the textbox, "Got It" button, and brain parts label visible when this brain part is chosen
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;

            //This determines what happens when the amygdala is clicked twice 
            if (e.ClickCount == 2)
            {
                brainstemBox.Visibility = System.Windows.Visibility.Hidden;
                hippocampusBox.Visibility = System.Windows.Visibility.Hidden;
                pituitaryGlandBox.Visibility = System.Windows.Visibility.Hidden;
                cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                pictureCanvasScaler.Visibility = System.Windows.Visibility.Visible;
            }
        }

        //This function determines what happens when the brainstem is left clicked 
        private void brainstemBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            brainPartsLabel.Text = "Brainstem";
            // In this event, we get the current mouse position on the control to use it in the MouseMove event.
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            Console.WriteLine(sender);


            Canvas canvas = img.Parent as Canvas;

            firstXPos = e.GetPosition(img).X;
            firstYPos = e.GetPosition(img).Y;

            movingObject = sender;

            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            foreach (System.Windows.Controls.Image child in canvas.Children)
                if (top < Canvas.GetZIndex(child))
                    top = Canvas.GetZIndex(child);
            Canvas.SetZIndex(img, top + 1);

            Mouse.Capture(img);//So the mouse pointer doesn't "slip" off image when being dragged and dropped 

            //These if statements determine what to display in the textbox and disables whichever radio buttons are not being used
            if (functionsFlag)
            {
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "The Brainstem controls flow of messages between the brain and the rest of the body.Also controls basic body function like breathing, swallowing, heartbeat(medulla), sleep regulation(pons), etc.";
            }

            if (healthyBehaviorsFlag)
            {
                functions.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "Healthy Behaviors that affect the brainstem";

            }
            if (unhealthyBehaviorsFlag)
            {
                functions.IsEnabled = false;
                healthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "Unhealthy Behaviors that affect the brainstem";
            }

            factsMessageBox.Visibility = System.Windows.Visibility.Visible;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;

            if (e.ClickCount == 2) //What happens when the Brainstem is selected 
            {
                hippocampusBox.Visibility = System.Windows.Visibility.Hidden;
                amygdalaBox.Visibility = System.Windows.Visibility.Hidden;
                pituitaryGlandBox.Visibility = System.Windows.Visibility.Hidden;
                cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                pictureCanvasScaler.Visibility = System.Windows.Visibility.Visible;
            }

        }

        //This function determines what happens when the pituitary gland is left clicked 
        private void pituitaryGlandBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            brainPartsLabel.Text = "Pituitary Gland";
            // In this event, we get the current mouse position on the control to use it in the MouseMove event.
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            Console.WriteLine(sender);

            Canvas canvas = img.Parent as Canvas;

            firstXPos = e.GetPosition(img).X;
            firstYPos = e.GetPosition(img).Y;

            movingObject = sender;

            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            foreach (System.Windows.Controls.Image child in canvas.Children)
                if (top < Canvas.GetZIndex(child))
                    top = Canvas.GetZIndex(child);
            Canvas.SetZIndex(img, top + 1);

            Mouse.Capture(img);//So the mouse pointer doesn't "slip" off image when being dragged and dropped 

            //These if statements determine what to display in the textbox and disables whichever radio buttons are not being used

            if (functionsFlag)
            {
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = " The pituitary gland regulates growth hormone, triggers onset of puberty. Attached to the hypothalamus, in Limbic System";
            }
            if (healthyBehaviorsFlag)
            {
                functions.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "Healthy Behaviors that affect the pituitary gland";

            }
            if (unhealthyBehaviorsFlag)
            {
                functions.IsEnabled = false;
                healthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "Unhealthy Behaviors that affect the pituitary gland";
            }

            factsMessageBox.Visibility = System.Windows.Visibility.Visible;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;

            if (e.ClickCount == 2) //What happens when the pituitary gland is double clicked 
            {
                brainstemBox.Visibility = System.Windows.Visibility.Hidden;
                amygdalaBox.Visibility = System.Windows.Visibility.Hidden;
                hippocampusBox.Visibility = System.Windows.Visibility.Hidden;
                cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                pictureCanvasScaler.Visibility = System.Windows.Visibility.Visible;
            }
        }

        //All images use this functions as the "drop" part of their drag and drop operation
        private void img_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            Canvas canvas = img.Parent as Canvas;

            movingObject = null;

            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            foreach (System.Windows.Controls.Image child in canvas.Children)
                if (top > Canvas.GetZIndex(child))
                    top = Canvas.GetZIndex(child);
            Canvas.SetZIndex(img, top + 1);
            Mouse.Capture(null);
        }

        //All images use this function for their "drag" part of their drag and drop operation
        private void img_PreviewMouseMove(object sender, MouseEventArgs e)
        {
          //  double newLeft;
            if (e.LeftButton == MouseButtonState.Pressed && sender == movingObject)
            {
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                Canvas canvas = img.Parent as Canvas;

                double newLeft = e.GetPosition(canvas).X - firstXPos - canvas.Margin.Left;
                // newLeft inside canvas right-border?
                if (newLeft > canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth)
                {
                    newLeft = canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth;
                    Console.WriteLine("new left within canvas right border");
                }
                // newLeft inside canvas left-border?
                else if (newLeft < canvas.Margin.Left)
                {
                    newLeft = canvas.Margin.Left;
                    Console.WriteLine("new left within canvas right border");
                }
                img.SetValue(Canvas.LeftProperty, newLeft);

                double newTop = e.GetPosition(canvas).Y - firstYPos - canvas.Margin.Top;
                // newTop inside canvas bottom-border?
                if (newTop > canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight)
                    newTop = canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight;
                // newTop inside canvas top-border?
                else if (newTop < canvas.Margin.Top)
                    newTop = canvas.Margin.Top;
                img.SetValue(Canvas.TopProperty, newTop);
            }
        }

        

        private void editButton_Click(object sender, RoutedEventArgs e)
        {

        }

        //This function determines what happens when the Got It button is clicked
        //It makes certain controls hidden and others visible. It also reenables 
        //The radio buttons that weren't being used as well as reset the scaler to 
        //the value of 1.0
        private void gotItButton_Click(object sender, RoutedEventArgs e)
        {
            factsMessageBox.Visibility = System.Windows.Visibility.Hidden;
            gotItButton.Visibility = System.Windows.Visibility.Hidden;
            pictureCanvasScaler.Visibility = System.Windows.Visibility.Hidden;
            brainPartsLabel.Visibility = System.Windows.Visibility.Hidden;

            brainstemBox.Visibility = System.Windows.Visibility.Visible;
            amygdalaBox.Visibility = System.Windows.Visibility.Visible;
            pituitaryGlandBox.Visibility = System.Windows.Visibility.Visible;
            cutoutLowerBrain.Visibility = System.Windows.Visibility.Visible;
            hippocampusBox.Visibility = System.Windows.Visibility.Visible;

            pictureCanvasScaler.Value = 1.0;

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
            factsMessageBox.Text = "";     
        }  
    }
}
