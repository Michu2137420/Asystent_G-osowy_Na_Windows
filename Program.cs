using System;
using System.Globalization;
using System.Speech.Recognition;
using MatrixAnimation;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SpeechRecognitionApp
{
    class Program
    {
        private bool Active = false;
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Umożliwia wyświetlanie polskich znaków w konsoli
            MatrixAnimation.AnimacjaMatrix Animacja = new MatrixAnimation.AnimacjaMatrix();
            Program program = new Program();
            Animacja.DisplayAnimation();
            program.Initialization();
            

        }

        private void Initialization()
        {
            LoadSpeechRecognition();
        }

        private void LoadSpeechRecognition()
        {
            try
            {
                // Create a SpeechRecognitionEngine instance with the desired culture
                CultureInfo culture = new CultureInfo("en-US");
                SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(culture);

                // Create a Choices object with the commands
                Choices commands = new Choices(new string[] { "open browser", "close application", "play music", "start", "off","open kompilator" });

                // Create a GrammarBuilder and set its culture to match the recognizer
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Culture = culture;
                grammarBuilder.Append("Monday");
                grammarBuilder.Append(commands);

                // Create a Grammar from the GrammarBuilder
                Grammar grammar = new Grammar(grammarBuilder);

                // Load the grammar into the recognizer
                recognizer.LoadGrammar(grammar);

                // Register the SpeechRecognized event handler
                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

                // Set the input to the default audio device
                recognizer.SetInputToDefaultAudioDevice();

                // Start asynchronous, continuous speech recognition
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                Console.WriteLine("\nAsystent 'Monday' jest aktywny. Powiedz 'Monday start', aby go uruchomić. Naciśnij Enter, aby zakończyć...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas inicjalizacji rozpoznawania mowy: " + ex.Message);
            }
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string command = e.Result.Text.ToLower().Replace("monday", "").Trim();

            if (command == "start")
            {
                Active = true;
                Console.WriteLine("Asystent Monday został aktywowany.");
                Console.WriteLine("Możesz teraz wydawać polecenia.");
                return;
            }

            if (command == "off")
            {
                Active = false;
                Console.WriteLine("Asystent Monday został uśpiony.");
                return;
            }

            if (!Active)
            {
                Console.WriteLine("Asystent Monday jest wyłączony lub uśpiony. Należy go włączyć za pomocą komendy 'Monday start'.");
                return;
            }

            switch (command)
            {
                case "open browser":
                    Console.WriteLine("Otwieranie przeglądarki...");
                    OpenBrowser("https://www.google.com");
                    break;
                case "close application":
                    Console.WriteLine("Zamykanie aplikacji...");
                    CloseApplication();
                    break;
                case "play music":
                    Console.WriteLine("Odtwarzanie muzyki...");
                    OpenBrowser("https://open.spotify.com/");
                    break;
                case "open kompilator":
                    Console.WriteLine("Otwieranie kompilatora...");
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = @"C:\Program Files (x86)\Dev-Cpp\devcpp.exe",
                        UseShellExecute = true
                    };
                    Process.Start(startInfo);
                    break;
                default:
                    Console.WriteLine("Nie rozpoznano polecenia.");
                    break;
            }
            
        }
        private void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        private void CloseApplication()
        {
            Environment.Exit(0);
        }
    }
}

