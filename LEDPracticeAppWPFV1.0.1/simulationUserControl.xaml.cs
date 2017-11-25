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

namespace LEDPracticeAppWPFV1._0._1
{
    /// <summary>
    /// Interaction logic for simulationUserControl.xaml
    /// </summary>
    public partial class simulationUserControl : UserControl
    {
        private static simulationUserControl _instance; //render userControl based on button pressed
        public simulationUserControl()
        {
            InitializeComponent();

        }
        public static simulationUserControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new simulationUserControl();
                }
                return _instance;

            }
        }
    }
}
