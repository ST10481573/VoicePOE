using System;
using static System.Console; // Allows us to use WriteLine() directly
using System.Media; //  For WAV playback
using System.IO;// For path handling, I forgot it so I had to google it
using System.Threading; // For the typing effect (delays), also had to google it coz I couldn't get it 

namespace VoicePOE
{
    public class Chatbot
    {
        
        // Property to store the user's name for personalization
        public string UserName { get; set; }

        // This method runs the tasks in the correct order
        public void Start()
        {
            PlayVoiceGreeting();//Voice
            DisplayAsciiHeader(); //Logo and Colors
            PerformSetup();// Name and Validation
            RunChatLoop(); // The Conversation
        }

        //Typing effect to simulate a real conversation
        private void TypeWrite(string text)
        {
            foreach (char c in text)
            {
                Write(c);
                Thread.Sleep(30); // 30ms delay makes it look like the bot is typing
            }
            WriteLine();
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                // Using your logic to find the file in the project folder
                string path_directory = AppDomain.CurrentDomain.BaseDirectory;
                string recordPath = path_directory.Replace("\\bin\\Debug", "");
                string record = Path.Combine(recordPath, "Halo!.wav");

                using (SoundPlayer speechObj = new SoundPlayer(record))
                {
                    speechObj.PlaySync(); // Plays audio before showing the text
                }
            }
            catch (Exception error)
            {
                // Graceful error handling
                WriteLine($"[Audio System Note]: {error.Message}");
            }
        }

        private void DisplayAsciiHeader()
        {
            // ASCII Art and Colour Formatting
            ForegroundColor = ConsoleColor.Cyan;
            WriteLine("=========================================================");
            // Using @ allows us to print the ASCII art easily
            WriteLine(@"
              _______     ______  ______ _____  
             / ____\ \   / /  _ \|  ____|  __ \ 
            | |     \ \_/ /| |_) | |__  | |__) |
            | |      \   / |  _ <|  __| |  _  / 
            | |____   | |  | |_) | |____| | \ \ 
             \_____|  |_|  |____/|______|_|  \_\
                                                  
                CYBERSECURITY AWARENESS BOT");
            WriteLine("=========================================================");
            ResetColor(); // Very important, reset so the whole screen isn't blue
        }

        private void PerformSetup()
        {
            ForegroundColor = ConsoleColor.Yellow;
            TypeWrite("Bot: Hello! I am your Cybersecurity Assistant.");
            TypeWrite("Bot: To get started, please tell me your name:");
            ResetColor();

            Write("User Name: ");
            string input = ReadLine();

            // Input Validation, checks for empty entries
            while (string.IsNullOrWhiteSpace(input))
            {
                ForegroundColor = ConsoleColor.Red;
                TypeWrite("Bot: I'm sorry, I need a name to continue. Please enter your name:");
                ResetColor();
                input = ReadLine();
            }

            UserName = input; // Save name for Task 3
            TypeWrite($"\nBot: Welcome, {UserName}! How can I help you stay safe today?");
            WriteLine("-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
        }

        private void RunChatLoop()
        {
            bool active = true;
            while (active)
            {
                //spacing for readability
                WriteLine("\n");
                ForegroundColor = ConsoleColor.White;
                Write($"\n{UserName} > ");
                string input = ReadLine()?.ToLower().Trim(); // Handle input gracefully

                WriteLine("-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");

                //Slight delay to simulate thinking
                Thread.Sleep(500);

                if (string.IsNullOrEmpty(input))
                {
                    TypeWrite("Bot: I didn't quite understand that. Could you please rephrase?");
                }
                // Predefined responses for specific questions
                else if (input.Contains("how are you"))
                {
                    TypeWrite($"Bot: I'm feeling secure and ready to help, {UserName}!");
                }
                else if (input.Contains("purpose"))
                {
                    TypeWrite("Bot: My purpose is to help you understand online threats like phishing.");
                }
                else if (input.Contains("ask you") || input.Contains("help"))
                {
                    TypeWrite("Bot: You can ask me about 'passwords', 'phishing', or 'safe browsing'.");
                }
                else if (input.Contains("password"))
                {
                    TypeWrite("Bot: [TIP] Use a passphrase of 4 random words for better security.");
                }
                else if (input.Contains("phishing"))
                {
                    TypeWrite("Bot: [TIP] Check the sender's email address carefully before clicking links.");
                }
                else if (input.Contains("exit") || input.Contains("bye"))
                {
                    TypeWrite($"Bot: Goodbye {UserName}. Remember to update your software regularly!");
                    active = false; // Ends the loop and closes the app
                }
                else
                {
                    // Default response for unsupported queries
                    TypeWrite("Bot: I'm not sure about that yet. Try asking 'What can I ask you about?'");
                }
                WriteLine("-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
            }
        }
    }
}