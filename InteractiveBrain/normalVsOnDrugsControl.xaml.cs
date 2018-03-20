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
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class normalVsOnDrugsControl : UserControl
    {
        private static normalVsOnDrugsControl _instance; //render userControl based on button pressed

        public normalVsOnDrugsControl()
        {
            InitializeComponent();

            leftImage.Visibility = Visibility.Visible;
            rightImage.Visibility = Visibility.Visible;
            drugLabel.Visibility = Visibility.Visible;
            rehabLabel.Visibility = Visibility.Hidden;
            drugButton.Visibility = Visibility.Hidden;
            rehabButton.Visibility = Visibility.Visible;
            rehabImageRight1.Visibility = Visibility.Hidden;
            rehabImageRight2.Visibility = Visibility.Hidden;
            rehabImageLeft.Visibility = Visibility.Hidden;
            rehabSlider.Visibility = Visibility.Hidden;
        }

        public static normalVsOnDrugsControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new normalVsOnDrugsControl();
                }
                _instance = new normalVsOnDrugsControl();
                return _instance;

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String drugListValue = (drugList.SelectedItem as ComboBoxItem).Content.ToString();
            if (drugListValue == "Alcohol")
            {
                leftImage.Visibility = Visibility.Hidden;
                leftImage.Source = new BitmapImage(new Uri("Resources/non_alcoholic_brain.png", UriKind.Relative));
                leftImage.Visibility = Visibility.Visible;

                rightImage.Visibility = Visibility.Hidden;
                rightImage.Source = new BitmapImage(new Uri("Resources/alcoholic_brain.png", UriKind.Relative));
                rightImage.Visibility = Visibility.Visible;
            }
            else if (drugListValue == "Smoker")
            {
                leftImage.Visibility = Visibility.Hidden;
                leftImage.Source = new BitmapImage(new Uri("Resources/non_smoker_brain.png", UriKind.Relative));
                leftImage.Visibility = Visibility.Visible;

                rightImage.Visibility = Visibility.Hidden;
                rightImage.Source = new BitmapImage(new Uri("Resources/smoker_brain.png", UriKind.Relative));
                rightImage.Visibility = Visibility.Visible;
            }
            else if (drugListValue == "Cocaine")
            {
                leftImage.Visibility = Visibility.Hidden;
                leftImage.Source = new BitmapImage(new Uri("Resources/non_cocaine_brain.png", UriKind.Relative));
                leftImage.Visibility = Visibility.Visible;

                rightImage.Visibility = Visibility.Hidden;
                rightImage.Source = new BitmapImage(new Uri("Resources/cocaine_brain.png", UriKind.Relative));
                rightImage.Visibility = Visibility.Visible;
            }
        }

        private void Rehab_Button_Click(object sender, RoutedEventArgs e)
        {
            leftImage.Visibility = Visibility.Hidden;
            rightImage.Visibility = Visibility.Hidden;
            drugLabel.Visibility = Visibility.Hidden;
            rehabButton.Visibility = Visibility.Hidden;
            drugList.Visibility = Visibility.Hidden;

            rehabSlider.Visibility = Visibility.Visible;
            rehabImageRight1.Visibility = Visibility.Visible;
            rehabImageLeft.Visibility = Visibility.Visible;
            rehabImageLeft.Source = new BitmapImage(new Uri("Resources/normal_brain.png", UriKind.Relative));
            drugButton.Visibility = Visibility.Visible;
            rehabLabel.Visibility = Visibility.Visible;

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            if (rehabSlider.Value == 1)
            {
                rehabImageRight2.Visibility = Visibility.Hidden;
                rehabImageRight1.Visibility = Visibility.Visible;
            } else if (rehabSlider.Value == 2)
            {
                rehabImageRight1.Visibility = Visibility.Hidden;
                rehabImageRight2.Visibility = Visibility.Visible;
            }
     
        }

        private void drugButton_Click(object sender, RoutedEventArgs e)
        {
            leftImage.Visibility = Visibility.Visible;
            rightImage.Visibility = Visibility.Visible;
            drugLabel.Visibility = Visibility.Visible;
            rehabButton.Visibility = Visibility.Visible;
            drugList.Visibility = Visibility.Visible;

            rehabSlider.Visibility = Visibility.Hidden;
            rehabImageRight1.Visibility = Visibility.Hidden;
            rehabImageRight2.Visibility = Visibility.Hidden;
            rehabImageLeft.Visibility = Visibility.Hidden;
            drugButton.Visibility = Visibility.Hidden;
            rehabLabel.Visibility = Visibility.Hidden;
        }
    }
}

