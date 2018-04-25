using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InteractiveBrain
{
    /// <summary>
    /// The brainDevelopmentControl is an animation utlizing a slider which allows the user
    /// to slide through several images of the developing brain from childhood to adulthood.
    /// </summary>
    public partial class brainDevelopmentControl : UserControl
    {
        //render userControl based on button pressed
        private static brainDevelopmentControl _instance; 

       //This function serves as the entry point to the brainDevelopment userControl page.
       //The userControl is initialized and the description text for the pictures is written.
        public brainDevelopmentControl()
        {
            InitializeComponent();
            
            //Description for pictures on the brainDevelopment page
            brainDevelopmentText.Text = "Gray matter thins from child age to young adult age in order to 'prune' inefficient synaptic connections. This increases overall brain performance and cognitive function.";
        }

        //This method allows the brainDevelopmentControl to be rendered when called
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

        //This method keeps track of the sliders current value so that it can switch between
        //5 pictures when the value is changed.
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


