using System;
using System.Collections.Generic;
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

namespace LEDPracticeAppWPFV1._0._1
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
        public static interactiveBrainControl Instance

        {
            get
            {
                if (_instance == null)
                {
                    _instance = new interactiveBrainControl();
                }
                    return _instance;
               
            }
        }
        public interactiveBrainControl()
        {
            InitializeComponent();
            brainPart = false;
            activity = false;
            substance = false;
            // animation.From = 1.0;
            // animation.To = 0.5;
            // animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            //// animation.AutoReverse = true;
            // animation.RepeatBehavior = RepeatBehavior.Forever;
            // img.BeginAnimation(OpacityProperty,animation);
            
            
        }

        private void selectionMessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            if (brainPart)
            {
                selectionMessageBox.Text = selectedBrainPart + " was chosen. " + displayMessage;
                activitiesListBox.SelectedItem = false;
                substancesListBox.SelectedItem = false;
                brainPart = false;
                animation.From = 1.0;
                animation.To = 0.5;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
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
                selectionMessageBox.Text = selectedSubstances + " was chosen. " + displayMessage;
                substance = false;
            }
            if(activity)
            {
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
        }

        private void Amygdala_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Amygdala light up";
        }
        private void pituitaryGland_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Pituitary Gland light up";
        }
        private void hippocampus_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Hippocampus light up";
        }
        private void cerebellum_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Cerebellum light up";
        }
        private void parietalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Parietal Lobe light up";
        }
        private void temporalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Temporal Lobe light up";
        }
        private void frontalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Frontal Lobe light up";
        }
        private void occipitalLobe_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "Note the LED corresponding to the Occipital Lobe light up";
        }

        private void stimulants_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
        }
        private void depressants_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
        }
        private void hallucinogens_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
        }

        private void dietAndNutrition_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
        }

        private void healthAndExercise_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
        }

        private void cognitiveActivity_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
        }

        private void socialEngagement_Selected(object sender, RoutedEventArgs e)
        {
            displayMessage = "This selection will be programmed next semester";
        }
    }
}
