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

namespace LEDPracticeAppWPFV1._0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort SerialPort1;
        bool isConnected = false;
        String[] ports;
        public MainWindow()
        {
           
            InitializeComponent();
            getAvailableComPorts();
            foreach (string port in ports)
            {
                comPortNumberComboBox.Items.Add(port);
                Console.WriteLine(port);
                if (ports[0] != null)
                {
                    comPortNumberComboBox.SelectedItem = ports[0];
                }
            }
        }

        private void onButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("This is the on button");
            try
            {
                SerialPort1.Write("1");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("This is the disconnect button");
            try
            {
                isConnected = false;
                SerialPort1.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }

        private void offButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("This is the off button");
            
            try{
                SerialPort1.Write("0");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("This is the connect button");
            try
            {
                isConnected = true;
                string selectedPort = comPortNumberComboBox.SelectedItem.ToString();
                SerialPort1 = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
                SerialPort1.Open();
                SerialPort1.Write("#STAR\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        void getAvailableComPorts()
        {
            ports = SerialPort.GetPortNames();
        }

    }



    }
