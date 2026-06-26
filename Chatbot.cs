using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;  // make sure this is at the top

namespace POE_Part3
{
    public class Chatbot
    {
        private string connectionString = "server=localhost;user=root;password=Kaymo26611;database=CyberBotDB;";
        private Dictionary<string, string> responses;
        private List<string> phishingTips;
        private List<string> passwordTips;
        private List<string> privacyTips;
        private List<string> activityLog;
        private List<Question> quizQuestions;
        private int currentQuizIndex;

        private string lastTopic;
        private Dictionary<string, string> memory;
        private Random rand;

        public Chatbot()
        {
            responses = new Dictionary<string, string>()
            {
                {"scam", "Scams usually try to scare you or rush you. Always double check before giving info."},
                {"malware", "Malware often hides in downloads or attachments. Keep antivirus updated."},
                {"update", "Updates fix security holes. Don’t skip them."}
            };

            phishingTips = new List<string>()
            {
                "Watch out for emails asking for personal info.",
                "Hover over links before clicking to see where they go.",
                "If something feels off, slow down and check.",
                "Bad spelling or weird formatting is often a red flag."
            };

            passwordTips = new List<string>()
            {
                "Use long passphrases instead of short words.",
                "Don’t use birthdays or names.",
                "Turn on two-factor authentication.",
                "Password managers help keep track of unique passwords."
            };

            privacyTips = new List<string>()
            {
                "Check your social media privacy settings.",
                "Think twice before sharing personal details online.",
                "Use encrypted apps for sensitive chats.",
                "Clear cookies and history to reduce tracking."
            };

            memory = new Dictionary<string, string>();
            rand = new Random();
            activityLog = new List<string>();
            quizQuestions = LoadQuizQuestions();
            currentQuizIndex = 0;
        }

        // --- NLP + Responses ---
        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "I didn’t catch that, can you say it differently?";

            input = input.Trim();
            string lowerInput = input.ToLower();
            string[] words = lowerInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // --- NLP triggers ---
            if (lowerInput.Contains("add task") || lowerInput.Contains("remind"))
            {
                activityLog.Add($"User requested to add a task at {DateTime.Now}");
                return "Sure! Please provide the task title and description.";
            }

            if (lowerInput.Contains("quiz") || lowerInput.Contains("game"))
            {
                activityLog.Add($"Quiz started at {DateTime.Now}");
                return StartQuiz();
            }

            if (lowerInput.Contains("show activity log") || lowerInput.Contains("what have you done"))
            {
                return ShowActivityLog();
            }

            // --- Small Talk ---
            if (words.Contains("hi") || words.Contains("hello"))
                return "Hey there! Nice to see you. How are you doing today?";

            if (lowerInput.Contains("how are you"))
                return "I’m feeling great, thanks for asking. Always ready to chat about security or just hang out. How about you?";

            if (lowerInput.Contains("i'm good") || lowerInput.Contains("im good"))
                return "That’s awesome! Glad you’re doing well.";

            // --- Name memory ---
            if (lowerInput.StartsWith("my name is"))
            {
                string name = input.Substring(10).Trim();
                memory["username"] = name;
                activityLog.Add($"Remembered user name: {name} at {DateTime.Now}");
                return $"Cool, I’ll remember your name is {name}.";
            }

            if (lowerInput.Contains("what is my name") && memory.ContainsKey("username"))
                return $"You told me your name is {memory["username"]}.";

            // --- Feelings memory ---
            if (lowerInput.StartsWith("i feel"))
            {
                string feeling = input.Substring(6).Trim();
                memory["lastFeeling"] = feeling;
                activityLog.Add($"User feeling recorded: {feeling} at {DateTime.Now}");
                return $"I hear you. You said you feel {feeling}. Thanks for sharing that.";
            }

            if (lowerInput.Contains("how did i feel") && memory.ContainsKey("lastFeeling"))
                return $"Earlier you mentioned feeling {memory["lastFeeling"]}. Do you still feel that way?";

            // --- Security topics ---
            if (lowerInput.Contains("password"))
            {
                lastTopic = "password";
                activityLog.Add("Shared password tip");
                return $"Here’s a password tip: {passwordTips[rand.Next(passwordTips.Count)]}";
            }

            if (lowerInput.Contains("privacy"))
            {
                lastTopic = "privacy";
                activityLog.Add("Shared privacy tip");
                return $"Here’s a privacy tip: {privacyTips[rand.Next(privacyTips.Count)]}";
            }

            if (lowerInput.Contains("phishing"))
            {
                lastTopic = "phishing";
                activityLog.Add("Shared phishing tip");
                return $"Here’s something to watch out for: {phishingTips[rand.Next(phishingTips.Count)]}";
            }

            if (responses.ContainsKey(lowerInput))
            {
                lastTopic = lowerInput;
                activityLog.Add($"Shared response for {lowerInput}");
                return responses[lowerInput];
            }

            // --- Continue topic ---
            if (lowerInput == "more")
            {
                if (lastTopic == "phishing")
                    return $"Another phishing tip: {phishingTips[rand.Next(phishingTips.Count)]}";
                if (lastTopic == "password")
                    return $"Another password tip: {passwordTips[rand.Next(passwordTips.Count)]}";
                if (lastTopic == "privacy")
                    return $"Another privacy tip: {privacyTips[rand.Next(privacyTips.Count)]}";
            }

            // --- Mood detection ---
            if (lowerInput.Contains("worried") || lowerInput.Contains("frustrated") || lowerInput.Contains("scared"))
                return "I get that. Security stuff can feel stressful, but small steps really help. You’re not alone in this.";

            // --- Exit ---
            if (words.Contains("exit") || words.Contains("bye"))
                return "Catch you later! Stay safe online.";

            // --- Fallback ---
            return "Hmm, I’m not sure about that one. Try asking me about tasks, quiz, activity log, or security topics.";
        }

        // --- Task Assistant Stub ---
        public string AddTask(string title, string description, DateTime? reminderDate)
        {
            string reminderText = reminderDate.HasValue ? reminderDate.Value.ToShortDateString() : "None";
            activityLog.Add($"Task added: {title} (Reminder: {reminderText}) at {DateTime.Now}");
            return $"Task added: '{title}'. Would you like me to remind you?";
        }

        // --- Quiz Methods ---
        private List<Question> LoadQuizQuestions()
        {
            return new List<Question>
    {
        new Question("What should you do if you receive an email asking for your password?",
            new List<string>{ "Reply with your password", "Delete the email", "Report as phishing", "Ignore it" }, 2,
            "Correct! Reporting phishing emails helps prevent scams."),

        new Question("True or False: Using '123456' as a password is safe.",
            new List<string>{ "True", "False" }, 1,
            "False! Weak passwords are easy to guess."),

        new Question("Which of these is the strongest password?",
            new List<string>{ "Kamohelo123", "Password!", "Tr0ub4dor&3", "MyDogIsCute2026!" }, 3,
            "Correct! Long passphrases with mixed characters are strongest."),

        new Question("True or False: You should click links in emails without checking them.",
            new List<string>{ "True", "False" }, 1,
            "False! Always hover over links to check where they go."),

        new Question("What does 2FA stand for?",
            new List<string>{ "Two-Factor Authentication", "Two-Firewall Access", "Two-Files Allowed", "Two-Factor Authorization" }, 0,
            "Correct! Two-Factor Authentication adds an extra layer of security."),

        new Question("True or False: Public Wi-Fi is always safe to use for banking.",
            new List<string>{ "True", "False" }, 1,
            "False! Public Wi-Fi can expose your data. Use a VPN."),

        new Question("Which of these is a sign of phishing?",
            new List<string>{ "Urgent tone", "Spelling mistakes", "Suspicious links", "All of the above" }, 3,
            "Correct! All of these are common phishing signs."),

        new Question("True or False: Antivirus software should be updated regularly.",
            new List<string>{ "True", "False" }, 0,
            "True! Updates keep your system protected against new threats."),

        new Question("What should you do if your account is hacked?",
            new List<string>{ "Ignore it", "Change your password", "Tell your friends", "Delete the account" }, 1,
            "Correct! Change your password immediately and enable 2FA."),

        new Question("True or False: Sharing your password with a friend is safe.",
            new List<string>{ "True", "False" }, 1,
            "False! Never share your password with anyone."),

        new Question("Which of these is the safest way to store passwords?",
            new List<string>{ "Write them on paper", "Save in a text file", "Use a password manager", "Memorize only one password" }, 2,
            "Correct! Password managers securely store unique passwords for each account.")
    };
        }

        public Question GetCurrentQuestion()
        {
            if (currentQuizIndex < quizQuestions.Count)
                return quizQuestions[currentQuizIndex];
            return null;
        }



        // --- Add Task to DB ---
        public string AddTask_Click(string title, string description, DateTime? reminderDate)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Tasks (Title, Description, ReminderDate) VALUES (@title, @desc, @reminder)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@desc", description);
                    cmd.Parameters.AddWithValue("@reminder", reminderDate.HasValue ? (object)reminderDate.Value : DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }

            activityLog.Add($"Task added: {title} (Reminder: {reminderDate?.ToShortDateString() ?? "None"}) at {DateTime.Now}");
            return $"Task added: '{title}'. Would you like me to remind you?";
        }

        // --- View Tasks ---
        public List<string> GetTasks()
        {
            var tasks = new List<string>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT TaskID, Title, Description, ReminderDate, IsCompleted FROM Tasks";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string reminder = reader["ReminderDate"] == DBNull.Value ? "None" : Convert.ToDateTime(reader["ReminderDate"]).ToShortDateString();
                        string status = (bool)reader["IsCompleted"] ? "Completed" : "Pending";
                        tasks.Add($"{reader["TaskID"]}: {reader["Title"]} - {reader["Description"]} (Reminder: {reminder}) [{status}]");
                    }
                }
            }
            return tasks;
        }

        // --- Delete Task ---
        public string DeleteTask(int taskId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Tasks WHERE TaskID=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
            activityLog.Add($"Task deleted: {taskId} at {DateTime.Now}");
            return $"Task {taskId} deleted successfully.";
        }

        // --- Mark Task Completed ---
        public string CompleteTask(int taskId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE Tasks SET IsCompleted=1 WHERE TaskID=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
            activityLog.Add($"Task completed: {taskId} at {DateTime.Now}");
            return $"Task {taskId} marked as completed.";
        }


        public string StartQuiz()
        {
            currentQuizIndex = 0;
            return quizQuestions[currentQuizIndex].Text;
        }

        public string SubmitAnswer(int choice)
        {
            var q = quizQuestions[currentQuizIndex];
            string feedback = (choice == q.CorrectIndex) ? "Correct! " + q.Explanation : "Oops! " + q.Explanation;
            activityLog.Add($"Quiz answered: {q.Text} at {DateTime.Now}");
            currentQuizIndex++;
            if (currentQuizIndex < quizQuestions.Count)
                feedback += "\nNext question: " + quizQuestions[currentQuizIndex].Text;
            else
                feedback += "\nQuiz completed!";
            return feedback;
        }

        // --- Activity Log ---
        public string ShowActivityLog()
        {
            int count = activityLog.Count;
            int start = Math.Max(0, count - 5);
            var recent = activityLog.Skip(start).Take(5);
            return "Here’s a summary of recent actions:\n" + string.Join("\n", recent);
        }
    }

    public class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }

        public Question(string text, List<string> options, int correctIndex, string explanation)
        {
            Text = text;
            Options = options;
            CorrectIndex = correctIndex;
            Explanation = explanation;
        }
    }
}
