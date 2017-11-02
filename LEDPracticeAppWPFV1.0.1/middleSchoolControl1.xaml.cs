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
    /// Interaction logic for middleSchoolControl1.xaml
    /// </summary>
    public partial class middleSchoolControl1 : UserControl
    {
        private static middleSchoolControl1 _instance;
        public static middleSchoolControl1 Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new middleSchoolControl1();
                }
                return _instance;

            }
        }
        public middleSchoolControl1()
        {
            InitializeComponent();
           
        }
    }
}
