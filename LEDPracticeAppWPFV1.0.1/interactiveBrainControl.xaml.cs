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
        }

        private void selectionMessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            selectionMessageBox.Text = selectedBrainPart + " was chosen. Illuminate " + selectedBrainPart + " on Brain";
        }

        private void brainPartsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            selectedBrainPart = ((ListBoxItem)brainPartsListBox.SelectedItem).Content.ToString();
        }
    }
}
