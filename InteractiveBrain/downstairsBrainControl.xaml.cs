using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InteractiveBrain
{
    /// <summary>
    /// The High School User control is used for interaction with the downstairs brain
    /// </summary>
    /// 

    public partial class downstairsBrainControl : UserControl
    {
        private static downstairsBrainControl _instance; //render userControl based on button pressed

        //flags used to determine which radio button has been used 
        bool functionsFlag = false; //The functions flag used to ensure functions are listed in the facts message box
        bool healthyBehaviorsFlag = false; //The healthyBehaviorsFlag is used to ensure healthy behaviors are listed in the facts message box
        bool unhealthyBehaviorsFlag = false; //The unhealthybehaviorsflag is used to ensure unhealthy behaviors are listed in the facts message box

        //flags used to determine which radio button has been used on the editPopup
        bool editFunctionsFlag = false;
        bool editHealthyBehaviorsFlag = false;
        bool editUnhealthyBehaviorsFlag = false;

        private object movingObject; //The image being dragged and dropped 
        private double firstXPos, firstYPos;

        //used to save the original position of the brain needed for the rest button 
        //functionality
        double originalHippocampusX;
        double originalHippocampusY;
        double originalAmygdalaX;
        double originalAmygdalaY;
        double originalPituitaryGlandX;
        double originalPituitaryGlandY;
        double originalBrainstemX;
        double originalBrainstemY;

        Storyboard sbH; //Storyboard for the frontal lobe
        Storyboard sbA; //Storyboard for the temporal lobe
        Storyboard sbPG; // storyboard for the parietal lobe
        Storyboard sbBS; //storyboard for the occipital lobe 

        //1-5 are used to determine which storyboard should be affected by the action
        //sbH = 1, sbA = 2, sbPG = 3, sbBS = 4
        int storyboardFlag;

        string brainPart; //used to save the selection of the drop down list in editPopup

        bool disabled = false;//used to distinguish between single and double click in same function

        string changedFactsMessageBoxText;//used to save content of textbox in editPopup

        //Initialize texts for Downstairs Brain 
        string defaultFunctionsHippocampus = Properties.Settings.Default.defaultFunctionsHippocampus;
        string changedFunctionsHippocampus = null;
        bool defaultFunctionsHFlag = true;
        string defaultHealthyBehaviorsHippocampus = Properties.Settings.Default.defaultHealthyBehaviorsHippocampus;
        string changedHealthyBehaviorsHippocampus = null;
        bool defaultHBHFlag = true;
        string changedUnhealthyBehaviorsHippocampus = null;
        string defaultUnhealthyBehaviorsHippocampus = Properties.Settings.Default.defaultUnhealthyBehaviorsHippocampus;
        bool defaultUHBHFlag = true;

        string changedFunctionsAmygdala = null;
        string defaultFunctionsAmygdala = Properties.Settings.Default.defaultFunctionsAmygdala;
        bool defaultFunctionsAFlag = true;
        string changedHealthyBehaviorsAmygdala = null;
        string defaultHealthyBehaviorsAmygdala = Properties.Settings.Default.defaultHealthyBehaviorsAmygdala;
        bool defaultHBAFlag = true;
        string changedUnhealthyBehaviorsAmygdala = null;
        string defaultUnhealthyBehaviorsAmygdala = Properties.Settings.Default.defaultUnhealthyBehaviorsAmygdala;
        bool defaultUHBAFlag = true;

        string changedFunctionsPituitaryGland = null;
        string defaultFunctionsPituitaryGland = Properties.Settings.Default.defaultFunctionsPituitaryGland;
        bool defaultFunctionsPGFlag = true;
        string changedHealthyBehaviorsPituitaryGland = null;
        string defaultHealthyBehaviorsPituitaryGland = Properties.Settings.Default.defaultHealthyBehaviorsPituitaryGland;
        bool defaultHBPGFlag = true;
        string changedUnhealthyBehaviorsPituitaryGland = null;
        string defaultUnhealthyBehaviorsPituitaryGland = Properties.Settings.Default.defaultUnhealthyBehaviorsPituitaryGland;
        bool defaultUHBPGFlag = true;

        string changedFunctionsBrainstem = null;
        string defaultFunctionsBrainstem = Properties.Settings.Default.defaultFunctionsBrainstem;
        bool defaultFunctionsBSFlag = true;
        string changedHealthyBehaviorsBrainstem = null;
        string defaultHealthyBehaviorsBrainstem = Properties.Settings.Default.defaultHealthyBehaviorsBrainstem;
        bool defaultHBBSFlag = true;
        string changedUnhealthyBehaviorsBrainstem = null;
        string defaultUnhealthyBehaviorsBrainstem = Properties.Settings.Default.defaultUnhealthyBehaviorsBrainstem;
        bool defaultUHBBSFlag = true;

        //This is the entry point for the userControl page 
        //This function initializes the userControl with EventHandlers and other characteristics
        //EventHandlers routed to controls in xaml file
        //Bind animations for brain parts to instatiated storyboards in the xaml file 
        public downstairsBrainControl()
        {
            InitializeComponent();

            functions.IsChecked = true; //The page begins with the functions Radio Button being checked first 

            //Event handlers for the images and gotItButton
            hippocampusBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(HippocampusBox_PreviewMouseLeftButtonDown);
            hippocampusBox.PreviewMouseMove += new MouseEventHandler(Img_PreviewMouseMove);
            hippocampusBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Img_PreviewMouseLeftButtonUp);
            pituitaryGlandBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(PituitaryGlandBox_PreviewMouseLeftButtonDown);
            pituitaryGlandBox.PreviewMouseMove += new MouseEventHandler(Img_PreviewMouseMove);
            pituitaryGlandBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Img_PreviewMouseLeftButtonUp);

            brainstemBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(BrainstemBox_PreviewMouseLeftButtonDown);
            brainstemBox.PreviewMouseMove += new MouseEventHandler(Img_PreviewMouseMove);
            brainstemBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Img_PreviewMouseLeftButtonUp);

            amygdalaBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(AmygdalaBox_PreviewMouseLeftButtonDown);
            amygdalaBox.PreviewMouseMove += new MouseEventHandler(Img_PreviewMouseMove);
            amygdalaBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Img_PreviewMouseLeftButtonUp);

            originalHippocampusX = Canvas.GetLeft((UIElement)hippocampusBox);
            originalHippocampusY = Canvas.GetTop((UIElement)hippocampusBox);
            originalAmygdalaX = Canvas.GetLeft((UIElement)amygdalaBox);
            originalAmygdalaY = Canvas.GetTop((UIElement)amygdalaBox);
            originalPituitaryGlandX = Canvas.GetLeft((UIElement)pituitaryGlandBox);
            originalPituitaryGlandY = Canvas.GetTop((UIElement)pituitaryGlandBox);
            originalBrainstemX = Canvas.GetLeft((UIElement)brainstemBox);
            originalBrainstemY = Canvas.GetTop((UIElement)brainstemBox);

            //Binding the declared storyboards to storyboards in resources
            sbH = (Storyboard)Resources["zoomingH"];
            sbA = (Storyboard)Resources["zoomingA"];
            sbPG = (Storyboard)Resources["zoomingPG"];
            sbBS = (Storyboard)Resources["zoomingBS"];

            storyboardFlag = 0; // Initiates storyboard flag at zero
        }

        public static downstairsBrainControl Instance //This allows a new highSchool User Control to be instantiated 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new downstairsBrainControl();
                }
                _instance = new downstairsBrainControl();
                return _instance;

            }
        }
        //This function sets flags to true and false
        private void FunctionsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            functionsFlag = true;
            healthyBehaviorsFlag = false;
            unhealthyBehaviorsFlag = false;
        }
        //This function sets flags as true and false 
        private void HealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            healthyBehaviorsFlag = true;
            functionsFlag = false;
            unhealthyBehaviorsFlag = false;
        }
        //This function set flags as true and false 
        private void UnhealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            unhealthyBehaviorsFlag = true;
            functionsFlag = false;
            healthyBehaviorsFlag = false;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!editPopup.IsOpen)
            {
                editPopup.IsOpen = true;
            }
        }
        //This function saves the editingTextBox text into a variable to be saved 
        //as default setting later
        private void EditingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            changedFactsMessageBoxText = editingTextBox.Text;
        }
        //This function is to save the selected brain part from the drop down list in the 
        //editPopup into a string to determine the correct message to display when saving
        private void BrainPartComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                brainPart = ((ComboBoxItem)brainPartComboBox.SelectedItem).Content.ToString();
            }
            catch
            {

            }
        }
        //This function resets the editPopUp to its original state after its been closed
        private void CloseEditPopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 

            if (editPopup.IsOpen) { editPopup.IsOpen = false; }
            editingTextBox.Text = "";
            editFunctionsRadioButton.IsChecked = false;
            editFunctionsFlag = false;
            editHealthyBehaviorsRadioButton.IsChecked = false;
            editFunctionsFlag = false;
            editUnhealthyBehaviorsRadioButton.IsChecked = false;
            editUnhealthyBehaviorsFlag = false;
            brainPartComboBox.SelectedItem = null;
            editingMessageBlock.Text = "";

        }
        private void EditFunctionsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            editFunctionsFlag = true;
            editHealthyBehaviorsFlag = false;
            editUnhealthyBehaviorsFlag = false;
            if (brainPart != null)
            {
                switch (brainPart)
                {
                    case "Hippocampus":
                        editingTextBox.Text = defaultFunctionsHippocampus;
                        break;
                    case "Brainstem":
                        editingTextBox.Text = defaultFunctionsBrainstem;
                        break;
                    case "Pituitary Gland":
                        editingTextBox.Text = defaultFunctionsPituitaryGland;
                        break;
                    case "Amygdala":
                        editingTextBox.Text = defaultFunctionsAmygdala;
                        break;
                }
            }
        }

        private void EditHealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            editFunctionsFlag = false;
            editHealthyBehaviorsFlag = true;
            editUnhealthyBehaviorsFlag = false;
            if (brainPart != null)
            {
                switch (brainPart)
                {
                    case "Hippocampus":
                        editingTextBox.Text = defaultHealthyBehaviorsHippocampus;
                        break;
                    case "Brainstem":
                        editingTextBox.Text = defaultHealthyBehaviorsBrainstem;
                        break;
                    case "Pituitary Gland":
                        editingTextBox.Text = defaultHealthyBehaviorsPituitaryGland;
                        break;
                    case "Amygdala":
                        editingTextBox.Text = defaultHealthyBehaviorsAmygdala;
                        break;


                }
            }
        }
        private void EditUnhealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            editFunctionsFlag = false;
            editHealthyBehaviorsFlag = false;
            editUnhealthyBehaviorsFlag = true;
            if (brainPart != null)
            {
                switch (brainPart)
                {
                    case "Hippocampus":
                        editingTextBox.Text = defaultUnhealthyBehaviorsHippocampus;
                        break;
                    case "Amygdala":
                        editingTextBox.Text = defaultUnhealthyBehaviorsAmygdala;
                        break;
                    case "Pituitary Gland":
                        editingTextBox.Text = defaultUnhealthyBehaviorsPituitaryGland;
                        break;
                    case "Brainstem":
                        editingTextBox.Text = defaultUnhealthyBehaviorsBrainstem;
                        break;


                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (brainPart == null && !(editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
            {
                editingMessageBlock.Text = "Choose Brain Part and either functions, healthy behaviors and unhealthy behaviors";
            }
            if (brainPart == null && (editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
            {
                editingMessageBlock.Text = "Choose Brain Part";
            }

            if (brainPart != null && !(editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
            { editingMessageBlock.Text = "Choose either functions, healthy behaviors, and unhealthy behaviors"; }
            if (brainPart != null && (editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
            {

                if (editFunctionsFlag)
                {
                    switch (brainPart)
                    {
                        case "Hippocampus":
                            changedFunctionsHippocampus = changedFactsMessageBoxText;
                            defaultFunctionsHFlag = false;
                            break;
                        case "Amygdala":
                            changedFunctionsAmygdala = changedFactsMessageBoxText;
                            defaultFunctionsAFlag = false;
                            break;
                        case "Pituitary Gland":
                            changedFunctionsPituitaryGland = changedFactsMessageBoxText;
                            defaultFunctionsPGFlag = false;
                            break;
                        case "Brainstem":
                            changedFunctionsBrainstem = changedFactsMessageBoxText;
                            defaultFunctionsBSFlag = false;
                            break;
                    }
                }

                if (editHealthyBehaviorsFlag)
                {
                    switch (brainPart)
                    {
                        case "Hippocampus":

                            changedHealthyBehaviorsHippocampus = changedFactsMessageBoxText;
                            defaultHBHFlag = false;
                            break;
                        case "Amygdala":
                            changedHealthyBehaviorsAmygdala = changedFactsMessageBoxText;
                            defaultHBAFlag = false;
                            break;
                        case "Pituitary Gland":
                            changedHealthyBehaviorsPituitaryGland = changedFactsMessageBoxText;
                            defaultHBPGFlag = false;
                            break;
                        case "Brainstem":
                            changedHealthyBehaviorsBrainstem = changedFactsMessageBoxText;
                            defaultHBBSFlag = false;
                            break;

                    }
                }
                if (editUnhealthyBehaviorsFlag)
                {
                    switch (brainPart)
                    {
                        case "Hippocampus":
                            changedUnhealthyBehaviorsHippocampus = changedFactsMessageBoxText;
                            defaultUHBHFlag = false;
                            break;
                        case "Amygdala":
                            changedUnhealthyBehaviorsAmygdala = changedFactsMessageBoxText;
                            defaultUHBAFlag = false;
                            break;
                        case "Pituitary Gland":
                            changedUnhealthyBehaviorsPituitaryGland = changedFactsMessageBoxText;
                            defaultUHBPGFlag = false;
                            break;
                        case "Brainstem":
                            changedUnhealthyBehaviorsBrainstem = changedFactsMessageBoxText;
                            defaultUHBBSFlag = false;
                            break;

                    }

                }
            }
        }
            //This function determines what happens when the hippocampus is clicked on with the mouse's left button
            private void HippocampusBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                if (e.ClickCount == 1 && !disabled)
                {
                    brainPartsLabel.Text = "Hippocampus"; //Label for the brain part 
                                                          // In this event, we get the current mouse position on the control to use it in the MouseMove event.
                    System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                    Canvas canvas = img.Parent as Canvas;
                    firstXPos = e.GetPosition(img).X;
                    firstYPos = e.GetPosition(img).Y;
                    movingObject = sender;

                    // Put the image currently being dragged on top of the others
                    int top = Canvas.GetZIndex(img);
                    foreach (System.Windows.Controls.Image child in canvas.Children)
                        if (top < Canvas.GetZIndex(child))
                            top = Canvas.GetZIndex(child);
                    Canvas.SetZIndex(img, top + 1);
                    Mouse.Capture(img); //So the mouse pointer doesn't "slip" off of image while dragging and dropping 

                //The following determines what to be displayed in factsMessageBox based on
                //the selected radio button and whether or not the original content has been
                //changed from the default.
                //If the content has been changed from the default, display then save to become
                //the new default
                //Disable the radio buttons when content is being displayed for individual part
                if (functionsFlag)
                    {
                        if (defaultFunctionsHFlag)
                        {
                            factsMessageBox.Text = "Functions of the Hippocampus: " + defaultFunctionsHippocampus;
                        }
                        else
                        { 
                            factsMessageBox.Text = "Functions of the Hippocampus: " + changedFunctionsHippocampus;
                            defaultFunctionsHippocampus = changedFunctionsHippocampus;
                            Properties.Settings.Default.defaultFunctionsHippocampus = changedFunctionsHippocampus;
                            Properties.Settings.Default.Save();
                            defaultFunctionsHFlag = true;
                        }
                        healthyBehaviors.IsEnabled = false;
                        unhealthyBehaviors.IsEnabled = false;
                    }
                    if (healthyBehaviorsFlag)
                    {
                        if (defaultHBHFlag)
                        {
                            factsMessageBox.Text = "Healthy Behaviors that affect the Hippocampus: " + defaultHealthyBehaviorsHippocampus;
                        }
                        else
                        {
                            factsMessageBox.Text = "Healthy Behaviors that affect the Hippocampus: " + changedHealthyBehaviorsHippocampus;
                            Properties.Settings.Default.defaultHealthyBehaviorsHippocampus = changedHealthyBehaviorsHippocampus;
                            Properties.Settings.Default.Save();
                            defaultHBHFlag = true;
                        }
                        functions.IsEnabled = false;
                        unhealthyBehaviors.IsEnabled = false;
                    }
                    if (unhealthyBehaviorsFlag)
                    {
                        if (defaultUHBHFlag)
                        {
                            factsMessageBox.Text = "Unhealthy Behaviors that affect the Hippocampus : " + Properties.Settings.Default.defaultUnhealthyBehaviorsHippocampus;
                        }
                        else
                        {
                            factsMessageBox.Text = "Unhealthy Behaviors that affect the Hipocampus: " + changedUnhealthyBehaviorsHippocampus;
                            Properties.Settings.Default.defaultUnhealthyBehaviorsHippocampus = changedUnhealthyBehaviorsHippocampus;
                            Properties.Settings.Default.Save();
                            defaultUHBHFlag = true;
                        }
                        healthyBehaviors.IsEnabled = false;
                        functions.IsEnabled = false;
                    }
            }//Determines what happens when the image is clicked twice
            if (e.ClickCount == 2)
                {
                    storyboardFlag = 1;
                    //without reverseOn every other time autoreverse = true
                    if (sbH.AutoReverse)
                    {
                        ReverseOff(1);
                    }
                    else { ReverseOn(1); }
                //Hide other parts and make facts box and go button visible
                    hippocampusBox.SetValue(Canvas.LeftProperty, originalHippocampusX);
                    hippocampusBox.SetValue(Canvas.TopProperty, originalHippocampusY);
                    brainstemBox.Visibility = System.Windows.Visibility.Hidden;
                    amygdalaBox.Visibility = System.Windows.Visibility.Hidden;
                    pituitaryGlandBox.Visibility = System.Windows.Visibility.Hidden;
                    cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                    scrambleButton.Visibility = System.Windows.Visibility.Hidden;
                    resetButton.Visibility = System.Windows.Visibility.Hidden;
                    factsMessageBox.Visibility = System.Windows.Visibility.Visible;
                    gotItButton.Visibility = System.Windows.Visibility.Visible;
                    brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
                    disabled = true;
                }
            }

            //Determines what happens when the amygdala is clicked on 
            private void AmygdalaBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                if (e.ClickCount == 1 && !disabled)
                {
                    brainPartsLabel.Text = "Amygdala";
                    // In this event, we get the current mouse position on the control to use it in the MouseMove event.
                    System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                    Canvas canvas = img.Parent as Canvas;
                    firstXPos = e.GetPosition(img).X;
                    firstYPos = e.GetPosition(img).Y;
                    movingObject = sender;

                    // Put the image currently being dragged on top of the others
                    int top = Canvas.GetZIndex(img);
                    foreach (System.Windows.Controls.Image child in canvas.Children)
                        if (top < Canvas.GetZIndex(child))
                            top = Canvas.GetZIndex(child);
                    Canvas.SetZIndex(img, top + 1);
                    Mouse.Capture(img); //So the mouse pointer doesn't "slip" off image when being dragged and dropped 

                    //These if statements determine what to display in the textbox and disables whichever radio buttons are not being used 
                    if (functionsFlag)
                    {
                        if (defaultFunctionsAFlag)
                        {
                            factsMessageBox.Text = "Functions of the Amygdala: " + defaultFunctionsAmygdala;
                        }
                        else
                        {
                            factsMessageBox.Text = "Functions of the Amygdala: " + changedFunctionsAmygdala;
                            defaultFunctionsAmygdala = changedFunctionsAmygdala;
                            Properties.Settings.Default.defaultFunctionsAmygdala = changedFunctionsAmygdala;
                            Properties.Settings.Default.Save();
                            defaultFunctionsAFlag = true;
                        }
                        healthyBehaviors.IsEnabled = false;
                        unhealthyBehaviors.IsEnabled = false;
                    }
                    if (healthyBehaviorsFlag)
                    {
                        if (defaultHBAFlag)
                        {
                            factsMessageBox.Text = "Healthy Behaviors that affect the Amygdala: " + defaultHealthyBehaviorsAmygdala;
                        }
                        else
                        {
                            factsMessageBox.Text = "Healthy Behaviors that affect the Amygdala: " + changedHealthyBehaviorsAmygdala;
                            Properties.Settings.Default.defaultHealthyBehaviorsAmygdala = changedHealthyBehaviorsAmygdala;
                            Properties.Settings.Default.Save();
                            defaultHBAFlag = true;
                        }
                        functions.IsEnabled = false;
                        unhealthyBehaviors.IsEnabled = false;
                    }
                    if (unhealthyBehaviorsFlag)
                    {
                        if (defaultUHBAFlag)
                        {
                            factsMessageBox.Text = "Unhealthy Behaviors that affect the Amygdala: " + Properties.Settings.Default.defaultUnhealthyBehaviorsAmygdala;

                        }
                        else
                        {
                            factsMessageBox.Text = "Unhealthy Behaviors that affect the Amygdala: " + changedUnhealthyBehaviorsAmygdala;
                            Properties.Settings.Default.defaultUnhealthyBehaviorsAmygdala = changedUnhealthyBehaviorsAmygdala;
                            Properties.Settings.Default.Save();
                            defaultUHBAFlag = true;
                        }
                        healthyBehaviors.IsEnabled = false;
                        functions.IsEnabled = false;
                    }
                    //makes the textbox, "Got It" button, and brain parts label visible when this brain part is chosen
                }

                //This determines what happens when the amygdala is clicked twice 
                if (e.ClickCount == 2)
                {
                    storyboardFlag = 2;
                    //without reverseOn every other time autoreverse = true
                    if (sbA.AutoReverse)
                    { ReverseOff(2); }
                    else { ReverseOn(2); }
                    amygdalaBox.SetValue(Canvas.LeftProperty, originalAmygdalaX);
                    amygdalaBox.SetValue(Canvas.TopProperty, originalAmygdalaY);
                    brainstemBox.Visibility = System.Windows.Visibility.Hidden;
                    hippocampusBox.Visibility = System.Windows.Visibility.Hidden;
                    pituitaryGlandBox.Visibility = System.Windows.Visibility.Hidden;
                    cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                    scrambleButton.Visibility = System.Windows.Visibility.Hidden;
                    resetButton.Visibility = System.Windows.Visibility.Hidden;
                    factsMessageBox.Visibility = System.Windows.Visibility.Visible;
                    gotItButton.Visibility = System.Windows.Visibility.Visible;
                    brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
                    disabled = true;
                }
            }

            //This function determines what happens when the brainstem is left clicked 
            private void BrainstemBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                if (e.ClickCount == 1 && !disabled)
                {
                    brainPartsLabel.Text = "Brainstem";
                    // In this event, we get the current mouse position on the control to use it in the MouseMove event.
                    System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                    Canvas canvas = img.Parent as Canvas;
                    firstXPos = e.GetPosition(img).X;
                    firstYPos = e.GetPosition(img).Y;
                    movingObject = sender;

                    // Put the image currently being dragged on top of the others
                    int top = Canvas.GetZIndex(img);
                    foreach (System.Windows.Controls.Image child in canvas.Children)
                        if (top < Canvas.GetZIndex(child))
                            top = Canvas.GetZIndex(child);
                    Canvas.SetZIndex(img, top + 1);
                    Mouse.Capture(img);//So the mouse pointer doesn't "slip" off image when being dragged and dropped 

                    //These if statements determine what to display in the textbox and disables whichever radio buttons are not being used
                    if (functionsFlag)
                    {
                        if (defaultFunctionsBSFlag)
                        {
                            factsMessageBox.Text = "Functions of the Brainstem: " + defaultFunctionsBrainstem;
                        }
                        else
                        {
                            factsMessageBox.Text = "Functions of the Brainstem: " + changedFunctionsBrainstem;
                            defaultFunctionsBrainstem = changedFunctionsBrainstem;
                            Properties.Settings.Default.defaultFunctionsBrainstem = changedFunctionsBrainstem;
                            Properties.Settings.Default.Save();
                            defaultFunctionsBSFlag = true;
                        }
                        healthyBehaviors.IsEnabled = false;
                        unhealthyBehaviors.IsEnabled = false;
                    }
                    if (healthyBehaviorsFlag)
                    {
                        if (defaultHBBSFlag)
                        {
                            factsMessageBox.Text = "Healthy Behaviors that affect the Brainstem: " + defaultHealthyBehaviorsBrainstem;
                        }
                        else
                        {
                            factsMessageBox.Text = "Healthy Behaviors that affect the Brainstem: " + changedHealthyBehaviorsBrainstem;
                            Properties.Settings.Default.defaultHealthyBehaviorsBrainstem = changedHealthyBehaviorsBrainstem;
                            Properties.Settings.Default.Save();
                            defaultHBBSFlag = true;
                        }
                        functions.IsEnabled = false;
                        unhealthyBehaviors.IsEnabled = false;
                    }
                    if (unhealthyBehaviorsFlag)
                    {
                        if (defaultUHBBSFlag)
                        {
                            factsMessageBox.Text = "Unhealthy Behaviors that affect the Brainstem : " + Properties.Settings.Default.defaultUnhealthyBehaviorsBrainstem;

                        }
                        else
                        {
                            factsMessageBox.Text = "Unhealthy Behaviors that affect the Brainstem: " + changedUnhealthyBehaviorsBrainstem;
                            Properties.Settings.Default.defaultUnhealthyBehaviorsBrainstem = changedUnhealthyBehaviorsBrainstem;
                            Properties.Settings.Default.Save();
                            defaultUHBBSFlag = true;
                        }
                        healthyBehaviors.IsEnabled = false;
                        functions.IsEnabled = false;
                    }
                }
                if (e.ClickCount == 2) //What happens when the Brainstem is selected 
                {
                    storyboardFlag = 4;
                    //without reverseOn every other time autoreverse = true
                    if (sbBS.AutoReverse)
                    {
                        ReverseOff(4);
                    }
                    else { ReverseOn(4); }
                    brainstemBox.SetValue(Canvas.LeftProperty, originalBrainstemX);
                    brainstemBox.SetValue(Canvas.TopProperty, originalBrainstemY);

                    hippocampusBox.Visibility = System.Windows.Visibility.Hidden;
                    amygdalaBox.Visibility = System.Windows.Visibility.Hidden;
                    pituitaryGlandBox.Visibility = System.Windows.Visibility.Hidden;
                    cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                    resetButton.Visibility = System.Windows.Visibility.Hidden;
                    scrambleButton.Visibility = System.Windows.Visibility.Hidden;
                    factsMessageBox.Visibility = System.Windows.Visibility.Visible;
                    gotItButton.Visibility = System.Windows.Visibility.Visible;
                    brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
                    disabled = true;
                }

            }

            //This function determines what happens when the pituitary gland is left clicked 
            private void PituitaryGlandBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {

                if (e.ClickCount == 1 && !disabled)
                {
                    brainPartsLabel.Text = "Pituitary Gland";
                    // In this event, we get the current mouse position on the control to use it in the MouseMove event.
                    System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                    Canvas canvas = img.Parent as Canvas;
                    firstXPos = e.GetPosition(img).X;
                    firstYPos = e.GetPosition(img).Y;
                    movingObject = sender;

                    // Put the image currently being dragged on top of the others
                    int top = Canvas.GetZIndex(img);
                    foreach (System.Windows.Controls.Image child in canvas.Children)
                        if (top < Canvas.GetZIndex(child))
                            top = Canvas.GetZIndex(child);
                    Canvas.SetZIndex(img, top + 1);
                    Mouse.Capture(img);//So the mouse pointer doesn't "slip" off image when being dragged and dropped 

                    //These if statements determine what to display in the textbox and disables whichever radio buttons are not being used

                    if (functionsFlag)
                    {
                        if (defaultFunctionsPGFlag)
                        {
                            factsMessageBox.Text = "Functions of the Pituitary Gland: " + defaultFunctionsPituitaryGland;
                        }
                        else
                        {
                            factsMessageBox.Text = "Functions of the Pituitary Gland: " + changedFunctionsPituitaryGland;
                            defaultFunctionsPituitaryGland = changedFunctionsPituitaryGland;
                            Properties.Settings.Default.defaultFunctionsPituitaryGland = changedFunctionsPituitaryGland;
                            Properties.Settings.Default.Save();
                            defaultFunctionsPGFlag = true;
                        }
                        healthyBehaviors.IsEnabled = false;
                        unhealthyBehaviors.IsEnabled = false;
                    }
                    if (healthyBehaviorsFlag)
                    {
                        if (defaultHBPGFlag)
                        {
                            factsMessageBox.Text = "Healthy Behaviors that affect the Pituitary Gland: " + defaultHealthyBehaviorsPituitaryGland;
                        }
                        else
                        {
                            factsMessageBox.Text = "Healthy Behaviors that affect the Pituitary Gland: " + changedHealthyBehaviorsPituitaryGland;
                            Properties.Settings.Default.defaultHealthyBehaviorsPituitaryGland = changedHealthyBehaviorsPituitaryGland;
                            Properties.Settings.Default.Save();
                            defaultHBPGFlag = true;
                        }
                        functions.IsEnabled = false;
                        unhealthyBehaviors.IsEnabled = false;
                    }
                    if (unhealthyBehaviorsFlag)
                    {
                        if (defaultUHBPGFlag)
                        {
                            factsMessageBox.Text = "Unhealthy Behaviors that affect the Pituitary Gland : " + Properties.Settings.Default.defaultUnhealthyBehaviorsPituitaryGland;

                        }
                        else
                        {
                            factsMessageBox.Text = "Unhealthy Behaviors that affect the Pituitary Gland: " + changedUnhealthyBehaviorsPituitaryGland;
                            Properties.Settings.Default.defaultUnhealthyBehaviorsPituitaryGland = changedUnhealthyBehaviorsPituitaryGland;
                            Properties.Settings.Default.Save();
                            defaultUHBPGFlag = true;
                        }
                        healthyBehaviors.IsEnabled = false;
                        functions.IsEnabled = false;
                    }


                }
                if (e.ClickCount == 2) //What happens when the pituitary gland is double clicked 
                {
                    storyboardFlag = 3;
                    //without reverseOn every other time autoreverse = true
                    if (sbPG.AutoReverse)
                    { ReverseOff(3); }
                    else { ReverseOn(3); }
                    pituitaryGlandBox.SetValue(Canvas.LeftProperty, originalPituitaryGlandX);
                    pituitaryGlandBox.SetValue(Canvas.TopProperty, originalPituitaryGlandY);
                    brainstemBox.Visibility = System.Windows.Visibility.Hidden;
                    amygdalaBox.Visibility = System.Windows.Visibility.Hidden;
                    hippocampusBox.Visibility = System.Windows.Visibility.Hidden;
                    cutoutLowerBrain.Visibility = System.Windows.Visibility.Hidden;
                    resetButton.Visibility = System.Windows.Visibility.Hidden;
                    scrambleButton.Visibility = System.Windows.Visibility.Hidden;
                    factsMessageBox.Visibility = System.Windows.Visibility.Visible;
                    gotItButton.Visibility = System.Windows.Visibility.Visible;
                    brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
                    disabled = true;
                }
            }

            //All images use this functions as the "drop" part of their drag and drop operation
            private void Img_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                Canvas canvas = img.Parent as Canvas;
                Mouse.OverrideCursor = null;
                movingObject = null;

                // Put the image currently being dragged on top of the others
                int top = Canvas.GetZIndex(img);
                foreach (System.Windows.Controls.Image child in canvas.Children)
                    if (top > Canvas.GetZIndex(child))
                        top = Canvas.GetZIndex(child);
                Canvas.SetZIndex(img, top + 1);
                Mouse.Capture(null);
            }

            //All images use this function for their "drag" part of their drag and drop operation
            private void Img_PreviewMouseMove(object sender, MouseEventArgs e)
            {
                Mouse.OverrideCursor = null;
                if (e.LeftButton == MouseButtonState.Pressed && sender == movingObject)
                {
                    System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                    Canvas canvas = img.Parent as Canvas;
                    Mouse.OverrideCursor = null;
                    double newLeft = e.GetPosition(canvas).X - firstXPos - canvas.Margin.Left;
                    // newLeft inside canvas right-border?
                    if (newLeft > canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth)
                    {
                        newLeft = canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth;
                        Mouse.OverrideCursor = Cursors.No;
                    }
                    // newLeft inside canvas left-border?
                    else if (newLeft < canvas.Margin.Left)
                    {
                        newLeft = canvas.Margin.Left;
                        Mouse.OverrideCursor = Cursors.No;
                    }
                    img.SetValue(Canvas.LeftProperty, newLeft);

                    double newTop = e.GetPosition(canvas).Y - firstYPos - canvas.Margin.Top;
                    // newTop inside canvas bottom-border?
                    if (newTop > canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight)
                    {
                        newTop = canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight;
                        Mouse.OverrideCursor = Cursors.No;
                    }
                    // newTop inside canvas top-border?
                    else if (newTop < canvas.Margin.Top)
                    {
                        newTop = canvas.Margin.Top;
                        Mouse.OverrideCursor = Cursors.No;
                    }
                    img.SetValue(Canvas.TopProperty, newTop);
                }
            }
        //This function is to reset brain parts to their original position
        private void ResetImages(object sender, RoutedEventArgs e)
            {

                amygdalaBox.SetValue(Canvas.LeftProperty, originalAmygdalaX);
                amygdalaBox.SetValue(Canvas.TopProperty, originalAmygdalaY);
                pituitaryGlandBox.SetValue(Canvas.LeftProperty, originalPituitaryGlandX);
                pituitaryGlandBox.SetValue(Canvas.TopProperty, originalPituitaryGlandY);
                brainstemBox.SetValue(Canvas.LeftProperty, originalBrainstemX);
                brainstemBox.SetValue(Canvas.TopProperty, originalBrainstemY);
                hippocampusBox.SetValue(Canvas.LeftProperty, originalHippocampusX);
                hippocampusBox.SetValue(Canvas.TopProperty, originalHippocampusY);

            }
        //This function is to randowmly place different brain parts within pictureCanvas
        private void ScrambleButton_Click(object sender, RoutedEventArgs e)
            {
                Random rnd = new Random();
                amygdalaBox.SetValue(Canvas.LeftProperty, Convert.ToDouble(rnd.Next(0, 660 - Convert.ToInt32(amygdalaBox.ActualWidth))));
                amygdalaBox.SetValue(Canvas.TopProperty, Convert.ToDouble(rnd.Next(0, 375 - Convert.ToInt32(amygdalaBox.ActualHeight))));
                pituitaryGlandBox.SetValue(Canvas.LeftProperty, Convert.ToDouble(rnd.Next(0, 660 - Convert.ToInt32(pituitaryGlandBox.ActualWidth))));
                pituitaryGlandBox.SetValue(Canvas.TopProperty, Convert.ToDouble(rnd.Next(0, 375 - Convert.ToInt32(pituitaryGlandBox.ActualHeight))));
                brainstemBox.SetValue(Canvas.LeftProperty, Convert.ToDouble(rnd.Next(0, 660 - Convert.ToInt32(brainstemBox.ActualWidth))));
                brainstemBox.SetValue(Canvas.TopProperty, Convert.ToDouble(rnd.Next(0, 375 - Convert.ToInt32(brainstemBox.ActualHeight))));
                hippocampusBox.SetValue(Canvas.LeftProperty, Convert.ToDouble(rnd.Next(0, 660 - Convert.ToInt32(hippocampusBox.ActualWidth))));
                hippocampusBox.SetValue(Canvas.TopProperty, Convert.ToDouble(rnd.Next(0, 375 - Convert.ToInt32(hippocampusBox.ActualHeight))));



            }

        //Depending on the brain part
        //This functions sets the autoreverse attribute 
        //of that brain part's storyboard false
        private void ReverseOff(int storyboardFlag)
        {
            switch (storyboardFlag)
            {
                case 1:
                    sbH.AutoReverse = false;
                    break;
                case 2:
                    sbA.AutoReverse = false;
                    break;
                case 3:
                    sbPG.AutoReverse = false;
                    break;
                case 4:
                    sbBS.AutoReverse = false;
                    break;

            }
        }
        //Depending on the brain part
        //This functions sets the autoreverse attribute 
        //of that brain part's storyboard true
        private void ReverseOn(int storyboardFlag)
        {
            switch (storyboardFlag)
            {
                case 1:
                    sbH.Begin(this, true);
                    sbH.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbH.AutoReverse = true;
                    break;
                case 2:
                    sbA.Begin(this, true);
                    sbA.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbA.AutoReverse = true;
                    break;
                case 3:
                    sbPG.Begin(this, true);
                    sbPG.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbPG.AutoReverse = true;
                    break;
                case 4:
                    sbBS.Begin(this, true);
                    sbBS.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbBS.AutoReverse = true;
                    break;
            }
        }
        //This function helps ensure the correct AutoReverse attribute of the selected brain part
        //Which brain parts to make visible based on brain part selection
        //Make the gotItbutton, facts message box, and brain parts label hidden
        private void GotItButton_Click(object sender, RoutedEventArgs e)
            {
                //sbH = 1, sbTL = 2, sbPL = 3, sbOL = 4, 
                //Safeguard against incorrect AutoREverse attribute of selected brain part 
                if (sbH.AutoReverse)
                { ReverseOn(1); }
                else { ReverseOff(1); }
                if (sbA.AutoReverse)
                { ReverseOn(2); }
                else { ReverseOff(2); }
                if (sbPG.AutoReverse)
                { ReverseOn(3); }
                else { ReverseOff(3); }
                if (sbBS.AutoReverse)
                { ReverseOn(4); }
                else { ReverseOff(4); }


                //Based on the brain part selected, determines which parts
                //should become visible again
                switch (storyboardFlag)
                {
                    case 1:
                        pituitaryGlandBox.Visibility = System.Windows.Visibility.Visible;
                        amygdalaBox.Visibility = System.Windows.Visibility.Visible;
                        brainstemBox.Visibility = System.Windows.Visibility.Visible;

                        break;
                    case 2:
                        pituitaryGlandBox.Visibility = System.Windows.Visibility.Visible;
                        hippocampusBox.Visibility = System.Windows.Visibility.Visible;
                        brainstemBox.Visibility = System.Windows.Visibility.Visible;

                        break;
                    case 3:
                        hippocampusBox.Visibility = System.Windows.Visibility.Visible;
                        brainstemBox.Visibility = System.Windows.Visibility.Visible;
                        amygdalaBox.Visibility = System.Windows.Visibility.Visible;

                        break;
                    case 4:
                        pituitaryGlandBox.Visibility = System.Windows.Visibility.Visible;
                        hippocampusBox.Visibility = System.Windows.Visibility.Visible;
                        amygdalaBox.Visibility = System.Windows.Visibility.Visible;

                        break;

                }
                cutoutLowerBrain.Visibility = System.Windows.Visibility.Visible;
                resetButton.Visibility = System.Windows.Visibility.Visible;
                scrambleButton.Visibility = System.Windows.Visibility.Visible;
                factsMessageBox.Visibility = System.Windows.Visibility.Hidden;
                gotItButton.Visibility = System.Windows.Visibility.Hidden;
                brainPartsLabel.Visibility = System.Windows.Visibility.Hidden;

                //Safegaurd against incorrect AutoReverse attribute of selected brain part 
                if (!sbH.AutoReverse && storyboardFlag == 1)
                { ReverseOn(1); }
                else { ReverseOff(1); }
                if (!sbA.AutoReverse && storyboardFlag == 2)
                { ReverseOn(2); }
                else { ReverseOff(2); }
                if (!sbPG.AutoReverse && storyboardFlag == 3)
                { ReverseOn(3); }
                else { ReverseOff(3); }
                if (!sbBS.AutoReverse && storyboardFlag == 4)
                { ReverseOn(4); }
                else { ReverseOff(4); }


                if (functionsFlag)
                {
                    healthyBehaviors.IsEnabled = true;
                    unhealthyBehaviors.IsEnabled = true;
                }

                if (healthyBehaviorsFlag)
                {
                    functions.IsEnabled = true;
                    unhealthyBehaviors.IsEnabled = true;
                }
                if (unhealthyBehaviorsFlag)
                {

                    healthyBehaviors.IsEnabled = true;
                    functions.IsEnabled = true;
                }
                disabled = false;
                storyboardFlag = 0;
            }
            
    }
}
