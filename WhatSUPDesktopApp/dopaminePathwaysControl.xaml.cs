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

namespace WhatSUPDesktopApp
{
    /// <summary>
    /// The dopaminePathwaysControl consists of 9 gifs which effectly show the user the 
    /// effects of drugs on dopamine pathways such that tolerance occurs and the drug
    /// user is addicted
    /// </summary>
    public partial class DopaminePathwaysControl : UserControl
    {

        //render userControl based on button pressed
        private static DopaminePathwaysControl _instance;

        // A timer to display the video's location
        private DispatcherTimer timer; 

        //This method serves as the entry point to the dopaminePathwaysControl.
        //It sets the defualt gif, description text, label, and combo box list item.
        public DopaminePathwaysControl()
        {
            InitializeComponent();
            
            //Sets the defualt gif
            Gif1.Source = new Uri(string.Format(@"{0}Resources\gif_brain_reward_pathways.mp4", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

            if (File.Exists(string.Format(@"{0}Resources\gif_brain_reward_pathways.mp4", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))) {
                MessageBox.Show(string.Format(@"{0}\Resources\gif_brain_reward_pathways.mp4", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            }
            
            //Initializes video to play
            Gif1.Play();

            //Selects default 'name of gif' for the combo box
            videoList.SelectedIndex = 0;

            //Sets defualt gif label
            videoLabel.Content = "1/9 Reward Pathways";

            //Sets default gif description text
            dopamineText.Text = "In a healthy person, the brain's reward system reinforces healthy behaviors, such as eating. The reward system ensures that you eat, because it knows that after eating, you will feel good.";

            //Hides the 'Previous' button to switch between gifs
            previousButton.Visibility = Visibility.Hidden;

            //Shows the 'Next' button to switch between gifs
            nextButton.Visibility = Visibility.Visible;

            //This block of code initializes the video timer
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.1)
            };
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();


        }

        //This method allows the dopaminePathwaysControl to be rendered when called
        public static DopaminePathwaysControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DopaminePathwaysControl();
                }
                _instance = new DopaminePathwaysControl();
                return _instance;

            }
        }

        //This method ties the scrollbar to the gif and allows it to accurately move
        //with the timing of the gif
        private void Gif1_MediaOpened(object sender, RoutedEventArgs e)
        {
            sbarPosition.Minimum = 0;
            sbarPosition.Maximum =
                Gif1.NaturalDuration.TimeSpan.TotalSeconds;
            sbarPosition.Visibility = Visibility.Visible;
        }

        // Show the current play position of the gif in seconds
        private void ShowPosition()
        {
            sbarPosition.Value = Gif1.Position.TotalSeconds;
        }

        //Sets a label for the gif and prints out the current time and total duration of 
        //the gif in the label
        private void Timer_Tick(object sender, EventArgs e)
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

        //This method determines whether or not the gif ended by comparing its current
        //time position to the gifs total duration
        private void Gif1_MediaEnded(object sender, RoutedEventArgs e)
        {
            Gif1.Position = TimeSpan.FromSeconds(0);

        }

        //This method allows the user to go back to the previous gif by clicking the 
        //'Previous' button. The previous gif, description text, label, and combo box
        //list item will all be loaded.
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            
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
                dopamineText.Text = "The action impulse triggers the release of neurotransmitters in the synaptic cleft, a space between neurons. The neurotransmitters then bind to receptors of a neighboring neuron, generating a signal in it, thereby sending the information to that neuron.";
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
                dopamineText.Text = "After the dopamine binds to the receptors, pleasurable feelings or rewarding effects are produced. A special protein, called a dopamine transporter, then removes the dopamine from the synaptic cleft and transports them back to the sending neuron.";
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
                dopamineText.Text = "The major reward pathways involve transmission of dopamine from the ventral tegmental area (VTA) of the midbrain to the limbic system in the frontal cortex. Some substances, such as alcohol, heroin, and nicotine, indirectly excite dopamine-producing neurons in the VTA so that more action potentials are generated.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_continuous_dopamine_stimulation.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_blocked_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 5;
                videoLabel.Content = "6/9 Substance Binding";
                dopamineText.Text = "Substance, such as meth, bind to the dopamine transporter and block the reuptake of dopamine. In addition, they can enter the neruron into the dopamine-containing vesticles, where they trigger dopamine release, even in the absence of action potentials.";
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
                dopamineText.Text = "Different substances act in different ways, but the common outcome is that dopamine builds up in the synapse to a much greater amount than normal. This results in overstimulation in receiving neurons and is responsible for prolonged and intense euphoria experienced by substance users. ";
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
                dopamineText.Text = "Substances will desensitize the reward system. The system is no longer responsive to everyday stimuli. The only thing that is rewarding is the substance.";
            }
        }

        //This method takes the user to the next gif when the 'Next' button is clicked. 
        //The description text, label, and combo box list item for that gif are loaded
        //as well.
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
                dopamineText.Text = "The action impulse triggers the release of neurotransmitters in the synaptic clift, a space between neurons. The neurotransmitters then bind to receptors of a neighboring neuron, generating a signal in it, thereby sending the information to that neuron.";
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
                dopamineText.Text = "After the dopamine binds to the receptors, pleasurable feelings or rewarding effects are produced. A special protein, called a dopamine transporter, then removes the dopamine from the synaptic cleft and transports them back to the sending neuron.";
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
                dopamineText.Text = "The major reward pathways involve transmission of dopamine from the ventral tegmental area (VTA) of the midbrain to the limbic system in the frontal cortex. Some substances, such as alcohol, heroin, and nicotine, indirectly excite dopamine-producing neurons in the VTA so that more action potentials are generated.";
            }
            else if (Gif1.Source == new Uri(@".\Resources\gif_drug_stimulation.mp4", UriKind.RelativeOrAbsolute))
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_blocked_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoList.SelectedIndex = 5;
                videoLabel.Content = "6/9 Substance Binding";
                dopamineText.Text = "Substances, such as meth, bind to the dopamine transporter and block the reuptake of dopamine. In addition, they can enter the neruron into the dopamine-containing vesticles, where they trigger dopamine release, even in the absence of action potentials.";
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
                dopamineText.Text = "Different substances act in different ways, but the common outcome is that dopamine builds up in the synapse to a much greater amount than normal. This results in overstimulation in receiving neurons and is responsible for prolonged and intense euphoria experienced by substance users. ";
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
                dopamineText.Text = "Substances will desensitize the reward system. The system is no longer responsive to everyday stimuli. The only thing that is rewarding is the substances.";
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
                dopamineText.Text = "Eventually, even the substance loses its ability to reward and higher doses are required to achieve the rewarding effect. This may lead to drug overdose.";
            }
        }

        //This method defines which gif, description text, and label is loaded depending on
        //which combo box list item is selected from the drop down list
        private void VideoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                dopamineText.Text = "The major reward pathways involve transmission of dopamine from the ventral tegmental area (VTA) of the midbrain to the limbic system in the frontal cortex. Some substances, such as alcohol, heroin, and nicotine, indirectly excite dopamine-producing neurons in the VTA so that more action potentials are generated.";
            }
            else if (videoListValue == "Substance Binding")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_blocked_dopamine_transporters.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "6/9 Substance Binding";
                dopamineText.Text = "Substances, such as meth, bind to the dopamine transporter and block the reuptake of dopamine. In addition, they can enter the neruron into the dopamine-containing vesticles, where they trigger dopamine release, even in the absence of action potentials.";
            }
            else if (videoListValue == "Overstimulation")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_continuous_dopamine_stimulation.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "7/9 OverStimulation";
                dopamineText.Text = "Different substances act in different ways, but the common outcome is that dopamine builds up in the synapse to a much greater amount than normal. This results in overstimulation in receiving neurons and is responsible for prolonged and intense euphoria experienced by substance users. ";
            }
            else if (videoListValue == "Tolerance")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Visible;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_tolerance.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "8/9 Tolerance";
                dopamineText.Text = "Substances will desensitize the reward system. The system is no longer responsive to everyday stimuli. The only thing that is rewarding is the substance.";
            }
            else if (videoListValue == "Increased Tolerance")
            {
                Gif1.Stop();
                nextButton.Visibility = Visibility.Hidden;
                previousButton.Visibility = Visibility.Visible;
                Gif1.Source = new Uri(@".\Resources\gif_increased_tolerance.mp4", UriKind.RelativeOrAbsolute);
                Gif1.Play();
                videoLabel.Content = "9/9 Increased Tolerance";
                dopamineText.Text = "Eventually, even the substance loses its ability to reward and higher doses are required to achieve the rewarding effect. This leads to drug overdose.";
            }

        }

        //This function plays the gif when the 'Play' button is clicked
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Gif1.Play();
        }

        //This function pauses the gif when the 'Pause' button is clicked
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Gif1.Pause();
        }

    }
}

