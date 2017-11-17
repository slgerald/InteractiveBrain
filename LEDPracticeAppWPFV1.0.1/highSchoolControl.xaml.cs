using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for highSchoolControl.xaml
    /// </summary>
    /// 

    public partial class highSchoolControl : UserControl
    {
        bool functionsFlag;
        bool healthyBehaviorsFlag;
        bool unhealthyBehaviorsFlag;
        System.Windows.Point MouseDownLocation;
        double originalImageHeight;
        double originalImageWidth;
        private object movingObject;
        private double firstXPos, firstYPos;
      


        private static highSchoolControl _instance; //render userControl based on button pressed
        public highSchoolControl()
        {
            InitializeComponent();
            InitializeComponent();
            functions.IsChecked = true;
            amygdalaBox.MouseDown += new MouseButtonEventHandler(amygdalaBox_MouseDown);
       //     hippocampusBox.MouseDown += new MouseButtonEventHandler(hippocampusBox_MouseDown);
            brainstemBox.MouseDown += new MouseButtonEventHandler(brainstemBox_MouseDown);
            pituitaryGlandBox.MouseDown += new MouseButtonEventHandler(pituitaryGlandBox_MouseDown);
       
            hippocampusBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(img_PreviewMouseLeftButtonDown);
            hippocampusBox.PreviewMouseMove += new MouseEventHandler(img_PreviewMouseMove);
            hippocampusBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(img_PreviewMouseLeftButtonUp);
            pituitaryGlandBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(img_PreviewMouseLeftButtonDown);
            pituitaryGlandBox.PreviewMouseMove += new MouseEventHandler(img_PreviewMouseMove);
            pituitaryGlandBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(img_PreviewMouseLeftButtonUp);
            brainstemBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(img_PreviewMouseLeftButtonDown);
            brainstemBox.PreviewMouseMove += new MouseEventHandler(img_PreviewMouseMove);
            brainstemBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(img_PreviewMouseLeftButtonUp);
            amygdalaBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(img_PreviewMouseLeftButtonDown);
            amygdalaBox.PreviewMouseMove += new MouseEventHandler(img_PreviewMouseMove);
            amygdalaBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(img_PreviewMouseLeftButtonUp);
        }
        private void img_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // In this event, we get the current mouse position on the control to use it in the MouseMove event.
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            Canvas canvas = img.Parent as Canvas;

            firstXPos = e.GetPosition(img).X;
            firstYPos = e.GetPosition(img).Y;
           
            movingObject = sender;

            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            foreach (System.Windows.Controls.Image child in canvas.Children)
                if (top < Canvas.GetZIndex(child))
                    top = Canvas.GetZIndex(child);
            Canvas.SetZIndex(img, top + 1);
            Mouse.Capture(img);
        }
        private void img_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            Canvas canvas = img.Parent as Canvas;

            movingObject = null;

            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            foreach (System.Windows.Controls.Image child in canvas.Children)
                if (top > Canvas.GetZIndex(child))
                    top = Canvas.GetZIndex(child);
            Canvas.SetZIndex(img, top + 1);
            Mouse.Capture(null);
        }
        private void img_PreviewMouseMove(object sender, MouseEventArgs e)
        {
          //  double newLeft;
            if (e.LeftButton == MouseButtonState.Pressed && sender == movingObject)
            {
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                Canvas canvas = img.Parent as Canvas;

                double newLeft = e.GetPosition(canvas).X - firstXPos - canvas.Margin.Left;
                // newLeft inside canvas right-border?
                if (newLeft > canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth)
                {
                    newLeft = canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth;
                    Console.WriteLine("new left within canvas right border");
                }
                // newLeft inside canvas left-border?
                else if (newLeft < canvas.Margin.Left)
                {
                    newLeft = canvas.Margin.Left;
                    Console.WriteLine("new left within canvas right border");
                }
                img.SetValue(Canvas.LeftProperty, newLeft);

                double newTop = e.GetPosition(canvas).Y - firstYPos - canvas.Margin.Top;
                // newTop inside canvas bottom-border?
                if (newTop > canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight)
                    newTop = canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight;
                // newTop inside canvas top-border?
                else if (newTop < canvas.Margin.Top)
                    newTop = canvas.Margin.Top;
                img.SetValue(Canvas.TopProperty, newTop);
            }
        }
           
        public static highSchoolControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new highSchoolControl();
                }
                return _instance;

            }
        }
        //This function sets flags to true and false
        private void functions_Checked(object sender, RoutedEventArgs e)
        {
            functionsFlag = true;
            healthyBehaviorsFlag = false;
            unhealthyBehaviorsFlag = false;
        }
        //This function sets flags as true and false 
        private void healthyBehaviors_Checked(object sender, RoutedEventArgs e)
        {
            healthyBehaviorsFlag = true;
            functionsFlag = false;
            unhealthyBehaviorsFlag = false;
        }
        //This function set flags as true and false 
        private void unhealthyBehaviors_Checked(object sender, RoutedEventArgs e)
        {
            unhealthyBehaviorsFlag = true;
            functionsFlag = false;
            healthyBehaviorsFlag = false;
        }
        private void amygdalaBox_MouseDown(object sender, MouseEventArgs e)
        {
        }
       
        private void brainstemBox_MouseDown(object sender, MouseEventArgs e)
        {

        }
        private void pituitaryGlandBox_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pictureCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
