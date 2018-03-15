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
            Image2.Visibility = Visibility.Hidden;
            Image3.Visibility = Visibility.Hidden;
            Image4.Visibility = Visibility.Hidden;
            Image5.Visibility = Visibility.Hidden;
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
                Image2.Visibility = Visibility.Hidden;
                Image3.Visibility = Visibility.Hidden;
                Image4.Visibility = Visibility.Hidden;
                Image5.Visibility = Visibility.Hidden;
                Image1.Visibility = Visibility.Visible;
            }
            else if (slValue.Value == 8.75)
            {
                Image1.Visibility = Visibility.Hidden;
                Image3.Visibility = Visibility.Hidden;
                Image4.Visibility = Visibility.Hidden;
                Image5.Visibility = Visibility.Hidden;
                Image2.Visibility = Visibility.Visible;
            }
            else if (slValue.Value == 12.5)
            {
                Image1.Visibility = Visibility.Hidden;
                Image2.Visibility = Visibility.Hidden;
                Image4.Visibility = Visibility.Hidden;
                Image5.Visibility = Visibility.Hidden;
                Image3.Visibility = Visibility.Visible;
            }
            else if (slValue.Value == 16.25)
            {
                Image1.Visibility = Visibility.Hidden;
                Image2.Visibility = Visibility.Hidden;
                Image3.Visibility = Visibility.Hidden;
                Image5.Visibility = Visibility.Hidden;
                Image4.Visibility = Visibility.Visible;
            }
            else if (slValue.Value == 20)
            {
                Image1.Visibility = Visibility.Hidden;
                Image2.Visibility = Visibility.Hidden;
                Image3.Visibility = Visibility.Hidden;
                Image4.Visibility = Visibility.Hidden;
                Image5.Visibility = Visibility.Visible;
            }

        }

    }
}


