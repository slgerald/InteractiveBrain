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
        bool editSubstancesFlag;
        bool editHealthyBehaviorsFlag;
        ListBoxItem newListBoxItem = new ListBoxItem();
        string newListBoxContentText;

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
            editHealthyBehaviorsFlag = false;
            editSubstancesFlag = false;
            GetAvailableComPorts();
            try
            {
                SerialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            serialDataQueue = new System.Collections.Concurrent.ConcurrentQueue<char>();
            foreach (string port in ports) //used to list available Port Names
            {
                comPortNumberComboBox.Items.Add(port);
                Console.WriteLine(port);
                if (ports[0] != null)
                {
                    comPortNumberComboBox.SelectedItem = ports[0];
                }
            }
        }

        private void SelectionMessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //This function determines what happens when the Go Button is Clicked
        //Depending on the selection, make affected areas glow 
        private void GoButton_Click(object sender, RoutedEventArgs e)
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
        //The following code determines what happens when the selection between the three
        //lists change
        #region
        //This function determines what happens when the selection of the brain Part list changes 
        //Set brain part flag true and other flags false 
        //Make the Examples Button Unavailable 
        //When selection changes stop parts glowing based on the previous selection

        private void BrainPartsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            brainPart = true;
            substance = false;
            activity = false;
            activitiesListBox.SelectedItem = false;
            substancesListBox.SelectedItem = false;
            selectedBrainPart = ((ListBoxItem)brainPartsListBox.SelectedItem).Content.ToString();

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

        //This function determines what happens when the selection of the brain Part list changes 
        //Set substance flag true and other flags false 
        //Make the Examples Button available with appropriate examples
        //When selection changes stop parts glowing based on the previous selection
        private void SubstancesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            substance = true;
            brainPart = false;
            activity = false;
            brainPartsListBox.SelectedItem = false;
            activitiesListBox.SelectedItem = false;
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

        //This function determines what happens when the selection of the brain Part list changes 
        //Set activities flag true and other flags false 
        //Make the Examples Button Unavailable
        //When selection changes stop parts glowing based on the previous selection

        private void ActivitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activity = true;
            substance = false;
            brainPart = false;
            brainPartsListBox.SelectedItem = false;
            substancesListBox.SelectedItem = false;
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
        #endregion
        //The following code determines what is displayed in the selectionMessageBox
        //based on the selection
        #region
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
        private void PituitaryGland_Selected(object sender, RoutedEventArgs e)
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
        private void Hippocampus_Selected(object sender, RoutedEventArgs e)
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
        private void Cerebellum_Selected(object sender, RoutedEventArgs e)
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
        private void ParietalLobe_Selected(object sender, RoutedEventArgs e)
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
        private void TemporalLobe_Selected(object sender, RoutedEventArgs e)
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
        private void FrontalLobe_Selected(object sender, RoutedEventArgs e)
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
        private void OccipitalLobe_Selected(object sender, RoutedEventArgs e)
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
        private void Brainstem_Selected(object sender, RoutedEventArgs e)
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

        private void Stimulants_Selected(object sender, RoutedEventArgs e)
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
        private void Depressants_Selected(object sender, RoutedEventArgs e)
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
        private void Hallucinogens_Selected(object sender, RoutedEventArgs e)
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
        private void Opioids_Selected(object sender, RoutedEventArgs e)
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

        private void DietAndNutrition_Selected(object sender, RoutedEventArgs e)
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

        private void HealthAndExercise_Selected(object sender, RoutedEventArgs e)
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

        private void CognitiveActivity_Selected(object sender, RoutedEventArgs e)
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

        private void SocialEngagement_Selected(object sender, RoutedEventArgs e)
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
        #endregion

        //This function determines which substances are listed based on the substance selected
        private  void ExamplesButton_Click(object sender, RoutedEventArgs e)
        {  // open the Popup if it isn't open already 
           // if (!examplesPopup.IsOpen) { examplesPopup.IsOpen = true; }
            examplesButton.Visibility = System.Windows.Visibility.Visible;
            if (lastSubstanceSelected == "stimulants")
            {       
              MessageBox.Show("Examples of Stimulants:\r\n\r\n\u2022 Dexedrine® \r\n\u2022 Adderall® \r\n\u2022 Ritalin® \r\n\u2022 Concerta®\r\n\r\n(www.drugabuse.gov) ");
                //   titleTextBlock.Text = "Examples of Stimulants:";
                //   listedExamples.Text = "Dexedrine® ,\r\nAdderall®, \r\nRitalin®, \r\nConcerta® \r\n(www.drugabuse.gov) ";                
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
        
        //The connection button is a toggle button
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = (ToggleButton)sender;
                
                //If there aren't any serial ports to connect to
                //Display message not able to connect and the togglebutton doesn't
                //become checked
                if (ports == null || ports.Length == 0)
                {
                    selectionMessageBox.Text = "Not able to connect, check connection, then try again. ";
                    toggleButton.IsChecked = false;
                }
                else
                {

                    try //Try to connect to serial port
                    {
                        isConnected = true;
                        selectedPort = comPortNumberComboBox.SelectedItem.ToString();
                        //selectedPort = ports[0];
                        Console.WriteLine("Connected to " + selectedPort);
                        SerialPort1 = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
                        SerialPort1.Open();
                        SerialPort1.Write("#STAR\n");
                        // MessageBox.Show("Connected to " + selectedPort)

                        // open the Popup if it isn't open already 
                        if (!connectionPopup.IsOpen)
                        { connectionPopup.IsOpen = true; }

                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
        
        }


        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
                Console.WriteLine(isConnected);
                ToggleButton toggle = (ToggleButton)sender;  
                //If there was an established serial port connection, disconnect
                if (isConnected)
                {
                    //MessageBox.Show("Disconnected");
                    try
                    {
                        isConnected = false;
                        SerialPort1.Close();
                        ImageBrush brush1 = new ImageBrush(new BitmapImage(new Uri("Resources/if_connect_no.ico")));
                       
                        toggleButton.Background = brush1;
                    }

                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                }

                //If there wasn't a successful connection
                else {
                    Console.WriteLine(ports.Length);
                    if (ports == null || ports.Length == 0)
                    {
                        selectionMessageBox.Text = "Not able to connect, check connection, then try again. ";
                       // MessageBox.Show("Not connected yet");
                    }
                    else { 
                        selectionMessageBox.Text = "Not connected yet";
                    }
                }
           
        }

        //This function gets the available serial ports 
        void GetAvailableComPorts()
        {

            ports = SerialPort.GetPortNames();

        }

        //This function handles if data is received through the serial port 
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

        //This function handles what to do with received data
        private void ReadSerialDataQueue()
        {
        

            try
            {
                while (serialDataQueue.TryDequeue(out char ch))
                {
                    // do something with ch, add it to a textbox 
                    // for example to see that it actually works
                   // selectionMessageBox.Text += ch;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        // This function closes the pop up and changes the background of the connection
        //button to show that a connection has been established 
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 

            if (connectionPopup.IsOpen) { connectionPopup.IsOpen = false; }
            ImageBrush brush1 = new ImageBrush(new BitmapImage(new Uri("Resources/if_connect_established.ico")));
         
            toggleButton.Background = brush1;
        }

        private void ClosePopupClicked2(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 

            if (editListsPopup.IsOpen) { editListsPopup.IsOpen = false; }
            editHealthyBehaviorsFlag = false;
            editSubstancesFlag = false;

        }

        private void ClosePopupClicked3(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 

            if (contentPopup.IsOpen) { contentPopup.IsOpen = false; }


        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!editListsPopup.IsOpen)
            { editListsPopup.IsOpen = true; }
           
        }

        private void SubstancesRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            titleTextBlock.Text = "";
            editHealthyBehaviorsFlag = false;
            editSubstancesFlag = true;
            currentListBox.Items.Clear();
            for (int i = 0; i < substancesListBox.Items.Count; i++)
            {
                ListBoxItem li = new ListBoxItem();
                string item = ((ListBoxItem)substancesListBox.Items[i]).Content.ToString();
                Console.WriteLine(item);
                li.Content = item;
                currentListBox.Items.Add(li);

            }

        }

        private void HealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            titleTextBlock.Text = "";
            editHealthyBehaviorsFlag = true;
            editSubstancesFlag = false;
            currentListBox.Items.Clear();
            for (int i = 0; i < activitiesListBox.Items.Count; i++)
            {
                ListBoxItem li = new ListBoxItem();
                string item = ((ListBoxItem)activitiesListBox.Items[i]).Content.ToString();
                Console.WriteLine(item);
                li.Content = item;
                currentListBox.Items.Add(li);

            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (editHealthyBehaviorsFlag || editSubstancesFlag)
            {
                if (!contentPopup.IsOpen)
                {
                    contentPopup.IsOpen = true;
                }
            }
            else
            {
                titleTextBlock.Text = "Select Substances or Healthy Behaviors";
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int lastIndex;
            if (editHealthyBehaviorsFlag || editSubstancesFlag)
            {
                if (editHealthyBehaviorsFlag)
                {
                    lastIndex = activitiesListBox.SelectedIndex;
                    currentListBox.Items.Remove(lastIndex);
                    // activitiesListBox.Items.Remove(lastIndex);
                }
                if (editSubstancesFlag)
                {
                    lastIndex = substancesListBox.SelectedIndex;
                    currentListBox.Items.Remove(lastIndex);
                    // activitiesListBox.Items.Remove(lastIndex);
                }
            }

        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        { int index;
            bool effectsChecked = false;

            if (editHealthyBehaviorsFlag)
            {
               // ListBoxItem newListBoxItem = new ListBoxItem();
                index = activitiesListBox.Items.Count + 1;

                if (editAmygdalaCheckbox.IsChecked.Value == true)
                {
                    effectsChecked = true;
                    Console.WriteLine("In Amgydala edit checkbox");
                }
                else if (editCerebellumCheckbox.IsChecked.HasValue && editCerebellumCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editTemporalLobeCheckbox.IsChecked.HasValue && editTemporalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editPituitaryGlandCheckbox.IsChecked.HasValue && editPituitaryGlandCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editFrontalLobeCheckbox.IsChecked.HasValue && editFrontalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editParietalLobeCheckbox.IsChecked.HasValue && editParietalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editOccipitalLobeCheckbox.IsChecked.HasValue && editOccipitalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
               else  if (editBrainstemCheckbox.IsChecked.HasValue && editBrainstemCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editHippocampusCheckbox.IsChecked.HasValue && editHippocampusCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
               
                
            }
            if (editSubstancesFlag)
            {
                ListBoxItem newListBoxItem = new ListBoxItem();
                index = substancesListBox.Items.Count + 1;
                if ( editAmygdalaCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    Console.WriteLine("In Amgydala edit checkbox");
                }
               else  if (editCerebellumCheckbox.IsChecked.HasValue && editCerebellumCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editTemporalLobeCheckbox.IsChecked.HasValue && editTemporalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
               else  if (editPituitaryGlandCheckbox.IsChecked.HasValue && editPituitaryGlandCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editFrontalLobeCheckbox.IsChecked.HasValue && editFrontalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editParietalLobeCheckbox.IsChecked.HasValue && editParietalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editOccipitalLobeCheckbox.IsChecked.HasValue && editOccipitalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
               else  if (editBrainstemCheckbox.IsChecked.HasValue && editBrainstemCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                else if (editHippocampusCheckbox.IsChecked.HasValue && editHippocampusCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                }
                
               
            }
                if (!effectsChecked && newListBoxContentText == null)
                {
                    errorTextBlock.Text = "No content was inserted or effects are checked.";
                Console.WriteLine(effectsChecked);
                Console.WriteLine(editAmygdalaCheckbox.IsChecked.Value);
                }
                else if (!effectsChecked && newListBoxContentText != null)
                {
                    errorTextBlock.Text = "No effects are checked.";
                Console.WriteLine(effectsChecked);
            }
                else if (effectsChecked && newListBoxContentText == null)
                {
                    errorTextBlock.Text = "No content was inserted.";
                Console.WriteLine(effectsChecked);
            }
                else if (effectsChecked && newListBoxContentText != null)
                {
                    newListBoxItem.Content = newListBoxContentText;
                    currentListBox.Items.Add(newListBoxItem);
                    errorTextBlock.Text = "Saved.";
                Console.WriteLine(effectsChecked);
            }
        }

        private void ListBoxContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            newListBoxContentText = listBoxContent.Text;
        }


       
    }
}
