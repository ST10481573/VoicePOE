using System;
using System.Collections.Generic;
using System.Media;
using System.Speech.Synthesis;
using System.Windows;

namespace POE_Part3
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

            // Configure voice
            synth.SelectVoice("Microsoft Zira Desktop");
            synth.Rate = -1;
            synth.Volume = 100;

            // Play WAV greeting
            try
            {
                SoundPlayer player = new SoundPlayer("Halo!.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                lstChat.Items.Add($"Bot: (Could not play Halo!.wav) {ex.Message}");
            }

            // Initial greeting
            string greeting = "Hello! I am your Cybersecurity Assistant. Ask me about passwords, phishing, privacy, scams, tasks, or quizzes.";
            lstChat.Items.Add($"Bot [{DateTime.Now:T}]: {greeting}");

            // Speak greeting with SSML
            string ssmlGreeting = @"
                <speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>
                    <voice name='Microsoft Zira Desktop'>
                        Welcome to the <emphasis>Cybersecurity Awareness Bot</emphasis>...
                        I am here to help you <break time='500ms'/> stay safe online.
                    </voice>
                </speak>";
            synth.SpeakSsmlAsync(ssmlGreeting);
        }

        // --- Chatbot Tab ---
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string userInput = txtUserInput.Text;
            if (string.IsNullOrWhiteSpace(userInput)) return;

            lstChat.Items.Add($"User [{DateTime.Now:T}]: {userInput}");

            string botResponse = chatbot.GetResponse(userInput);
            lstChat.Items.Add($"Bot [{DateTime.Now:T}]: {botResponse}");

            string ssmlResponse = $@"
                <speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>
                    <voice name='Microsoft Zira Desktop'>
                        {botResponse}
                    </voice>
                </speak>";
            synth.SpeakSsmlAsync(ssmlResponse);

            txtUserInput.Clear();
        }

        // --- Tasks Tab ---
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitle.Text;
            string description = TaskDescription.Text;
            DateTime? reminder = ReminderDate.SelectedDate;

            if (string.IsNullOrWhiteSpace(title)) return;

            string response = chatbot.AddTask(title, description, reminder);
            lstChat.Items.Add($"Bot [{DateTime.Now:T}]: {response}");

            // Refresh Task List from DB
            TaskList.Items.Clear();
            foreach (var task in chatbot.GetTasks())
            {
                TaskList.Items.Add(task);
            }
        }

        // --- Quiz Tab ---
        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            string questionText = chatbot.StartQuiz();
            QuizQuestion.Text = questionText;

            var currentQuestion = chatbot.GetCurrentQuestion();
            if (currentQuestion != null)
            {
                LoadQuizOptions(currentQuestion.Options);
            }
        }



        private void LoadQuizOptions(List<string> options)
        {
            QuizOptions.Items.Clear();
            foreach (var option in options)
            {
                QuizOptions.Items.Add(option);
            }
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (QuizOptions.SelectedIndex == -1)
            {
                QuizFeedback.Text = "Please select an option.";
                return;
            }

            int choice = QuizOptions.SelectedIndex;
            string feedback = chatbot.SubmitAnswer(choice);
            QuizFeedback.Text = feedback;

            var currentQuestion = chatbot.GetCurrentQuestion();
            if (currentQuestion != null)
            {
                QuizQuestion.Text = currentQuestion.Text;
                LoadQuizOptions(currentQuestion.Options);
            }
            else
            {
                QuizQuestion.Text = "Quiz completed!";
                QuizOptions.Items.Clear();
            }
        }

        // --- Activity Log Tab ---
        private void RefreshLog_Click(object sender, RoutedEventArgs e)
        {
            ActivityLog.Items.Clear();
            string log = chatbot.ShowActivityLog();
            foreach (var line in log.Split('\n'))
            {
                ActivityLog.Items.Add(line);
            }
        }
    }
}
