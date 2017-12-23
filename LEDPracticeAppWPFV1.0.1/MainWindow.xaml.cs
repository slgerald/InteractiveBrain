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
        bool defaultFlag;
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
            pageMarker.Visibility = System.Windows.Visibility.Visible;
            buttonTopValue = Canvas.GetTop(interactiveBrainButton);
            Canvas.SetTop(pageMarker, buttonTopValue);
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
            pageMarker.Visibility = System.Windows.Visibility.Visible;
            buttonTopValue = Canvas.GetTop(simulationButton);
            Canvas.SetTop(pageMarker, buttonTopValue);
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
           
            pageMarker.Visibility  = System.Windows.Visibility.Visible;
            buttonTopValue = Canvas.GetTop(serialCommsButton);
            Canvas.SetTop(pageMarker, buttonTopValue);
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
            pageMarker.Visibility = System.Windows.Visibility.Visible;
           
            buttonTopValue = Canvas.GetTop(middleSchoolButton);
            Canvas.SetTop(pageMarker, buttonTopValue);
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
            pageMarker.Visibility = System.Windows.Visibility.Visible;
            
            buttonTopValue = Canvas.GetTop(highSchoolButton);
            Canvas.SetTop(pageMarker, buttonTopValue);
            myCanvas.Children.Clear();
            if (!myCanvas.Children.Contains(highSchoolControl.Instance))
            {
                myCanvas.Children.Add(highSchoolControl.Instance);
            }
            else
                myCanvas.Children.Add(highSchoolControl.Instance);
        }

        private void menuButton_Checked(object sender, RoutedEventArgs e)
        {
           
                ImageBrush brush1 = new ImageBrush();
                brush1.ImageSource = new BitmapImage(new Uri("Resources/images/if_menu.ico"));
                menuButton.Background = brush1;
           
        }
        private void menuButton_Unchecked(object sender, RoutedEventArgs e) { 
            
                ImageBrush brush1 = new ImageBrush();
                brush1.ImageSource = new BitmapImage(new Uri("Resources/images/if_icon-arrow-left.ico"));
                menuButton.Background = brush1;
            
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




    
