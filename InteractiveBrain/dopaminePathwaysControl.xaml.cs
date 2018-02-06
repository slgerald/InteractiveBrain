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
    public partial class dopaminePathwaysControl : UserControl
    {
        private static dopaminePathwaysControl _instance; //render userControl based on button pressed

        public dopaminePathwaysControl()
        {
            InitializeComponent();
        }

        public static dopaminePathwaysControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new dopaminePathwaysControl();
                }
                _instance = new dopaminePathwaysControl();
                return _instance;

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
