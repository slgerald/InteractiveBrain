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

namespace LEDPracticeAppWPFV1._0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        


        public MainWindow()
        {

            InitializeComponent();

            interactiveBrainButton.Tag = new interactiveBrainControl(); 

            
        }

       

        private void interactiveBrainButton_Click(object sender, RoutedEventArgs e)
        {

           if(!myCanvas.Children.Contains(interactiveBrainControl.Instance))
            {
                myCanvas.Background = Brushes.White;
                myCanvas.Children.Remove(serialCommsControl.Instance);
                myCanvas.Children.Remove(middleSchoolControl1.Instance);
                myCanvas.Children.Add(interactiveBrainControl.Instance);
            }
           else
                myCanvas.Children.Add(interactiveBrainControl.Instance);
        }


        private void simulationButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void serialCommsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!myCanvas.Children.Contains(serialCommsControl.Instance))
            {
                myCanvas.Background = Brushes.White;
                myCanvas.Children.Remove(interactiveBrainControl.Instance);
                myCanvas.Children.Remove(middleSchoolControl1.Instance);
                myCanvas.Children.Add(serialCommsControl.Instance);
            }
            else
                myCanvas.Children.Add(serialCommsControl.Instance);

        }

        private void middleSchoolButton_Click(object sender, RoutedEventArgs e)
        {
            if (!myCanvas.Children.Contains(middleSchoolControl1.Instance))
            {
                myCanvas.Background = Brushes.White;
                myCanvas.Children.Remove(interactiveBrainControl.Instance);
                myCanvas.Children.Remove(serialCommsControl.Instance);
                myCanvas.Children.Add(middleSchoolControl1.Instance);
            }
            else
                myCanvas.Children.Add(middleSchoolControl1.Instance);
        }

        private void highSchoolButton_Click(object sender, RoutedEventArgs e)
        {

        }

     
    }
}

    
