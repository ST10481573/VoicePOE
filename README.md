# Cybersecurity Awareness Chatbot (POE Part 3)

## Project Overview
This project is Part 3 of the PROG6221 Practical Outcome Evaluation (POE).  
It builds on the earlier console and GUI chatbot versions by adding more advanced features and polishing the application for final submission.

The chatbot is designed to **educate users about cybersecurity** while also feeling interactive and engaging. It now includes a task assistant, a quiz game, and an activity log, alongside memory, sentiment detection, and voice features.

---

## Features

### Core Chatbot
- **WPF GUI** with tabs for Chatbot, Tasks, Quiz, and Activity Log  
- **ASCII Art Banner** displayed at startup  
- **Small talk**: greetings, “how are you?”, casual replies  
- **Cybersecurity topics**: passwords, phishing, privacy, scams, malware, updates  
- **Memory**: remembers your name and feelings, can recall them later  
- **Sentiment detection**: empathetic responses when you say you’re worried or frustrated  
- **Voice interaction**:
  - Plays a custom WAV greeting (`Halo!.wav`)  
  - Speaks responses with SSML for natural pauses and emphasis  
- **Randomized tips**: each topic gives a different tip for variety  

### Task Assistant
- Add, view, complete, and delete tasks  
- Tasks stored in a **MySQL database** (`CyberBotDB`)  
- Each task includes title, description, optional reminder date, and completion status  
- GUI integration for managing tasks directly  

### Quiz Game
- Interactive **cybersecurity quiz** with multiple choice and true/false questions  
- Immediate feedback after each answer  
- Tracks progress until quiz completion  
- Designed to reinforce cybersecurity awareness in a fun way  

### Activity Log
- Records chatbot actions such as:
  - Tasks added, completed, or deleted  
  - Quiz activity  
  - Memory interactions  
- Accessible via the Activity Log tab in the GUI  

---

## How to Run
1. Clone the repository:
   ```bash
   git clone < https://youtu.be/XqyBiLfi7Ns >
