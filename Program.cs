using System;
using static System.Console; //import for using the WriteLine without writing Console

namespace VoicePOE
{
    public class Program
    {
        static void Main(string[] args)
        {
            // We create an 'object' of our Chatbot class
            Chatbot myBot = new Chatbot();

            // We call the Start method to begin the application
            myBot.Start();
        }
    }
}