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
    public partial class healthyActivitiesControl : UserControl
    {
        private static healthyActivitiesControl _instance; //render userControl based on button pressed

        public healthyActivitiesControl()
        {
            InitializeComponent();

            activityList.SelectedIndex = 0;
            leftLabel.Content = "Brain at Rest";
            rightLabel.Content = "Brain after Walking";
            imageSourceLabel.Content = "Image from https://www.yesware.com/blog/how-exercise-can-boost-your-sales-performance/";
            imageText.Text = "Exercising can help strengthen learning, improve memory, and enhance motivation.";
        }

        public static healthyActivitiesControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new healthyActivitiesControl();
                }
                _instance = new healthyActivitiesControl();
                return _instance;

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

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
                imageSourceLabel.Content = "Image from https://www.yesware.com/blog/how-exercise-can-boost-your-sales-performance/";
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
                imageSourceLabel.Content = "Image from http://www.tourmorgantown.com/four-therapeutic-benefits-music/";
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
                imageSourceLabel.Content = "Image from https://www.booksinc.net/blog/booksinckids/tandem%C2%AE-loves%E2%80%A6-picture-books-build-brains";
                imageText.Text = "Reading can improve memory, increase brain power, and enhance empathic skills.";
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
