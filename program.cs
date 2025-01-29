using System;
using System.IO;

namespace PlumberAdventure
{
    class Program
    {
        static void Main()
        {
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.Clear();
                Console.WriteLine("=== The Plumber's Adventure ===");
                Console.WriteLine("1. New Game");
                Console.WriteLine("2. Load Game");
                Console.WriteLine("3. Credits");
                Console.WriteLine("4. Exit");
                Console.Write("\nPlease choose an option (1-4): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var newGame = new Game();
                        newGame.Run();
                        break;
                    case "2":
                        if (File.Exists("savegame.txt"))
                        {
                            var loadedGame = new Game();
                            bool success = loadedGame.LoadGame();
                            if (success)
                            {
                                loadedGame.Run();
                            }
                            else
                            {
                                Console.WriteLine("\nFailed to load the saved game. Press any key to continue...");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nNo saved game found. Press any key to continue...");
                            Console.ReadKey();
                        }
                        break;
                    case "3":
                        ShowCredits();
                        break;
                    case "4":
                        keepRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowCredits()
        {
            Console.Clear();
            Console.WriteLine("=== Credits ===");
            Console.WriteLine("Game Developer: Arda Urkmez");
            Console.WriteLine("Written with the help of Claude.ai and ChatGPT");
            Console.WriteLine("In some cases ai used for understand the logic in some cases direct citations used");
            Console.WriteLine("There are notes in the code for where the code directly written by the ai");
            Console.WriteLine("Plumbing & FBI references are purely fictional");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}
