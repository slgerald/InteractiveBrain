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
    /// Interaction logic for settingsUserControl.xaml
    /// </summary>
    public partial class settingsUserControl : UserControl
    {
        private static settingsUserControl _instance;
        public settingsUserControl()
        {
            InitializeComponent();
        }

        public static settingsUserControl Instance

        {
            get
            {
                if (_instance == null)
                {
                    _instance = new settingsUserControl();
                }
                return _instance;

            }
        }
    }
}
