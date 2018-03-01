using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for serialCommsControl.xaml
    /// </summary>
    public partial class serialCommsControl : UserControl
    {
        SerialPort SerialPort1;
        bool isConnected = false;
        String[] ports;
        static ConcurrentQueue<char> serialDataQueue;
        private static serialCommsControl _instance;
        string selectedPort;
        string append;
        string id = "9999";
        public static serialCommsControl Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new serialCommsControl();
    }
                    return _instance;
               
            }
        }
        public serialCommsControl()
        {
            InitializeComponent();
            getAvailableComPorts();
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
            }
        }
        private void onButton_Click(object sender, RoutedEventArgs e)
        {
            int n = 0;
            
            if (isConnected)
            {
                
                try
                {
                    SerialPort1.Open();
                    Console.WriteLine("Serial Port just opened");
                    n = 0;
                    append = null;
                    while (n < 10)
                    {
                        int num = 1 * n;
                        string numV2 = num.ToString();
                        append += numV2;
                        SerialPort1.WriteLine(num.ToString()); //null after eeach number
                        //SerialPort1.WriteLine(id+append);
                        messageTextBox.Text = num.ToString() + "\n";
                        //Console.WriteLine(append); //WriteLine includes null at the end
                       Console.WriteLine(num.ToString());
                        //Thread.Sleep(20);//needed so not writing to port too quickly
                        Thread.Sleep(1);
                        n++;
                    }

                   SerialPort1.Close();
                    //Thread CloseDown = new Thread(new ThreadStart(CloseSerialOnExit)); //close port in new thread to avoid hang

                   // CloseDown.Start(); //close port in new thread to avoid hang
                    Console.WriteLine("Serial Port just closed ");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
               
            }
        }

private void CloseSerialOnExit(){

    try

    {

        SerialPort1.Close(); //close the serial port

    }
    catch (Exception ex)
    {

        Console.WriteLine(ex.Message); //catch any serial port closing error messages

    }
}

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            

           if (ports == null || ports.Length == 0)
         {
             messageTextBox.Text = "Could not find any available Ports";
                
           }
            else
            {
                try
                {
                    isConnected = true;
                    selectedPort = comPortNumberComboBox.SelectedItem.ToString();
                    Console.WriteLine("Connected to " + selectedPort);
                    SerialPort1 = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);                 
                   // SerialPort1.Write("#STAR\n");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            if (isConnected) { messageTextBox.Text = "Connected to " + selectedPort ; }
           // else { messageTextBox.Text = "Not Connected"; }
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

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("This is the disconnect button");
            if (isConnected)
            {
                messageTextBox.Text = "Disconnected";
                try
                {
                    isConnected = false;
                    SerialPort1.Close();
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

            }
            else { messageTextBox.Text = "Was not connected yet"; }
           
        }

        private void messageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private void comPortNumberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void getAvailableComPorts()
        {
            
            ports = SerialPort.GetPortNames();
            
        }
            

        
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
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
                Console.WriteLine(ex.Message);
            }
        }
    }
}
