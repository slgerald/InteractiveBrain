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
using System.Management;
using System.IO;
using Windows.Storage;
using Windows.ApplicationModel;
using System.Reflection;
using System.Security.AccessControl;



namespace InteractiveBrain
{
    /// <summary>
    /// Interaction logic for interactiveBrainControl.xaml
    /// </summary>
    /// 
    public partial class interactiveBrainControl : UserControl
    {
        //The following variables are for the userControl interaction
        private static interactiveBrainControl _instance; //used to instantiate new instance of interactiveBrainControl

        string selectedBrainPart;//string to determine which brain part was selected
        string displayMessage;//Text displayed above both brain maps
        string selectedHealthyBehaviors;//string to determine which healthy behavior was selected 

        bool brainPart = false; //was a brain part selected?
        bool healthybehaviorBOOL = false; //was a healthy behavior selected?

       
        Border border; //The border of the drop down list showing the results of the healthy behavior query
        bool dropDownFlag = false; //determine which arrow to show for the drop down list(up or down

        //Storyboard use to apply scaling and "glowing" animations
        Storyboard storyboard = new Storyboard();
        DoubleAnimation growXAnimation = new DoubleAnimation();
        DoubleAnimation growYAnimation = new DoubleAnimation();
        DoubleAnimation glowAnimation = new DoubleAnimation();//animation used for the glowing effect
        ScaleTransform scale = new ScaleTransform();

        //THe following variables are for serial communication
        SerialPort SerialPort1; //SerialPort for serial communication with brain
        bool isConnected = false;//using isConnected instead
        string[] ports; //Array of strings that list the available com ports 
        static ConcurrentQueue<char> serialDataQueue; // for serial communication
        string selectedPort;//for serial communication, which port was chosen


        //The following variables are for the editing function of the interactiveBrainControl
        bool editHealthyBehaviorsFlag = false;//for editPopup, load healthy behaviors from database
        string newListBoxItemContent;
        bool defaultFlag = false;
        bool selectionMade = false;

        List<string> col = new List<string>();//Used for the serach AutocompleteTextbox
        
        //The following array will help determine which parts should illuminate on the app and interactive brain
        //based on the selection of healthy behaviors 
        char[] lightingSequenceToDatabase = { '0', '0', '0', '0', '0', '0', '0', '0', '0', '\0' }; //Writing to the database
        char[] lightingSequenceFromDatabase = { '0', '0', '0', '0', '0', '0', '0', '0', '0', '\0' };//Reading from the 
        string lightingSequenceString;
        int index;

        //Global connection string used when connecting to local machine    
        string dbConnectionString;

        //The following variables are used to transfer the database to a place where its writable 
        //unlike the Program Files
        Boolean searchResult = false;
        StorageFile sampleFile;
        StorageFolder storageFolder;
        private StorageFolder m_downloadfolder;

        public StorageFolder Download_Folder
        {
            get { return m_downloadfolder; }
            set { m_downloadfolder = value; }
        }

        string query;
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
         
          //Works in visual studio but write to progam files folder
          //currentPath= Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
          //dbConnectionString = string.Format(@"DataSource={0}/interactiveBrainDatabase.db", currentPath);

            CopyDatabase();//This function is used to copy the database file to a a location the user can write
                           //to the database file
                           //    dbConnectionString = string.Format("DataSource={0}\\InteractiveBrain\\interactiveBrainDatabase.db", AppDomain.CurrentDomain.GetData("DataDirectory"));

            // dbConnectionString = string.Format("DataSource={0}\\Interactive Brain\\interactiveBrainDatabase.db", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            dbConnectionString = string.Format("DataSource={0}\\Interactive Brain\\interactiveBrainDatabase.db", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            // dbConnectionString = "DataSource=C:\\ProgramData\\Interactive Brain\\interactiveBrainDatabase.db";
            Console.WriteLine(dbConnectionString);

            //Two way communication wasn't used 
            //Preparing for possible two way serial connection
            //try
            //{
           //  SerialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            serialDataQueue = new System.Collections.Concurrent.ConcurrentQueue<char>();

            foreach (string port in ports)
            {
                comPortNumberComboBox.Items.Add(port);
                Console.WriteLine(port);
            }

            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString,true);

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
                MessageBox.Show("Wasn't able to load Healthy Behaviors list");

               // MessageBox.Show(ex.ToString());
            }

            //This function is used for the listbox not search autocomplete textbox
            col = col.OrderBy(q => q).ToList();
            Fill_ListBox();
        }

        //This function is used to copy the database file from the installation folder
        //C:/Program Files/Interactive Brain/ to the app's Application Data Local Folder 
        //so user is able to write to it.
        public  void CopyDatabase()
        {   //Hard coded string for program files and data string works but not  AppDomain.CurrentDomain.GetData("DataDirectory");
            string fileName = "interactiveBrainDatabase.db";
            //MessageBox.Show(fileName); //For debugging Purposes
            string sourcePath = "C:\\Program Files\\Interactive Brain"; //Where original database file was installed
                                                                        //MessageBox.Show(sourcePath); //For debugging Purposes

            //string targetPath = string.Format("{0}\\Interactive Brain", AppDomain.CurrentDomain.GetData("DataDirectory"));
            //string targetPath = string.Format("{0}\\Interactive Brain",
            //Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)); //different than AppDomain.CurrentDomain.GetData("DataDirectory)
            //string targetPath = "C:\\ProgramData\\Interactive Brain"; 
            //MessageBox.Show(targetPath); //For debugging purposes

            string targetPath = string.Format("{0}\\Interactive Brain",
                 Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            Console.WriteLine("GetFolderPath: {0}",
                 Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            
            //To copy a folder's contents to a new location:
            //Create a new target folder, if necessary.
            if (!System.IO.Directory.Exists(targetPath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }
                catch { Console.WriteLine("Couldn't create folder " + targetPath); }
            }
            else
            {
                Console.WriteLine(targetPath + " already exists ");
            }

            // Determines the path to the database file in the installation folder
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            if (System.IO.File.Exists(sourceFile))
            {
                Console.WriteLine(HasWritePermissionOnDir(sourceFile));//For debugging purposes
                //MessageBox.Show(sourceFile); //For debugging Purposes
            }
            else {
                Console.WriteLine(sourceFile + "does not exist");
            }
            
            //Determines the path to the database file that has been copied from the 
            //installation folder to a location with read/write privelages
            string destFile = System.IO.Path.Combine(targetPath, fileName);
            if (System.IO.File.Exists(destFile))
            {
                Console.WriteLine(HasWritePermissionOnDir(destFile));
                //MessageBox.Show(destFile); //For debugging purposes
            }
            else {
                Console.WriteLine(destFile + "does not exist");
            }

            // To copy a file to another location if it doesn't already exists.
            // Don't overwrite the destination file if it already exists.
            if (!System.IO.File.Exists(destFile))
            {     
                try
                {
                    System.IO.File.Copy(sourceFile, destFile, true);
                }
                catch { Console.WriteLine("Couldn't copy database file from installation folder C:\\Program Files\\Interactive Brain "); }
            }
            else
            { 
                Console.WriteLine("Database file already exists, did not copy from installation folder again");
            }
            Thread.Sleep(50);
        }
        //This function is used to determine if certain folders and file have write
        //privelages for the database file 
        public static bool HasWritePermissionOnDir(string path)
        {
            var writeAllow = false;
            var writeDeny = false;
            var accessControlList = Directory.GetAccessControl(path);
            if (accessControlList == null)
                return false;
            var accessRules = accessControlList.GetAccessRules(true, true,
                                        typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                    continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    writeAllow = true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    writeDeny = true;
            }

            return writeAllow && !writeDeny;
        }

        //This function is called when the connection button is pressed 
        private void connectionButton_Click(object sender, RoutedEventArgs e)
        {   //The default flag is used to treat button as toggle button
            if (defaultFlag == false)
            {
                //If there aren't any serial ports to connect to
                //Display message not able to connect and the togglebutton doesn't
                //change
                if (ports == null || ports.Length == 0)
                {
                    MessageBox.Show("Check connection, then try again.");
                }
                else
                {
                    try //Try to connect to serial port
                    {
                        isConnected = true;
                      
                        if (!connectionPopup.IsOpen)
                        { connectionPopup.IsOpen = true; }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message);
                        MessageBox.Show("Couldn't open the pop-up to allow for connection to the Brain");
                        isConnected = false;
                    }
                }
                defaultFlag = true;
            }
            else if (defaultFlag == true)
            {
                //If the app is connected, change the image of button
                if (isConnected)
                {
                    MessageBox.Show("Disconnected");
                    try
                    {
                        Uri resourceUri = new Uri("Resources/if_connect_no.ico", UriKind.Relative);
                        StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                        BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                        var brush = new ImageBrush();
                        brush.ImageSource = temp;
                        connectionButton.Background = brush;

                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    isConnected = false;
                }
                //If there wasn't already successful connection
                else
                {   //if there aren't any serial ports available
                    if (ports == null || ports.Length == 0)
                    {
                        MessageBox.Show("Not able to connect, check connection, then try again.");
                    }
                    else  //if they are serial ports available that app aren't in use
                    {
                        MessageBox.Show("The Brain wasn't connected yet.");
                    }
                }
                defaultFlag = false;
            }
            ListBoxSelectionChanged();
        }
        //if com port is selected Check if selected port is being used by another process,Instantiate the serial port,
        //and change the connection button otherwise don't change connection button image
        //ensure connectionPopup is opened with next click of connection button
        private void CloseConnectionPopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            //Make attempt to keep serail port open a short time to see if it will sucessfully
            //open before moving forward with the selected com port
            if (connectionPopup.IsOpen) { connectionPopup.IsOpen = false; }
            try
            {
                selectedPort = comPortNumberComboBox.SelectedItem.ToString();
            }
            catch {

                Console.WriteLine("No COM Port Chosen");
            }
            if (selectedPort != null) //now change the background of the connection button to show connected
            {
                //COMPortInfo selectedCOMPort = (COMPortInfo)selectedPort;
                try
                {
                    
                    SerialPort1 = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
                    SerialPort1.Open();
                    Thread.Sleep(10);
                    SerialPort1.Close();
                    Console.WriteLine("Connected to " + selectedPort);
                    SerialPort1 = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
                    SerialPort1.ReadTimeout = 250;
                    SerialPort1.WriteTimeout =250;
                    Uri resourceUri = new Uri("Resources/if_connect_established.ico", UriKind.Relative);
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush();
                    brush.ImageSource = temp;

                    connectionButton.Background = brush;
                }

                catch (UnauthorizedAccessException ex)//The selected comPort is being used by another process
                {

                    MessageBox.Show("Wasn't able to connect to chosen COM port, choose another port for connection");
                    Console.WriteLine(ex.Message);
                    Uri resourceUri = new Uri("Resources/if_connect_no.ico", UriKind.Relative);
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush();
                    brush.ImageSource = temp;
                    connectionButton.Background = brush;
                }
                catch (Exception ex) { MessageBox.Show("Couldn't successfully connect to selected COM port"); Console.WriteLine(ex.Message);
                    Uri resourceUri = new Uri("Resources/if_connect_no.ico", UriKind.Relative);
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush();
                    brush.ImageSource = temp;
                    connectionButton.Background = brush;


                }
                
            }
         
            else  //if there are serial ports available that app aren't in use
            {
                MessageBox.Show("No COM PORT was chosen");
                isConnected = false;
                defaultFlag = false;
            }
            // open the Popup if it isn't open already 
        }
       

        //This button opens the editing popup 
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!editListsPopup.IsOpen)
            { editListsPopup.IsOpen = true; }
            ListBoxSelectionChanged();
        }
        //This function loads the listbox in the editListPopup with the current
        //items for the healthyBehaviors table in the database
        private void EditHealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            errorMessageTextBlock.Text = "";
            editHealthyBehaviorsFlag = true; 
            Fill_ListBox();
        }

        //This function opens the content button to add an item to the healthy behaviors list
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (editHealthyBehaviorsFlag)
            {
                if (!contentPopup.IsOpen)
                {
                    contentPopup.IsOpen = true;
                }
            }
            
        }

        //THis function is used to remove an item from the healthy behaviors list
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (editHealthyBehaviorsFlag)
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
            }
            else
            {
                errorMessageTextBlock.Text = "Select  a healthy behaviors";
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

            if (!effectsChecked && string.IsNullOrEmpty(newListBoxItemContent))
            {
                errorTextBlock.Text = "No content was inserted or effects are checked.";
            }
            else if (!effectsChecked && !string.IsNullOrEmpty(newListBoxItemContent))
            {
                errorTextBlock.Text = "No effects are checked.";
            }
            else if (effectsChecked && string.IsNullOrEmpty(newListBoxItemContent))
            {
                errorTextBlock.Text = "No content was inserted.";
            }
            else if (effectsChecked && !string.IsNullOrEmpty(newListBoxItemContent))
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
        //reset the editListsPopup 
        private void CloseEditListsPopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (editListsPopup.IsOpen) { editListsPopup.IsOpen = false; }
            editHealthyBehaviorsFlag = false;
            editingListBox.Items.Clear();
            editHealthyBehaviorsRadioButton.IsChecked = false;
            errorMessageTextBlock.Text = "";
            UpdateIndices();
            Fill_ListBox();
        }

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
            editingTabControl.TabIndex = 0;
            editAmygdalaCheckbox.IsChecked = false;
            editParietalLobeCheckbox.IsChecked = false;
            editTemporalLobeCheckbox.IsChecked = false;
            editFrontalLobeCheckbox.IsChecked = false;
            editCerebellumCheckbox.IsChecked = false;
            editHippocampusCheckbox.IsChecked = false;
            editPituitaryGlandCheckbox.IsChecked = false;
            editBrainstemCheckbox.IsChecked = false;
            editOccipitalLobeCheckbox.IsChecked = false;
            Fill_ListBox();
        }
        #endregion

        //This function determines what happens when the selection of the brain Part list changes 
        //Set brain part flag true and other flags false 
        //When selection changes stop parts glowing based on the previous selection

        private void ListBoxSelectionChanged()
        {
            //Stop each part from flashing
            amygdalaImage.BeginAnimation(OpacityProperty, null);
       
            pituitaryGlandImage.BeginAnimation(OpacityProperty, null);
            hippocampusImage.BeginAnimation(OpacityProperty, null);
       
            brainstemImage.BeginAnimation(OpacityProperty, null);
        
            frontalLobeImage.BeginAnimation(OpacityProperty, null);
         
            temporalLobeImage.BeginAnimation(OpacityProperty, null);
          
            parietalLobeImage.BeginAnimation(OpacityProperty, null);
         
            occipitalLobeImage.BeginAnimation(OpacityProperty, null);
          
            cerebellumImage.BeginAnimation(OpacityProperty, null);
        
            //end scaling animation
            if (selectionMade) {
                storyboard.Children.Remove(growXAnimation);
                storyboard.Children.Remove(growYAnimation);
                selectionMade = false;
            }

            //reset each images render transform to prevent unintentional scaling from previous selection
            brainstemImage.RenderTransform = null;
            cerebellumImage.RenderTransform = null;
            hippocampusImage.RenderTransform = null;
            frontalLobeImage.RenderTransform = null;
            temporalLobeImage.RenderTransform = null;
            occipitalLobeImage.RenderTransform = null;
            parietalLobeImage.RenderTransform = null;
            amygdalaImage.RenderTransform = null;
            pituitaryGlandImage.RenderTransform = null;

            
           // if (isConnected)
           //   {
                //send serial message for stop
            //    try
            //    {
                    // lightingSequenceFromDatabase = "0000000000".ToArray();
            //        SerialPort1.DiscardOutBuffer();
            //        SerialPort1.Open();

            //        SerialPort1.Write("000000000");
            //        Thread.Sleep(20);
            //        SerialPort1.Close();
            //    }
            //    catch { MessageBox.Show("Check connection to Brain"); }
            //  }
        }


        private void BrainPartsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            brainPart = true;
            selectionMade = true;
            // healthybehaviorBOOL = false;
            //healthyBehaviorsListBox.SelectedItem = false;
            try
            {
                selectedBrainPart = ((ListBoxItem)brainPartsListBox.SelectedItem).Content.ToString();
            }
            catch {
                if (selectedBrainPart == "" || selectedBrainPart == null)
                {
                    MessageBox.Show("No brain part was selected. Select a brain part.");
                }
            }
            ListBoxSelectionChanged();
        }
        
        //This function determines what happens when the selection of the brain Part list changes 
        //Set activities flag true and other flags false 
        //Make the Examples Button Unavailable
        //When selection changes stop parts glowing based on the previous selection
        //The ListBox for healthy behaviors wasn't used 
        private void HealthyBehaviorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            brainPart = false;
            brainPartsListBox.SelectedItem = false;
            try
            {
                selectedHealthyBehaviors = ((ListBoxItem)healthyBehaviorsListBox.SelectedItem).Content.ToString();
            }
            catch
            {
              //  MessageBox.Show(selectedBrainPart );
            }
            Console.WriteLine(selectedHealthyBehaviors);
            ListBoxSelectionChanged();
        }
      
        //This function is called when cerebellu is chosen 
        private void Cerebellum_Selected(object sender, RoutedEventArgs e)
        {
            // displayMessage = "Note the LED corresponding to the Cerebellum light up";
            lightingSequenceFromDatabase = "100000000".ToArray();
        }

        //This function is called when the brainstem is selected
        private void Brainstem_Selected(object sender, RoutedEventArgs e)
        {
           // displayMessage = "Note the LED corresponding to the Brainstem light up";
            lightingSequenceFromDatabase = "010000000".ToArray();
        }

        //This function is called when the pituitary gland is selected 
        private void PituitaryGland_Selected(object sender, RoutedEventArgs e)
        {
           // displayMessage = "Note the LED corresponding to the Pituitary Gland light up";
            lightingSequenceFromDatabase = "001000000".ToArray();
        }

        //This function is called when the amygdala is selected 
        private void Amygdala_Selected(object sender, RoutedEventArgs e)
        {
          //  displayMessage = "Note the LED corresponding to the Amygdala light up";
            lightingSequenceFromDatabase = "000100000".ToArray();
        }

        //This function is called when the hippocampus is selected 
        private void Hippocampus_Selected(object sender, RoutedEventArgs e)
        {
          //  displayMessage = "Note the LED corresponding to the Hippocampus light up";
            lightingSequenceFromDatabase = "000010000".ToArray();
        }

        //This function is called when the temporal lobe is selected 
        private void TemporalLobe_Selected(object sender, RoutedEventArgs e)
        {
          //  displayMessage = "Note the LED corresponding to the Temporal Lobe light up";
            lightingSequenceFromDatabase = "000001000".ToArray();
        }

        //This function is called when the occipital lobe is selected 
        private void OccipitalLobe_Selected(object sender, RoutedEventArgs e)
        {
            //   displayMessage = "Note the LED corresponding to the Occipital Lobe light up";
            lightingSequenceFromDatabase = "000000100".ToArray();
        }

        //This function is called when the parietal lobe is selected 
        private void ParietalLobe_Selected(object sender, RoutedEventArgs e)
        {
           // displayMessage = "Note the LED corresponding to the Parietal Lobe light up";

            lightingSequenceFromDatabase = "000000010".ToArray();
        }
      
        //This function is called when the frontal lobe is selected
        private void FrontalLobe_Selected(object sender, RoutedEventArgs e)
        {
           // displayMessage = "Note the LED corresponding to the Frontal Lobe light up";
            lightingSequenceFromDatabase = "000000001".ToArray();
           
        }

        //This function determines what happens when the Go Button is Clicked
        //Depending on the selection, make affected areas glow and scale larger and smaller
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            
            if ( string.IsNullOrEmpty(selectedBrainPart) && string.IsNullOrEmpty(searchTextBox.Text))
            {

                MessageBox.Show("Select a brain part or healthy behavior before pressing the go button");
            }
            else
            {
                //Initialize the glowing animation
                glowAnimation.From = 1.0;
                glowAnimation.To = 0.4;
                glowAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
                glowAnimation.AutoReverse = true;
                glowAnimation.RepeatBehavior = RepeatBehavior.Forever;

                //Initialize the scalex animation
                growXAnimation.Duration = TimeSpan.FromMilliseconds(500);
                growXAnimation.From = 1;
                growXAnimation.To = 1.1;
                growXAnimation.AutoReverse = true;
                growXAnimation.RepeatBehavior = RepeatBehavior.Forever;

                //Initialize the scaley animation
                growYAnimation.Duration = TimeSpan.FromMilliseconds(500);
                growYAnimation.From = 1;
                growYAnimation.To = 1.1;
                growYAnimation.AutoReverse = true;
                growYAnimation.RepeatBehavior = RepeatBehavior.Forever;

                //
                storyboard.Children.Add(growXAnimation);
                storyboard.Children.Add(growYAnimation);
                Storyboard.SetTargetProperty(growXAnimation, new PropertyPath("RenderTransform.ScaleX"));
                Storyboard.SetTargetProperty(growYAnimation, new PropertyPath("RenderTransform.ScaleY"));

                //If brain part is selected show the name of part selected
                if (brainPart)
                {
                    selectionMessageBox.Text = selectedBrainPart + " was chosen. ";
                    brainPart = false;
                    //Don't need to look in database because hard coded 
                    LightingSequence();
                }
                //if the searchtextbox isn't empty
                if (!string.IsNullOrEmpty(searchTextBox.Text))
                {
                    brainPartsListBox.SelectedItem = false;
                    selectedHealthyBehaviors = searchTextBox.Text;
                    selectionMessageBox.Text = selectedHealthyBehaviors + " was chosen. " + displayMessage;
                    healthybehaviorBOOL = false;

                    SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);

                    try
                    {
                        string Query;
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
                        MessageBox.Show("Choose a option available from the drop down list ");
                    }
                    else
                    {
                        searchTextBox.Text = "";
                        resultsStack.Children.Clear();
                        border.Visibility = System.Windows.Visibility.Collapsed;
                        border.Background = Brushes.Transparent;
                        lightingSequenceFromDatabase = lightingSequenceString.ToArray();
                        LightingSequence();
                    }

                }
            }
        }

        //This function converts a string to character array
        public char[] StringToCharArray(string convertString)
        {
            char[] newCharArr = convertString.ToCharArray();
            return newCharArr;
        }

        //This function converts a character array to a string
        public string CharArrayToString(char[] convertArray)
        {
            string newString = new string(convertArray);
            return newString;
        }
        //This functions determines which parts of the brain on the app side to illuminate based on
        //selection of healthy behaviors
        private void LightingSequence()
        {
            if (lightingSequenceFromDatabase[0] == '1')
            {
                cerebellumImage.RenderTransform = scale;
                scale.ScaleX = 1.0;
                scale.ScaleY = 1.0;
                growXAnimation.From = 1;
                growXAnimation.To = 1.1;
                growYAnimation.From = 1;
                growYAnimation.To = 1.1;
                cerebellumImage.BeginAnimation(OpacityProperty, glowAnimation);
                
                storyboard.Stop();
              
                Storyboard.SetTarget(growXAnimation, cerebellumImage);
                Storyboard.SetTarget(growYAnimation, cerebellumImage);
    
                storyboard.Begin();
                storyboard.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.BeginTime);
                // Put the image currently being dragged on top of the others
                Canvas canvas = cerebellumImage.Parent as Canvas;
                int top = Canvas.GetZIndex(cerebellumImage);
                Canvas.SetZIndex(cerebellumImage, top + 1);
            }
            if (lightingSequenceFromDatabase[1] == '1')
            {
                brainstemImage.RenderTransform = scale;
                scale.ScaleX = 1.0;
                scale.ScaleY = 1.0;
                growXAnimation.From = 1;
                growXAnimation.To = 1.1;
                growYAnimation.From = 1;
                growYAnimation.To = 1.1;
                brainstemImage.BeginAnimation(OpacityProperty, glowAnimation);
                  
                storyboard.Stop();
        
                Storyboard.SetTarget(growXAnimation, brainstemImage);
                Storyboard.SetTarget(growYAnimation, brainstemImage);
                storyboard.Begin();
                storyboard.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.BeginTime);

                // Put the image currently being dragged on top of the others
                Canvas canvas = brainstemImage.Parent as Canvas;
                int top = Canvas.GetZIndex(brainstemImage);
                Canvas.SetZIndex(brainstemImage, top + 1);
            }
            if (lightingSequenceFromDatabase[2] == '1')
            {
                pituitaryGlandImage.RenderTransform = scale;
                scale.ScaleX = 1.0;
                scale.ScaleY = 1.0;
                growXAnimation.From = 1;
                growXAnimation.To = 1.5;
                growYAnimation.From = 1;
                growYAnimation.To = 1.5;
                pituitaryGlandImage.BeginAnimation(OpacityProperty, glowAnimation);
                         
                storyboard.Stop();        
                Storyboard.SetTarget(growXAnimation, pituitaryGlandImage);
                Storyboard.SetTarget(growYAnimation, pituitaryGlandImage);
                Storyboard.SetTargetProperty(growXAnimation, new PropertyPath("RenderTransform.ScaleX"));
                Storyboard.SetTargetProperty(growYAnimation, new PropertyPath("RenderTransform.ScaleY"));
                storyboard.Begin();
                storyboard.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.BeginTime);

                // Put the image currently being dragged on top of the others
                Canvas canvas = pituitaryGlandImage.Parent as Canvas;
                int top = Canvas.GetZIndex(pituitaryGlandImage);
                Canvas.SetZIndex(pituitaryGlandImage, top + 1);
            }
            if (lightingSequenceFromDatabase[3] == '1')
            {
                amygdalaImage.RenderTransform = scale;

                scale.ScaleX = 1.0;
                scale.ScaleY = 1.0;
                growXAnimation.From = 1;
                growXAnimation.To = 1.5;
                growYAnimation.From = 1;
                growYAnimation.To = 1.5;
                amygdalaImage.BeginAnimation(OpacityProperty, glowAnimation);
              
                storyboard.Stop();              
                Storyboard.SetTarget(growXAnimation, amygdalaImage);
                Storyboard.SetTarget(growYAnimation, amygdalaImage);
                storyboard.Begin();
                storyboard.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.BeginTime);

                // Put the image currently being dragged on top of the others
                Canvas canvas = amygdalaImage.Parent as Canvas;
                int top = Canvas.GetZIndex(amygdalaImage);
                Canvas.SetZIndex(amygdalaImage, top + 1);
            }
            if (lightingSequenceFromDatabase[4] == '1')
            {
                hippocampusImage.RenderTransform = scale;
                scale.ScaleX = 1.0;
                scale.ScaleY = 1.0;
                growXAnimation.From = 1;
                growXAnimation.To = 1.1;
                growYAnimation.From = 1;
                growYAnimation.To = 1.1;
                hippocampusImage.BeginAnimation(OpacityProperty, glowAnimation);
      
                
                storyboard.Stop();    
                Storyboard.SetTarget(growXAnimation, hippocampusImage);
                Storyboard.SetTarget(growYAnimation, hippocampusImage);      
                storyboard.Begin();
                storyboard.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.BeginTime);

                // Put the image currently being dragged on top of the others
                Canvas canvas = hippocampusImage.Parent as Canvas;
                int top = Canvas.GetZIndex(hippocampusImage);
                Canvas.SetZIndex(hippocampusImage, top + 1);
            }
            if (lightingSequenceFromDatabase[5] == '1')
            {
                temporalLobeImage.RenderTransform = scale;

                scale.ScaleX = 1.0;
                scale.ScaleY = 1.0;
                growXAnimation.From = 1;
                growXAnimation.To = 1.1;
                growYAnimation.From = 1;
                growYAnimation.To = 1.1;
                temporalLobeImage.BeginAnimation(OpacityProperty, glowAnimation);
               
                storyboard.Stop();    
                Storyboard.SetTarget(growXAnimation, temporalLobeImage);
                Storyboard.SetTarget(growYAnimation, temporalLobeImage);          
                storyboard.Begin();
                storyboard.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.BeginTime);


                // Put the image currently being dragged on top of the others
                Canvas canvas = temporalLobeImage.Parent as Canvas;
                int top = Canvas.GetZIndex(temporalLobeImage);
                Canvas.SetZIndex(temporalLobeImage, top + 1);
            }
            if (lightingSequenceFromDatabase[6] == '1')
            {
                occipitalLobeImage.RenderTransform = scale;

                scale.ScaleX = 1.0;
                scale.ScaleY = 1.0;
                growXAnimation.From = 1;
                growXAnimation.To = 1.1;
                growYAnimation.From = 1;
                growYAnimation.To = 1.1;
                occipitalLobeImage.BeginAnimation(OpacityProperty, glowAnimation);
                            
                storyboard.Stop();
                Storyboard.SetTarget(growXAnimation, occipitalLobeImage);
                Storyboard.SetTarget(growYAnimation, occipitalLobeImage);
                storyboard.Begin();
                storyboard.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.BeginTime);

                Canvas canvas = occipitalLobeImage.Parent as Canvas;
                int top = Canvas.GetZIndex(occipitalLobeImage);
                Canvas.SetZIndex(occipitalLobeImage, top + 1);
            }
            if (lightingSequenceFromDatabase[7] == '1')
            {
                parietalLobeImage.RenderTransform = scale;

                scale.ScaleX = 1.0;
                scale.ScaleY = 1.0;
                growXAnimation.From = 1;
                growXAnimation.To = 1.1;
                growYAnimation.From = 1;
                growYAnimation.To = 1.1;
                parietalLobeImage.BeginAnimation(OpacityProperty, glowAnimation);
                         
                storyboard.Stop();
                Storyboard.SetTarget(growXAnimation, parietalLobeImage);
                Storyboard.SetTarget(growYAnimation, parietalLobeImage);
                storyboard.Begin();
                storyboard.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.BeginTime);

                Canvas canvas = parietalLobeImage.Parent as Canvas;
                int top = Canvas.GetZIndex(parietalLobeImage);
                Canvas.SetZIndex(parietalLobeImage, top + 1);
            }
            if (lightingSequenceFromDatabase[8] == '1')
            {
                frontalLobeImage.RenderTransform = scale;

                scale.ScaleX = 1.0;
                scale.ScaleY = 1.0;
                growXAnimation.From = 1;
                growXAnimation.To = 1.1;
                growYAnimation.From = 1;
                growYAnimation.To = 1.1;
                frontalLobeImage.BeginAnimation(OpacityProperty, glowAnimation);

                storyboard.Stop();
                Storyboard.SetTarget(growXAnimation, frontalLobeImage);
                Storyboard.SetTarget(growYAnimation, frontalLobeImage);
                storyboard.Begin();
                storyboard.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.BeginTime);

                Canvas canvas = frontalLobeImage.Parent as Canvas;
                int top = Canvas.GetZIndex(frontalLobeImage);
                Canvas.SetZIndex(frontalLobeImage, top + 1);
            }
            WriteLightingSequenceMessage();
        }

        //This function is used to write to the serial port so the brain may know what 
        //parts to illuminate
        private void WriteLightingSequenceMessage()
        {

            //1. open serial port
            //2. Add raspberrypi identifier(character array)          
            //3. lightingSequenceFromDatabase 
            //4. concatenate all the parts in a character array
            //5. Send serial message for stop to turn off all the lights
            //6. Wait 20 milliseconds
            //7. Convert charArray as string, because zeros may be read as nulls
            //8. close serial port
            if (isConnected)
            {
                try
                {
                    SerialPort1.Open();
                    string id = "9999";
                    string ending = "n";
                    Console.WriteLine("Serial Port just opened");
                    // string concatenateArray = id + lightingSequenceFromDatabase.ToArray() + ending;

                    Console.WriteLine(CharArrayToString(lightingSequenceFromDatabase));
                    SerialPort1.Write(CharArrayToString(lightingSequenceFromDatabase));
                    Thread.Sleep(20);
                    SerialPort1.Close();
                }
                catch (UnauthorizedAccessException ex)
                {

                    MessageBox.Show( "Chosen COM Port in use, connect to another");
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wasn't able to successfully connect to Brain");
                    Console.WriteLine(ex.Message);
                }

            }
            else
            {               
                Console.WriteLine("Not connected to COM PORT for Brain");
            }

        }
        //This function gets the available serial ports
        //From the Microsoft C# Documentation
        //The port names are obtained from the system registry(for example, HKEY_LOCAL_MACHINE\HARDWARE\DEVICEMAP\SERIALCOMM). 
        //If the registry contains stale or otherwise incorrect data then the GetPortNames method will return incorrect data.

       void GetAvailableComPorts()
        {
            ports = SerialPort.GetPortNames();
            // remove duplicates, sort alphabetically and convert to array
            ports = ports.Distinct().OrderBy(s => s).ToArray();
            //https://dariosantarelli.wordpress.com/2010/10/18/c-how-to-programmatically-find-a-com-port-by-friendly-name/
            foreach (COMPortInfo comPort in COMPortInfo.GetCOMPortsInfo())

            {
                Console.WriteLine(string.Format("{0} – {1}", comPort.Name, comPort.Description));
            }
        }

        //This function handles if data is received through the serial port 
        //NOT USED 
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
        
        //This function is called when the user begins typing in the healthy behavior search box 
        private void SearchTextBox_KeyUp(object sender, KeyboardEventArgs e)
        {
            ListBoxSelectionChanged();
          //  bool found = false;
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
                    addItem(obj, "PeachPuff");
                    
                  //  found = true;
                }
                else
                {
                    addItem(obj, "White");
                }
            }
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e) {
            if (dropDownFlag == false)
            {
                border = (resultsStack.Parent as ScrollViewer).Parent as Border;
                border.Visibility = System.Windows.Visibility.Visible;
                border.Background = Brushes.White;
                // Clear the list   
                resultsStack.Children.Clear();

                Uri resourceUri = new Uri("Resources/if_arrow_sans_up_103304.png", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                // var brush = new ImageBrush();
                // brush.ImageSource = temp;

                dropDown.Source = temp;

                // Add the result   
                foreach (string obj in col)
                {

                    addItem(obj, "White");

                }
                dropDownFlag = true;
            }
            else {
                Uri resourceUri = new Uri("Resources/if_arrow-dropdown_3017945.png", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
               // var brush = new ImageBrush();
               // brush.ImageSource = temp;

                dropDown.Source = temp;
                border = (resultsStack.Parent as ScrollViewer).Parent as Border;
                border.Visibility = System.Windows.Visibility.Collapsed;
                dropDownFlag = false;
            }
        }
        //This function is used to add an item to the stackpanel holding 
        //the items of the healthy behaviors list
        private void addItem(string text, string color)
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
            if (color == "PeachPuff")
            {

                block.BringIntoView();
            }
        }

        //This function determines what happens when a selection is made from the healthy behaviors list
        private void ResultsStackMouseDown(object sender, MouseButtonEventArgs e)
        {
            resultsStack.Children.Clear();
            border.Visibility = System.Windows.Visibility.Collapsed;
            border.Background = Brushes.Transparent;

        }
        //This function handles what to do with received data
        //Never used
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
              
                else //If this function is being called other than during editing
                {
                    healthyBehaviorsListBox.Items.Clear();
                    query = "select * from HealthyBehaviors";
                  
                }
                SQLiteCommand createCommand = new SQLiteCommand(query, sqlitecon);
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string healthyBehaviorListBoxItemContent;
                    ListBoxItem li = new ListBoxItem();
                    if (editHealthyBehaviorsFlag) //This updates the editingListBox
                    {
                        healthyBehaviorListBoxItemContent = dr.GetString(1);
                        li.Content = healthyBehaviorListBoxItemContent;
                        editingListBox.Items.Add(li);
                        li.FontFamily = new FontFamily("Calibri");
                        li.FontSize = 16;
                    }
                    else  //This updates the editingListBox
                    {
                        healthyBehaviorListBoxItemContent = dr.GetString(1);
                        li.Content = healthyBehaviorListBoxItemContent;
                        healthyBehaviorsListBox.Items.Add(li);
                        li.FontSize = 20;
                        li.FontFamily = new FontFamily("Calibri");
                       
                    }
                }
                sqlitecon.Close();
                var list = editingListBox.Items.Cast<ListBoxItem>().OrderBy(item => item.Content).ToList();
                editingListBox.Items.Clear();
                foreach (ListBoxItem listItem in list)
                {
                    editingListBox.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Wasn't able to load Healthy Behaviors list");
                Console.WriteLine(ex);
            }
        }
        //This function updates indices of database table with each new remove,add or update
        private void UpdateIndices()
        {
            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
            int index = 0;
            int changingIndex = 0;
            while (changingIndex < editingListBox.Items.Count+1)
            {
                Console.WriteLine(changingIndex);  
                try
                {
                   // string query;
                    sqlitecon.Open();
                    if (editHealthyBehaviorsFlag)
                    {
                        query = "UPDATE HealthyBehaviors SET Id='" + changingIndex + "' WHERE healthyBehaviors='" + ((ListBoxItem)editingListBox.Items[index]).Content.ToString() + "' ";
                    }
                    SQLiteCommand createCommand = new SQLiteCommand(query, sqlitecon);
                    createCommand.ExecuteNonQuery();
                    sqlitecon.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("wasn't able to update selected healthy behavior");
                    Console.WriteLine(ex);
                }
                index++;
                changingIndex++;
            }
        }

        //This function is used to add to the database
        private void AddNewItemToDatabase()
        {
            ListBoxItem li = new ListBoxItem();
            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
            //string query;
            index = editingListBox.Items.Count ;
            col.Add(newListBoxItemContent);
            col = col.OrderBy(q => q).ToList();
            try
            {
                sqlitecon.Open();
                if (editHealthyBehaviorsFlag)
                {
                    query = "INSERT INTO HealthyBehaviors (Id, healthyBehaviors, lightingSequenceArray) VALUES ('" + index + "','" + newListBoxItemContent + "','" + lightingSequenceString.Substring(0, 9) + "')";
                    
                }
                SQLiteCommand createCommand = new SQLiteCommand(query, sqlitecon);
                createCommand.ExecuteNonQuery();
                sqlitecon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wasn't able to add new healthy behavior");
                Console.WriteLine(ex);
            }        
            UpdateIndices();
            Fill_ListBox();
        }
       
        //This function is used to remove an item from the database
        private void RemoveItemToDatabase()
        {
            ListBoxItem li = new ListBoxItem();
            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
            index = editingListBox.SelectedIndex;
            col.Remove(((ListBoxItem)editingListBox.SelectedItem).Content.ToString());
            col = col.OrderBy(q => q).ToList();
            //string query;
            try
            {
                sqlitecon.Open();
                if (editHealthyBehaviorsFlag)
                {
                    query = "DELETE FROM HealthyBehaviors WHERE Id='" + index + "' ";
                    Console.WriteLine(query);
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

        //This function was but could be used to update an exiting item in the healthy behaviors
        //list
        //NOT USED 
        private void UpdateItemToDatabase()
        {
            ListBoxItem li = new ListBoxItem();
            SQLiteConnection sqlitecon = new SQLiteConnection(dbConnectionString);
            index = editingListBox.SelectedIndex;
            //string query;
            try
            {
                sqlitecon.Open();
                if (editHealthyBehaviorsFlag)
                {
                    query = "update HealthyBehaviors set Id='" + index + "',healthyBehaviors='" + newListBoxItemContent + "',lightingSequenceArray'" + lightingSequenceString + "'where Id='" + index + "' ";
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
