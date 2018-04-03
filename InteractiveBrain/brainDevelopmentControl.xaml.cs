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

namespace InteractiveBrain
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class brainDevelopmentControl : UserControl
    {

        private static brainDevelopmentControl _instance; //render userControl based on button pressed
        public brainDevelopmentControl()
        {
            InitializeComponent();

            brainDevelopmentText.Text = "Gray matter thins from child age to young adult age in order to 'prune' inefficient synaptic connections. This increases overall brain performance and cognitive function.";
        }

        public static brainDevelopmentControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new brainDevelopmentControl();
                }
                _instance = new brainDevelopmentControl();
                return _instance;

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (slValue.Value == 5)
            {
                brainDevelopmentImage.Source = new BitmapImage(new Uri("Resources/gray_matter_5_years.png", UriKind.Relative));
            }
            else if (slValue.Value == 8.75)
            {
                brainDevelopmentImage.Source = new BitmapImage(new Uri("Resources/gray_matter_early_teen.png", UriKind.Relative));
            }
            else if (slValue.Value == 12.5)
            {
                brainDevelopmentImage.Source = new BitmapImage(new Uri("Resources/gray_matter_teen.png", UriKind.Relative));
            }
            else if (slValue.Value == 16.25)
            {
                brainDevelopmentImage.Source = new BitmapImage(new Uri("Resources/gray_matter_late_teen.png", UriKind.Relative));
            }
            else if (slValue.Value == 20)
            {
                brainDevelopmentImage.Source = new BitmapImage(new Uri("Resources/gray_matter_young_adult.png", UriKind.Relative));
            }

        }

    }
}


