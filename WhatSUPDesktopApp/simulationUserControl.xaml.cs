using System.Windows;
using System.Windows.Controls;

namespace WhatSUPDesktopApp
{
    /// <summary>
    /// The simulationUserControl page is used as a hub to get to the 4 animations pages:
    /// brainDevelopment, dopaminePathways, healthyActivities, and normalVsOnDrugs.
    /// </summary>
    public partial class SimulationUserControl : UserControl
    {
        //render userControl based on button pressed
        private static SimulationUserControl _instance; 

        //Used to get location value of buttons 
        double buttonTopValue;

        //This method serves as the entry point to the simulationUserControl page.
        public SimulationUserControl()
        {
            InitializeComponent();

        }

        //This method allows the simulationUserControl to be rendered when called.
        public static SimulationUserControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SimulationUserControl();
                }
                _instance = new SimulationUserControl();
                return _instance;

            }
        }

        //This method loads the page for the brainDevelopmentControl when the 
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
            if (!buttonPanel.Children.Contains(BrainDevelopmentControl.Instance))
            {


                buttonPanel.Children.Add(BrainDevelopmentControl.Instance);
            }
            else
                buttonPanel.Children.Add(BrainDevelopmentControl.Instance);
        }

        //This method loads the page for the dopaminePathwaysControl when the 
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
            if (!buttonPanel.Children.Contains(DopaminePathwaysControl.Instance))
            {


                buttonPanel.Children.Add(DopaminePathwaysControl.Instance);
            }
            else
                buttonPanel.Children.Add(DopaminePathwaysControl.Instance);
        }

        //This method loads the page for the healthyActivitiesControl when the 
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
            if (!buttonPanel.Children.Contains(HealthyActivitiesControl.Instance))
            {


                buttonPanel.Children.Add(HealthyActivitiesControl.Instance);
            }
            else
                buttonPanel.Children.Add(HealthyActivitiesControl.Instance);
        }

        //This method loads the page for the normalVsOnDrugsControl when the 
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
            if (!buttonPanel.Children.Contains(NormalVsOnDrugsControl.Instance))
            {


                buttonPanel.Children.Add(NormalVsOnDrugsControl.Instance);
            }
            else
                buttonPanel.Children.Add(NormalVsOnDrugsControl.Instance);
        }

        //This button loads the simulationUserControl page when clicked. It appears
        //at the bottom of the brainDevelopment, dopaminePathways, healthyActivities, and
        //normalVsOnDrugs pages.
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            buttonPanel.Children.Clear();
            buttonPanel.Children.Add(SimulationUserControl.Instance);
        }

    }
 }
