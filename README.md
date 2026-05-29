# Cybersecurity Awareness Chatbot (POE Part 2)

 Project Overview
This project is Part 2 of the PROG6221 Practical Outcome Evaluation (POE).  
It expands on the Part 1 console chatbot by building a **GUI application** with added features:
- Memory (remembers user details like name and feelings)
- Sentiment detection (responds empathetically to emotions)
- Voice interaction (WAV greeting + text-to-speech with SSML)
- ASCII art display
- Randomized cybersecurity tips

The chatbot is designed to **educate users about cybersecurity** while also feeling more alive and interactive through small talk and contextual replies.

 Features
- **Graphical User Interface (GUI)** built with WPF.
- **ASCII Art Banner** displayed at startup.
- **Chatbot Responses**:
  - Cybersecurity topics: passwords, phishing, privacy, scams, malware, updates.
  - Small talk: greetings, “how are you?”, “I’m good and you?”.
  - Memory: remembers your name and feelings.
  - Contextual replies: “I remembered you said…” references last topic.
- **Sentiment Detection**: empathetic responses when user expresses worry or frustration.
- **Voice Interaction**:
  - Plays a custom WAV greeting (`Halo!.wav`).
  - Speaks responses using `SpeechSynthesizer` with SSML for natural pauses and emphasis.
- **Randomized Tips**: each topic gives a different tip for variety.


---

## 🚀 How to Run
1. Clone the repository:
   ```bash
   git clone <https://youtu.be/PDz6JnDVBBY>
