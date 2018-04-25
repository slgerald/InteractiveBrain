using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InteractiveBrain
{
    /// <summary>
    /// The normalVsOnDrugs userControl is used to show the user examples of the effects
    /// of substance abuse on the brain as well as the damage that remains even after rehab.
    /// </summary>
    public partial class normalVsOnDrugsControl : UserControl
    {
        //render userControl based on button pressed
        private static normalVsOnDrugsControl _instance; 

        //This method serves as the entry point to the normalVsOnDrugs userControl page.
        //This method also loads the default pictures and description text for an example of
        //the effects substance abuse on the brain.
        public normalVsOnDrugsControl()
        {
            InitializeComponent();


            //Can set Initial visibility in .xaml file
            drugList.SelectedIndex = 0;
            sourceLabel.Visibility = Visibility.Visible;
            drugButton.Visibility = Visibility.Hidden;
            rehabButton.Visibility = Visibility.Visible;
            rehabSlider.Visibility = Visibility.Hidden;
            leftImageLabel.Content = "Typical Brain";
            rightImageLabel.Content = "Heavy Alcohol Use";
            sourceLabel.Content = "Source: http://www.encognitive.com/files/images/brain-scan-alcoholic-drug-addict-obese-normal.preview.jpg";
            rehabText.Text = "Drugs can alter the way people think, feel, and behave by disrupting neurotransmission, the process of communication between neurons.";
        }

        //This method allows the normalVsOnDrugs userControl to be rendered when called.
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

        //This method loads the substance abuse example the user chooses through a combobox list.
        //This includes the pictures along with the description text and labels for those pictures.
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String drugListValue = (drugList.SelectedItem as ComboBoxItem).Content.ToString();
            if (drugListValue == "Alcohol")
            {
                leftImage.Visibility = Visibility.Hidden;
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));
                leftImage.Visibility = Visibility.Visible;

                rightImage.Visibility = Visibility.Hidden;
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_alcoholic.png", UriKind.Relative));
                rightImage.Visibility = Visibility.Visible;

                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Heavy Alcohol Use";
                sourceLabel.Content = "Source: http://www.encognitive.com/files/images/brain-scan-alcoholic-drug-addict-obese-normal.preview.jpg";
            }
            else if (drugListValue == "Smoker")
            {
                leftImage.Visibility = Visibility.Hidden;
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_non_smoker.png", UriKind.Relative));
                leftImage.Visibility = Visibility.Visible;

                rightImage.Visibility = Visibility.Hidden;
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_smoker.png", UriKind.Relative));
                rightImage.Visibility = Visibility.Visible;

                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Regular Nicotine Use";
                sourceLabel.Content = "Source: http://selfchec.org/main/wp-content/uploads/2010/04/brain.jpg";
            }
            else if (drugListValue == "Cocaine")
            {
                leftImage.Visibility = Visibility.Hidden;
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));
                leftImage.Visibility = Visibility.Visible;

                rightImage.Visibility = Visibility.Hidden;
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_cocaine.png", UriKind.Relative));
                rightImage.Visibility = Visibility.Visible;

                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Cocaine Use";
                sourceLabel.Content = "Source: http://www.encognitive.com/files/images/brain-scan-alcoholic-drug-addict-obese-normal.preview.jpg";
            }
        }

        //This method clears the current page and loads the content for the lasting 
        //effects of substance abuse even after rehab when the 'Rehab' button is clicked. 
        //This includes pictures, a slider to switch between pictures, 
        //and the description text for those pictures.
        private void Rehab_Button_Click(object sender, RoutedEventArgs e)
        {
            drugList.Visibility = Visibility.Hidden;
            rehabButton.Visibility = Visibility.Hidden;

            rehabSlider.Visibility = Visibility.Visible;
            drugButton.Visibility = Visibility.Visible;
            sourceLabel.Content = "Source: The Journal of Neuroscience, 21(23):9414-9418. 2001";
            rehabText.Text = "Even after rehab, the effects of substance abuse on the brain are never completely healed.";

            if (rehabSlider.Value == 1)
            {
                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Brain off Meth 1 Month";
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_meth_rehab_1month.PNG", UriKind.Relative));
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));
            } else if (rehabSlider.Value == 2)
            {
                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Brain off Meth 14 Months";
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_meth_rehab_14month.PNG", UriKind.Relative));
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));
            }

        }

        //This method allows the user to switch between pictures in the rehab page.
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           
            if (rehabSlider.Value == 1)
            {
                try
                {
                    rightImage.Source = new BitmapImage(new Uri("Resources/brain_meth_rehab_1month.PNG", UriKind.Relative));
                    leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));

                    leftImageLabel.Content = "Typical Brain";
                    rightImageLabel.Content = "Brain off Meth 1 Month";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (rehabSlider.Value == 2)
            {
                try
                {
                    rightImage.Source = new BitmapImage(new Uri("Resources/brain_meth_rehab_14month.PNG", UriKind.Relative));
                    leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));

                    leftImageLabel.Content = "Typical Brain";
                    rightImageLabel.Content = "Brain off Meth 14 Months";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        //This method clears the rehab page and loads content for the substance abuse page when the 
        //'Substances' button is clicked. This includes a combobox list to switch between pictures, 
        //the picture that was last loaded, and the description text for this page.
        private void drugButton_Click(object sender, RoutedEventArgs e)
        {
            rehabButton.Visibility = Visibility.Visible;
            drugList.Visibility = Visibility.Visible;

            rehabSlider.Visibility = Visibility.Hidden;
            drugButton.Visibility = Visibility.Hidden;
            rehabText.Text = "Drugs can alter the way people think, feel, and behave by disrupting neurotransmission, the process of communication between neurons.";

            String drugListValue = (drugList.SelectedItem as ComboBoxItem).Content.ToString();
            if (drugListValue == "Alcohol")
            {
                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Heavy Alcohol Use";
                sourceLabel.Content = "Source: http://www.encognitive.com/files/images/brain-scan-alcoholic-drug-addict-obese-normal.preview.jpg";
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_alcoholic.png", UriKind.Relative));
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));
            } else if (drugListValue == "Smoker")
            {
                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Regular Nicotine Use";
                sourceLabel.Content = "Source: http://selfchec.org/main/wp-content/uploads/2010/04/brain.jpg";
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_smoker.png", UriKind.Relative));
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_non_smoker.png", UriKind.Relative));
            } else if (drugListValue == "Cocaine")
            {
                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Cocaine Use";
                sourceLabel.Content = "Source: http://www.encognitive.com/files/images/brain-scan-alcoholic-drug-addict-obese-normal.preview.jpg";
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_cocaine.png", UriKind.Relative));
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));
            }
        }
    }
}

