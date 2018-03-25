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
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
        }

        //There is one function for each of the buttons on the left panel
        //It moves the page marker to highlight the button that was clicked 
        //It clears previously instantiated user controls to make room for the
        //button that was most recently clicked 
        private void InteractiveBrainButton_Click(object sender, RoutedEventArgs e)
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


        private void SimulationButton_Click(object sender, RoutedEventArgs e)
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

        private void SerialCommsButton_Click(object sender, RoutedEventArgs e)
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

        private void UpstairsBrainButton_Click(object sender, RoutedEventArgs e)
        {
          //  pageMarker.Visibility = System.Windows.Visibility.Visible;
           
            buttonTopValue = Canvas.GetTop(upstairsBrainButton);
            if (buttonPanel.Width != 45)
            {
              //  Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            myCanvas.Children.Clear();
            if (!myCanvas.Children.Contains(upstairsBrainControl.Instance))
            { 
                myCanvas.Children.Add(upstairsBrainControl.Instance);
            }
            else
                myCanvas.Children.Add(upstairsBrainControl.Instance);
        }

        private void DownstairsBrainButton_Click(object sender, RoutedEventArgs e)
        {
           // pageMarker.Visibility = System.Windows.Visibility.Visible;
            
            buttonTopValue = Canvas.GetTop(downstairsBrainButton);
            if (buttonPanel.Width != 45)
            {
               // Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            myCanvas.Children.Clear();
            if (!myCanvas.Children.Contains(downstairsBrainControl.Instance))
            {
                myCanvas.Children.Add(downstairsBrainControl.Instance);
            }
            else
                myCanvas.Children.Add(downstairsBrainControl.Instance);
        }

        
        private void ExpandMenuButton_Click(object sender, RoutedEventArgs e) {

            Thickness margin;
            
            if (buttonViewbox.Width == 220 && buttonViewbox.Height == 600)
            {
                buttonViewbox.Width = 45;
                buttonViewbox.Height = 600;
                buttonPanel.Width = 45;
                buttonViewbox.Height = 600;

                myCanvasViewbox.Width = 855;
                myCanvasViewbox.Height = 755;
                myCanvas.Height = 755;
              
                margin = myCanvasViewbox.Margin;
                margin.Left = -175;
                myCanvasViewbox.Margin = margin;
           
             
                
             
                
                

            }
            else
            {
                buttonViewbox.Width = 220;
                buttonViewbox.Height = 600;
                buttonPanel.Width = 220;
                buttonViewbox.Height = 600;

                margin = myCanvasViewbox.Margin;
                margin.Left = 0;
                myCanvasViewbox.Margin = margin;
                myCanvasViewbox.Height = 600;
                myCanvas.Height = 600;
                myCanvasViewbox.Width = 690;
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




    
