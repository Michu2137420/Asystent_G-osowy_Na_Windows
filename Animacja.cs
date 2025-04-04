using System;
using System.Threading;

namespace MatrixAnimation
{
    class AnimacjaMatrix
    {
        public void DisplayAnimation()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.CursorVisible = false;

            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            Random random = new Random();

            // Definiujemy kształt liter w napisie "Monday"
            string[] mShape = new string[]
            {
                "M       M",
                "MM     MM",
                "M M   M M",
                "M  M M  M",
                "M   M   M",
                "M       M",
                "M       M"
            };

            string[] oShape = new string[]
            {
                " OOOOO ",
                "O     O",
                "O     O",
                "O     O",
                "O     O",
                "O     O",
                " OOOOO "
            };

            string[] nShape = new string[]
            {
                "N     N",
                "NN    N",
                "N N   N",
                "N  N  N",
                "N   N N",
                "N    NN",
                "N     N"
            };

            string[] dShape = new string[]
            {
                "DDDDDD  ",
                "D     D ",
                "D     D ",
                "D     D ",
                "D     D ",
                "D     D ",
                "DDDDDD  "
            };

            string[] aShape = new string[]
            {
                "   A   ",
                "  A A  ",
                " A   A ",
                "AAAAAAA",
                "A     A",
                "A     A",
                "A     A"
            };

            string[] yShape = new string[]
            {
                "Y     Y",
                " Y   Y ",
                "  Y Y  ",
                "   Y   ",
                "   Y   ",
                "   Y   ",
                "   Y   "
            };

            string[][] letterShapes = new string[][] { mShape, oShape, nShape, dShape, aShape, yShape };
            int letterWidth = 10;
            int letterHeight = 7;
            int totalWidth = letterWidth * 6;
            int startX = (width - totalWidth) / 2;
            int startY = height / 4;

            // Tablica do śledzenia pozycji spadających liter w tle
            int[] yPositionsBackground = new int[width];
            for (int i = 0; i < width; i++)
            {
                yPositionsBackground[i] = random.Next(height); // Ustawiamy losową początkową pozycję
            }

            // Zmienna do śledzenia czasu trwania animacji
            DateTime startTime = DateTime.Now;

            bool wordFormed = false;
            while (!wordFormed)
            {
                // Sprawdzamy, czy minęło 5 sekund
                if ((DateTime.Now - startTime).TotalSeconds > 5)
                {
                    break;
                }

                Console.Clear();

                // Rysujemy spadające litery w tle
                for (int x = 0; x < width; x++)
                {
                    if (yPositionsBackground[x] < height)
                    {
                        Console.SetCursorPosition(x, yPositionsBackground[x]);
                        Console.Write(GetRandomChar(random));
                        yPositionsBackground[x]++;
                    }
                    else
                    {
                        yPositionsBackground[x] = 0; // Zresetowanie pozycji do góry po osiągnięciu końca
                    }
                }

                // Rysujemy napis "Monday" na pierwszym planie
                for (int i = 0; i < 6; i++)
                {
                    string[] shape = letterShapes[i];
                    for (int j = 0; j < letterHeight; j++)
                    {
                        Console.SetCursorPosition(startX + i * letterWidth, startY + j);
                        Console.WriteLine(shape[j]);
                    }
                }

                // Sprawdzamy, czy wszystkie litery napisu "Monday" są już na ekranie
                wordFormed = true;
                foreach (int yPos in yPositionsBackground)
                {
                    if (yPos < height)
                    {
                        wordFormed = false;
                        break;
                    }
                }

                Thread.Sleep(10);
            }

            // Czekamy chwilę, aby użytkownik zobaczył wynik
            Thread.Sleep(5000);
        }

        static char GetRandomChar(Random random)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return chars[random.Next(chars.Length)];
        }
    }
}
