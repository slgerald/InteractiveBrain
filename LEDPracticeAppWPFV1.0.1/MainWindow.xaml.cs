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

namespace LEDPracticeAppWPFV1._0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double buttonTopValue;
        public MainWindow()
        {

            InitializeComponent();

           

            
        }

       

        private void interactiveBrainButton_Click(object sender, RoutedEventArgs e)
        {
            pageMarker.Visibility = System.Windows.Visibility.Visible;
            
            buttonTopValue = Canvas.GetTop(interactiveBrainButton);
            Canvas.SetTop(pageMarker, buttonTopValue);

              if(!myCanvas.Children.Contains(interactiveBrainControl.Instance))
               {
                
                myCanvas.Children.Clear();
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
        }

        private void serialCommsButton_Click(object sender, RoutedEventArgs e)
        {
           // pageMarker.Visbile = true;
            pageMarker.Visibility  = System.Windows.Visibility.Visible;
            buttonTopValue = Canvas.GetTop(serialCommsButton);
            Canvas.SetTop(pageMarker, buttonTopValue);
            if (!myCanvas.Children.Contains(serialCommsControl.Instance))
            {
                myCanvas.Children.Clear();
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
                myCanvas.Background = Brushes.White;
                
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

        }

        private void editingButton_Click(object sender, RoutedEventArgs e)
        {
            pageMarker.Visibility = System.Windows.Visibility.Visible;
           
            buttonTopValue = Canvas.GetTop(editingButton);
            Canvas.SetTop(pageMarker, buttonTopValue);
            myCanvas.Children.Clear();
        }
    }
}

    
