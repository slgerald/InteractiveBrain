﻿using System;
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

namespace LEDPracticeAppWPFV1._0._1
{
    /// <summary>
    /// Interaction logic for highSchoolControl.xaml
    /// </summary>
    /// 

    public partial class highSchoolControl : UserControl
    {
        bool functionsFlag;
        bool healthyBehaviorsFlag;
        bool unhealthyBehaviorsFlag;
        System.Windows.Point MouseDownLocation;
        double originalImageHeight;
        double originalImageWidth;
        private object movingObject;
        private double firstXPos, firstYPos;
        private static highSchoolControl _instance; //render userControl based on button pressed



        public highSchoolControl()
        {
            InitializeComponent();
       
            functions.IsChecked = true;
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
        void RestoreScalingFactor(object sender, MouseButtonEventArgs args)

        {

            ((Slider)sender).Value = 1.0;
            brainstemBox.Visibility = System.Windows.Visibility.Visible;
            amygdalaBox.Visibility = System.Windows.Visibility.Visible;
            pituitaryGlandBox.Visibility = System.Windows.Visibility.Visible;
            cutoutLowerBrain.Visibility = System.Windows.Visibility.Visible;
        }
        private void hippocampusBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            brainPartsLabel.Text = "Hippocampus";
            // In this event, we get the current mouse position on the control to use it in the MouseMove event.
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            Console.WriteLine(functionsFlag);

            
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
            
            Mouse.Capture(img);
            if (functionsFlag)
            {
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "The Hippocampus processes short-term memory to convert into long term memory, in limbic system(note long term memories are stored all over the brain)";

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
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
            if (e.ClickCount == 2)
            {
                brainstemBox.Visibility = System.Windows.Visibility.Hidden;
               amygdalaBox.Visibility = System.Windows.Visibility.Hidden;
                pituitaryGlandBox.Visibility = System.Windows.Visibility.Hidden;
                cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                pictureCanvasScaler.Visibility = System.Windows.Visibility.Visible;
            }

        }
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

            Mouse.Capture(img);
            if (functionsFlag)
            {
                healthyBehaviors.IsEnabled = false;
                unhealthyBehaviors.IsEnabled = false;
                factsMessageBox.Text = "The Amygdala is responsible for emotions, particularly survival instincts(fear & aggression), hypersensitive to stress, role in storing emotional memories, in Limbic System";

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
            factsMessageBox.Visibility = System.Windows.Visibility.Visible;
            gotItButton.Visibility = System.Windows.Visibility.Visible;
            brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
            if (e.ClickCount == 2)
            {
                brainstemBox.Visibility = System.Windows.Visibility.Hidden;
                hippocampusBox.Visibility = System.Windows.Visibility.Hidden;
                pituitaryGlandBox.Visibility = System.Windows.Visibility.Hidden;
                cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                pictureCanvasScaler.Visibility = System.Windows.Visibility.Visible;
            }
        }
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

            Mouse.Capture(img);
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
            if (e.ClickCount == 2)
            {
                hippocampusBox.Visibility = System.Windows.Visibility.Hidden;
                amygdalaBox.Visibility = System.Windows.Visibility.Hidden;
                pituitaryGlandBox.Visibility = System.Windows.Visibility.Hidden;
                cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                pictureCanvasScaler.Visibility = System.Windows.Visibility.Visible;
            }

        }
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

            Mouse.Capture(img);
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
            if (e.ClickCount == 2)
            {
                brainstemBox.Visibility = System.Windows.Visibility.Hidden;
                amygdalaBox.Visibility = System.Windows.Visibility.Hidden;
                hippocampusBox.Visibility = System.Windows.Visibility.Hidden;
                cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                pictureCanvasScaler.Visibility = System.Windows.Visibility.Visible;
            }
        }
        public static highSchoolControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new highSchoolControl();
                }
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
