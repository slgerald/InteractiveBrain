using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Windows.Resources;
using System.Data;
//using interactiveBrainModel;


namespace InteractiveBrain
{
    /// <summary>
    /// Interaction logic for interactiveBrainControl.xaml
    /// </summary>
    public partial class interactiveBrainControl : UserControl
    {
        //The following variable are for the userControl interaction
        private static interactiveBrainControl _instance; //used to instantiate new instance of interactiveBrainControl

        string selectedBrainPart;//string to determine which brain part was selected
        string displayMessage;//What should be displayed above both brain maps
        string selectedSubstances;//string to determine which substance was selected 
        string lastSubstanceSelected; //used to determine which substance to displayed when examplesButton is pressed
        string selectedHealthyBehaviors;//string to determine which healthy behavior was selected 

        bool brainPart = false; //was a brain part selected?
        bool healthybehaviorBOOL = false; //was a healthy behavior selected?
        bool substance = false;//was a substance selected?
        Border border;

        DoubleAnimation animation = new DoubleAnimation();//animation used for the glowing effect


        //THe following variables are for serial communication
        SerialPort SerialPort1; //SerialPort for serial communication with brain
        bool isConnected = false;//using isConnected instead
        string[] ports; //Array of strings that list the available com ports 
        static ConcurrentQueue<char> serialDataQueue; // for serial communication
        string selectedPort;//for serial communication, which port was chosen


        //The following variables are for the editing function of the interactiveBrainControl
        bool editSubstancesFlag = false; //for editPopup, load substances from database
        bool editHealthyBehaviorsFlag = false;//for editPopup, load healthy behaviors from database
        ListBoxItem newListBoxItem = new ListBoxItem();
        string newListBoxItemContent;
        bool defaultFlag = false;

        List<string> col = new List<string>();//Used for the serach AutocompleteTextbox
        //The following array will help determine which parts should illuminate on the app and interactive brain
        //based on the selection of substances or healthy behaviors 

        char[] lightingSequenceToDatabase = { '0', '0', '0', '0', '0', '0', '0', '0', '0', '\0' }; //Writing to the database
        char[] lightingSequenceFromDatabase = { '0', '0', '0', '0', '0', '0', '0', '0', '0', '\0' };//Reading from the 
        string lightingSequenceString;
        int index;

        //How to change connection string when transferring to local machine
        string dbConnectionString = @"data source=C:\Users\Owner\source\repos\InteractiveBrain\interactiveBrainDatabase.db";


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
            GetAvailableComPorts(); //list available serial ports in array of strings

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

            foreach (string port in ports)
            {
                comPortNumberComboBox.Items.Add(port);
                Console.WriteLine(port);
            }

            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
            string query;

            //Add healthy behaviors in database to list of strings
            try
            {
                sqlitecon.Open();
                query = "select * from HealthyBehaviors";
                SQLiteCommand createCommand = new SQLiteCommand(query, sqlitecon);
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    col.Add(dr.GetString(1));
                }
                sqlitecon.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            //This function is used for the listbox not search autocomplete textbox
            Fill_ListBox();
        }

        //The following region pertains to the connection button and its popup and when
        //the pop up is closed 
        #region
        private void toggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (defaultFlag == false)
            {
                //If there aren't any serial ports to connect to
                //Display message not able to connect and the togglebutton doesn't
                //become checked
                if (ports == null || ports.Length == 0)
                {
                    selectionMessageBox.Text = "Not able to connect, check connection, then try again. ";

                }
                else
                {
                    try //Try to connect to serial port
                    {
                        isConnected = true;
                        if (!connectionPopup.IsOpen)
                        { connectionPopup.IsOpen = true; }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
                defaultFlag = true;
            }
            else if (defaultFlag == true)
            {
                //If the app is connected, change the image of button

                if (isConnected)
                {
                    selectionMessageBox.Text = "Disconnected";
                    try
                    {
                        Uri resourceUri = new Uri("Resources/if_connect_no.ico", UriKind.Relative);
                        StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                        BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                        var brush = new ImageBrush();
                        brush.ImageSource = temp;
                        toggleButton.Background = brush;

                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    isConnected = false;
                }
                //If there wasn't already successful connection
                else
                {   //if there aren't any serial ports available
                    if (ports == null || ports.Length == 0)
                    {
                        selectionMessageBox.Text = "Not able to connect, check connection, then try again. ";
                    }
                    else  //if they are serial ports available that app isn't use
                    {
                        selectionMessageBox.Text = "Not connected yet";
                    }
                }
                defaultFlag = false;
            }
        }
        //if com port is selected Check if selected port is being used by another process,Instantiate the serial port,
        //and change the connection button otherwise don't change connection button image
        //ensure connectionPopup is opened with next click of connection button
        private void CloseConnectionPopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (connectionPopup.IsOpen) { connectionPopup.IsOpen = false; }
            try
            {
                selectedPort = comPortNumberComboBox.SelectedItem.ToString();
                SerialPort1 = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
                SerialPort1.Open();
                Thread.Sleep(250);
                SerialPort1.Close();
            }

            catch (UnauthorizedAccessException ex)//The selected comPort is being used by another process
            {

                selectionMessageBox.Text = "Chosen COM Port in use, connect to another";
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            if (selectedPort != null)
            {
                Console.WriteLine("Connected to " + selectedPort);
                SerialPort1 = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
                SerialPort1.ReadTimeout = 500;
                SerialPort1.WriteTimeout = 500;
                Uri resourceUri = new Uri("Resources/if_connect_established.ico", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush();
                brush.ImageSource = temp;

                toggleButton.Background = brush;
            }
            else  //if there are serial ports available that app isn't use
            {
                selectionMessageBox.Text = "COM PORTS available, no COM PORT NUMBER chosen";
                isConnected = false;
                defaultFlag = false;
            }
            // open the Popup if it isn't open already 
        }

        #endregion

        //The following region pertains to the editListsPopUp
        #region
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!editListsPopup.IsOpen)
            { editListsPopup.IsOpen = true; }
        }
        //This function loads the listbox in the editListPopup with the current
        //items for the substances table in the database
        private void EditSubstancesRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            errorMessageTextBlock.Text = "";
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
        //This function loads the listbox in the editListPopup with the current
        //items for the healthyBehaviors table in the database
        private void EditHealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            errorMessageTextBlock.Text = "";
            editHealthyBehaviorsFlag = true;
            editSubstancesFlag = false;
            editingListBox.Items.Clear();
            Fill_ListBox();
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
                errorMessageTextBlock.Text = "Select Substances or Healthy Behaviors";
            }
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (editHealthyBehaviorsFlag || editSubstancesFlag)
            {
                if (editHealthyBehaviorsFlag)
                {

                    if (editingListBox.SelectedItem == null)
                    {
                        errorMessageTextBlock.Text = "First select an item in the list";
                    }
                    else
                    {
                        errorMessageTextBlock.Text = "";
                        RemoveItemToDatabase();
                    }

                }
                if (editSubstancesFlag)
                {

                    if (editingListBox.SelectedItem == null)
                    {
                        errorMessageTextBlock.Text = "First select an item in the list";
                    }
                    else
                    {
                        errorMessageTextBlock.Text = "";
                    }

                }
            }
            else
            {
                errorMessageTextBlock.Text = "Select Substances or Healthy Behaviors";
            }

        }
        //The string in the content textbox becomes the content in the new ListBox item
        //the checkboxes checked mean '1' in the lighting sequence for that part and 
        //'0' for unchecked checkboxes, the item is then added to the database 
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool effectsChecked = false;
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

            lightingSequenceString = CharArrayToString(lightingSequenceToDatabase);

            if (!effectsChecked && newListBoxItemContent == null)
            {
                errorTextBlock.Text = "No content was inserted or effects are checked.";
            }
            else if (!effectsChecked && newListBoxItemContent != null)
            {
                errorTextBlock.Text = "No effects are checked.";
            }
            else if (effectsChecked && newListBoxItemContent == null)
            {
                errorTextBlock.Text = "No content was inserted.";
            }
            else if (effectsChecked && newListBoxItemContent != null)
            {
                AddNewItemToDatabase();
                listBoxContent.Text = "";
                effectsChecked = false;
                editAmygdalaCheckbox.IsChecked = false;
                editParietalLobeCheckbox.IsChecked = false;
                editTemporalLobeCheckbox.IsChecked = false;
                editFrontalLobeCheckbox.IsChecked = false;
                editCerebellumCheckbox.IsChecked = false;
                editHippocampusCheckbox.IsChecked = false;
                editPituitaryGlandCheckbox.IsChecked = false;
                editBrainstemCheckbox.IsChecked = false;
                editOccipitalLobeCheckbox.IsChecked = false;
                errorTextBlock.Text = "Saved.";
            }
        }
        //resest the editListsPopup
        private void CloseEditListsPopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (editListsPopup.IsOpen) { editListsPopup.IsOpen = false; }
            editHealthyBehaviorsFlag = false;
            editSubstancesFlag = false;
            editingListBox.Items.Clear();
            editHealthyBehaviorsRadioButton.IsChecked = false;
            editSubstancesRadioButton.IsChecked = false;
            errorMessageTextBlock.Text = "";
            Fill_ListBox();
        }
        #endregion
        //The following region pertains to the contentPopup
        #region
        private void ListBoxContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            newListBoxItemContent = listBoxContent.Text;
        }
        private void CloseContentPopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (contentPopup.IsOpen) { contentPopup.IsOpen = false; }
            listBoxContent.Text = "";
            errorTextBlock.Text = "";
            editAmygdalaCheckbox.IsChecked = false;
            editParietalLobeCheckbox.IsChecked = false;
            editTemporalLobeCheckbox.IsChecked = false;
            editFrontalLobeCheckbox.IsChecked = false;
            editCerebellumCheckbox.IsChecked = false;
            editHippocampusCheckbox.IsChecked = false;
            editPituitaryGlandCheckbox.IsChecked = false;
            editBrainstemCheckbox.IsChecked = false;
            editOccipitalLobeCheckbox.IsChecked = false;
        }
        #endregion

        //The following region pertains to the examplesPopup
        #region
        //This function determines which substances are listed based on the substance selected
        private void ExamplesButton_Click(object sender, RoutedEventArgs e)
        {  // open the Popup if it isn't open already 
            if (!examplesPopup.IsOpen) { examplesPopup.IsOpen = true; }
            examplesButton.Visibility = System.Windows.Visibility.Visible;
            if (lastSubstanceSelected == "stimulants")
            {
                listedExamples.Text = "Examples of Stimulants:\r\n\r\n\u2022 Dexedrine® \r\n\u2022 Adderall® \r\n\u2022 Ritalin® \r\n\u2022 Concerta®\r\n\r\n(www.drugabuse.gov) ";
            }
            if (lastSubstanceSelected == "depressants")
            {
                examplesButton.Content = "Click for examples of Depressants";
                listedExamples.Text = "Examples of Depressants: \r\n\r\n\u2022 Valium® \r\n\u2022 Xanax® \r\n\u2022 Halcion® \r\n\u2022 Ambien® \r\n\u2022 Lunesta® \r\n\u2022 Alcohol \r\n" +
                   "\u2022 Hydrocone(e.g.Vicodin®) \r\n\u2022 Oxycodone(e.g., OxyContin®, Percocet®) \r\n\u2022 Oxymorphone(e.g., Opana®) \r\n\u2022 Morphine(e.g., Kadian®, Avinza®)\r\n\u2022 Codeine, Fentanyl, and others\r\n\r\n(www.drugabuse.gov) ";
            }
            if (lastSubstanceSelected == "hallucinogens")
            {
                examplesButton.Content = "Click for examples of Hallucinogens";
                listedExamples.Text = "Examples of Hallucinogens: \r\n\r\n\u2022 LSD \r\n\u2022 Phencyclidine(PCP), \r\n\u2022 Psilocybin(e.g. shrooms) \r\n\r\n(www.drugabuse.gov) ";
            }

        }
        private void CloseExamplesPopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (examplesPopup.IsOpen) { examplesPopup.IsOpen = false; }
        }
        #endregion

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

            //  if (isConnected)
            //  {
            //send serial message for stop
            //  }
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
            Console.WriteLine(selectedHealthyBehaviors);
            examplesButton.Visibility = System.Windows.Visibility.Hidden;

            ListBoxSelectionChanged();


        }
        #endregion
        //The following code determines what is displayed in the selectionMessageBox
        //based on the selection
        #region
        private void Cerebellum_Selected(object sender, RoutedEventArgs e)
        {
            // displayMessage = "Note the LED corresponding to the Cerebellum light up";
            lightingSequenceFromDatabase = "1000000000".ToArray();
            //  WriteLightingSequenceMessage();
        }
        private void Brainstem_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Brainstem light up";
            lightingSequenceFromDatabase = "0100000000".ToArray();
            //  WriteLightingSequenceMessage();
        }
        private void PituitaryGland_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Pituitary Gland light up";
            lightingSequenceFromDatabase = "0010000000".ToArray();
            //  WriteLightingSequenceMessage();
        }
        private void Amygdala_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Amygdala light up";
            lightingSequenceFromDatabase = "0001000000".ToArray();
            //  WriteLightingSequenceMessage();
        }

        private void Hippocampus_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Hippocampus light up";
            lightingSequenceFromDatabase = "0000100000".ToArray();
            //  WriteLightingSequenceMessage();
        }
        private void TemporalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Temporal Lobe light up";
            lightingSequenceFromDatabase = "0000010000".ToArray();
            //  WriteLightingSequenceMessage();
        }
        private void ParietalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Parietal Lobe light up";

            lightingSequenceFromDatabase = "0000001000".ToArray();
            //  WriteLightingSequenceMessage();
        }
        private void OccipitalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Occipital Lobe light up";
            lightingSequenceFromDatabase = "0000000100".ToArray();
            //  WriteLightingSequenceMessage();
        }

        private void FrontalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Frontal Lobe light up";
            lightingSequenceFromDatabase = "0000000010".ToArray();
            //  WriteLightingSequenceMessage();
        }


        private void Stimulants_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            lastSubstanceSelected = "stimulants";
            //  WriteLightingSequenceMessage();
        }
        private void Depressants_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            lastSubstanceSelected = "depressants";
            //  WriteLightingSequenceMessage();
        }
        private void Hallucinogens_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            lastSubstanceSelected = "hallucinogens";
            //  WriteLightingSequenceMessage();
        }
        private void Opioids_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
            lastSubstanceSelected = "opioids";
            //  WriteLightingSequenceMessage();

        }

        #endregion

        //The following region pertains to the AutoComplete search box 
        #region
        private void SearchTextBox_KeyUp(object sender, KeyboardEventArgs e)
        {
            bool found = false;
            border = (resultsStack.Parent as ScrollViewer).Parent as Border;
            string query_Two = (sender as TextBox).Text;

            if (query_Two.Length == 0)
            {
                // Clear   
                resultsStack.Children.Clear();
                border.Visibility = System.Windows.Visibility.Collapsed;
                border.Background = Brushes.Transparent;
            }
            else
            {
                border.Visibility = System.Windows.Visibility.Visible;
                border.Background = Brushes.White;
            }

            // Clear the list   
            resultsStack.Children.Clear();

            // Add the result   
            foreach (string obj in col)
            {
                if (obj.ToLower().StartsWith(query_Two.ToLower()))
                {
                    // The word starts with this... Autocomplete must work   
                    addItem(obj,"PeachPuff");
                    found = true;
                }
                else
                {
                    addItem(obj, "White");
                }
            }
            if (!found)
            {
                resultsStack.Children.Clear();
                resultsStack.Children.Add(new TextBlock() { Text = "No results found.", FontSize = 20, FontFamily = new FontFamily("Calibri") });
            }
        }
    
        

        private void addItem(string text,string color)
        {
            TextBlock block = new TextBlock();
                                    

            // Add the text   
            block.Text = text;

            // A little style...   
            block.Margin = new Thickness(2, 3, 2, 3);
            block.Cursor = Cursors.Hand;
            block.FontFamily = new FontFamily("Calibri");
            block.FontSize = 20;
            block.Background = new BrushConverter().ConvertFromString(color) as SolidColorBrush;

            // Mouse events   
            block.MouseLeftButtonUp += (sender, e) =>
            {
                searchTextBox.Text = (sender as TextBlock).Text;
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.PeachPuff;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.Transparent;
            };

            // Add to the panel   
            resultsStack.Children.Add(block);
        }
       private void ResultsStackMouseDown(object sender,MouseButtonEventArgs e) {
            resultsStack.Children.Clear();
            border.Visibility = System.Windows.Visibility.Collapsed;
            border.Background = Brushes.Transparent;

        }
        #endregion
        //This function determines what happens when the Go Button is Clicked
        //Depending on the selection, make affected areas glow 
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {

            animation.From = 1.0;
            animation.To = 0.4;
            animation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            animation.AutoReverse = true;
            animation.RepeatBehavior = RepeatBehavior.Forever;
            if (brainPart)
            {
                
                selectionMessageBox.Text = selectedBrainPart + " was chosen. ";
                healthyBehaviorsListBox.SelectedItem = false;
                substancesListBox.SelectedItem = false;
                brainPart = false;
                //Don't need to look in database because hard coded 
                LightingSequence();
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
            else
            {
                substancesListBox.SelectedItem = false;
                brainPartsListBox.SelectedItem = false;
                selectedHealthyBehaviors = searchTextBox.Text;
                selectionMessageBox.Text = selectedHealthyBehaviors + " was chosen. " + displayMessage;
                healthybehaviorBOOL = false;
                
                SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
                string Query;
                try
                {
                    sqlitecon.Open();
                    Query = "select * from HealthyBehaviors where healthyBehaviors='" + selectedHealthyBehaviors + "' ";
                    SQLiteCommand createCommand = new SQLiteCommand(Query, sqlitecon);
                    SQLiteDataReader dr = createCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        string receivedString = dr.GetString(2);
                        lightingSequenceString = receivedString;
                        Console.WriteLine(lightingSequenceString);
                    }
                    sqlitecon.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                if (lightingSequenceString == null)
                {
                    selectionMessageBox.Text = "Choose a option available from the autocomplete drop down list ";
                }
                else
                {
                    resultsStack.Children.Clear();
                    border.Visibility = System.Windows.Visibility.Collapsed;
                    border.Background = Brushes.Transparent;
                    lightingSequenceFromDatabase = lightingSequenceString.ToArray();
                    LightingSequence();
                }

            }

        }
        public char[] StringToCharArray(string convertString)
        {
            char[] newCharArr = convertString.ToCharArray();
            return newCharArr;
        }
        public string CharArrayToString(char[] convertArray)
        {
            string newString = new string(convertArray);
            return newString;
        }
        //This functions deterine which parts of the brain on the app side to illuminate based on
        //selection of substances and healthy behaviors
        private void LightingSequence()
        {
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
            WriteLightingSequenceMessage();
        }


        private void WriteLightingSequenceMessage()
        {

            //open serial port
            //Add raspberrypi identifier(character array)
            //lightingSequenceFromDatabase 
            //concatenate all the parts in a character array
            //Send serial message for stop to turn off all the lights
            //should i wait a few milliseconds?
            //character array as string
            // Convert charArray as string, because zeros may be read as nulls
            //close serial port
            if (isConnected)
            {
                try
                {
                    SerialPort1.Open();
                    // SerialPort1.Write(new string(concatenateArray));
                    SerialPort1.Close();
                }
                catch (UnauthorizedAccessException ex)
                {

                    selectionMessageBox.Text = "Chosen COM Port in use, connect to another";
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            else
            {
                Console.WriteLine("Not connected to serial port to write serial message");
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
       
        //tHIS FUNCTION NEEDS TO ADD SUBSTANCESlISTbOX
        //This function is used to fill the listbox with items from the database
        //used to also update list after item has been added or removed 
        public void Fill_ListBox()
        {
            string query;
            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
            try
            {
                sqlitecon.Open();
                if (editHealthyBehaviorsFlag)
                {
                    editingListBox.Items.Clear();
                    query = "select * from HealthyBehaviors";
                }
                else if (editSubstancesFlag)
                {
                    editingListBox.Items.Clear();
                    query = "select * from Substances";
                }
                else //If this function is being called other than during editing
                {
                    healthyBehaviorsListBox.Items.Clear();
                    query = "select * from HealthyBehaviors";
                    //query = "select * from Substances";
                }
                SQLiteCommand createCommand = new SQLiteCommand(query, sqlitecon);
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    ListBoxItem li = new ListBoxItem();
                    if (editHealthyBehaviorsFlag)
                    {
                        string healthyBehaviorListBoxItemContent = dr.GetString(1);
                        li.Content = healthyBehaviorListBoxItemContent;

                        editingListBox.Items.Add(li);
                        li.FontFamily = new FontFamily("Calibri");
                        li.FontSize = 16;
                        //    string substancesListBoxItemContent = dr.GetString(1);
                        //    li.Content = substancesListBoxItemContent;
                        //    substancesListBox.Items.Add(li);

                    }
                    else if (editSubstancesFlag)
                    {
                        string substancesListBoxItemContent = dr.GetString(1);
                        li.Content = substancesListBoxItemContent;
                        editingListBox.Items.Add(li);
                        li.FontFamily = new FontFamily("Calibri");
                        li.FontSize = 16;
                    }
                    else
                    {
                        string healthyBehaviorListBoxItemContent = dr.GetString(1);
                        li.Content = healthyBehaviorListBoxItemContent;
                        healthyBehaviorsListBox.Items.Add(li);
                        //    string substancesListBoxItemContent = dr.GetString(1);
                        //    li.Content = substancesListBoxItemContent;
                        //    substancesListBox.Items.Add(li);

                        li.FontFamily = new FontFamily("Calibri");
                        li.FontSize = 20;
                    }
                }
                sqlitecon.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //This function updates indices of database table with each new remove,add or update
        private void UpdateIndices()
        {
            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
            int index = 0;
            int changingIndex = 0;
            while (index < editingListBox.Items.Count)
            {
                Console.WriteLine(editingListBox.Items.Count);
                changingIndex = index;
                Console.WriteLine(changingIndex);
                string query;
                try
                {
                    sqlitecon.Open();
                    if (editHealthyBehaviorsFlag)
                    {
                        query = "UPDATE HealthyBehaviors SET Id='" + changingIndex + "' where healthyBehaviors='" + ((ListBoxItem)editingListBox.Items[index]).Content.ToString() + "' ";
                    }
                    else
                    {
                        query = "update Substances set Id='" + index + "',substanceName='" + newListBoxItemContent + "',lightingSequenceArray'" + lightingSequenceString + "'where Id='" + index + "' ";
                    }

                    SQLiteCommand createCommand = new SQLiteCommand(query, sqlitecon);
                    createCommand.ExecuteNonQuery();
                    sqlitecon.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                index++;
            }
        }
        private void AddNewItemToDatabase()
        {
            ListBoxItem li = new ListBoxItem();
            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
            string query;
            index = editingListBox.Items.Count - 1;

            try
            {
                sqlitecon.Open();
                if (editHealthyBehaviorsFlag)
                {
                    query = "insert into HealthyBehaviors (Id, healthyBehaviors, lightingSequenceArray) values ('" + index + "','" + newListBoxItemContent + "','" + lightingSequenceString.Substring(0, 9) + "')";

                }
                else
                {
                    query = "insert into Substances (Id, substanceName, lightingSequenceArray) values ('" + index + "','" + newListBoxItemContent + "','" + lightingSequenceString + "')";
                }

                SQLiteCommand createCommand = new SQLiteCommand(query, sqlitecon);
                createCommand.ExecuteNonQuery();
                sqlitecon.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            UpdateIndices();
            Fill_ListBox();
        }
       
        private void RemoveItemToDatabase()
        {
            ListBoxItem li = new ListBoxItem();
            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
            index = editingListBox.SelectedIndex;
            string query;
            try
            {
                sqlitecon.Open();
                if (editHealthyBehaviorsFlag)
                {
                    query = "delete from HealthyBehaviors where Id='" + index + "' ";
                    Console.WriteLine(query);
                }
                else
                {
                    query = "delete from Substances where Id='" + index + "' ";
                }
                SQLiteCommand createCommand = new SQLiteCommand(query, sqlitecon);
                createCommand.ExecuteNonQuery();
                sqlitecon.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Fill_ListBox();
            UpdateIndices();
        }
        private void UpdateItemToDatabase()
        {
            ListBoxItem li = new ListBoxItem();
            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
            index = editingListBox.SelectedIndex;
            string query;
            try
            {
                sqlitecon.Open();
                if (editHealthyBehaviorsFlag)
                {
                    query = "update HealthyBehaviors set Id='" + index + "',healthyBehaviors='" + newListBoxItemContent + "',lightingSequenceArray'" + lightingSequenceString + "'where Id='" + index + "' ";
                }
                else
                {
                    query = "update Substances set Id='" + index + "',substanceName='" + newListBoxItemContent + "',lightingSequenceArray'" + lightingSequenceString + "'where Id='" + index + "' ";
                }

                SQLiteCommand createCommand = new SQLiteCommand(query, sqlitecon);
                createCommand.ExecuteNonQuery();
                sqlitecon.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            UpdateIndices();
            Fill_ListBox();
        }
    }
}

 
