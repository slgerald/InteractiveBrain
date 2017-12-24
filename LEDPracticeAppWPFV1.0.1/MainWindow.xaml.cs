using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Collections.Concurrent;
using System.Windows.Media.Animation;

namespace InteractiveBrain

{
    /// <summary>
    /// This is the entry point of the application
    /// </summary>
    public partial class MainWindow : Window
    {
        double buttonTopValue;
        
        public MainWindow()
        { 
            InitializeComponent();
        }

        //There is one function for each of the buttons on the left panel
        //It moves the page marker to highlight the button that was clicked 
        //It clears previously instantiated user controls to make room for the
        //button that was most recently clicked 
        private void interactiveBrainButton_Click(object sender, RoutedEventArgs e)
        {
          //  pageMarker.Visibility = System.Windows.Visibility.Visible;
            buttonTopValue = Canvas.GetTop(interactiveBrainButton);
            if (buttonPanel.Width != 45)
            {
               // Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            myCanvas.Children.Clear();
            if (!myCanvas.Children.Contains(interactiveBrainControl.Instance))
               {
                
                 
                 myCanvas.Children.Add(interactiveBrainControl.Instance);
             }
             else
                myCanvas.Children.Add(interactiveBrainControl.Instance);
        }


        private void simulationButton_Click(object sender, RoutedEventArgs e)
        {
          //  pageMarker.Visibility = System.Windows.Visibility.Visible;
            buttonTopValue = Canvas.GetTop(simulationButton);
            if (buttonPanel.Width != 45)
            {
              //  Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            myCanvas.Children.Clear();
            if (!myCanvas.Children.Contains(simulationUserControl.Instance))
            {

                myCanvas.Children.Add(simulationUserControl.Instance);
            }
            else
                myCanvas.Children.Add(simulationUserControl.Instance);
        }

        private void serialCommsButton_Click(object sender, RoutedEventArgs e)
        {
           
          //  pageMarker.Visibility  = System.Windows.Visibility.Visible;
            buttonTopValue = Canvas.GetTop(serialCommsButton);
            if (buttonPanel.Width != 45)
            {
               // Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            myCanvas.Children.Clear();
            if (!myCanvas.Children.Contains(serialCommsControl.Instance))
            {
                
                myCanvas.Children.Add(serialCommsControl.Instance);
            }
            else
                myCanvas.Children.Add(serialCommsControl.Instance);

        }

        private void middleSchoolButton_Click(object sender, RoutedEventArgs e)
        {
          //  pageMarker.Visibility = System.Windows.Visibility.Visible;
           
            buttonTopValue = Canvas.GetTop(middleSchoolButton);
            if (buttonPanel.Width != 45)
            {
              //  Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            myCanvas.Children.Clear();
            if (!myCanvas.Children.Contains(middleSchoolControl1.Instance))
            { 
                myCanvas.Children.Add(middleSchoolControl1.Instance);
            }
            else
                myCanvas.Children.Add(middleSchoolControl1.Instance);
        }

        private void highSchoolButton_Click(object sender, RoutedEventArgs e)
        {
           // pageMarker.Visibility = System.Windows.Visibility.Visible;
            
            buttonTopValue = Canvas.GetTop(highSchoolButton);
            if (buttonPanel.Width != 45)
            {
               // Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            myCanvas.Children.Clear();
            if (!myCanvas.Children.Contains(highSchoolControl.Instance))
            {
                myCanvas.Children.Add(highSchoolControl.Instance);
            }
            else
                myCanvas.Children.Add(highSchoolControl.Instance);
        }

        
        private void expandMenuButton_Click(object sender, RoutedEventArgs e) {

            Thickness margin;
            Thickness margin2;
            if (buttonViewbox.Width == 220 && buttonViewbox.Height == 600)
            {
                buttonViewbox.Width = 45;
                buttonViewbox.Height = 600;
                myCanvasViewbox.Height = 755;
                myCanvas.Height = 755;
              
                buttonPanel.Width = 45;
                
                margin = myCanvasViewbox.Margin;
             //  margin2 = myCanvas.Margin;
                margin.Left = -175;
             //   margin2.Left = -175;
                myCanvasViewbox.Margin = margin;
           //    myCanvas.Margin = margin2;
             
                myCanvasViewbox.Width = 855;
             //   myCanvas.Width = 855;
                buttonViewbox.Height = 600;
                

            }
            else
            {
                buttonViewbox.Width = 220;
                buttonViewbox.Height = 600;
                buttonPanel.Width = 220;
                buttonViewbox.Height = 600;

              //  margin = myCanvasViewbox.Margin;
             //   margin.Left = 0;
             //   myCanvasViewbox.Margin = margin;
              //  myCanvasViewbox.Height = 600;
              
              //  myCanvasViewbox.Width = 680;
            }

        }

       
    }


        //  private void editingButton_Click(object sender, RoutedEventArgs e)
        //{
        //    pageMarker.Visibility = System.Windows.Visibility.Visible;

        //    buttonTopValue = Canvas.GetTop(editingButton);
        //   Canvas.SetTop(pageMarker, buttonTopValue);
        //    myCanvas.Children.Clear();
        //    if (!myCanvas.Children.Contains(settingsUserControl.Instance))
        //    {
        //        myCanvas.Children.Add(settingsUserControl.Instance);
        //    }
        //    else
        //        myCanvas.Children.Add(settingsUserControl.Instance);
        // }
    }




    
