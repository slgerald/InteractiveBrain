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
        string selectedSubstances;
        string selectedActivities;
        bool brainPart;
        bool activity;
        bool substance;
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
        }

        private void selectionMessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            if (brainPart)
            {
                selectionMessageBox.Text = selectedBrainPart + " was chosen. Illuminate " + selectedBrainPart + " on Brain";
                brainPart = false;
            }
            if (substance)
            {
                selectionMessageBox.Text = selectedSubstances + " was chosen. Illuminate " + selectedSubstances + " on Brain";
                brainPart = false;
            }
            if(activity)
            {
                selectionMessageBox.Text = selectedActivities + " was chosen. Illuminate " + ActivitiesSubstances + " on Brain";
                brainPart = false;
            }
        }

        private void brainPartsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            brainPart = true;
            substance = false;
            activity = false;
            selectedBrainPart = ((ListBoxItem)brainPartsListBox.SelectedItem).Content.ToString();
        }

        private void substancesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            substance = true;
            brainPart = false;
            activity = false;
            selectedSubstances = ((ListBoxItem)substancesListBox.SelectedItem).Content.ToString();
        }

        private void activitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activity = true;
            substance = false;
            brainPart = false;
            selectedActivities = ((ListBoxItem)activitiesListBox.SelectedItem).Content.ToString();
        }
    }
}
