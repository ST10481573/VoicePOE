using System;
using System.Collections.Generic;
using System.Linq;

namespace POE_Part2
{
    public class Chatbot
    {
        private Dictionary<string, string> responses;
        private List<string> phishingTips;
        private List<string> passwordTips;
        private List<string> privacyTips;

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
        }

        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "I didn’t catch that, can you say it differently?";

            // Normalize input
            input = input.Trim();
            string lowerInput = input.ToLower();
            string[] words = lowerInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


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
                return $"Cool, I’ll remember your name is {name}.";
            }

            if (lowerInput.Contains("what is my name") && memory.ContainsKey("username"))
                return $"You told me your name is {memory["username"]}.";

            // --- Feelings memory ---
            if (lowerInput.StartsWith("i feel"))
            {
                string feeling = input.Substring(6).Trim();
                memory["lastFeeling"] = feeling;
                return $"I hear you. You said you feel {feeling}. Thanks for sharing that.";
            }

            if (lowerInput.Contains("how did i feel") && memory.ContainsKey("lastFeeling"))
                return $"Earlier you mentioned feeling {memory["lastFeeling"]}. Do you still feel that way?";

            if (lowerInput.Contains("i remembered you said"))
            {
                if (!string.IsNullOrEmpty(lastTopic))
                    return $"Yes, I did mention {lastTopic}. Want me to expand on that?";
                else
                    return "Hmm, I don’t recall saying that yet.";
            }

            // --- Security topics ---
            if (lowerInput.Contains("password"))
            {
                lastTopic = "password";
                return $"Here’s a password tip: {passwordTips[rand.Next(passwordTips.Count)]}";
            }

            if (lowerInput.Contains("privacy"))
            {
                lastTopic = "privacy";
                return $"Here’s a privacy tip: {privacyTips[rand.Next(privacyTips.Count)]}";
            }

            if (lowerInput.Contains("phishing"))
            {
                lastTopic = "phishing";
                return $"Here’s something to watch out for: {phishingTips[rand.Next(phishingTips.Count)]}";
            }

            if (responses.ContainsKey(lowerInput))
            {
                lastTopic = lowerInput;
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
            return "Hmm, I’m not sure about that one. Try asking me about passwords, privacy, phishing, scams, or updates.";
        }
    }
}
