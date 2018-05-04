using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WhatSUPDesktopApp
{
    /// <summary>
    /// Ths healthActivitiesContol is used in order to give the user examples of 
    /// healthy activities and how they affect the brain by using pictures. 
    /// </summary>
    public partial class HealthyActivitiesControl : UserControl
    {
        //render userControl based on button pressed
        private static HealthyActivitiesControl _instance;

        //This method is used as an entry point to the healthyActivitiesControl page.
        //The default labels, pictures, description text, and combo box list item are 
        //loaded for the first healthy activity example.
        public HealthyActivitiesControl()
        {
            InitializeComponent();

            activityList.SelectedIndex = 0;
            leftLabel.Content = "Brain at Rest";
            rightLabel.Content = "Brain after Walking";
            imageSourceLabel.Content = "Source: Dr.Chuck Hillman, University of Illinois";
            imageText.Text = "Exercising can help strengthen learning, improve memory, and enhance motivation.";
        }

        //This method allows the healthyActivitiesControl to be rendered when called.
        public static HealthyActivitiesControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HealthyActivitiesControl();
                }
                _instance = new HealthyActivitiesControl();
                return _instance;

            }
        }

        //This methood allows the user to select a healthy activity from a combo box list.
        //This activity, along with its description text and labels, is then loaded.
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String activityListValue = (activityList.SelectedItem as ComboBoxItem).Content.ToString();
            if (activityListValue == "Walking")
            {
                leftActivity.Visibility = Visibility.Visible;
                rightActivity.Visibility = Visibility.Visible;
                leftLabel.Visibility = Visibility.Visible;
                rightLabel.Visibility = Visibility.Visible;
                readingLabel.Visibility = Visibility.Hidden;
                readingBook.Visibility = Visibility.Hidden;
                leftActivity.Source = new BitmapImage(new Uri("Resources/brain_activity_none.png", UriKind.Relative));
                rightActivity.Source = new BitmapImage(new Uri("Resources/brain_activity_walking.png", UriKind.Relative));
                leftLabel.Content = "Brain at Rest";
                rightLabel.Content = "Brain after Walking";
                imageSourceLabel.Content = "Source: Dr.Chuck Hillman, University of Illinois";
                imageText.Text = "Exercising can help strengthen learning, improve memory, and enhance motivation.";
            } else if (activityListValue == "Listening to Music")
            {
                leftActivity.Visibility = Visibility.Visible;
                rightActivity.Visibility = Visibility.Visible;
                leftLabel.Visibility = Visibility.Visible;
                rightLabel.Visibility = Visibility.Visible;
                readingLabel.Visibility = Visibility.Hidden;
                readingBook.Visibility = Visibility.Hidden;
                leftActivity.Source = new BitmapImage(new Uri("Resources/brain_music_none.png", UriKind.Relative));
                rightActivity.Source = new BitmapImage(new Uri("Resources/brain_music_activity.png", UriKind.Relative));
                leftLabel.Content = "Brain before Music";
                rightLabel.Content = "Brain after Music";
                imageSourceLabel.Content = "Source: https://drjockers.com/3-ways-music-improves-brain-function/";
                imageText.Text = "Music stimulates more parts of the brain than any other human function. Faster music makes people more alert, while slower music makes them feel relaxed.";
            } else if (activityListValue == "Reading a Book")
            {
                leftActivity.Visibility = Visibility.Hidden;
                rightActivity.Visibility = Visibility.Hidden;
                readingBook.Visibility = Visibility.Visible;
                leftLabel.Visibility = Visibility.Hidden;
                rightLabel.Visibility = Visibility.Hidden;
                readingLabel.Visibility = Visibility.Visible;
                readingLabel.Content = "Brain after Reading";
                imageSourceLabel.Content = "Source: Marcus E. Raichle, Department of Radiology, Washington University School of Medicine, St. Louis, Missouri";
                imageText.Text = "Reading can improve memory, increase brain power, and enhance empathic skills.";
            }
        }

    }
}
