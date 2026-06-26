using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.VisualBasic;
namespace POE_part2
{
    public class CyberSecurityQuiz
    {
      private List<Question> questions;
        private int currentQuestion;
        private int score;

        public CyberSecurityQuiz()
        {
            questions = new List<Question>();

            questions.Add(new Question(
                "What should you do if you receive an email asking for your password?",
                new string[]
                {
                    "Reply with your password",
                    "Delete the email",
                    "Report it as phishing",
                    "Ignore it"
                },
                2));

            questions.Add(new Question(
                "True or False: You should use the same password everywhere.",
                new string[]
                {
                    "True",
                    "False"
                },
                1));

            questions.Add(new Question(
                "Which password is strongest?",
                new string[]
                {
                    "123456",
                    "Password",
                    "John123",
                    "J@9vP!x2L#8"
                },
                3));

            questions.Add(new Question(
                "Public Wi-Fi is always safe.",
                new string[]
                {
                    "True",
                    "False"
                },
                1));

            questions.Add(new Question(
                "What does HTTPS mean for a website?",
                new string[]
                {
                    "Secure connection",
                    "Faster internet",
                    "Free website",
                    "Virus free"
                },
                0));

            currentQuestion = 0;
            score = 0;
        }

        public Question GetCurrentQuestion()
        {
            return questions[currentQuestion];
        }

        public bool CheckAnswer(int answer)
        {
            if (answer == questions[currentQuestion].CorrectAnswer)
            {
                score++;
                return true;
            }

            return false;
        }

        public bool NextQuestion()
        {
            currentQuestion++;
            return currentQuestion < questions.Count;
        }

        public int Score
        {
            get { return score; }
        }

        public int TotalQuestions
        {
            get { return questions.Count; }
        }


    }
}