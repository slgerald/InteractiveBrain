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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.UI.Core;

namespace InteractiveBrain
{
    /// <summary>
    /// Interaction logic for interactiveBrainControl.xaml
    /// </summary>
    public partial class interactiveBrainControl : UserControl
    {
        string selectedBrainPart;
        private static interactiveBrainControl _instance;
        string displayMessage;
        string selectedSubstances;
        string selectedActivities;
        bool brainPart;
        bool activity;
        bool substance;
        DoubleAnimation animation = new DoubleAnimation();//animation used for the glowing effect
        String lastSubstanceSelected;

        SerialPort SerialPort1;
        bool isConnected = false;
        String[] ports;
        static ConcurrentQueue<char> serialDataQueue;
        string selectedPort;

        public static interactiveBrainControl Instance
        
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new interactiveBrainControl();
                }
                _instance = new interactiveBrainControl();
                return _instance;
               
            }
        }
        public interactiveBrainControl()
        {
            InitializeComponent();
            brainPart = false;
            activity = false;
            substance = false;
            isConnected = false;
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
                if (ports[0] != null)
                {
                    comPortNumberComboBox.SelectedItem = ports[0];
                }
            }
        }

        private void selectionMessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            if (brainPart)
            {
                // selectionMessageBox.Text = selectedBrainPart + " was chosen. " + displayMessage;
                selectionMessageBox.Text = selectedBrainPart + " was chosen. " ;
                activitiesListBox.SelectedItem = false;
                substancesListBox.SelectedItem = false;
                brainPart = false;
                animation.From = 1.0;
                animation.To = 0.4;
                animation.Duration = new Duration(TimeSpan.FromSeconds(.5));
                animation.AutoReverse = true;
                animation.RepeatBehavior = RepeatBehavior.Forever;
                switch (selectedBrainPart)
                {
                    case "Amygdala":
                        
                        amygdalaImage.BeginAnimation(OpacityProperty, animation);

                        break;
                    case "Pituitary Gland":

                        pituitaryGlandImage.BeginAnimation(OpacityProperty, animation);

                        break;
                    case "Temporal Lobe":

                        temporalLobeImage.BeginAnimation(OpacityProperty, animation);

                        break;
                    case "Occipital Lobe":

                        occipitalLobeImage.BeginAnimation(OpacityProperty, animation);

                        break;
                    case "Parietal Lobe":

                        parietalLobeImage.BeginAnimation(OpacityProperty, animation);

                        break;
                    case "Cerebellum":

                        cerebellumImage.BeginAnimation(OpacityProperty, animation);

                        break;
                    case "Hippocampus":

                        hippocampusImage.BeginAnimation(OpacityProperty, animation);

                        break;
                    case "Frontal Lobe":

                        frontalLobeImage.BeginAnimation(OpacityProperty, animation);

                        break;
                    case "Brainstem":

                       brainstemImage.BeginAnimation(OpacityProperty, animation);

                        break;



                }
            }
            if (substance)
            {
                activitiesListBox.SelectedItem = false;
                brainPartsListBox.SelectedItem = false;
                selectionMessageBox.Text = selectedSubstances + " was chosen. " + displayMessage;
                substance = false;
                examplesButton.Visibility = System.Windows.Visibility.Visible;
               
            }
            if(activity)
            {
                substancesListBox.SelectedItem = false;
                brainPartsListBox.SelectedItem = false;
                selectionMessageBox.Text = selectedActivities + " was chosen. " + displayMessage;
                activity = false;
            }
        }

        private void brainPartsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            brainPart = true;
            substance = false;
            activity = false;
            activitiesListBox.SelectedItem = false;
            substancesListBox.SelectedItem = false;
            //   substancesListBox.SelectedIndex = -1;
            //   activitiesListBox.SelectedIndex = -1;
            selectedBrainPart = ((ListBoxItem)brainPartsListBox.SelectedItem).Content.ToString();
            Console.WriteLine(selectedBrainPart);
            amygdalaImage.BeginAnimation(OpacityProperty, null);
            pituitaryGlandImage.BeginAnimation(OpacityProperty, null);
            hippocampusImage.BeginAnimation(OpacityProperty, null);
            brainstemImage.BeginAnimation(OpacityProperty, null);
            frontalLobeImage.BeginAnimation(OpacityProperty, null);
            temporalLobeImage.BeginAnimation(OpacityProperty, null);
            parietalLobeImage.BeginAnimation(OpacityProperty, null);
            occipitalLobeImage.BeginAnimation(OpacityProperty, null);
            cerebellumImage.BeginAnimation(OpacityProperty, null);
            examplesButton.Visibility = System.Windows.Visibility.Hidden;
        }

        private void substancesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            substance = true;
            brainPart = false;
            activity = false;
            brainPartsListBox.SelectedItem = false;
            activitiesListBox.SelectedItem = false;
            // brainPartsListBox.SelectedIndex = -1;
            // activitiesListBox.SelectedIndex = -1;
            selectedSubstances = ((ListBoxItem)substancesListBox.SelectedItem).Content.ToString();
            examplesButton.Content = "Click for Examples for " + selectedSubstances;
            examplesButton.Visibility = System.Windows.Visibility.Visible;

            amygdalaImage.BeginAnimation(OpacityProperty, null);
            pituitaryGlandImage.BeginAnimation(OpacityProperty, null);
            hippocampusImage.BeginAnimation(OpacityProperty, null);
            brainstemImage.BeginAnimation(OpacityProperty, null);
            frontalLobeImage.BeginAnimation(OpacityProperty, null);
            temporalLobeImage.BeginAnimation(OpacityProperty, null);
            parietalLobeImage.BeginAnimation(OpacityProperty, null);
            occipitalLobeImage.BeginAnimation(OpacityProperty, null);
            cerebellumImage.BeginAnimation(OpacityProperty, null);
        }

        private void activitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activity = true;
            substance = false;
            brainPart = false;
            brainPartsListBox.SelectedItem = false;
            substancesListBox.SelectedItem = false;

            // substancesListBox.SelectedIndex = -1;
            // activitiesListBox.SelectedIndex = -1;
            selectedActivities = ((ListBoxItem)activitiesListBox.SelectedItem).Content.ToString();
            examplesButton.Visibility = System.Windows.Visibility.Hidden;

            amygdalaImage.BeginAnimation(OpacityProperty, null);
            pituitaryGlandImage.BeginAnimation(OpacityProperty, null);
            hippocampusImage.BeginAnimation(OpacityProperty, null);
            brainstemImage.BeginAnimation(OpacityProperty, null);
            frontalLobeImage.BeginAnimation(OpacityProperty, null);
            temporalLobeImage.BeginAnimation(OpacityProperty, null);
            parietalLobeImage.BeginAnimation(OpacityProperty, null);
            occipitalLobeImage.BeginAnimation(OpacityProperty, null);
            cerebellumImage.BeginAnimation(OpacityProperty, null);
        }

        private void Amygdala_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Amygdala light up";
          //  if (SerialPort1.IsOpen)
         //   {
                //  SerialPort1.Write("0");
           // }
           // else
           // {
               // selectionMessageBox.Text = "Check the connection, then redo the selection";
           // }
            }
        private void pituitaryGland_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Pituitary Gland light up";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }
        private void hippocampus_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Hippocampus light up";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // };
        }
        private void cerebellum_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Cerebellum light up";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }
        private void parietalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Parietal Lobe light up";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }
        private void temporalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Temporal Lobe light up";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }
        private void frontalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Frontal Lobe light up";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }
        private void occipitalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Occipital Lobe light up";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }
        private void brainstem_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Brainstem light up";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }

        private void stimulants_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            lastSubstanceSelected = "stimulants";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }
        private void depressants_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            lastSubstanceSelected = "depressants";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }
        private void hallucinogens_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            lastSubstanceSelected = "hallucinogens";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }
        private void opioids_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            lastSubstanceSelected = "opioids";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }

        private void dietAndNutrition_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }

        private void healthAndExercise_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }

        private void cognitiveActivity_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }

        private void socialEngagement_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            //  if (SerialPort1.IsOpen)
            //   {
            //  SerialPort1.Write("0");
            // }
            // else
            // {
            // selectionMessageBox.Text = "Check the connection, then redo the selection";
            // }
        }

        private  void examplesButton_Click(object sender, RoutedEventArgs e)
        {  // open the Popup if it isn't open already 
         //   if (!examplesPopup.IsOpen) { examplesPopup.IsOpen = true; }
            examplesButton.Visibility = System.Windows.Visibility.Visible;
            if (lastSubstanceSelected == "stimulants")
            {
                
               
             //   titleTextBlock.Text = "Examples of Stimulants:";

                MessageBox.Show("Examples of Stimulants:\r\n\r\n\u2022 Dexedrine® \r\n\u2022 Adderall® \r\n\u2022 Ritalin® \r\n\u2022 Concerta®\r\n\r\n(www.drugabuse.gov) ");
              //  listedExamples.Text = "Dexedrine® ,\r\nAdderall®, \r\nRitalin®, \r\nConcerta® \r\n(www.drugabuse.gov) ";
              //  Console.WriteLine(listedExamples.Text);
                
            }
            if (lastSubstanceSelected == "depressants")
            {

                examplesButton.Content = "Click for examples for Depressants";
               
                MessageBox.Show("Examples of Depressants: \r\n\r\n\u2022 Valium® \r\n\u2022 Xanax® \r\n\u2022 Halcion® \r\n\u2022 Ambien® \r\n\u2022 Lunesta® \r\n\u2022 Alcohol \r\n\r\n(www.drugabuse.gov)");
            }
            if (lastSubstanceSelected == "hallucinogens")
            {
                examplesButton.Content = "Click for examples for Hallucinogens";
                MessageBox.Show("Examples of Hallucinogens: \r\n\r\n\u2022 LSD \r\n\u2022 Phencyclidine(PCP), \r\n\u2022 Psilocybin(e.g. shrooms) \r\n\r\n(www.drugabuse.gov) ");
            }
            if(lastSubstanceSelected == "opioids")
            {
                examplesButton.Content = "Click for examples for Opioids";
                MessageBox.Show("Examples of Opioids: \r\n\r\n\u2022 Hydrocone(e.g. Vicodin®) \r\n\u2022 Oxycodone (e.g., OxyContin®, Percocet®) \r\n\u2022 Oxymorphone (e.g., Opana®) \r\n\u2022 Morphine (e.g., Kadian®, Avinza®)\r\n\u2022 Codeine, Fentanyl, and others\r\n\r\n(www.drugabuse.gov)");
            }
        }
        // Handles the Click event on the Button inside the Popup control and 
        // closes the Popup. 
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (examplesPopup.IsOpen) { examplesPopup.IsOpen = false; }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        { 

            ToggleButton toggle = (ToggleButton)sender;
            
                if (ports == null || ports.Length == 0)
                {

                


                selectionMessageBox.Text = "Not able to connect, check connection, then try again. ";
                   toggleButton.IsChecked = false;
                
                }
                else
                {

                // open the Popup if it isn't open already 
                if (!examplesPopup.IsOpen) { examplesPopup.IsOpen = true; }

                try
                {
                        isConnected = true;
                         selectedPort = comPortNumberComboBox.SelectedItem.ToString();
                        //selectedPort = ports[0];
                        Console.WriteLine("Connected to " + selectedPort);
                        SerialPort1 = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
                        SerialPort1.Open();
                        SerialPort1.Write("#STAR\n");
                       // MessageBox.Show("Connected to " + selectedPort);
                       
                    
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
        
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(isConnected);
                ToggleButton toggle = (ToggleButton)sender;
           
                Console.WriteLine("This is the disconnect button");
                if (isConnected)
                {
                MessageBox.Show("Disconnected");
                    try
                    {
                        isConnected = false;
                        SerialPort1.Close();
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                }
                else {
                Console.WriteLine(ports.Length);
                if (ports == null || ports.Length == 0)
                {
                    // selectionMessageBox.Text = "Not able to connect, check connection, then try again. ";
                   // MessageBox.Show("Not connected yet");
                }
               // else { 
               // selectionMessageBox.Text = "Not connected yet";
               // }
            }
           
        }
            void getAvailableComPorts()        {

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
                    selectionMessageBox.Text += ch;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
