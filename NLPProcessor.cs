using System;
using System.Collections.Generic;
using System.Linq;
namespace POE_part2
{
    public class NLPProcessor
    {
        // Stores recent activities
        private List<string> activityLog = new List<string>();

        // Stores the current task
        private string currentTask = "";

        // NLP keywords
        private readonly Dictionary<string, List<string>> keywords =
            new Dictionary<string, List<string>>()
        {
            {
                "task",
                new List<string>()
                {
                    "add task",
                    "create task",
                    "remember to",
                    "i need to",
                    "make a task"
                }
            },

            {
                "reminder",
                new List<string>()
                {
                    "remind me",
                    "set reminder",
                    "notify me",
                    "don't forget"
                }
            },

            {
                "activity",
                new List<string>()
                {
                    "activity log",
                    "show activity",
                    "history",
                    "what have you done",
                    "recent actions"
                }
            },

            {
                "quiz",
                new List<string>()
                {
                    "quiz",
                    "start quiz",
                    "test me"
                }
            }
        };

        // Detect what the user wants
        public string DetectIntent(string message)
        {
            message = message.ToLower();

            foreach (var item in keywords)
            {
                foreach (string word in item.Value)
                {
                    if (message.Contains(word))
                        return item.Key;
                }
            }

            return "";
        }

        // Add a task
        public string AddTask(string message)
        {
            currentTask = message;

            LogActivity("Task added: " + currentTask);

            return "Task added successfully.\nWould you like me to set a reminder?";
        }

        // Set reminder
        public string SetReminder()
        {
            if (currentTask == "")
                return "There is no task to remind you about.";

            LogActivity("Reminder created for: " + currentTask);

            return "Reminder set for:\n" + currentTask;
        }

        // Save activity
        public void LogActivity(string activity)
        {
            activityLog.Add(DateTime.Now.ToString("HH:mm")
                            + " - "
                            + activity);

            if (activityLog.Count > 10)
                activityLog.RemoveAt(0);
        }

        // Return activity log
        public string ShowActivity()
        {
            if (activityLog.Count == 0)
                return "No recent activity.";

            string output = "Recent Activity\n\n";

            foreach (string item in activityLog)
            {
                output += "• " + item + "\n";
            }

            return output;
        }
    }
}