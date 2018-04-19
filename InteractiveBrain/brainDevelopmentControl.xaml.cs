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

            brainDevelopmentText.Text = "Gray matter is the thin, folding, outer layer of the brain, known as the cortex. The cortex is where we form thoughts and memories. The amount of gray matter increases during childhood, peaks in adolescence, and then declines. Gray matter helps strengthen learning, and it declines as we age, pruning back connections to make the brain more efficient. ";
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


