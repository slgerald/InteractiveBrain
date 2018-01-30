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
using System.Collections.Concurrent;
using System.Windows.Media.Animation;

namespace InteractiveBrain
{
    /// <summary>
    /// Interaction logic for simulationUserControl.xaml
    /// </summary>
    public partial class simulationUserControl : UserControl
    {
        private static simulationUserControl _instance; //render userControl based on button pressed
        double buttonTopValue;
        public simulationUserControl()
        {
            InitializeComponent();

        }
        public static simulationUserControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new simulationUserControl();
                }
                return _instance;

            }
        }

        private void BrainDevelopmentButton_Click(object sender, RoutedEventArgs e)
        {
            buttonTopValue = Canvas.GetTop(brainDevelopmentButton);
            if (buttonPanel.Width != 45)
            {
                // Canvas.SetTop(pageMarker, buttonTopValue);
            }
            else
            {
                pageMarker.Visibility = System.Windows.Visibility.Hidden;
            }
            buttonPanel.Children.Clear();
            if (!buttonPanel.Children.Contains(brainDevelopmentControl.Instance))
            {


                buttonPanel.Children.Add(brainDevelopmentControl.Instance);
            }
            else
                buttonPanel.Children.Add(brainDevelopmentControl.Instance);
        }

        private void DopaminePathwaysButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HealthyActivitiesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NormalVsOnDrugsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
