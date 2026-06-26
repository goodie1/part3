using System;                        
using System.Collections.Generic;      
using System.IO;                      
using System.Linq;                     
using System.Text;                     
using System.Text.RegularExpressions; 
using System.Threading.Tasks;          
using System.Windows;                  
using System.Windows.Controls;         
using System.Windows.Data;             
using System.Windows.Documents;        
using System.Windows.Input;            
using System.Windows.Media;           
using System.Windows.Media.Imaging;    
using System.Windows.Navigation;      
using System.Windows.Shapes;          


namespace POE_part2
{

       public partial class MainWindow : Window
    {
        // Quiz
        CyberSecurityQuiz quiz = new CyberSecurityQuiz();
        private bool quizStarted = false;



        //create an instance for the class tasks
        tasks manage_tasks = new tasks();

        //global variables to hold task details
        string task_name, task_description, task_dudate, task_status = string.Empty;


        // Stores the user's name entered during login
        string userName = "";

        // Random object used to randomly select chatbot responses
        Random random = new Random();

        // File name used to store chatbot memory information
        string memoryFile = "memory.txt";

        // Keeps track of the last cybersecurity topic discussed
        string currentTopic = "";

        // Paragraph used for displaying "typing..." effect
        Paragraph typingParagraph;

        NLPProcessor nlp = new NLPProcessor();

        // Dictionary storing cybersecurity topics and related responses
        private readonly Dictionary<string, string[]> cyberResponses =

             new Dictionary<string, string[]>()
             {

               // PHISHING RESPONSES
               {
                    "phishing",
                    new string[]
                    {
                     // Different phishing explanations and solutions
                     "Email phishing happens when cybercriminals send fake emails pretending to be trusted companies or people to steal personal information such as passwords or banking details. These emails often contain suspicious links or attachments.\r\nSolution: Do not click unknown links or attachments.Verify the sender’s email address.Use spam filters and antivirus software.",

                     "Clone phishing involves copying a legitimate email and replacing safe links or attachments with malicious ones. The attacker resends the email pretending it is an updated version.\r\nSolution: Double-check email attachments and links before opening them.Verify unusual email requests with the sender.Keep software and security systems updated.",

                     "Spear phishing is a targeted phishing attack aimed at a specific person or organisation. Attackers use personal information to make the message appear more believable.\r\nSolution: Confirm requests for sensitive information through another communication method.Train employees to recognise suspicious messages.Enable multi-factor authentication (MFA).",

                     "Smishing uses fake text messages to trick users into clicking malicious links or sharing confidential information. The messages may claim to be from banks, delivery companies, or service providers.\r\nSolution: Avoid clicking links in unexpected text messages.Block and report suspicious numbers.Use mobile security applications."
                    }
               },

                // MALWARE RESPONSES
                {
                    "malware",
                    new string[]
                    {
                      "A virus is a type of malware that attaches itself to files or programs and spreads when the infected file is opened. It can corrupt data, slow down the computer, or damage the operating system.\r\nSolution: Install and regularly update antivirus software, avoid downloading unknown files, and scan USB devices before use.",

                      "A worm is malware that spreads automatically through networks without needing user interaction. It can overload systems, consume bandwidth, and spread rapidly between computers.\r\nSolution: Keep operating systems updated, use firewalls, and avoid connecting to unsecured networks.",

                      "A Trojan Horse disguises itself as legitimate software to trick users into installing it. Once installed, it can steal information, give hackers access, or damage files.\r\nSolution: Only download software from trusted sources, avoid suspicious email attachments, and use security software to detect threats.",

                      "Spyware secretly monitors user activity and collects personal information such as passwords, banking details, or browsing history. It often runs without the user noticing.\r\nSolution: Use anti-spyware tools, avoid visiting unsafe websites, enable two-factor authentication, and regularly update passwords."
                    }

                },

                // RANSOMWARE RESPONSES
                {
                    "ransomware",
                    new string[]
                     {
                         "Mobile ransomware targets smartphones and tablets by locking the screen or encrypting files stored on the device. It often spreads through malicious apps or unsafe downloads.\r\nSolution: Download apps only from official app stores.Keep mobile devices updated.Enable cloud backups so files can be restored if infected.",

                         "Scareware pretends to be legitimate security software and displays fake warnings about viruses or system problems. It pressures users into paying money for fake fixes or software.\r\nSolution: Install software only from trusted websites or app stores.Ignore pop-up warnings that demand urgent payment.Run a genuine antivirus scan to remove the malicious program.",

                         "Locker ransomware locks users out of their devices completely, preventing access to the operating system or applications. A message then appears demanding payment to unlock the device.\r\nSolution: Keep operating systems and software updated with security patches.Use strong passwords and multi-factor authentication.Boot the device in safe mode and use trusted security tools to remove the malware.",

                         "This type of ransomware encrypts files on a computer or network, making them inaccessible until a ransom is paid. Victims are usually asked to pay in cryptocurrency to receive a decryption key.\r\nSolution: Regularly back up important files to an external drive or cloud storage.Use updated antivirus software.Avoid opening suspicious email attachments or links."
                     }

                },

                 // PASSWORD RESPONSES
                 {
                     "passwords",
                     new string[]
                     {
                        "Weak passwords are short or easy to guess, such as “123456” or “password,” making accounts vulnerable to hackers.\r\nSolution: Create strong passwords using a mix of uppercase letters, lowercase letters, numbers, and symbols.",

                        "Users may forget complex passwords, which can prevent access to important accounts and systems.\r\nSolution: Use password recovery options, security questions, or a trusted password manager to securely store passwords.",

                        "Password reuse happens when the same password is used for multiple accounts, increasing the risk if one account is hacked.\r\nSolution: Use a different password for every account and store them safely in a password manager.",

                        "Sharing passwords with friends, coworkers, or other users can lead to unauthorized access and security breaches.\r\nSolution: Never share passwords and enable two-factor authentication (2FA) for extra protection."
                     }
                 },

                 // PRIVACY RESPONSES
                 {
                     "privacy",
                     new string[]
                     {
                         "Data privacy refers to protecting personal or sensitive information from unauthorized access, sharing, or misuse. This includes information such as passwords, banking details, medical records, and contact information.\r\nSolution: Use strong passwords, encryption, and access controls to keep data secure.",

                         "Online privacy involves keeping a person’s activities, searches, and personal details safe while using the internet. Websites and apps may collect user information without permission.\r\nSolution: Adjust privacy settings, avoid sharing sensitive information online, and use secure websites (HTTPS).",

                         "Physical privacy refers to a person’s right to personal space and protection from unauthorized observation or intrusion. Examples include searching someone’s belongings without permission or unauthorized surveillance.\r\nSolution: Use security measures such as locked rooms, ID verification, and privacy policies to protect individuals’ personal space.",

                         "Communication privacy ensures that conversations, emails, messages, and phone calls remain confidential between the intended people. Hackers or unauthorized users may intercept communication.\r\nSolution: Use secure communication tools, encryption, and two-factor authentication."

                     }
                 }
             };

        // Dictionary containing keywords linked to specific topics
        Dictionary<string, string[]> topicKeyWord = new Dictionary<string, string[]>()
        {
            { "phishing", new string[]{ "fake emails", "clone", "phishing" } },
            { "malware", new string[]{ "virus", "spyware", "trojan", "malware"} },
            {"ransomware", new string[]{"Ransomware", "Scareware", "Mobile"}},
            {"passwords", new string[]{"weak","password","forgotten"}},
            {"privacy", new string[]{"communication","online","physical","privacy"}}
        };

        
        public MainWindow()
        {
            
            InitializeComponent();

            //call testing method for connection
            manage_tasks.test_connection();

            // Creates an object that plays a voice greeting
            VoiceGreeting voiceobj = new VoiceGreeting();
        }

        // Runs when the Start button is clicked
        public void Start_Click(object sender, RoutedEventArgs e)
        {
            // Hide welcome screen
            WelcomeGrid.Visibility = Visibility.Collapsed;

            // Show name input screen
            NameGrid.Visibility = Visibility.Visible;

        }

        // Runs when the Submit button is clicked
        public void Submit_Click(object sender, RoutedEventArgs e)
        {

            // Remove unnecessary spaces from textbox input
            string name = NameTextBox.Text.Trim();

            // Check if textbox is empty
            if (string.IsNullOrWhiteSpace(name))
            {
                ErrorText.Text = "Name cannot be empty \n\rPlese re-enter your name";
                return;
            }

            // Validate that only letters are entered
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            {
                ErrorText.Text = "Enter a valid name \n\rPlese re-enter your name";
                return;

            }

            // Check minimum name length
            if (name.Length < 2)
            {
                ErrorText.Text = "Name should be more then two characters long...\n\rPlese re-enter your name";
                return;
            }

            // Store user's name
            userName = name;

            // Clear any error message
            ErrorText.Text = "";

            // Hide name screen
            NameGrid.Visibility = Visibility.Collapsed;

            // Show chat screen
            ChatGrid.Visibility = Visibility.Visible;

            // Display welcome message and chatbot menu
            AddStyledMessage("Chatbot",
            $"Welcome Mr/Mrs: {userName} to AI assistance...\n" +
            "1: Phishing\n" +
            "2: Malware\n" +
            "3: Ransomware\n" +
            "4: Passwords\n" +
            "5: Privacy", false);

        }

        // Runs when the Send button is clicked
        public async void Send_Click(object sender, RoutedEventArgs e)
        {
            // 1. Get input ONCE
            string message = MessageBox.Text.Trim();
            string lowerMessage = message.ToLower();

            // 2. Validate empty input
            if (string.IsNullOrWhiteSpace(message))
            {
                AddStyledMessage("Chatbot", "Please enter a message.", false);
                return;
            }

            // 3. Show user message
            AddStyledMessage(userName, message, true);

            // 4. CLEAR textbox early (prevents reuse bugs)
            MessageBox.Clear();

            // =========================
            // 5. QUIZ LOGIC (highest priority)
            // ========================
            if (quizStarted)
            {
                if (!int.TryParse(lowerMessage, out int answer))
                {
                    AddStyledMessage("Quiz", "Please enter a number (1-4).", false);
                    return;
                }

                answer--; // convert to 0-based index

                if (quiz.CheckAnswer(answer))
                {
                    AddStyledMessage("Quiz", "Correct!", false);
                }
                else
                {
                    AddStyledMessage("Quiz", "Incorrect!", false);
                }

                if (quiz.NextQuestion())
                {
                    AskQuestion();
                }
                else
                {
                    AddStyledMessage(
                        "Quiz",
                        $"Quiz Finished!\nScore: {quiz.Score}/{quiz.TotalQuestions}",
                        false
                    );

                    nlp.LogActivity($"Quiz completed. Score: {quiz.Score}/{quiz.TotalQuestions}");

                    quizStarted = false;
                }

                return;
            }

            //  QUIZ START (FIXED - VERY IMPORTANT)
           
            bool isQuizStart =
                lowerMessage == "quiz" ||
                lowerMessage.Contains("start quiz") ||
                lowerMessage.Contains("quiz start") ||
                lowerMessage.Contains("do a quiz") ||
                lowerMessage.Contains("give me a quiz") ||
                lowerMessage.Contains("i want a quiz") ||
                (lowerMessage.Contains("quiz") && lowerMessage.Contains("start"));

            if (isQuizStart)
            {
                quizStarted = true;

                nlp.LogActivity("Quiz started.");

                AddStyledMessage(
                    "Quiz",
                    "Cybersecurity Quiz Started!\nReply with 1, 2, 3 or 4.",
                    false
                );

                AskQuestion();
                return;
            }


            // =========================
            // 6. TASK / REMINDER LOGIC
            // =========================
            if (lowerMessage.StartsWith("add task"))
            {
                task_name = message.Replace("add task", "").Trim();
                task_description = "User created task";

                chats.Items.Add("Task added. Would you like a reminder?");
                return;
            }

            if (lowerMessage.StartsWith("yes, remind me in"))
            {
                string reminder = lowerMessage.Replace("yes, remind me in", "").Trim();

                string daysOnly = Regex.Replace(reminder, @"[^0-9]", "");

                if (int.TryParse(daysOnly, out int days))
                {
                    DateTime reminderDate = DateTime.Now.AddDays(days);
                    string formatted = reminderDate.ToString("MMMM dd yyyy");

                    task_dudate = formatted;
                    task_status = "pending";

                    manage_tasks.insert_task(task_name, task_description, task_dudate, task_status);

                    chats.Items.Add($"Reminder set in {days} days ({formatted})");
                }
                else
                {
                    AddStyledMessage("Chatbot", "Invalid reminder format.", false);
                }

                return;
            }

            // =========================
            // 7. MEMORY FEATURE
            // =========================
            if (lowerMessage.Contains("interested in"))
            {
                SaveToFile(lowerMessage);
                return;
            }

            if (lowerMessage.Contains("favorite topic"))
            {
                if (File.Exists(memoryFile))
                {
                    string saved = File.ReadAllText(memoryFile);
                    AddStyledMessage("Chatbot", $"Your favorite topic is: {saved}", false);
                }
                else
                {
                    AddStyledMessage("Chatbot", "No favorite topic saved yet.", false);
                }
                return;
            }

            // =========================
            // 8. CHATBOT RESPONSE (FINAL FALLBACK)
            // =========================

            await ShowTypingEffect();

            string botResponse = chatBotResponse(lowerMessage);

            AddStyledMessage("Chatbot", botResponse, false);
        }
        // Generates chatbot responses
        public string chatBotResponse(string message)
        {
            // Detect emotion/sentiment
            string sentiment = DetectSentiment(message);

            // Check if user is asking for more information
            bool moreInfor = isFollowUp(message);

            // Detect topic discussed
            string topic = DetectTopic(message);

            // If user asks follow-up question, continue previous topic
            if (string.IsNullOrEmpty(topic) && moreInfor && !string.IsNullOrEmpty(currentTopic))
            {
                topic = currentTopic;
            }

            // If topic is detected
            if (!string.IsNullOrEmpty(topic))
            {
                // Store current topic
                currentTopic = topic;

                // Build and return chatbot response
                return BuildResponses(topic, sentiment, moreInfor);
            }

            // If only sentiment detected
            if (!string.IsNullOrEmpty(sentiment))
            {
                return $"{GetSentimentSupport(sentiment)}, Tell me which cybersecurity topic is bothering you, such as phishing, malware, passwords, ransomwer,privacy and  I will assit step by step. ";
            }

            // Default chatbot message
            return "I am build to respond to cybersecurity related questions\n";
        }

        // Detects topic from user's message
        public string DetectTopic(string message)
        {
            // Check keywords dictionary
            foreach (var topic in topicKeyWord)
            {
                if (topic.Value.Any(word => message.Contains(word)))
                {
                    return topic.Key;
                }
            }

            // Check direct topic names
            foreach (var topic in cyberResponses)
            {
                if (message.Contains(topic.Key))
                {
                    return topic.Key;
                }
            }

            return "";
        }

        // Builds chatbot response
        public string BuildResponses(string topic, string sentiment, bool moreInfor)
        {
            // Get responses related to detected topic
            string[] foundResponce = cyberResponses[topic];

            // Generate random response
            int index = random.Next(foundResponce.Length);

            // Store selected response
            string responce = foundResponce[index];

            // Get emotional support message
            string support = GetSentimentSupport(sentiment);

            nlp.LogActivity("Discussed topic: " + topic);

            // Combine support and response
            return support + "\n\n" + responce;
        }

        // Provides emotional support responses
        public string GetSentimentSupport(string sentiment)
        {
            // If user feels worried
            if (sentiment == "worried")
            {
                return $"{userName}, it's okay to feel worried. I'm here to help you stay safe online,most cybersecurity issues can be fixed quickly let's make sure your information is safe.";
            }

            // If user feels frustrated
            if (sentiment == "frustrated")
            {
                return $"{userName}, I understand you're angry. let's try solve the issue together i'll help you fix the problem.";
            }

            return "";
        }

        // Detects emotional sentiment in user's message
        public string DetectSentiment(string message)
        {
            // Check worried-related words
            if (message.Contains("worried") ||
                message.Contains("anxious") ||
                message.Contains("nervous") ||
                message.Contains("unsure") ||
                message.Contains("afraid"))
            {
                return "worried";
            }

            // Check frustrated-related words
            if (message.Contains("frustrated") ||
                message.Contains("annoyed") ||
                message.Contains("angry") ||
                message.Contains("confused") ||
                message.Contains("stuck"))
            {
                return "frustrated";
            }

            return "";
        }

        // Checks if user is asking for more explanation
        public bool isFollowUp(string message)
        {
            return message.Contains("explain more") ||
                message.Contains("more details") ||
                message.Contains("i did not understand");
        }

        // Saves favorite topic to file
        public void SaveToFile(string message)
        {
            // Check if user said "interested in"
            if (message.Contains("interested in"))
            {
                // Extract topic from message
                string topic = message.Replace("i am interested in", "").Trim();

                // Save topic into text file
                File.WriteAllText(memoryFile, topic);

                nlp.LogActivity("Favourite topic saved: " + topic);
                // Notify user
                ChatListBox.AppendText($"Chatbot: I will remember that your favorite topic is {topic}\n");
            }

        }

        // Creates typing animation effect
        public async Task ShowTypingEffect()
        {
            // Create typing paragraph
            typingParagraph = new Paragraph(new Run("Chatbot is typing..."))
            {
                Background = Brushes.LightGray,
                Padding = new Thickness(8),
                Margin = new Thickness(5),
                FontStyle = FontStyles.Italic
            };

            // Display typing message
            ChatListBox.Document.Blocks.Add(typingParagraph);

            // Auto-scroll chat window
            ChatListBox.ScrollToEnd();

            // Delay for typing simulation
            await Task.Delay(1700);

            // Remove typing effect
            ChatListBox.Document.Blocks.Remove(typingParagraph);
        }


        // Displays styled chat messages in the RichTextBox
        public void AddStyledMessage(string sender, string message, bool isUser)
        {
            // Create paragraph container
            Paragraph paragraph = new Paragraph();

            // Create sender name text
            Run nameRun = new Run(sender + ": ");

            // Style user messages
            if (isUser)
            {
                nameRun.Foreground = Brushes.MediumPurple;
                nameRun.FontWeight = FontWeights.Bold;
            }

            // Style chatbot messages
            else
            {
                nameRun.Foreground = Brushes.DarkViolet;
                nameRun.FontWeight = FontWeights.Bold;
            }

            // Create message text
            Run messageRun = new Run(message);

            // User message text color
            if (isUser)
            {
                messageRun.Foreground = Brushes.Black;
            }

            // Chatbot message text color
            else
            {
                messageRun.Foreground = Brushes.Black;
            }

            // Add sender name into paragraph
            paragraph.Inlines.Add(nameRun);

            // Add message text into paragraph
            paragraph.Inlines.Add(messageRun);

            // Set paragraph background color
            paragraph.Background = isUser
                ? new SolidColorBrush(Color.FromRgb(45, 28, 127))
                : Brushes.Indigo;

            // Add internal spacing
            paragraph.Padding = new Thickness(10);

            // Add external spacing
            paragraph.Margin = new Thickness(5);

            // Display paragraph in chat area
            ChatListBox.Document.Blocks.Add(paragraph);

            // Automatically scroll to newest message
            ChatListBox.ScrollToEnd();
        }

        private void AskQuestion()
        {
            Question q = quiz.GetCurrentQuestion();

            string text = q.QuestionText + "\n\n";

            for (int i = 0; i < q.Options.Length; i++)
            {
                text += (i + 1) + ". " + q.Options[i] + "\n";
            }

            AddStyledMessage("Quiz", text, false);
        }

       
    }

}
