using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
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
        //The following variable are for the userControl interaction
        string selectedBrainPart;//string to determine which brain part was selected
        private static interactiveBrainControl _instance; //used to instantiate new instance of interactiveBrainControl
        string displayMessage;//What should be displayed above both brain maps
        string selectedSubstances;//string to determine which substance was selected 
        string selectedHealthyBehaviors;//string to determine which healthy behavior was selected 
        bool brainPart = false; //was a brain part selected?
        bool healthybehaviorBOOL = false; //was a healthy behavior selected?
        bool substance = false;//was a substance selected?
        DoubleAnimation animation = new DoubleAnimation();//animation used for the glowing effect
        String lastSubstanceSelected; //used to determine which substance to displayed when examplesButton is pressed

        //THe following variables are for serial communication
        SerialPort SerialPort1; //SerialPort for serial communication with brain
       // bool isConnected = false;//using SerialPort1.IsOpen instead
        String[] ports; //Array of strings that list the available com ports 
        static ConcurrentQueue<char> serialDataQueue; // for serial communication
        string selectedPort;//for serial communication, which port was chosen

        //The following variables are for the editing function of the interactiveBrainControl
        bool editSubstancesFlag; //for editPopup, load substances from database
        bool editHealthyBehaviorsFlag;//for editPopup, load healthy behaviors from database
        ListBoxItem newListBoxItem = new ListBoxItem();
        string newListBoxItemContent;
        //The following array will help determine which parts should illuminate on the app and interactive brain
        //based on the selection of substances or healthy behaviors 

       // public static IList<Substance> substances = new List<Substance>();
        public static IList<HealthyBehavior> healthyBehaviors = new List<HealthyBehavior>();
        char[] lightingSequenceToDatabase = {'0','0','0','0','0','0','0','0','0'}; //Writing to the database
        char[] lightingSequenceFromDatabase = { '0', '0', '0', '0', '0', '0', '0', '0', '0' };//Reading from the 
        string lightingSequenceString;
        
        //THis function instantiates a new insteractiveBrainControl when called
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

        //This function determines how the interactiveBrainCintrol is initialized
        public interactiveBrainControl()
        {
            InitializeComponent();
            brainPart = false;
            healthybehaviorBOOL = false;
            substance = false;
           // isConnected = false;
            editHealthyBehaviorsFlag = false;
            editSubstancesFlag = false;
            GetAvailableComPorts();

            var getData = FetchData(); //getting data for listboxes(substancesListBox and healthyBehaviorsListBox

            //Preparing for possible serial connection
            try
            {
                SerialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            serialDataQueue = new System.Collections.Concurrent.ConcurrentQueue<char>();
            foreach (string port in ports) //used to list available COM Port Names
            {
                comPortNumberComboBox.Items.Add(port);
                Console.WriteLine(port);
                if (ports[0] != null)
                {
                    comPortNumberComboBox.SelectedItem = ports[0];
                }
            }
        }

      

        //This function determines what happens when the Go Button is Clicked
        //Depending on the selection, make affected areas glow 
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            if (brainPart)
            {
                // selectionMessageBox.Text = selectedBrainPart + " was chosen. " + displayMessage;
                selectionMessageBox.Text = selectedBrainPart + " was chosen. " ;
                healthyBehaviorsListBox.SelectedItem = false;
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
                //Write to serial port the individual part that should lit
                //if(SerialPort.IsOpen)
                //{
                //  SerialPort
                //}
            }
            if (substance)
            {
                healthyBehaviorsListBox.SelectedItem = false;
                brainPartsListBox.SelectedItem = false;
                selectionMessageBox.Text = selectedSubstances + " was chosen. " + displayMessage;
                //Call the function to determine which parts to illuminate
                substance = false;
                
                examplesButton.Visibility = System.Windows.Visibility.Visible;
               
            }
            if(healthybehaviorBOOL)
            {
                substancesListBox.SelectedItem = false;
                brainPartsListBox.SelectedItem = false;
                selectionMessageBox.Text = selectedHealthyBehaviors + " was chosen. " + displayMessage;
                //Call the function to determine which parts to illuminate
                healthybehaviorBOOL = false;
                //Work on this first with database 
            }
        }
        //The following code determines what happens when the selection between the three
        //lists change
        #region
        //This function determines what happens when the selection of the brain Part list changes 
        //Set brain part flag true and other flags false 
        //Make the Examples Button Unavailable 
        //When selection changes stop parts glowing based on the previous selection

        private void ListBoxSelectionChanged()
        {

            amygdalaImage.BeginAnimation(OpacityProperty, null);
            pituitaryGlandImage.BeginAnimation(OpacityProperty, null);
            hippocampusImage.BeginAnimation(OpacityProperty, null);
            brainstemImage.BeginAnimation(OpacityProperty, null);
            frontalLobeImage.BeginAnimation(OpacityProperty, null);
            temporalLobeImage.BeginAnimation(OpacityProperty, null);
            parietalLobeImage.BeginAnimation(OpacityProperty, null);
            occipitalLobeImage.BeginAnimation(OpacityProperty, null);
            cerebellumImage.BeginAnimation(OpacityProperty, null);

            if (SerialPort1.IsOpen)
            {
                //send serial message for stop
            }
        }


        private void BrainPartsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            brainPart = true;
            substance = false;
            healthybehaviorBOOL = false;
            healthyBehaviorsListBox.SelectedItem = false;
            substancesListBox.SelectedItem = false;
            selectedBrainPart = ((ListBoxItem)brainPartsListBox.SelectedItem).Content.ToString();

            examplesButton.Visibility = System.Windows.Visibility.Hidden;

            ListBoxSelectionChanged();
            
        }

        
        //This function determines what happens when the selection of the brain Part list changes 
        //Set substance flag true and other flags false 
        //Make the Examples Button available with appropriate examples
        //When selection changes stop parts glowing based on the previous selection
        private void SubstancesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            substance = true;
            brainPart = false;
            healthybehaviorBOOL = false;
            brainPartsListBox.SelectedItem = false;
            healthyBehaviorsListBox.SelectedItem = false;
            selectedSubstances = ((ListBoxItem)substancesListBox.SelectedItem).Content.ToString();

            examplesButton.Content = "Click for Examples for " + selectedSubstances;
            examplesButton.Visibility = System.Windows.Visibility.Visible;

            ListBoxSelectionChanged();
        }

        //This function determines what happens when the selection of the brain Part list changes 
        //Set activities flag true and other flags false 
        //Make the Examples Button Unavailable
        //When selection changes stop parts glowing based on the previous selection

        private void HealthyBehaviorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            healthybehaviorBOOL = true;
            substance = false;
            brainPart = false;
            brainPartsListBox.SelectedItem = false;
            substancesListBox.SelectedItem = false;
            selectedHealthyBehaviors = ((ListBoxItem)healthyBehaviorsListBox.SelectedItem).Content.ToString();

            examplesButton.Visibility = System.Windows.Visibility.Hidden;

            ListBoxSelectionChanged();
        }
        #endregion
        //The following code determines what is displayed in the selectionMessageBox
        //based on the selection
        #region
        private void Cerebellum_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Cerebellum light up";
            // if (SerialPort1.IsOpen)
            // {   
            //     //lightingSequenceFromDatabase = { '1', '0', '0', '0', '0', '0', '0', '0', '0' };
            //     //WriteToSerialPortFunction
            // }
            //else{
            //
            // }
        }
        private void Brainstem_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Brainstem light up";
            // if (SerialPort1.IsOpen)
            // {   
            //     //lightingSequenceFromDatabase = { '0', '1', '0', '0', '0', '0', '0', '0', '0' };
            //     //WriteToSerialPortFunction
            // }
            //else{
            //
            // }
        }
        private void PituitaryGland_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Pituitary Gland light up";
            // if (SerialPort1.IsOpen)
            // {   
            //     //lightingSequenceFromDatabase = { '0', '0', '1', '0', '0', '0', '0', '0', '0' };
            //     //WriteToSerialPortFunction
            // }
            //else{
            //
            // }
        }
        private void Amygdala_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Amygdala light up"; 
           // if (SerialPort1.IsOpen)
           // {   
           //     //lightingSequenceFromDatabase = { '0', '0', '0', '1', '0', '0', '0', '0', '0' };
           //     //WriteToSerialPortFunction
           // }
           //else{
           //
           // }
        }
        
        private void Hippocampus_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Hippocampus light up";
            // if (SerialPort1.IsOpen)
            // {   
            //     //lightingSequenceFromDatabase = { '0', '0', '0', '0', '1', '0', '0', '0', '0' };
            //     //WriteToSerialPortFunction
            // }
            //else{
            //
            // }
        }
        private void TemporalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Temporal Lobe light up";
            // if (SerialPort1.IsOpen)
            // {   
            //     //lightingSequenceFromDatabase = { '0', '0', '0', '0', '0', '1', '0', '0', '0' };
            //     //WriteToSerialPortFunction
            // }
            //else{
            //
            // }
        }
        private void ParietalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Parietal Lobe light up";
            // if (SerialPort1.IsOpen)
            // {   
            //     //lightingSequenceFromDatabase = { '0', '0', '0', '0', '0', '0', '0', '1', '0' };
            //     //WriteToSerialPortFunction
            // }
            //else{
            //
            // }
        }
        private void OccipitalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Occipital Lobe light up";
            // if (SerialPort1.IsOpen)
            // {   
            //     //lightingSequenceFromDatabase = { '0', '0', '0', '0', '0', '0', '1', '0', '0' };
            //     //WriteToSerialPortFunction
            // }
            //else{
            //
            // }
        }

        private void FrontalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Frontal Lobe light up";
            // if (SerialPort1.IsOpen)
            // {   
            //     //lightingSequenceFromDatabase = { '0', '0', '0', '0', '0', '0', '0', '0', '1' };
            //     //WriteToSerialPortFunction
            // }
            //else{
            //
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

        //private void dietandnutrition_selected(object sender, routedeventargs e)
        //{
        //    displaymessage = "this selection will be programmed next semester";
        //    //  if (serialport1.isopen)
        //    //   {
        //    //  serialport1.write("0");
        //    // }
        //    // else
        //    // {
        //    // selectionmessagebox.text = "check the connection, then redo the selection";
        //    // }
        //}

        //private void healthandexercise_selected(object sender, routedeventargs e)
        //{
        //    displaymessage = "this selection will be programmed next semester";
        //    //  if (serialport1.isopen)
        //    //   {
        //    //  serialport1.write("0");
        //    // }
        //    // else
        //    // {
        //    // selectionmessagebox.text = "check the connection, then redo the selection";
        //    // }
        //}

        //private void cognitiveactivity_selected(object sender, routedeventargs e)
        //{
        //    displaymessage = "this selection will be programmed next semester";
        //    //  if (serialport1.isopen)
        //    //   {
        //    //  serialport1.write("0");
        //    // }
        //    // else
        //    // {
        //    // selectionmessagebox.text = "check the connection, then redo the selection";
        //    // }
        //}

        //private void socialengagement_selected(object sender, routedeventargs e)
        //{
        //    displaymessage = "this selection will be programmed next semester";
        //    //  if (serialport1.isopen)
        //    //   {
        //    //  serialport1.write("0");
        //    // }
        //    // else
        //    // {
        //    // selectionmessagebox.text = "check the connection, then redo the selection";
        //    // }
        //}
        #endregion


        //This functions deterine which parts of the brain on the app side to illuminate based on
        //selection of substances and healthy behaviors
        private void LightingSequence()
        { 
            //lightingSequenceString from database and copy it lightingSequenceFromDatabase


            if (lightingSequenceFromDatabase[0] == '1')
            {

                cerebellumImage.BeginAnimation(OpacityProperty, animation);

            }
            if (lightingSequenceFromDatabase[1] == '1')
            {

                brainstemImage.BeginAnimation(OpacityProperty, animation);

            }
            if (lightingSequenceFromDatabase[2] == '1')
            {

                pituitaryGlandImage.BeginAnimation(OpacityProperty, animation);

            }
            if (lightingSequenceFromDatabase[3] == '1')
            {
                amygdalaImage.BeginAnimation(OpacityProperty, animation);

            }
            if (lightingSequenceFromDatabase[4] == '1')
            {
                hippocampusImage.BeginAnimation(OpacityProperty, animation);
            }
             if (lightingSequenceFromDatabase[5] == '1')
            {

                temporalLobeImage.BeginAnimation(OpacityProperty, animation);

            }
             if (lightingSequenceFromDatabase[6] == '1')
            {
                occipitalLobeImage.BeginAnimation(OpacityProperty, animation);
            }
            if (lightingSequenceFromDatabase[7] == '1')
            {

                parietalLobeImage.BeginAnimation(OpacityProperty, animation);
            }
            if (lightingSequenceFromDatabase[8] == '1')
            {
                frontalLobeImage.BeginAnimation(OpacityProperty, animation);
            } 
            if (SerialPort1.IsOpen)
            {
                //Pass the array of the message to the function writing to the serial port
            }
        }

        //This function determines which substances are listed based on the substance selected
        private  void ExamplesButton_Click(object sender, RoutedEventArgs e)
        {  // open the Popup if it isn't open already 
            if (!examplesPopup.IsOpen) { examplesPopup.IsOpen = true; }
            examplesButton.Visibility = System.Windows.Visibility.Visible;
            if (lastSubstanceSelected == "stimulants")
            {       
              listedExamples.Text = "Examples of Stimulants:\r\n\r\n\u2022 Dexedrine® \r\n\u2022 Adderall® \r\n\u2022 Ritalin® \r\n\u2022 Concerta®\r\n\r\n(www.drugabuse.gov) ";
                //   titleTextBlock.Text = "Examples of Stimulants:";
                //   listedExamples.Text = "Dexedrine® ,\r\nAdderall®, \r\nRitalin®, \r\nConcerta® \r\n(www.drugabuse.gov) ";                
            }
            if (lastSubstanceSelected == "depressants")
            {
                examplesButton.Content = "Click for examples of Depressants"; 
                listedExamples.Text="Examples of Depressants: \r\n\r\n\u2022 Valium® \r\n\u2022 Xanax® \r\n\u2022 Halcion® \r\n\u2022 Ambien® \r\n\u2022 Lunesta® \r\n\u2022 Alcohol \r\n\r\n(www.drugabuse.gov)";
            }
            if (lastSubstanceSelected == "hallucinogens")
            {
                examplesButton.Content = "Click for examples of Hallucinogens";
                listedExamples.Text= "Examples of Hallucinogens: \r\n\r\n\u2022 LSD \r\n\u2022 Phencyclidine(PCP), \r\n\u2022 Psilocybin(e.g. shrooms) \r\n\r\n(www.drugabuse.gov) ";
            }
            if(lastSubstanceSelected == "opioids")
            {
                examplesButton.Content = "Click for examples of Opioids";
                listedExamples.Text="Examples of Opioids: \r\n\r\n\u2022 Hydrocone(e.g. Vicodin®) \r\n\u2022 Oxycodone (e.g., OxyContin®, Percocet®) \r\n\u2022 Oxymorphone (e.g., Opana®) \r\n\u2022 Morphine (e.g., Kadian®, Avinza®)\r\n\u2022 Codeine, Fentanyl, and others\r\n\r\n(www.drugabuse.gov)";
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
                     //   isConnected = true;
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
                Console.WriteLine(SerialPort1.IsOpen);
                ToggleButton toggle = (ToggleButton)sender;  
                //If there was an established serial port connection, disconnect
                if (SerialPort1.IsOpen)
                {
                                     //MessageBox.Show("Disconnected");
   try
                    {
                       //SerialPort1.Close();
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
        private void WriteLightSequenceMessage(char[] whichBrainPartsToIlluminate)
        {
            if (SerialPort1.IsOpen)
            {
                //Add raspberrypi identifier(character array)
                //add other parts of serial message protocol( character array)
                //concatenate all the parts in a character array
                //Send serial message for stop to turn off all the lights
                //should i wait a few milliseconds?
                SerialPort1.Write(new string(lightingSequenceFromDatabase));
                //character array as string
                // Convert string to char array, because zeros may be read as nulls
                //Sample code for character to array
                // string sentence = "Mahesh Chand";
                // char[] charArr = sentence.ToCharArray();
                // foreach (char ch in charArr)
                // {
                //
                //     Console.WriteLine(ch);
                // }
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
        private void ClosePopupClicked4(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 

            if (examplesPopup.IsOpen) { examplesPopup.IsOpen = false; }


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
            editingListBox.Items.Clear();
            for (int i = 0; i < substancesListBox.Items.Count; i++)
            {
                ListBoxItem li = new ListBoxItem();
                string item = ((ListBoxItem)substancesListBox.Items[i]).Content.ToString();
                Console.WriteLine(item);
                li.Content = item;
                editingListBox.Items.Add(li);

            }

        }

        private void HealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            titleTextBlock.Text = "";
            editHealthyBehaviorsFlag = true;
            editSubstancesFlag = false;
            editingListBox.Items.Clear();
            for (int i = 0; i < healthyBehaviorsListBox.Items.Count; i++)
            {
                ListBoxItem li = new ListBoxItem();
                string item = ((ListBoxItem)healthyBehaviorsListBox.Items[i]).Content.ToString();
                Console.WriteLine(item);
                li.Content = item;
                editingListBox.Items.Add(li);

            }
            //Where to populate data for editingListBox
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
                    
                    
                    if (editingListBox.SelectedItem == null)
                    {
                        titleTextBlock.Text = "First select an item in the list";
                    }
                    else
                    {
                        //  lastIndex = healthyBehaviorsListBox.SelectedIndex;
                        lastIndex = editingListBox.SelectedIndex;
                        Console.WriteLine(lastIndex);
                        editingListBox.Items.RemoveAt(lastIndex);

                    }
                    // healthyBehaviorsListBox.Items.Remove(lastIndex);
                }
                if (editSubstancesFlag)
                {
                    
                    if (editingListBox.SelectedItem == null)
                    {
                        titleTextBlock.Text = "First select an item in the list";
                    }
                    else
                    {
                        // lastIndex = substancesListBox.SelectedIndex;
                        lastIndex = editingListBox.SelectedIndex;
                        Console.WriteLine(lastIndex);
                        editingListBox.Items.RemoveAt(lastIndex);
                    }
                    // healthyBehaviorsListBox.Items.Remove(lastIndex);
                }
            }
            else
            {
                titleTextBlock.Text = "Select Substances or Healthy Behaviors";
            }

        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        { int index;
            bool effectsChecked = false;

            if (editHealthyBehaviorsFlag)
            {
               // ListBoxItem newListBoxItem = new ListBoxItem();


                index = healthyBehaviorsListBox.Items.Count + 1;

                
                 if (editCerebellumCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[0] = '1';
                }
                 if (editBrainstemCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[1] = '1';
                }
                 if (editPituitaryGlandCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[2] = '1';
                }
                 if (editAmygdalaCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[3] = '1';


                }
                 if (editHippocampusCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[4] = '1';
                }
                 if (editTemporalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[5] = '1';
                }
                 if (editOccipitalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[6] = '1';
                }
                 if (editParietalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[7] = '1';
                }
                 if ( editFrontalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[8] = '1';
                }
                lightingSequenceString = CharArrayToString(lightingSequenceToDatabase);
                //Copy it into database
            }
            if (editSubstancesFlag)
            {
                ListBoxItem newListBoxItem = new ListBoxItem();
                index = substancesListBox.Items.Count + 1;
                if (editCerebellumCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[0] = '1';
                }
                 if (editBrainstemCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[1] = '1';
                }
                 if (editPituitaryGlandCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[2] = '1';
                }
                 if (editAmygdalaCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[3] = '1';


                }
                 if (editHippocampusCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[4] = '1';
                }
                 if (editTemporalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[5] = '1';
                }
                 if (editOccipitalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[6] = '1';
                }
                if (editParietalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[7] = '1';
                }
                 if (editFrontalLobeCheckbox.IsChecked.Value)
                {
                    effectsChecked = true;
                    lightingSequenceToDatabase[8] = '1';
                }
                
                //Copy into database
            }
                if (!effectsChecked && newListBoxItemContent == null)
                {
                    errorTextBlock.Text = "No content was inserted or effects are checked.";
                Console.WriteLine(effectsChecked);
                Console.WriteLine(editAmygdalaCheckbox.IsChecked.Value);
                }
                else if (!effectsChecked && newListBoxItemContent != null)
                {
                    errorTextBlock.Text = "No effects are checked.";
                Console.WriteLine(effectsChecked);
            }
                else if (effectsChecked && newListBoxItemContent == null)
                {
                    errorTextBlock.Text = "No content was inserted.";
                Console.WriteLine(effectsChecked);
            }
                else if (effectsChecked && newListBoxItemContent != null)
                {
                // newListBoxItem.Content = newListBoxItemContent;
                // editingListBox.Items.Add(newListBoxItem);
                AddNewItemToDatabase();
                //Clear content box after saving
                // newListBoxItemContent.Text = string.Empty;
                errorTextBlock.Text = "Saved.";
                Console.WriteLine(effectsChecked);
            }
        }
        private void PopulateData()
        {   
            if(editHealthyBehaviorsFlag || editSubstancesFlag)
            {
                editingListBox.Items.Clear();
            }
            else
            {
                healthyBehaviorsListBox.Items.Clear();
                //cLEAR SUBSTANCE lISTbOX
            }
            foreach (HealthyBehavior item in healthyBehaviors)
            {
                if (editHealthyBehaviorsFlag)
                {
                    editingListBox.Items.Add(item.healthyBehaviors);
                }
                else
                {
                    healthyBehaviorsListBox.Items.Add(item.healthyBehaviors);
                }
            }

        }
        private void AddNewItemToDatabase()
        {
            if (editHealthyBehaviorsFlag)
            {
                HealthyBehavior newItem = new HealthyBehavior
                {
                    Id = healthyBehaviors.Count - 1,
                    healthyBehaviors = newListBoxItemContent,
                    lightingSequenceArray =lightingSequenceString
                };
                using(var db = new InteractiveBrainEntities())
                {
                    db.HealthyBehaviors.Add(newItem);
                    db.SaveChangesAsync();
                    db.Dispose();
                }
                var newData = FetchData();
                PopulateData();
                //Clear content box after saving
               // newListBoxItemContent.Text = string.Empty;
            }
        }
        public static async Task FetchData()
        {
            using (var db = new InteractiveBrainEntities())
            {
                healthyBehaviors = await (from healthyBehaviors in db.HealthyBehaviors select healthyBehaviors).ToListAsync();
                db.Dispose();
            }
        }
        public char[] StringToCharArray(String convertString)
        {
            char[] newCharArr = convertString.ToCharArray();
            return newCharArr;
        }
        public String CharArrayToString(char[] convertArray)
        {
            String newString = new string(convertArray);
            return newString;
        }
        private void ListBoxContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            newListBoxItemContent = listBoxContent.Text;
        }


       
    }
}
