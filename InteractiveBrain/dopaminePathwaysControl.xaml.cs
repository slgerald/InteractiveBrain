using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InteractiveBrain
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class dopaminePathwaysControl : UserControl
    {


        private static dopaminePathwaysControl _instance; //render userControl based on button pressed
        private DispatcherTimer timer; // A timer to display the video's location

        public dopaminePathwaysControl()
        {
            InitializeComponent();
            // Uri resourceUri = new Uri("\\Resources\\gif_brain_reward_pathways.mp4", UriKind.Relative);
            // StreamResourceInfo streamInfo = Application.GetContentStream(resourceUri);
            Gif1.Source = new Uri(string.Format(@"{0}Resources\gif_brain_reward_pathways.mp4", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            //Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName)
            MessageBox.Show(string.Format(@"{0}\Resources\gif_brain_reward_pathways.mp4", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            // Console.WriteLine(string.Format(@"{0}\Resources\gif_brain_reward_pathways.mp4", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            // Gif1.Source = new Uri(@"./Resources/gif_brain_reward_pathways.mp4", UriKind.Relative);
            Gif1.Play();
            videoList.SelectedIndex = 0;
            videoLabel.Content = "1/9 Reward Pathways";
            dopamineText.Text = "In a healthy person, the brain's reward system reinforces healthy behaviors, such as eating. The reward system ensures that you eat, because it knows that after eating, you will feel good.";
            previousButton.Visibility = Visibility.Hidden;
            nextButton.Visibility = Visibility.Visible;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            //timer.Tick += timer_Tick;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();


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

        private void Gif1_MediaOpened(object sender, RoutedEventArgs e)
        {
            sbarPosition.Minimum = 0;
            sbarPosition.Maximum =
                Gif1.NaturalDuration.TimeSpan.TotalSeconds;
            sbarPosition.Visibility = Visibility.Visible;
        }

        // Show the play position in the ScrollBar and TextBox.
        private void ShowPosition()
        {
            sbarPosition.Value = Gif1.Position.TotalSeconds;
            // txtPosition.Text = Gif1.Position.TotalSeconds.ToString("0.0");
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            ShowPosition();
            if (Gif1.Source != null)
            {
                if (Gif1.NaturalDuration.HasTimeSpan)
                    lblStatus.Content = String.Format("{0} / {1}", Gif1.Position.ToString(@"mm\:ss"), Gif1.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Content = "No file selected...";
        }

        private void Gif1_MediaEnded(object sender, RoutedEventArgs e)
        {
            Gif1.Position = TimeSpan.FromSeconds(0);

        }


        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            int videoListIndex;
            if (Gif1.Source == new Uri(@".\Resources\gif_neurons.mp4", UriKind.Relative))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Hidden;
                Gif1.Source = new Uri(@".\Resources\gif_brain_reward_pathways.mp4", UriKind.Relative);
                Gif1.Play();
                videoList.SelectedIndex = 0;
                videoLabel.Content = "1/9 Reward Pathways";
                dopamineText.Text = "In a healthy person, the brain's reward system reinforces healthy behaviors, such as eating. The reward system ensures that you eat, because it knows that after eating, you will feel good.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_dopamine_receptors.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_neurons.mp4", UriKind.Relative);
                Gif1.Play();
                videoList.SelectedIndex = 1;
                videoLabel.Content = "2/9 Neurotransmitters";
                dopamineText.Text = "A brain consists of billions of neurons, which communicate via neurotransmitters. After stimulation, an electrical impulse, called an action potential, is generated and travels down the axon to the nerve terminal.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_dopamine_receptors.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 2;
                videoLabel.Content = "3/9 Dopamine Receptors";
                dopamineText.Text = "The action impulse triggers the release of neurotransmitters in the synaptic clift, a space between neurons. The neurotransmitters then bind to receptors of a neighboring neuron, generating a signal in it, thereby transmitting the information to that neuron.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_drug_stimulation.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 3;
                videoLabel.Content = "4/9 Dopamine Transporters";
                dopamineText.Text = "After the dopamine binds to the receptors, pleasurable feelings or rewarding effects are produced. A special protein, called a dopamine transporter, then removes the dopamine from the synaptic clift and transports them back to the transmitting neuron.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_blocked_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_drug_stimulation.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 4;
                videoLabel.Content = "5/9 Action Potential";
                dopamineText.Text = "The major reward pathways involve transmission of dopamine from the ventral tegmental area (VTA) of the midbrain to the limbic system in the frontal cortex. Some drugs, such as alcohol, heroine, and nicotine, indirectly excite dopamine-producing neurons in the VTA so that more action potentials are generated.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_continuous_dopamine_stimulation.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_blocked_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 5;
                videoLabel.Content = "6/9 Drug Binding";
                dopamineText.Text = "Drugs, such as meth, bind to the dopamine transporter and block the reuptake of dopamine. In addition, they can enter the neruron into the dopamine-containing vesticles, where they trigger dopamine release, even in the absence of action potentials.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_tolerance.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_continuous_dopamine_stimulation.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 6;
                videoLabel.Content = "7/9 Overstimulation";
                dopamineText.Text = "Different drugs act in different ways, but the common outcome is that dopamine builds up in the synapse to a much greater amount than normal. This results in overstimulation in receiving neurons and is responsible for prolonged and intense euphoria experienced by drug users. ";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_increased_tolerance.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_tolerance.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 7;
                videoLabel.Content = "8/9 Tolerance";
                dopamineText.Text = "Drugs will desensitize the reward system. The system is no longer responsive to everyday stimuli. The only thing that is rewarding is the drug.";
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

            if (Gif1.Source == new Uri(@".\Resources\gif_brain_reward_pathways.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_neurons.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 1;
                videoLabel.Content = "2/9 Neurotransmitters";
                dopamineText.Text = "A brain consists of billions of neurons, which communicate via neurotransmitters. After stimulation, an electrical impulse, called an action potential, is generated and travels down the axon to the nerve terminal.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_neurons.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_dopamine_receptors.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 2;
                videoLabel.Content = "3/9 Dopamine Receptors";
                dopamineText.Text = "The action impulse triggers the release of neurotransmitters in the synaptic clift, a space between neurons. The neurotransmitters then bind to receptors of a neighboring neuron, generating a signal in it, thereby transmitting the information to that neuron.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_dopamine_receptors.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 3;
                videoLabel.Content = "4/9 Dopamine Transporters";
                dopamineText.Text = "After the dopamine binds to the receptors, pleasurable feelings or rewarding effects are produced. A special protein, called a dopamine transporter, then removes the dopamine from the synaptic clift and transports them back to the transmitting neuron.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_drug_stimulation.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 4;
                videoLabel.Content = "5/9 Action Potential";
                dopamineText.Text = "The major reward pathways involve transmission of dopamine from the ventral tegmental area (VTA) of the midbrain to the limbic system in the frontal cortex. Some drugs, such as alcohol, heroine, and nicotine, indirectly excite dopamine-producing neurons in the VTA so that more action potentials are generated.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_drug_stimulation.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_blocked_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 5;
                videoLabel.Content = "6/9 Drug Binding";
                dopamineText.Text = "Drugs, such as meth, bind to the dopamine transporter and block the reuptake of dopamine. In addition, they can enter the neruron into the dopamine-containing vesticles, where they trigger dopamine release, even in the absence of action potentials.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_blocked_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_continuous_dopamine_stimulation.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 6;
                videoLabel.Content = "7/9 Overstimulation";
                dopamineText.Text = "Different drugs act in different ways, but the common outcome is that dopamine builds up in the synapse to a much greater amount than normal. This results in overstimulation in receiving neurons and is responsible for prolonged and intense euphoria experienced by drug users. ";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_continuous_dopamine_stimulation.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_tolerance.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 7;
                videoLabel.Content = "8/9 Tolerance";
                dopamineText.Text = "Drugs will desensitize the reward system. The system is no longer responsive to everyday stimuli. The only thing that is rewarding is the drug.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_tolerance.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Hidden;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_increased_tolerance.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 8;
                videoLabel.Content = "9/9 Increased Tolerance";
                dopamineText.Text = "Eventually, even the drug loses its ability to reward and higher doses are required to achieve the rewarding effect. This leads to drug overdose.";
            }
        }

        private void videoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String videoListValue = (videoList.SelectedItem as ComboBoxItem).Content.ToString();
            if (videoListValue == "Reward Pathways")
            {
                Gif1.Stop();
                previousButton.Visibility = Visibility.Hidden;
                nextButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_brain_reward_pathways.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "1/9 Reward Pathways";
                dopamineText.Text = "In a healthy person, the brain's reward system reinforces healthy behaviors, such as eating. The reward system ensures that you eat, because it knows that after eating, you will feel good.";
            }
            else if (videoListValue == "Neurotransmitters")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_neurons.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "2/9 Neurotransmitters";
                dopamineText.Text = "A brain consists of billions of neurons, which communicate via neurotransmitters. After stimulation, an electrical impulse, called an action potential, is generated and travels down the axon to the nerve terminal.";
            }
            else if (videoListValue == "Dopamine Receptors")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_dopamine_receptors.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "3/9 Dopamine Receptors";
                dopamineText.Text = "The action impulse triggers the release of neurotransmitters in the synaptic clift, a space between neurons. The neurotransmitters then bind to receptors of a neighboring neuron, generating a signal in it, thereby transmitting the information to that neuron.";
            }
            else if (videoListValue == "Dopamine Transporters")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "4/9 Dopamine Transporters";
                dopamineText.Text = "After the dopamine binds to the receptors, pleasurable feelings or rewarding effects are produced. A special protein, called a dopamine transporter, then removes the dopamine from the synaptic clift and transports them back to the transmitting neuron.";
            }
            else if (videoListValue == "Action Potential")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_drug_stimulation.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "5/9 Action Potential";
                dopamineText.Text = "The major reward pathways involve transmission of dopamine from the ventral tegmental area (VTA) of the midbrain to the limbic system in the frontal cortex. Some drugs, such as alcohol, heroine, and nicotine, indirectly excite dopamine-producing neurons in the VTA so that more action potentials are generated.";
            }
            else if (videoListValue == "Drug Binding")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_blocked_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "6/9 Drug Binding";
                dopamineText.Text = "Drugs, such as meth, bind to the dopamine transporter and block the reuptake of dopamine. In addition, they can enter the neruron into the dopamine-containing vesticles, where they trigger dopamine release, even in the absence of action potentials.";
            }
            else if (videoListValue == "Overstimulation")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_continuous_dopamine_stimulation.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "7/9 OverStimulation";
                dopamineText.Text = "Different drugs act in different ways, but the common outcome is that dopamine builds up in the synapse to a much greater amount than normal. This results in overstimulation in receiving neurons and is responsible for prolonged and intense euphoria experienced by drug users. ";
            }
            else if (videoListValue == "Tolerance")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_tolerance.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "8/9 Tolerance";
                dopamineText.Text = "Drugs will desensitize the reward system. The system is no longer responsive to everyday stimuli. The only thing that is rewarding is the drug.";
            }
            else if (videoListValue == "Increased Tolerance")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Hidden;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_increased_tolerance.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "9/9 Increased Tolerance";
                dopamineText.Text = "Eventually, even the drug loses its ability to reward and higher doses are required to achieve the rewarding effect. This leads to drug overdose.";
            }

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            Gif1.Play();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            Gif1.Pause();
        }

        /*
        private void btnSetPosition_Click(object sender, RoutedEventArgs e)
        {

            TimeSpan timespan =
                TimeSpan.FromSeconds(double.Parse(txtPosition.Text));
            Gif1.Position = timespan;
            ShowPosition();
        }
        */

        /*
        public void PlayVideo(object sender, RoutedEventArgs e)


        {

           // VideoPreview.Visibility = Visibility.Collapsed;


           // dopaminePathwaysVideo.Visibility = Visibility.Visible;


           // dopaminePathwaysVideo.Play();


        }

        
        public void PauseVideo(object sender, RoutedEventArgs e)


        {


            VideoPreview.Visibility = Visibility.Collapsed;


            dopaminePathwaysVideo.Visibility = Visibility.Visible;


            dopaminePathwaysVideo.Pause();


        }


        public void StopVideo(object sender, RoutedEventArgs e)


        {


            VideoPreview.Visibility = Visibility.Collapsed;


            dopaminePathwaysVideo.Visibility = Visibility.Visible;


            dopaminePathwaysVideo.Stop();


        }
        */

    }
}

