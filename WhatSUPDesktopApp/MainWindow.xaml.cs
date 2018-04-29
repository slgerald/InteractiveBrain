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

namespace WhatSUPDesktopApp 

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
            //This creates the local folder of the app on the local machine 
            // AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            mainWindow.MaxHeight = System.Windows.SystemParameters.FullPrimaryScreenHeight;
            mainWindow.MaxWidth = System.Windows.SystemParameters.FullPrimaryScreenWidth;
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
            if (!myCanvas.Children.Contains(InteractiveBrainControl.Instance))
               {
                
                 
                 myCanvas.Children.Add(InteractiveBrainControl.Instance);
             }
             else
                myCanvas.Children.Add(InteractiveBrainControl.Instance);
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
            if (!myCanvas.Children.Contains(SerialCommsControl.Instance))
            {
                
                myCanvas.Children.Add(SerialCommsControl.Instance);
            }
            else
                myCanvas.Children.Add(SerialCommsControl.Instance);

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
            if (!myCanvas.Children.Contains(UpstairsBrainControl.Instance))
            { 
                myCanvas.Children.Add(UpstairsBrainControl.Instance);
            }
            else
                myCanvas.Children.Add(UpstairsBrainControl.Instance);
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
            if (!myCanvas.Children.Contains(DownstairsBrainControl.Instance))
            {
                myCanvas.Children.Add(DownstairsBrainControl.Instance);
            }
            else
                myCanvas.Children.Add(DownstairsBrainControl.Instance);
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


                poeCenterIcon.Visibility = System.Windows.Visibility.Hidden;





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

                poeCenterIcon.Visibility = System.Windows.Visibility.Visible;
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




    
