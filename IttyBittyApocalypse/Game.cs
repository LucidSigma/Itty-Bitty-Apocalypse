using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace IttyBittyApocalypse
{
    public class Game
    {
        private enum StartMenuOptions
        {
            Play = 0,
            Help = 1,
            Credits = 2,
            Exit = 3
        }

        private readonly List<string> levels = new List<string>();
        private readonly List<Enemy> enemies = new List<Enemy>();
        private readonly List<Event> events = new List<Event>();

        private static readonly List<Weapon> meleeWeapons = new List<Weapon>();
        private static readonly List<Weapon> rangedWeapons = new List<Weapon>();
        private static readonly List<Shield> shields = new List<Shield>();

        private readonly int[,] enemyData = new int[Enum.GetNames(typeof(EnemyIndices)).Length, Enum.GetNames(typeof(EnemyAttributes)).Length];

        private Menu startMenu;
        private Player player;

        private bool isRunning = true;
        private bool inStartMenu = true;
        private string currentLevel;

        private static Random randomGenerator = new Random();

        public static Weapon randomMeleeWeapon;
        public static Weapon randomRangedWeapon;
        public static Shield randomShield;

        public Game()
        {
            Console.SetWindowSize(120, 30);

            Console.Title = "Itty Bitty Apocalypse";
            Console.CursorVisible = false;
        }

        public bool Initialise()
        {
            bool failedInitialisation = false;

            player = null;

            levels.Clear();
            enemies.Clear();
            meleeWeapons.Clear();
            rangedWeapons.Clear();
            shields.Clear();
            events.Clear();

            try
            {
                GetLevels();

                GetEnemyData();
                GetEventData();

                GetWeaponData("data/melee.csv", meleeWeapons);
                GetWeaponData("data/ranged.csv", rangedWeapons);
                GetShieldData("data/shields.csv");
            }
            catch (FileNotFoundException fileNotFound)
            {
                Console.WriteLine($"File error: {fileNotFound.FileName} not found.");
                Console.WriteLine("Please redownload to fix.");

                failedInitialisation = true;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Levels directory is missing or corrupted, please redownload to fix.");
                Console.WriteLine("Make sure the game is extracted before opening the executable (this can cause this error too).");

                failedInitialisation = true;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Enemies XML file is corrupted, please redownload to fix.");

                failedInitialisation = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("CSV files corrupted, please redownload to fix.");

                failedInitialisation = true;
            }
            catch (NotEnoughLevelsException)
            {
                Console.WriteLine("Not enough level files in levels directory. Some may not have downloaded properly.");

                failedInitialisation = true;
            }

            if (failedInitialisation)
            {
                Console.WriteLine("Game not initialised correctly, terminating.");

                Console.ReadLine();

                return false;
            }

            List<string> startMenuOptions = new List<string>(new string[] { "Play", "Help/Controls", "Credits", "Exit" });
            startMenu = new Menu("Itty Bitty Apocalypse", startMenuOptions);
            inStartMenu = true;

            player = new Player
            {
                KillCount = 0,
                Score = 0,
            };

            return true;
        }

        public static void SetRandomWeapons()
        {
            int meleeIndex = randomGenerator.Next(0, meleeWeapons.Count);
            randomMeleeWeapon = meleeWeapons[meleeIndex];

            int rangedIndex = randomGenerator.Next(0, rangedWeapons.Count);
            randomRangedWeapon = rangedWeapons[rangedIndex];

            int shieldIndex = randomGenerator.Next(0, shields.Count);
            randomShield = shields[shieldIndex];
        }

        private void GetLevels()
        {
            const int MIN_LEVELS_REQUIRED = 23;

            string[] files = Directory.GetFiles("levels", "*.txt");

            foreach (string file in files)
            {
                levels.Add(file);
            }

            if (levels.Count < MIN_LEVELS_REQUIRED)
            {
                throw new NotEnoughLevelsException();
            }

			bool IsSafeHavenLevel(string levelName)
			{
				return levelName.Contains("safe_haven");
			}

			levels.RemoveAll(IsSafeHavenLevel);

            levels.Shuffle();
        }

        private void GetEnemyData()
        {
            XmlDocument enemyDocument = new XmlDocument();
            enemyDocument.Load("data/enemies.xml");

            XmlNodeList enemyList = enemyDocument.DocumentElement.SelectNodes("/enemies/enemy");

            if (enemyList.Count == 0)
            {
                throw new NullReferenceException();
            }

            EnemyIndices currentEnemy = EnemyIndices.Zombie;

            foreach (XmlNode node in enemyList)
            {
                enemyData[(int)currentEnemy, (int)EnemyAttributes.MinDamage] = Convert.ToInt32(node.SelectSingleNode("damage_min").InnerText);
                enemyData[(int)currentEnemy, (int)EnemyAttributes.MaxDamage] = Convert.ToInt32(node.SelectSingleNode("damage_max").InnerText);
                enemyData[(int)currentEnemy, (int)EnemyAttributes.Health] = Convert.ToInt32(node.SelectSingleNode("health").InnerText);
                enemyData[(int)currentEnemy, (int)EnemyAttributes.Range] = Convert.ToInt32(node.SelectSingleNode("range").InnerText);
                enemyData[(int)currentEnemy, (int)EnemyAttributes.Score] = Convert.ToInt32(node.SelectSingleNode("score").InnerText);

                currentEnemy++;
            }
        }

        private void GetEventData()
        {
            MethodInfo[] eventList = typeof(EventFunctions).GetMethods();

            foreach (MethodInfo currentEvent in eventList)
            {
                if (currentEvent.ReturnType == typeof(void))
                {
                    Event.EventDelegate currentEventDelegate = (Event.EventDelegate)Delegate.CreateDelegate(typeof(Event.EventDelegate), player, currentEvent);

                    events.Add(new Event(currentEventDelegate));
                }
            }

            events.Shuffle();
        }

        private void GetWeaponData(string filename, List<Weapon> weaponList)
        {
            const int NAME = 0;
            const int MIN_DAMAGE = 1;
            const int MAX_DAMAGE = 2;

            string[] weaponData = File.ReadAllLines(filename);

            foreach (string weapon in weaponData)
            {
                string[] currentWeaponData = weapon.Split(',');

                weaponList.Add(new Weapon(currentWeaponData[NAME], Convert.ToInt32(currentWeaponData[MIN_DAMAGE]), Convert.ToInt32(currentWeaponData[MAX_DAMAGE])));
            }
        }

        private void GetShieldData(string filename)
        {
            const int NAME = 0;
            const int MIN_DEFENCE = 1;
            const int MAX_DEFENCE = 2;

            string[] shieldData = File.ReadAllLines(filename);

            foreach (string shield in shieldData)
            {
                string[] currentShieldData = shield.Split(',');

                shields.Add(new Shield(currentShieldData[NAME], Convert.ToInt32(currentShieldData[MIN_DEFENCE]), Convert.ToInt32(currentShieldData[MAX_DEFENCE])));
            }
        }

        private void StartMenu()
        {
            do
            {
                Console.Clear();
                startMenu.Print();

                ConsoleKey userInput = Console.ReadKey().Key;

                switch (userInput)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        startMenu.MoveSelectorUp();

                        break;

                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        startMenu.MoveSelectorDown();

                        break;

                    case ConsoleKey.Enter:
                        ExecuteStartMenuOption(startMenu.SelectorLocation);

                        break;

                    default:
                        break;
                }
            }
            while (inStartMenu);
        }

        private void ExecuteStartMenuOption(int option)
        {
            switch (option)
            {
                case (int)StartMenuOptions.Play:
                    inStartMenu = false;

                    break;

                case (int)StartMenuOptions.Help:
                    HelpScreen();

                    break;

                case (int)StartMenuOptions.Credits:
                    CreditsScreen();

                    break;

                case (int)StartMenuOptions.Exit:
                    inStartMenu = false;
                    isRunning = false;

                    break;

                default:
                    break;
            }
        }

        private void CreditsScreen()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Itty Bitty Apocalypse Credits\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Programmed and designed by Matt Schafer AKA LucidSigma.");
            Console.WriteLine("Additional testing by Tom Chardon, Nick McFadden and Brody Martin.\n");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Made with Microsoft C#/.NET and Visual Studio 2017 Community.\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Made in less than twenty days for the FloppyJam.");
            Console.WriteLine("Organised by YohahiDev.\n");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Press enter to return to the main menu.");

            Console.ReadLine();
        }

        private void HelpScreen()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Introduction\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("You are a survivor in the relentless zombie apocalypse.");
            Console.WriteLine("You are represented by a @.");
            Console.WriteLine("Enemies are represented by green letters.");

            Console.WriteLine("\nPress enter to continue.");

            Console.ReadLine();
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Controls\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Use the arrow keys or the WASD keys to move.");
            Console.WriteLine("Use the IJKL keys to shoot (only if you have ammunition).");
            Console.WriteLine("Use H to heal if you have a medkit.");

            Console.WriteLine("\nPress enter to continue.");

            Console.ReadLine();
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Provisions\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Food keeps you alive each day.");
            Console.WriteLine("Ammunition is used by your ranged weapons.");
            Console.WriteLine("Medkits are used to heal yourself.");

            Console.WriteLine("\nPress enter to continue.");

            Console.ReadLine();
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Enemies\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Zombies lurk everywhere and there are many different types.");
            Console.WriteLine("Regular zombies don't do anything special.");
            Console.WriteLine("Fighters do more damage and tanks have more health.");
            Console.WriteLine("Seekers have a longer range than others.");
            Console.WriteLine("Allrounder zombies have a balanced mix of damage, defence and range.");
            Console.WriteLine("Writher zombies have little health but can do a lot of damage.");
            Console.WriteLine("Marksman zombies have ranged attacks.");
            Console.WriteLine("Reinforced zombies have armour that can block damage and healer zombies occasionally heal themselves.");
            Console.WriteLine("Juggernaut zombies have quite a bit of health and can pack a punch.");
            Console.WriteLine("Intelligent zombies are able to pathfind.");
            Console.WriteLine("Destroyer zombies can destroy breakable tiles.");

            Console.WriteLine("\nPress enter to continue.");

            Console.ReadLine();
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Special Tiles\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("There are several different types of containers that can be looted.");
            Console.WriteLine("O - just generic places that can be looted.");
            Console.WriteLine("0 - cabinets that are more likely to have ranged weapons and ammunition.");
            Console.WriteLine("+ - medical boxes mostly containing medkits.\n");
            Console.WriteLine(", - tiles that can be broken if your weapon is strong enough.");
            Console.WriteLine("^ - spikes that will hurt you if you step on them.\n");
            Console.WriteLine("/ - your car, get to it to end the current level.");

            Console.WriteLine("\nPress enter to continue.");

            Console.ReadLine();
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Play()
        {
            while (isRunning)
            {
                if (!Initialise())
                {
                    return;
                }

                StartMenu();

                if (isRunning)
                {
                    InitialScavenging();

                    const int TOTAL_DAYS = 5;
                    const int EVENTS_PER_DAY = 2;

                    for (int day = 0; day < TOTAL_DAYS; day++)
                    {
                        for (int currentEvent = 0; currentEvent < EVENTS_PER_DAY; currentEvent++)
                        {
                            player.PrintStats();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("ACTIVITY LOG");
                            Console.ForegroundColor = ConsoleColor.White;

                            events[0].Execute(player);
                            events.RemoveAt(0);

                            if (!player.IsAlive)
                            {
                                break;
                            }
                        }

                        if (player.IsAlive)
                        {
                            LevelMenu();

                            Roguelike roguelike = new Roguelike(currentLevel, player, enemyData, meleeWeapons, rangedWeapons, shields);
                            
                            roguelike.Play();

                            if (player.IsAlive)
                            {
                                player.PrintStats();

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("ACTIVITY LOG");
                                Console.ForegroundColor = ConsoleColor.White;

                                Console.WriteLine("As you get closer to the save haven, the sun sets down on another day.\n");

                                player.Eat();
                            }
                        }

                        if (!player.IsAlive)
                        {
							break;
                        }
                    }

					if (player.IsAlive)
					{
						player.PrintStats();

						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("ACTIVITY LOG");
						Console.ForegroundColor = ConsoleColor.White;

						Console.WriteLine("You see the save haven in the distance.");
						Console.WriteLine("The approach begins...");

						Console.WriteLine("\nPress enter to continue.");
						Console.ReadLine();

						List<string> safeHavenLevels = new List<string>(new string[] { "levels/safe_haven_entrance.txt", "levels/safe_haven_path.txt", "levels/safe_haven.txt" });
						Roguelike safeHaven;

						for (int i = 0; i < safeHavenLevels.Count; i++)
						{
							safeHaven = new Roguelike(safeHavenLevels[i], player, enemyData, meleeWeapons, rangedWeapons, shields);
							safeHaven.Play();

							if (!player.IsAlive)
							{
								break;
							}
						}
					}

					Console.Clear();
					Console.ForegroundColor = ConsoleColor.White;

					if (!player.IsAlive)
					{
						Console.WriteLine($"{player.Name} was killed on their journey to the safe haven.");
						Console.WriteLine("Let's hope others don't face such a gruesome fate.");
						Console.WriteLine("Better luck next time.\n");

						FinalScore();
					}

					if (player.IsSafe)
					{
						Console.WriteLine($"After several long and laborious days, {player.Name} has finally made it to the safe haven!");
						Console.WriteLine("Within the utopia is a flourishing society that should one day continue the survival of humanity.\n");

						FinalScore();

						Console.WriteLine("\nThank you for playing.");
					}

					Console.WriteLine("\nPress enter to continue.");
					Console.ReadLine();
				}
            }
        }

        private void InitialScavenging()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            player.Name = GetPlayerName();

            Console.Clear();

            Console.WriteLine("The zombie apocalypse has destroyed the world.");
            Console.WriteLine("The few survivors run in fear of the diseased, hoping they are not next.\n");

            Console.WriteLine($"{player.Name} hears rumours of a safe haven several days of driving away from where they currently are.");
            Console.WriteLine($"With nothing left to do around here, {player.Name} sets off for the save haven.\n");

            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();

            List<string> scavengeOptions = new List<string>(new string[] { "2", "4", "6", "8" });
            Menu scavengeMenu = new Menu("", scavengeOptions);

            bool inScavengeMenu = true;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($"Before {player.Name} sets off, they decide to scavenge for a few hours.");
                Console.WriteLine("How many hours do you want to scavenge for?");
                Console.WriteLine("The longer you scavenge the more resources you could find but the more health you are likely to lose.\n");

                scavengeMenu.Print();

                ConsoleKey userInput = Console.ReadKey().Key;

                switch (userInput)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        scavengeMenu.MoveSelectorUp();

                        break;

                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        scavengeMenu.MoveSelectorDown();

                        break;

                    case ConsoleKey.Enter:
                        Scavenge(Convert.ToInt32(scavengeMenu.MenuOptions[scavengeMenu.SelectorLocation]));
                        inScavengeMenu = false;

                        break;

                    default:
                        break;
                }
            }
            while (inScavengeMenu);
        }

        private void Scavenge(int hours)
        {
            const int MIN_HEALTH_MULTIPLIER = 3;
            const int MAX_HEALTH_MULTIPLIER = 6;

            const int MIN_FOOD_MULTIPLIER = 8;
            const int MAX_FOOD_MULTIPLIER = 12;

            const int MIN_AMMO_MULTIPLIER = 3;
            const int MAX_AMMO_MULTIPLIER = 5;

            const float MEDKIT_MULTIPLIER = 1.5f;

            int damage = randomGenerator.Next(MIN_HEALTH_MULTIPLIER, MAX_HEALTH_MULTIPLIER + 1) * hours;
            int scavengedFood = randomGenerator.Next(MIN_FOOD_MULTIPLIER, MAX_FOOD_MULTIPLIER + 1) * hours;
            int scavengedAmmo = randomGenerator.Next(MIN_AMMO_MULTIPLIER, MAX_AMMO_MULTIPLIER + 1) * hours;
            int scavengedMedkits = (int)Math.Round(randomGenerator.NextDouble() * MEDKIT_MULTIPLIER * hours);

            player.Health -= damage;
            player.Food = scavengedFood;
            player.Ammo = scavengedAmmo;
            player.Medkits = scavengedMedkits;

            const int MAX_MELEE_DAMAGE = 5;
            const int MAX_RANGED_DAMAGE = 12;

            do
            {
                int meleeIndex = randomGenerator.Next(0, meleeWeapons.Count);
                player.MeleeWeapon = meleeWeapons[meleeIndex];
            }
            while (player.MeleeWeapon.minDamage > MAX_MELEE_DAMAGE);

            do
            {
                int rangedIndex = randomGenerator.Next(0, rangedWeapons.Count);
                player.RangedWeapon = rangedWeapons[rangedIndex];
            }
            while (player.RangedWeapon.minDamage > MAX_RANGED_DAMAGE);

            player.ArmShield = new Shield
            {
                name = "None",
                minDefence = 0,
                maxDefence = 0
            };

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"You scavenged for {hours} hours.\n");
            Console.WriteLine($"You managed to find {scavengedFood} food.");
            Console.WriteLine($"You found {scavengedAmmo} ammunition.");
            Console.WriteLine($"You found {scavengedMedkits} {((scavengedMedkits == 1) ? "medkit" : "medkits")}\n");

            Console.WriteLine($"The best melee weapon you could find is {player.MeleeWeapon.name.GetArticle()} {player.MeleeWeapon.name.SentenceCase()}.");
            Console.WriteLine($"The best ranged weapon you could find is {player.RangedWeapon.name.GetArticle()} {player.RangedWeapon.name.SentenceCase()}.\n");

            Console.WriteLine($"However, during the scavenging, you took {damage} damage.\n");

            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
        }

        private string GetPlayerName()
        {
            const int MAX_NAME_LENGTH = 20;

            bool validInput = false;
            string userInput = "";

            do
            {
                Console.Write("Enter your name: ");
                userInput = Console.ReadLine();

                if (userInput.Length > MAX_NAME_LENGTH)
                {
                    Console.WriteLine($"Name must be less than {MAX_NAME_LENGTH} characters.");
                }
                else if (userInput.Length == 0)
                {
                    Console.WriteLine("Your name must be something.");
                }
                else
                {
                    validInput = true;
                }
            }
            while (!validInput);

            return userInput;
        }

        private void LevelMenu()
        {
            const int LEVELS_IN_MENU = 4;

            List<string> levelOptions = new List<string>();
            List<string> originalFilenames = new List<string>();

            for (int i = 0; i < LEVELS_IN_MENU; i++)
            {
                string levelName = levels[0].ToUpper();

                try
                {
                    levelName = levelName.Split('\\')[1];
                }
                catch (IndexOutOfRangeException)
                {
                    levelName = levelName.Split('/')[1];
                }

                levelName = levelName.Split('.')[0];
                levelName = levelName.Replace('_', ' ');

                levelOptions.Add(levelName);
                originalFilenames.Add(levels[0]);

                levels.RemoveAt(0);
            }

            Menu levelMenu = new Menu("", levelOptions);
            bool inLevelMenu = true;

            do
            {
                player.PrintStats();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ACTIVITY LOG");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($"{player.Name} decides it is time to scavenge around for a bit.");
                Console.WriteLine("They find four places of interest.");
                Console.WriteLine("Which place do you want to scavenge at?\n");

                levelMenu.Print();

                ConsoleKey userInput = Console.ReadKey().Key;

                switch (userInput)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        levelMenu.MoveSelectorUp();

                        break;

                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        levelMenu.MoveSelectorDown();

                        break;

                    case ConsoleKey.Enter:
                        int selectedLevelIndex = levelMenu.SelectorLocation;
                        currentLevel = originalFilenames[selectedLevelIndex];

                        inLevelMenu = false;

                        break;

                    default:
                        break;
                }
            }
            while (inLevelMenu);
        }

		private void FinalScore()
		{
			const int HEALTH_MULTIPLIER = 25;
			const int FOOD_MULTIPLIER = 10;
			const int AMMO_MULTIPLIER = 15;
			const int MEDKIT_MULTIPLIER = 50;

			const int PAUSE_TIME = 250;

			Console.Write("Score: ");
			Thread.Sleep(PAUSE_TIME);
			Console.WriteLine(player.Score);

			Thread.Sleep(PAUSE_TIME);

			int healthScore = player.Health * HEALTH_MULTIPLIER;

			Console.Write("Health: ");
			Thread.Sleep(PAUSE_TIME);
			Console.Write($"{player.Health} * {HEALTH_MULTIPLIER} = ");
			Thread.Sleep(PAUSE_TIME);
			Console.WriteLine(healthScore);

			Thread.Sleep(PAUSE_TIME);

			int foodScore = player.Food * FOOD_MULTIPLIER;

			Console.Write("Food: ");
			Thread.Sleep(PAUSE_TIME);
			Console.Write($"{player.Food} * {FOOD_MULTIPLIER} = ");
			Thread.Sleep(PAUSE_TIME);
			Console.WriteLine(foodScore);

			Thread.Sleep(PAUSE_TIME);

			int ammoScore = player.Ammo * AMMO_MULTIPLIER;

			Console.Write("Ammo: ");
			Thread.Sleep(PAUSE_TIME);
			Console.Write($"{player.Ammo} * {AMMO_MULTIPLIER} = ");
			Thread.Sleep(PAUSE_TIME);
			Console.WriteLine(ammoScore);

			Thread.Sleep(PAUSE_TIME);

			int medkitsScore = player.Medkits * MEDKIT_MULTIPLIER;

			Console.Write("Medkits: ");
			Thread.Sleep(PAUSE_TIME);
			Console.Write($"{player.Medkits} * {MEDKIT_MULTIPLIER} = ");
			Thread.Sleep(PAUSE_TIME);
			Console.WriteLine(medkitsScore);

			Thread.Sleep(PAUSE_TIME);

			int finalScore = player.Score + healthScore + foodScore + ammoScore + medkitsScore;

			Console.Write($"\nFinal score: ");
			Thread.Sleep(PAUSE_TIME);
			Console.WriteLine(finalScore);
		}
	}
}
