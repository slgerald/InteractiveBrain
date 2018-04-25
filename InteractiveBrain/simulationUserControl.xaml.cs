using System.Windows;
using System.Windows.Controls;

namespace InteractiveBrain
{
    /// <summary>
    /// The simulation userControl page is used as a hub to get to the 4 animations pages:
    /// brainDevelopment, dopaminePathways, healthyActivities, and normalVsOnDrugs.
    /// </summary>
    public partial class simulationUserControl : UserControl
    {
        //render userControl based on button pressed
        private static simulationUserControl _instance; 

        //Used to get location value of buttons 
        double buttonTopValue;

        //This method serves as the entry point to the simulation userControl page.
        public simulationUserControl()
        {
            InitializeComponent();

        }

        //This method allows the simulation usrControl to be rendered when called.
        public static simulationUserControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new simulationUserControl();
                }
                _instance = new simulationUserControl();
                return _instance;

            }
        }

        //This method loads the page for the brainDevelopment userControl when the 
        //'Brain Development' button is clicked.
        private void BrainDevelopmentButton_Click(object sender, RoutedEventArgs e)
        {
            buttonTopValue = Canvas.GetTop(brainDevelopmentButton);
            if (buttonPanel.Width != 45)
            {
                // Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                //pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            buttonPanel.Children.Clear();
            if (!buttonPanel.Children.Contains(brainDevelopmentControl.Instance))
            {


                buttonPanel.Children.Add(brainDevelopmentControl.Instance);
            }
            else
                buttonPanel.Children.Add(brainDevelopmentControl.Instance);
        }

        //This method loads the page for the dopaminePathways userControl when the 
        //'Dopamine Pathways' button is clicked.
        private void DopaminePathwaysButton_Click(object sender, RoutedEventArgs e)
        {
            buttonTopValue = Canvas.GetTop(dopaminePathwaysButton);
            if (buttonPanel.Width != 45)
            {
                // Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                //pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            buttonPanel.Children.Clear();
            if (!buttonPanel.Children.Contains(dopaminePathwaysControl.Instance))
            {


                buttonPanel.Children.Add(dopaminePathwaysControl.Instance);
            }
            else
                buttonPanel.Children.Add(dopaminePathwaysControl.Instance);
        }

        //This method loads the page for the healthyActivities userControl when the 
        //'Healthy Activities button is clicked.
        private void HealthyActivitiesButton_Click(object sender, RoutedEventArgs e)
        {
            buttonTopValue = Canvas.GetTop(healthyActivitiesButton);
            if (buttonPanel.Width != 45)
            {
                // Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                //pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            buttonPanel.Children.Clear();
            if (!buttonPanel.Children.Contains(healthyActivitiesControl.Instance))
            {


                buttonPanel.Children.Add(healthyActivitiesControl.Instance);
            }
            else
                buttonPanel.Children.Add(healthyActivitiesControl.Instance);
        }

        //This method loads the page for the normalVsOnDrugs userControl when the 
        //'Typical vs. Drug Use' button is clicked.
        private void NormalVsOnDrugsButton_Click(object sender, RoutedEventArgs e)
        {
            buttonTopValue = Canvas.GetTop(normalVsOnDrugsButton);
            if (buttonPanel.Width != 45)
            {
                // Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                //pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            buttonPanel.Children.Clear();
            if (!buttonPanel.Children.Contains(normalVsOnDrugsControl.Instance))
            {


                buttonPanel.Children.Add(normalVsOnDrugsControl.Instance);
            }
            else
                buttonPanel.Children.Add(normalVsOnDrugsControl.Instance);
        }

        //This button loads the simulation userControl page when clicked. It appears
        //at the bottom of the brainDevelopment, dopaminePathways, healthyActivities, and
        //normalVsOnDrugs pages.
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //buttonPanel.Children.Remove(brainDevelopmentControl.Instance);
            buttonPanel.Children.Clear();
            buttonPanel.Children.Add(simulationUserControl.Instance);


        }

    }
    }
