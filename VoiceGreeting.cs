using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Console;

namespace POE_part2
{
    internal class VoiceGreeting
    {
        public VoiceGreeting()
        {

            string path_directory = AppDomain.CurrentDomain.BaseDirectory;
            WriteLine($"{path_directory}");

            // Replace part of the path to adjust where the audio file is located
            string recoredPath = path_directory.Replace("bin\\Debug", "");

            // Combine the adjusted path with the audio file name 
            string recored = Path.Combine(recoredPath, "Welcome.wav");

            // Call method to play the audio file
            play_voice(recored);
        }

        // Method to play a voice/audio file
        public void play_voice(string voice)
        {
            try
            {
                // Create a SoundPlayer object using the file path
                using (SoundPlayer speechobj = new SoundPlayer(voice))
                {
                    speechobj.PlaySync(); // Play the audio file and wait until it finishes 
                }
            }
            catch (Exception error)
            {
                // If an error occurs, display the error message
                WriteLine($"{error.Message}");
            }
        }

    }
}
