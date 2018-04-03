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


            //Can set Initial visibility in .xaml file
            drugList.SelectedIndex = 0;
            sourceLabel.Visibility = Visibility.Visible;
            drugButton.Visibility = Visibility.Hidden;
            rehabButton.Visibility = Visibility.Visible;
            rehabSlider.Visibility = Visibility.Hidden;
            leftImageLabel.Content = "Typical Brain";
            rightImageLabel.Content = "Alcoholic";
            sourceLabel.Content = "Images from http://www.biggiesboxers.com/obese-people-cant-help-it-their-brains-are-hard-wired-to-eat-in-the-same-way-drug-addicts-crave-their-fix-scientists-say/";
            rehabText.Text = "Drugs can alter the way people think, feel, and behave by disrupting neurotransmission, the process of communication between neurons.";
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
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));
                leftImage.Visibility = Visibility.Visible;

                rightImage.Visibility = Visibility.Hidden;
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_alcoholic.png", UriKind.Relative));
                rightImage.Visibility = Visibility.Visible;

                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Alcoholic Brain";
                sourceLabel.Content = "Images from http://www.biggiesboxers.com/obese-people-cant-help-it-their-brains-are-hard-wired-to-eat-in-the-same-way-drug-addicts-crave-their-fix-scientists-say/";
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
                rightImageLabel.Content = "Smoker Brain";
                sourceLabel.Content = "Images from https://www.pinterest.com/pin/431501208017912683/";
            }
            else if (drugListValue == "Cocaine")
            {
                leftImage.Visibility = Visibility.Hidden;
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_non_cocaine.png", UriKind.Relative));
                leftImage.Visibility = Visibility.Visible;

                rightImage.Visibility = Visibility.Hidden;
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_cocaine.png", UriKind.Relative));
                rightImage.Visibility = Visibility.Visible;

                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Cocaine Brain";
                sourceLabel.Content = "Images from http://www.biggiesboxers.com/obese-people-cant-help-it-their-brains-are-hard-wired-to-eat-in-the-same-way-drug-addicts-crave-their-fix-scientists-say/";
            }
        }

        private void Rehab_Button_Click(object sender, RoutedEventArgs e)
        {
            drugList.Visibility = Visibility.Hidden;
            rehabButton.Visibility = Visibility.Hidden;

            rehabSlider.Visibility = Visibility.Visible;
            drugButton.Visibility = Visibility.Visible;
            sourceLabel.Content = "Images from http://www.recoveryinstitute.com/?lightbox=dataItem-ixhu2s2q2";

            if (rehabSlider.Value == 1)
            {
                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Brain on Meth 1 Month";
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_meth_rehab_1month.png", UriKind.Relative));
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));
            } else if (rehabSlider.Value == 2)
            {
                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Brain on Meth 14 Months";
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_meth_rehab_14month.png", UriKind.Relative));
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));
            }

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           
            if (rehabSlider.Value == 1)
            {
                try
                {
                    rightImage.Source = new BitmapImage(new Uri("Resources/brain_meth_rehab_1month.png", UriKind.Relative));
                    leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));

                    leftImageLabel.Content = "Typical Brain";
                    rightImageLabel.Content = " Brain on Meth 1 Month";
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
                    rightImage.Source = new BitmapImage(new Uri("Resources/brain_meth_rehab_14month.png", UriKind.Relative));
                    leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));

                    leftImageLabel.Content = "Typical Brain";
                    rightImageLabel.Content = "Brain on Meth 14 Months";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void drugButton_Click(object sender, RoutedEventArgs e)
        {
            rehabButton.Visibility = Visibility.Visible;
            drugList.Visibility = Visibility.Visible;

            rehabSlider.Visibility = Visibility.Hidden;
            drugButton.Visibility = Visibility.Hidden;

            String drugListValue = (drugList.SelectedItem as ComboBoxItem).Content.ToString();
            if (drugListValue == "Alcohol")
            {
                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Alcoholic Brain";
                sourceLabel.Content = "Images from http://www.biggiesboxers.com/obese-people-cant-help-it-their-brains-are-hard-wired-to-eat-in-the-same-way-drug-addicts-crave-their-fix-scientists-say/";
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_alcoholic.png", UriKind.Relative));
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_normal.png", UriKind.Relative));
            } else if (drugListValue == "Smoker")
            {
                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Smoker Brain";
                sourceLabel.Content = "Images from https://www.pinterest.com/pin/431501208017912683/";
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_smoker.png", UriKind.Relative));
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_non_smoker.png", UriKind.Relative));
            } else if (drugListValue == "Cocaine")
            {
                leftImageLabel.Content = "Typical Brain";
                rightImageLabel.Content = "Cocaine Brain";
                sourceLabel.Content = "Images from http://www.biggiesboxers.com/obese-people-cant-help-it-their-brains-are-hard-wired-to-eat-in-the-same-way-drug-addicts-crave-their-fix-scientists-say/";
                rightImage.Source = new BitmapImage(new Uri("Resources/brain_cocaine.png", UriKind.Relative));
                leftImage.Source = new BitmapImage(new Uri("Resources/brain_non_cocaine.png", UriKind.Relative));
            }
        }
    }
}

