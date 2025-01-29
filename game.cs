using System;
using System.IO;
using System.Collections.Generic;

namespace PlumberAdventure
{
    public class Game
    {
        // Scenes
        private enum Scene
        {
            House,
            Street,
            Castle,
            End
        }

        private Scene currentScene;
        private bool hasCrowbar;
        private bool isRunning;

        // Inventory system / written by ai
        private List<string> inventory;

        public Game()
        {
            currentScene = Scene.House; 
            hasCrowbar = false;
            isRunning = true;
            inventory = new List<string>();
        }

        public void Run()
        {
            Console.Clear();
            Console.WriteLine("Welcome to The Plumber's Adventure (Map + Save/Load + Inventory)!");

            while (isRunning)
            {
                switch (currentScene)
                {
                    case Scene.House:
                        HouseScene();
                        break;
                    case Scene.Street:
                        StreetScene();
                        break;
                    case Scene.Castle:
                        CastleScene();
                        break;
                    case Scene.End:
                        isRunning = false;
                        break;
                }
            }
        }
        
        // SAVE / LOAD FUNCTIONS
        // written by ai
        public bool LoadGame()
        {
            try
            {
                string[] lines = File.ReadAllLines("savegame.txt");
                if (lines.Length < 3) return false;

                if (!Enum.TryParse(lines[0], out Scene loadedScene))
                    return false;
                currentScene = loadedScene;

                hasCrowbar = bool.Parse(lines[1]);

                // Inventory is in line[2], written by ai
                inventory = new List<string>();
                if (!string.IsNullOrEmpty(lines[2]))
                {
                    string[] items = lines[2].Split(',');
                    foreach (var item in items)
                    {
                        string trimmed = item.Trim();
                        if (!string.IsNullOrEmpty(trimmed))
                            inventory.Add(trimmed);
                    }
                }

                isRunning = true;
                Console.WriteLine("\nGame Loaded Successfully!\n");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SaveGame()
        {
            try
            {
                using (var writer = new StreamWriter("savegame.txt"))
                {
                    writer.WriteLine(currentScene.ToString());
                    writer.WriteLine(hasCrowbar.ToString());

                    if (inventory.Count > 0)
                    {
                        string allItems = string.Join(", ", inventory);
                        writer.WriteLine(allItems);
                    }
                    else
                    {
                        writer.WriteLine("");
                    }
                }
                Console.WriteLine("Game saved successfully!");
            }
            catch
            {
                Console.WriteLine("Error occurred while saving the game.");
            }
        }
        
        // Print Menu
        // (Map), (Inventory), (Save Game), (Quit Game).
        private void PrintMenu(string[] storyOptions)
        {
            Console.WriteLine("\n-- Choices --");
            for (int i = 0; i < storyOptions.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {storyOptions[i]}");
            }

            int mapChoiceIndex = storyOptions.Length + 1;
            int invChoiceIndex = storyOptions.Length + 2;
            int saveChoiceIndex = storyOptions.Length + 3;
            int quitChoiceIndex = storyOptions.Length + 4;

            Console.WriteLine($"{mapChoiceIndex}. Look at the Map");
            Console.WriteLine($"{invChoiceIndex}. Show Inventory");
            Console.WriteLine($"{saveChoiceIndex}. Save Game");
            Console.WriteLine($"{quitChoiceIndex}. Quit Game");
            Console.Write("\nEnter your choice: ");
        }

        private int ReadChoice(string[] storyOptions)
        {
            int numberOfStoryOptions = storyOptions.Length;

            while (true)
            {
                string input = Console.ReadLine();
                int mapIndex = numberOfStoryOptions + 1;
                int invIndex = numberOfStoryOptions + 2;
                int saveIndex = numberOfStoryOptions + 3;
                int quitIndex = numberOfStoryOptions + 4;

                if (int.TryParse(input, out int choice))
                {
                    if (choice >= 1 && choice <= quitIndex)
                    {
                        if (choice == mapIndex)
                        {
                            DisplayMap();
                            Console.WriteLine("\nPick again...");
                            PrintMenu(storyOptions);
                            continue;
                        }
                        else if (choice == invIndex)
                        {
                            ShowInventory();
                            Console.WriteLine("\nPick again...");
                            PrintMenu(storyOptions);
                            continue;
                        }
                        else if (choice == saveIndex)
                        {
                            SaveGame();
                            Console.WriteLine("\nPick again...");
                            PrintMenu(storyOptions);
                            continue;
                        }
                        else if (choice == quitIndex)
                        {
                            Console.WriteLine("Quitting the game...");
                            isRunning = false;
                            return -1;
                        }
                        else
                        {
                            
                            return choice;
                        }
                    }
                }
                Console.WriteLine("Invalid choice. Try again: ");
            }
        }
        
        // Display the Map
        private void DisplayMap()
        {
            // The map: House -> Street -> Castle
            int sceneIndex = 0;
            switch (currentScene)
            {
                case Scene.House:  sceneIndex = 0; break;
                case Scene.Street: sceneIndex = 1; break;
                case Scene.Castle: sceneIndex = 2; break;
            }

            string[] sceneNames = { "House", "Street", "Castle" };

            Console.WriteLine("\n=== MAP ===");
            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (i == sceneIndex)
                    Console.Write(sceneNames[i] + "*");
                else
                    Console.Write(sceneNames[i]);

                if (i < sceneNames.Length - 1)
                    Console.Write(" -> ");
            }
            Console.WriteLine("\n");
        }
        
        // Show Inventory
        private void ShowInventory()
        {
            Console.WriteLine("\n=== INVENTORY ===");
            if (inventory.Count == 0)
            {
                Console.WriteLine("You have no items.");
            }
            else
            {
                foreach (var item in inventory)
                {
                    Console.WriteLine($"- {item}");
                }
            }
        }
        
        // SCENE: HOUSE
        private void HouseScene()
        {
            Console.Clear();
            Console.WriteLine("=== SCENE 1: HOUSE ===\n");

            // FBI phone call
            Console.WriteLine("Your phone rings. It's an FBI agent telling you there's a princess locked in a castle, and only you can save her.\n");
            Console.WriteLine("You see a rusty crowbar on the table.");

            string[] options =
            {
                "Pick up the crowbar",
                "Leave the crowbar"
            };

            PrintMenu(options);
            int choice = ReadChoice(options);
            if (choice == -1) return;  

            if (choice == 1)
            {
                hasCrowbar = true;
                if (!inventory.Contains("Crowbar"))
                    inventory.Add("Crowbar");

                Console.WriteLine("\nYou pick up the crowbar (added to your inventory).");
            }
            else
            {
                Console.WriteLine("\nAre you sure? A plumber without a crowbar?");
                string[] confirmOpts =
                {
                    "Fine, I'll take it.",
                    "No, I really don't want it."
                };
                PrintMenu(confirmOpts);
                int secondChoice = ReadChoice(confirmOpts);
                if (secondChoice == -1) return;  

                if (secondChoice == 1)
                {
                    hasCrowbar = true;
                    if (!inventory.Contains("Crowbar"))
                        inventory.Add("Crowbar");
                    Console.WriteLine("\nYou change your mind and grab it.");
                }
                else
                {
                    hasCrowbar = false;
                    Console.WriteLine("\nYou leave it behind.");
                }
            }

            Console.WriteLine("\nPress any key to leave your house...");
            Console.ReadKey();
            currentScene = Scene.Street;
        }
        
        // SCENE: STREET
        private void StreetScene()
        {
            Console.Clear();
            Console.WriteLine("=== SCENE 2: STREET ===\n");
            Console.WriteLine("An old man warns you about the castle...");

            string[] options =
            {
                "Ask about the castle",
                "Ask about the princess",
                "Ignore him"
            };

            PrintMenu(options);
            int choice = ReadChoice(options);
            if (choice == -1) return;  

            switch (choice)
            {
                case 1:
                    Console.WriteLine("\nOld Man: \"The door is locked tight. Hope you've something to pry it open.\"");
                    break;
                case 2:
                    Console.WriteLine("\nOld Man: \"They say only a plumber with a crowbar can save her.\"");
                    break;
                default:
                    Console.WriteLine("\nOld Man: \"Alright, but don't say I didn't warn you.\"");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            currentScene = Scene.Castle;
        }
        
        // SCENE: CASTLE
        private void CastleScene()
        {
            Console.Clear();
            Console.WriteLine("=== SCENE 3: CASTLE ===\n");
            Console.WriteLine("You reach the locked castle door. The princess is inside...");

            if (!hasCrowbar)
            {
                Console.WriteLine("\nNo crowbar means no way in. The mission fails!");
            }
            else
            {
                Console.WriteLine("\nYou pry open the door with your crowbar. The princess is relieved but very talkative...");

                string[] options =
                {
                    "Rescue her quickly ",
                    "You got annoyed, leave her behind"
                };

                PrintMenu(options);
                int choice = ReadChoice(options);
                if (choice == -1) return; 

                if (choice == 1)
                {
                    Console.WriteLine("\nShe hugs you happily! You're a true hero!");
                }
                else
                {
                    Console.WriteLine("\nYou decide you can't stand her chatter, leaving her behind.");
                }
            }

            Console.WriteLine("\nPress any key to finish...");
            Console.ReadKey();
            currentScene = Scene.End;
        }
    }
}
