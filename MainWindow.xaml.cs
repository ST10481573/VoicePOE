using System;
using System.Windows;
using System.Speech.Synthesis;
using System.Media;   // For playing WAV files

namespace POE_Part2
{
    public partial class MainWindow : Window
    {
        private Chatbot chatbot;
        private SpeechSynthesizer synth;

        public MainWindow()
        {
            InitializeComponent();
            chatbot = new Chatbot();
            synth = new SpeechSynthesizer();

            // Configure voice settings
            synth.SelectVoice("Microsoft Zira Desktop"); // Natural female voice
            synth.Rate = -1;   // Slightly slower for clarity
            synth.Volume = 100; // Full volume

            // Play your custom WAV greeting first
            try
            {
                SoundPlayer player = new SoundPlayer("Halo!.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                lstChat.Items.Add($"Bot: (Could not play Halo!.wav) {ex.Message}");
            }

            // Initial greeting in chat
            string greeting = "Hello! I am your Cybersecurity Assistant. Ask me about passwords, phishing, privacy, or scams.";
            lstChat.Items.Add($"Bot [{DateTime.Now:T}]: {greeting}");

            // Speak greeting with SSML for emphasis and pauses
            string ssmlGreeting = @"
                <speak version='1.0' 
                       xmlns='http://www.w3.org/2001/10/synthesis' 
                       xml:lang='en-US'>
                    <voice name='Microsoft Zira Desktop'>
                        Welcome to the <emphasis>Cybersecurity Awareness Bot</emphasis>...
                        I am here to help you <break time='500ms'/> stay safe online.
                    </voice>
                </speak>";
            synth.SpeakSsmlAsync(ssmlGreeting);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string userInput = txtUserInput.Text;
            if (string.IsNullOrWhiteSpace(userInput)) return;

            // Show user input in chat history with timestamp
            lstChat.Items.Add($"User [{DateTime.Now:T}]: {userInput}");

            // Get bot response from Chatbot class
            string botResponse = chatbot.GetResponse(userInput);
            lstChat.Items.Add($"Bot [{DateTime.Now:T}]: {botResponse}");

            // Speak response with SSML (adds pauses and emphasis)
            string ssmlResponse = $@"
                <speak version='1.0' 
                       xmlns='http://www.w3.org/2001/10/synthesis' 
                       xml:lang='en-US'>
                    <voice name='Microsoft Zira Desktop'>
                        {botResponse}
                    </voice>
                </speak>";
            synth.SpeakSsmlAsync(ssmlResponse);

            // Clear input box
            txtUserInput.Clear();
        }
    }
}
