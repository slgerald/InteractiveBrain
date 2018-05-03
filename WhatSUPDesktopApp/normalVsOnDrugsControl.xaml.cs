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

namespace WhatSUPDesktopApp
{
    /// <summary>
    /// The normalVsOnDrugsControl is used to give the user examples of drugs and how
    /// they effect brain activity. It also shows how the brain recovers after being off
    /// drugs for prolonged periods of time.
    /// </summary>
    public partial class NormalVsOnDrugsControl : UserControl
    {

        //render userControl based on button pressed
        private static NormalVsOnDrugsControl _instance; 

        //This method serves as the entry point to the normalVsOnDrugsControl.
        //It loads the default picture, label, description text, and combo box list item.
        public NormalVsOnDrugsControl()
        {
            InitializeComponent();

            //Selects defuault combo box list item to load
            drugList.SelectedIndex = 0;

            //Make label containing the source of the pictures visible
            sourceLabel.Visibility = Visibility.Visible;

            //Hides the 'Substances' button because the substances page is loaded by default
            drugButton.Visibility = Visibility.Hidden;

            //Makes the 'Rehab' button visible in order to go to the rehab page
            rehabButton.Visibility = Visibility.Visible;

            //Hides the slider for the rehab page
            rehabSlider.Visibility = Visibility.Hidden;

            //Loads initial typical brain image
            leftImageLabel.Content = "Typical Brain";

            //Loads initial substance abuse image
            rightImageLabel.Content = "Heavy Alcohol Use";

            //Sets the source for the initial pictures
            sourceLabel.Content = "Source: http://www.encognitive.com/files/images/brain-scan-alcoholic-drug-addict-obese-normal.preview.jpg";

            //Sets the initial description text
            rehabText.Text = "Substances can alter the way people think, feel, and behave by disrupting neurotransmission, the process of communication between neurons.";
        }

        //This method allows the normalVsOnDrugsControl to be rendered when called
        public static NormalVsOnDrugsControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NormalVsOnDrugsControl();
                }
                _instance = new NormalVsOnDrugsControl();
                return _instance;

            }
        }

        //Loads the pictures, labels, and description text based on which combo box item that was 
        //chosen from the drop down list
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

        //Sets everything on the 'Substances' page to hidden and sets everything on the 
        //'Rehab' page to visible by clicking the 'Rehab' button
        private void Rehab_Button_Click(object sender, RoutedEventArgs e)
        {
            drugList.Visibility = Visibility.Hidden;
            rehabButton.Visibility = Visibility.Hidden;

            rehabSlider.Visibility = Visibility.Visible;
            drugButton.Visibility = Visibility.Visible;
            sourceLabel.Content = "Source: The Journal of Neuroscience, 21(23):9414-9418. 2001";

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

        //Changes the picture, label, and description text based on the value/position of the slider.
        //This slider is located on the 'Rehab' page.
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

        //Sets everything on the 'Rehab' page to hidden and sets everything on the 
        //'Substances' page to visible by clicking the 'Substances' button
        private void DrugButton_Click(object sender, RoutedEventArgs e)
        {
            rehabButton.Visibility = Visibility.Visible;
            drugList.Visibility = Visibility.Visible;

            rehabSlider.Visibility = Visibility.Hidden;
            drugButton.Visibility = Visibility.Hidden;

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

