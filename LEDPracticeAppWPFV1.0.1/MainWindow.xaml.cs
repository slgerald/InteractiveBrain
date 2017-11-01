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
        SerialPort SerialPort1;
        bool isConnected = false;
        String[] ports;
        static ConcurrentQueue<char> serialDataQueue;
        public MainWindow()
        {

            InitializeComponent();
            getAvailableComPorts();
            StackPanel1.Height = middleSchoolButton.Height;

            try
            {
                SerialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            serialDataQueue = new System.Collections.Concurrent.ConcurrentQueue<char>();
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
            messageTextBox.Text = "Disconnected";
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

            try
            {
                SerialPort1.Write("0");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedPort;
            
            try
            {
                isConnected = true;
                selectedPort = comPortNumberComboBox.SelectedItem.ToString();
                Console.WriteLine("Connected to " + selectedPort);
                SerialPort1 = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
                SerialPort1.Open();
                SerialPort1.Write("#STAR\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            if (isConnected) { messageTextBox.Text = "Connected"; }
            else { messageTextBox.Text = "Not Connected"; }

        }
        void getAvailableComPorts()
        {
            ports = SerialPort.GetPortNames();
        }
        private static void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
        {
            SerialPort sp = sender as SerialPort;
            int bytesAvailable = sp.BytesToRead;

            // array to store the available data    
            char[] recBuf = new char[bytesAvailable];

            try
            {
                // get the data
                sp.Read(recBuf, 0, bytesAvailable);

                // put data, char by char into a threadsafe FIFO queue
                // a better aproach maybe is putting the data in a struct and enque the struct        
                for (int index = 0; index < bytesAvailable; index++)
                   serialDataQueue.Enqueue(recBuf[index]);

            }
            catch (TimeoutException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void readSerialDataQueue()
        {
            char ch;

            try
            {
                while (serialDataQueue.TryDequeue(out ch))
                {
                    // do something with ch, add it to a textbox 
                    // for example to see that it actually works
                    messageTextBox.Text += ch;
                }

            }
            catch (Exception ex)
            {
                // handle ex here
            }
        }


        private void middleSchoolButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel1.Height = middleSchoolButton.Height;
        }

        private void highSchoolButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel1.Height = highSchoolButton.Height;
        }

        private void simulationButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel1.Height = simulationButton.Height;
           // StackPanel1.Top = simulationButton.Top;
            selectionInteractiveBrain.BringToFront();
        }

        private void interactiveBrainButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel1.Height = interactiveBrainButton.Height;
        }
        private void serialTestButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel1.Height = serialTestButton.Height;
        }
    }
}

    
