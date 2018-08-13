using System;
using System.Collections.Generic;

namespace IttyBittyApocalypse
{
	internal class EventFunctions
	{
		private static Random randomGenerator = new Random();

		public void NuclearExplosion(Player player)
		{
			Console.WriteLine("You see a nuclear explosion a fair distance away.");
			Console.WriteLine("The suddenness of the event makes you feel sombre.");
			Console.WriteLine($"{player.Name} reminisces for a bit before continuing their journey.");

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void BeforeTheApocalypse(Player player)
		{
			Console.WriteLine($"{player.Name} remembers a time before the entire zombie apocalypse happened.");
			Console.WriteLine("They spend a few minutes looking back on the past before continuing on with their journey.");

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void Graveyard(Player player)
		{
			Console.WriteLine($"{player.Name} comes across a graveyard.");
			Console.WriteLine("They spend some time looking over the poor souls before getting back into their vehicle.");

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

        public void SiphonBandit(Player player)
        {
            Console.WriteLine("You catch a bandit siphoning gas from your vehicle.");
            Console.WriteLine("You manage to scare him away.");

            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();
        }

		public void PrettyView(Player player)
		{
			Console.WriteLine($"{player.Name} comes across a very mesmerising view over a cliffside.");
			Console.WriteLine("They spend a few moments looking at the marvel before getting back to the car.");

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void GunshotsInDistance(Player player)
		{
			Console.WriteLine("You hear gunshots in the fair distance.");
			Console.WriteLine("It's best if you continue on.");

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void SpoiledFood(Player player)
		{
			if (player.Food > 0)
			{
				const float SPOIL_MULTIPLIER = 0.15f;

				int foodSpoiled = (int)(player.Food * SPOIL_MULTIPLIER);

				Console.WriteLine($"{foodSpoiled} of your food has spoiled.");

				player.Food -= foodSpoiled;
			}
			else
			{
				Console.WriteLine("You start to feel delirious from hunger.");
				Console.WriteLine($"{player.Name} spends an hour on the side of the road coming back to their senses.");
			}

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void MissingAmmunition(Player player)
		{
			if (player.Ammo > 0)
			{
				const float MISSING_MULTIPLIER = 0.18f;

				int ammoMissing = (int)(player.Ammo * MISSING_MULTIPLIER);

				Console.WriteLine($"You notice that {ammoMissing} of your ammunition is missing.");
				Console.WriteLine("You're not sure how it could have gotten lost, but you don't spend too much time thinking about it.");

				player.Ammo -= ammoMissing;
			}
			else
			{
				Console.WriteLine("You are starting to wish you had some ammunition.");
			}

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void UnusableMedkit(Player player)
		{
			if (player.Medkits > 0)
			{
				Console.WriteLine("You examine your medkits and it turns out one is unusable. You dispose of it.");

				player.Medkits--;
			}
			else
			{
				Console.WriteLine("You are really wishing you had some medkits right now. Who knows what could happen in the future.");
			}

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void BanditAmmunition(Player player)
		{
			const int MIN_STEAL = 20;
			const int MAX_STEAL = 50;

			int ammoStolen = randomGenerator.Next(MIN_STEAL, MAX_STEAL + 1);

			if (player.Ammo == 0)
			{
				ammoStolen = 0;
			}

			Console.WriteLine("You catch a bandit in your supplies.");

			if (ammoStolen > player.Ammo)
			{
				ammoStolen = player.Ammo;

				Console.WriteLine("You scare him away but he steals all of your ammunition.");
			}
            else if (ammoStolen == 0)
            {
                Console.WriteLine("He seemed to be looking for ammunition, so you scare him away before he stole anything.");
            }
			else
			{
				Console.WriteLine($"You scare him away but he stole {ammoStolen} of your ammunition.");
			}

            player.Ammo -= ammoStolen;

            Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

        public void BanditFood(Player player)
        {
            const int MIN_STEAL = 25;
            const int MAX_STEAL = 35;

            int foodStolen = randomGenerator.Next(MIN_STEAL, MAX_STEAL + 1);

			if (player.Food == 0)
			{
				foodStolen = 0;
			}

            Console.WriteLine("You catch a bandit in your supplies.");

            if (foodStolen > player.Food)
            {
                foodStolen = player.Food;

                Console.WriteLine("You scare him away but he steals all of your food.");
            }
            else if (foodStolen == 0)
            {
                Console.WriteLine("He seemed to be looking for food, so you scare him away before he stole anything.");
            }
            else
            {
                Console.WriteLine($"You scare him away but he stole {foodStolen} of your food.");
            }

            player.Food -= foodStolen;

            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();
        }

        public void EvasiveManoeuvre(Player player)
		{
			const int FOOD = 0;
			const int AMMO = 1;

			Console.WriteLine("An animal jumps out onto the road in front of you.");
			Console.WriteLine("Fortunately you manage to perform some evasive manoeuvres to avoid it.");

			int lossType = randomGenerator.Next(FOOD, AMMO + 1);

			switch (lossType)
			{
				case FOOD:
					const int FOOD_MIN = 5;
					const int FOOD_MAX = 25;

					int foodLost = randomGenerator.Next(FOOD_MIN, FOOD_MAX + 1);

					if (player.Food < foodLost)
					{
						foodLost = player.Food;
					}

                    if (foodLost == 0)
                    {
                        Console.WriteLine("Nothing else bad happened.");
                    }
	                else
                    {
                        Console.WriteLine($"However, {foodLost} food flew out the window.");
                    }

					player.Food -= foodLost;

					break;

				case AMMO:
					const int AMMO_MIN = 5;
					const int AMMO_MAX = 30;

					int ammoLost = randomGenerator.Next(AMMO_MIN, AMMO_MAX + 1);

					if (player.Ammo < ammoLost)
					{
						ammoLost = player.Ammo;
					}

                    if (ammoLost == 0)
                    {
                        Console.WriteLine("Nothing else bad happened.");
                    }
                    else
                    {
                        Console.WriteLine($"However, {ammoLost} ammunition flew out the window.");
                    }

					player.Ammo -= ammoLost;

					break;

				default:
					break;
			}

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void CarAccident(Player player)
		{
			const int MIN_CRASH_DAMAGE = 5;
			const int MAX_CRASH_DAMAGE = 15;

			int damage = randomGenerator.Next(MIN_CRASH_DAMAGE, MAX_CRASH_DAMAGE + 1);

			Console.WriteLine($"{player.Name} partially loses control of their vehicle and have a minor accident.");
			Console.WriteLine("Nothing serious comes of it and the vehicle is still functioning.");
			
			if (damage > player.Health)
			{
				damage = player.Health;
			}

			player.TakeDamage(damage);

			Console.WriteLine($"However, you take {damage} damage.");

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

        public void Abrasion(Player player)
        {
            const int MIN_DAMAGE = 2;
            const int MAX_DAMAGE = 7;

            int damage = randomGenerator.Next(MIN_DAMAGE, MAX_DAMAGE + 1);

            if (damage > player.Health)
            {
                damage = player.Health;
            }

            Console.WriteLine($"Whilst taking a break from driving, {player.Name} injures themselves on some sharp rocks.");
            Console.WriteLine($"They take {damage} damage.");

            player.TakeDamage(damage);

            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();
        }

        public void ZombieAmbush(Player player)
        {
            const int MIN_DAMAGE = 15;
            const int MAX_DAMAGE = 30;

            int damage = randomGenerator.Next(MIN_DAMAGE, MAX_DAMAGE + 1);

            if (damage > player.Health)
            {
                damage = player.Health;
            }

            Console.WriteLine($"When {player.Name} was taking a break from driving, they were ambushed by a zombie from behind.");
            Console.WriteLine($"They managed to fight it off but took {damage} damage.");

            player.TakeDamage(damage);
            
            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();
        }

		public void CarBreaksDown(Player player)
		{
			Console.WriteLine("The car begins to make unusual noises.");
			Console.WriteLine($"However, after some inspection, {player.Name} is able to fix it.");
			Console.WriteLine("After a day, the car is functional again.\n");

			player.Eat();
		}

        public void LostOffTrail(Player player)
        {
            Console.WriteLine($"{player.Name} takes a wrong turn and ends up off the main road to the save haven.");
            Console.WriteLine("It takes them a full day to find their way back.\n");

            player.Eat();
        }

        public void UnsafeRoadAhead(Player player)
        {
            Console.WriteLine("The road ahead is very unsafe to drive on, it would be best to take a detour.");
            Console.WriteLine($"After a day of driving on the back roads, {player.Name} is finally back on the main road to the safe haven.\n");

            player.Eat();
        }

        public void SwarmedRoadAhead(Player player)
        {
            Console.WriteLine("Hundreds of zombies swarm the road ahead. They will easily overpower the vehicle.");
            Console.WriteLine($"{player.Name} decides to take a detour along the back roads. After a day they are finally back on the main road.\n");

            player.Eat();
        }

		public void RepairShop(Player player)
		{
			Console.WriteLine($"{player.Name} comes across an old mechanic shop.");
			Console.WriteLine("They decide to spend a day repairing their car.");

			player.Eat();
		}

		public void CowCatcher(Player player)
		{
			Console.WriteLine("You come across an old cow catcher on an old farm.");
			Console.WriteLine("After a day you finally get it attached to you car as a makeshift \"zombie-plow\".");

			player.Eat();
		}

		public void Cauterise(Player player)
		{
			const int DAMAGE_MIN = 12;
			const int DAMAGE_MAX = 18;

			Console.WriteLine($"When looking around outside, {player.Name} deeply cuts their leg.");
			Console.WriteLine("Without any stitches or any other decent first aid supplies for the wound, they decide to bite the bullet and cauterise the cut.");

			int damage = randomGenerator.Next(DAMAGE_MIN, DAMAGE_MAX + 1);

			if (damage > player.Health)
			{
				damage = player.Health;
			}

			Console.WriteLine($"The cut does {damage} damage to you.");

			player.TakeDamage(damage);

			if (player.IsAlive)
			{
				Console.WriteLine("You decide to wait a day for the wound to heal a bit.\n");

				player.Eat();
			}
			else
			{
				Console.WriteLine("\nPress enter to continue.");
				Console.ReadLine();
			}
		}

		public void GunRange(Player player)
		{
			Game.SetRandomWeapons();

			Console.WriteLine("You find an abandoned gun range. You decide to enter.");

			Weapon newWeapon = Game.randomRangedWeapon;

			if (newWeapon.name == player.RangedWeapon.name)
			{
				Console.WriteLine($"The gun range had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}, but you already have one.");
			}
			else
			{
				if (newWeapon.minDamage > player.RangedWeapon.minDamage)
				{
					Console.WriteLine($"The gun range had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}. You pick it up and replace your {player.RangedWeapon.name.SentenceCase()}.");

					player.RangedWeapon = newWeapon;
				}
				else if (newWeapon.minDamage == player.RangedWeapon.minDamage)
				{
					if (newWeapon.maxDamage > player.RangedWeapon.maxDamage)
					{
						Console.WriteLine($"The gun range had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}. You pick it up and replace your {player.RangedWeapon.name.SentenceCase()}.");

						player.RangedWeapon = newWeapon;
					}
					else
					{
						Console.WriteLine($"The gun range had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}, but your {player.RangedWeapon.name.SentenceCase()} is a better weapon.");
					}
				}
				else
				{
					Console.WriteLine($"The gun range had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}, but your {player.RangedWeapon.name.SentenceCase()} is a better weapon.");
				}
			}

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void HardwareStore(Player player)
		{
			Game.SetRandomWeapons();

			Console.WriteLine("You find an abandoned hardware store. You decide to enter.");

			Weapon newWeapon = Game.randomRangedWeapon;

			if (newWeapon.name == player.MeleeWeapon.name)
			{
				Console.WriteLine($"The hardware store had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}, but you already have one.");
			}
			else
			{
				if (newWeapon.minDamage > player.MeleeWeapon.minDamage)
				{
					Console.WriteLine($"The hardware store had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}. You pick it up and replace your {player.MeleeWeapon.name.SentenceCase()}.");

					player.MeleeWeapon = newWeapon;
				}
				else if (newWeapon.minDamage == player.MeleeWeapon.minDamage)
				{
					if (newWeapon.maxDamage > player.MeleeWeapon.maxDamage)
					{
						Console.WriteLine($"The hardware store had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}. You pick it up and replace your {player.MeleeWeapon.name.SentenceCase()}.");

						player.MeleeWeapon = newWeapon;
					}
					else
					{
						Console.WriteLine($"The hardware store had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}, but your {player.MeleeWeapon.name.SentenceCase()} is a better weapon.");
					}
				}
				else
				{
					Console.WriteLine($"The hardware store had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}, but your {player.MeleeWeapon.name.SentenceCase()} is a better weapon.");
				}
			}

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void DeadBodyWeapon(Player player)
		{
			const int MELEE = 0;
			const int RANGED = 1;

			Game.SetRandomWeapons();

			Console.WriteLine("You come across a dead body on the side of the road.");
			Console.WriteLine("They don't have any provisions but you do see some weapons.");

			int weaponType = randomGenerator.Next(MELEE, RANGED + 1);
			Weapon newWeapon = new Weapon();
			Weapon compareWeapon = new Weapon();

			switch (weaponType)
			{
				case MELEE:
					newWeapon = Game.randomMeleeWeapon;
					compareWeapon = player.MeleeWeapon;

					break;

				case RANGED:
					newWeapon = Game.randomRangedWeapon;
					compareWeapon = player.RangedWeapon;

					break;

				default:
					break;
			}

			if (newWeapon.name == compareWeapon.name)
			{
				Console.WriteLine($"The dead body had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}, but you already have one.");
			}
			else
			{
				if (newWeapon.minDamage > compareWeapon.minDamage)
				{
					Console.WriteLine($"The dead body had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}. You pick it up and replace your {compareWeapon.name.SentenceCase()}.");

					if (compareWeapon.name == player.MeleeWeapon.name)
					{
						player.MeleeWeapon = newWeapon;
					}
					else
					{
						player.RangedWeapon = newWeapon;
					}
				}
				else if (newWeapon.minDamage == compareWeapon.minDamage)
				{
					if (newWeapon.maxDamage > compareWeapon.maxDamage)
					{
						Console.WriteLine($"The dead body had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}. You pick it up and replace your {compareWeapon.name.SentenceCase()}.");

						if (compareWeapon.name == player.MeleeWeapon.name)
						{
							player.MeleeWeapon = newWeapon;
						}
						else
						{
							player.RangedWeapon = newWeapon;
						}
					}
					else
					{
						Console.WriteLine($"The dead body had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}, but your {compareWeapon.name.SentenceCase()} is a better weapon.");
					}
				}
				else
				{
					Console.WriteLine($"The dead body had {newWeapon.name.SentenceCase().GetArticle()} {newWeapon.name.SentenceCase()}, but your {compareWeapon.name.SentenceCase()} is a better weapon.");
				}
			}

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

        public void DeadBodyShield(Player player)
        {
            Game.SetRandomWeapons();

            Console.WriteLine("You come across a dead body on the side of the road.");
            Console.WriteLine("They don't have any provisions but you do see a shield.");

            Shield shield = Game.randomShield;

            if (player.ArmShield.name == "None")
            {
                Console.WriteLine($"The dead body had {shield.name.SentenceCase().GetArticle()} {shield.name.SentenceCase()}.");

                player.ArmShield = shield;
            }
            else if (shield.name == player.ArmShield.name)
            {
                Console.WriteLine($"The dead body had {shield.name.SentenceCase().GetArticle()} {shield.name.SentenceCase()}, but you already have one.");
            }
            else
            {
                if (shield.minDefence > player.ArmShield.minDefence)
                {
                    Console.WriteLine($"The dead body had {shield.name.SentenceCase().GetArticle()} {shield.name.SentenceCase()} and replace your {player.ArmShield.name.SentenceCase()}.");

                    player.ArmShield = shield;
                }
                else if (shield.minDefence == player.ArmShield.minDefence)
                {
                    if (shield.maxDefence > player.ArmShield.maxDefence)
                    {
                        Console.WriteLine($"The dead body had {shield.name.SentenceCase().GetArticle()} {shield.name.SentenceCase()} and replace your {player.ArmShield.name.SentenceCase()}.");

                        player.ArmShield = shield;
                    }
                    else
                    {
                        Console.WriteLine($"The dead body had {shield.name.SentenceCase().GetArticle()} {shield.name.SentenceCase()}, but your {player.ArmShield.name.SentenceCase()} is a better shield.");
                    }
                }
                else
                {
                    Console.WriteLine($"The dead body had {shield.name.SentenceCase().GetArticle()} {shield.name.SentenceCase()}, but your {player.ArmShield.name.SentenceCase()} is a better shield.");
                }
            }

            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();
        }

        public void DeadBodyProvisions(Player player)
		{
			Console.WriteLine("You come across a dead body on the side of the road.");

			const int FOOD_MIN = 20;
			const int FOOD_MAX = 35;
			const int AMMO_MIN = 10;
			const int AMMO_MAX = 25;
			const int MEDKIT_MIN = 0;
			const int MEDKIT_MAX = 1;

			int foodFound = randomGenerator.Next(FOOD_MIN, FOOD_MAX + 1);
			int ammoFound = randomGenerator.Next(AMMO_MIN, AMMO_MAX + 1);
			int medkitsFound = randomGenerator.Next(MEDKIT_MIN, MEDKIT_MAX + 1);

			Console.WriteLine($"The dead body had {foodFound} food and {ammoFound} ammunition.");

			if (medkitsFound > 0)
			{
				Console.WriteLine("You also found a medkit.");
			}

			player.Food += foodFound;
			player.Ammo += ammoFound;
			player.Medkits += medkitsFound;

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void ZombieHorde(Player player)
		{
			bool inMenu = true;
			Menu optionMenu = new Menu("", new List<string>(new string[] { "Drive through the horde", "Wait for the horde to disperse", "Find a detour" }));

			const int DRIVE = 0;
			const int WAIT = 1;
			const int DETOUR = 2;

			Console.ForegroundColor = ConsoleColor.White;

			void OptionSelect(int option)
			{
				switch (option)
				{
					case DRIVE:
						const int DAMAGE_MIN = 15;
						const int DAMAGE_MAX = 40;

						int damage = randomGenerator.Next(DAMAGE_MIN, DAMAGE_MAX + 1);

						if (damage > player.Health)
						{
							damage = player.Health;
						}

						Console.WriteLine("\nYou drive through the horde. They easily get to you and hurt you, but you manage to get through.");
						Console.WriteLine($"You take {damage} damage.");

						player.TakeDamage(damage);

						Console.WriteLine("\nPress enter to continue.");
						Console.ReadLine();

						break;

					case WAIT:
						Console.WriteLine("\nYou wait for the horde to disperse.");
						Console.WriteLine("After sunrise the next day, the path is clear to drive through.\n");

						player.Eat();

						break;

					case DETOUR:
						Console.WriteLine($"\n{player.Name} drives around the horde down the back roads.");
						Console.WriteLine($"After a couple of hours, {player.Name} is back on the main road to the safe haven.");

						Console.WriteLine("\nPress enter to continue.");
						Console.ReadLine();

						break;

					default:
						break;
				}
			}

			do
			{
				Console.Clear();
				player.PrintStats();

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("ACTIVITY LOG");
				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine("A large zombie horde blocks the road ahead.");
				Console.WriteLine("What do you want to do?\n");

				optionMenu.Print();

				ConsoleKey userInput = Console.ReadKey().Key;

				switch (userInput)
				{
					case ConsoleKey.W:
					case ConsoleKey.UpArrow:
						optionMenu.MoveSelectorUp();

						break;

					case ConsoleKey.S:
					case ConsoleKey.DownArrow:
						optionMenu.MoveSelectorDown();

						break;

					case ConsoleKey.Enter:
						OptionSelect(optionMenu.SelectorLocation);
						inMenu = false;

						break;

					default:
						break;
				}
			}
			while (inMenu);
		}

		public void MilitaryBase(Player player)
		{
			bool inMenu = true;
			Menu optionMenu = new Menu("", new List<string>(new string[] { "Explore", "Keep driving" }));

			const int EXPLORE = 0;
			const int IGNORE = 1;

			Console.ForegroundColor = ConsoleColor.White;

			void OptionSelect(int option)
			{
				switch (option)
				{
					case EXPLORE:
						const int DAMAGE_MIN = 15;
						const int DAMAGE_MAX = 40;
						const int AMMO_USED_MIN = 20;
						const int AMMO_USED_MAX = 40;

						int ammoUsed = randomGenerator.Next(AMMO_USED_MIN, AMMO_USED_MAX + 1);

						if (player.Ammo < ammoUsed)
						{
							int damage = randomGenerator.Next(DAMAGE_MIN, DAMAGE_MAX + 1);

							Console.WriteLine($"\n{player.Name} enters the military base.");
							Console.WriteLine($"It is infested with zombies and they easily overwhelm {player.Name}");
							Console.WriteLine("Fortunately you manage to get back to you car.");

							if (damage > player.Health)
							{
								damage = player.Health;
							}

							Console.WriteLine($"You took {damage} damage and used all of your ammunition in your escape.");

							player.TakeDamage(damage);
							player.Ammo = 0;
						}
						else
						{
							const int FOOD_MIN = 20;
							const int FOOD_MAX = 75;
							const int AMMO_MIN = 30;
							const int AMMO_MAX = 60;
							const int MEDKIT_MIN = 3;
							const int MEDKIT_MAX = 6;

							player.Ammo -= ammoUsed;

							Console.WriteLine("\nYou storm into the abandoned facility and manage to loot some utilities.");
							Console.WriteLine($"You used up {ammoUsed} of your ammunition.");

							int food = randomGenerator.Next(FOOD_MIN, FOOD_MAX + 1);
							int ammo = randomGenerator.Next(AMMO_MIN, AMMO_MAX + 1);
							int medkits = randomGenerator.Next(MEDKIT_MIN, MEDKIT_MAX + 1);

							Console.WriteLine($"You managed to find {food} food, {ammo} ammunition and {medkits} medkits.");
							Console.WriteLine("You didn't find any weapons however.");

							player.Food += food;
							player.Ammo += ammo;
							player.Medkits += medkits;
						}

						break;

					case IGNORE:
						Console.WriteLine("\nYou decide that it has already been explored and isn't worth your time.");

						break;

					default:
						break;
				}
			}

			do
			{
				Console.Clear();
				player.PrintStats();

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("ACTIVITY LOG");
				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine($"{player.Name} sees an abandonded military base on the side of the road.");
				Console.WriteLine("What do you want to do?\n");

				optionMenu.Print();

				ConsoleKey userInput = Console.ReadKey().Key;

				switch (userInput)
				{
					case ConsoleKey.W:
					case ConsoleKey.UpArrow:
						optionMenu.MoveSelectorUp();

						break;

					case ConsoleKey.S:
					case ConsoleKey.DownArrow:
						optionMenu.MoveSelectorDown();

						break;

					case ConsoleKey.Enter:
						OptionSelect(optionMenu.SelectorLocation);
						inMenu = false;

						break;

					default:
						break;
				}
			}
			while (inMenu);

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void InjuredPerson(Player player)
		{
			bool inMenu = true;
			Menu optionMenu = new Menu("", new List<string>(new string[] { "Yes", "No" }));

			const int YES = 0;
			const int NO = 1;

			Console.ForegroundColor = ConsoleColor.White;

			void OptionSelect(int option)
			{
				switch (option)
				{
					case YES:
						const int FOOD_MIN = 20;
						const int FOOD_MAX = 45;
						const int AMMO_MIN = 5;
						const int AMMO_MAX = 15;

						int foodGiven = randomGenerator.Next(FOOD_MIN, FOOD_MAX + 1);
						int ammoGiven = randomGenerator.Next(AMMO_MIN, AMMO_MAX + 1);

						Console.WriteLine("\nYou give the person one of your medkits. After a short while he heals himself up.");
						Console.WriteLine($"As a reward for helping them, they give you {foodGiven} food and {ammoGiven} ammunition.");

						player.Medkits--;
						player.Food += foodGiven;
						player.Ammo += ammoGiven;

						break;

					case NO:
						Console.WriteLine("\nYou decide that the person is done for and continue on with your journey.");
						Console.WriteLine("They look at you with disappointment as you depart.");

						break;

					default:
						break;
				}

				Console.WriteLine("\nPress enter to continue.");
				Console.ReadLine();
			}

			do
			{
				Console.Clear();
				player.PrintStats();

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("ACTIVITY LOG");
				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine("You find an injured person on the side of the road.");
				Console.WriteLine("They look to be in dire need of a medkit.");

				if (player.Medkits > 0)
				{
					Console.WriteLine("Will you help them?\n");

					optionMenu.Print();

					ConsoleKey userInput = Console.ReadKey().Key;

					switch (userInput)
					{
						case ConsoleKey.W:
						case ConsoleKey.UpArrow:
							optionMenu.MoveSelectorUp();

							break;

						case ConsoleKey.S:
						case ConsoleKey.DownArrow:
							optionMenu.MoveSelectorDown();

							break;

						case ConsoleKey.Enter:
							OptionSelect(optionMenu.SelectorLocation);
							inMenu = false;

							break;

						default:
							break;
					}
				}
				else
				{
					Console.WriteLine("Unfortunately you do not have any yourself.");

					inMenu = false;

					Console.WriteLine("\nPress enter to continue.");
					Console.ReadLine();
				}
			}
			while (inMenu);
		}

		public void ThornBush(Player player)
		{
			bool inMenu = true;
			Menu optionMenu = new Menu("", new List<string>(new string[] { "Yes", "No" }));

			const int YES = 0;
			const int NO = 1;

			Console.ForegroundColor = ConsoleColor.White;

			void OptionSelect(int option)
			{
				switch (option)
				{
					case YES:
						const int DAMAGE_MIN = 9;
						const int DAMAGE_MAX = 15;
						const int FOOD_MIN = 14;
						const int FOOD_MAX = 23;

						int damage = randomGenerator.Next(DAMAGE_MIN, DAMAGE_MAX + 1);
						int foodEarned = randomGenerator.Next(FOOD_MIN, FOOD_MAX + 1);

						if (damage > player.Health)
						{
							damage = player.Health;
						}

						Console.WriteLine("\nYou decide to pick the berries from the thorny bush.");
						Console.WriteLine($"You took {damage} damage doing so but collected {foodEarned} food.");

						player.TakeDamage(damage);
						player.Food += foodEarned;

						break;

					case NO:
						Console.WriteLine("\nYou decide that the amount of berries in the bush is not worth the injuries from the thorns.");

						break;

					default:
						break;
				}
			}

			do
			{
				Console.Clear();
				player.PrintStats();

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("ACTIVITY LOG");
				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine($"{player.Name} notices a very thorny bush with some berries.");
				Console.WriteLine("Do you want to pick the berries?\n");

				optionMenu.Print();

				ConsoleKey userInput = Console.ReadKey().Key;

				switch (userInput)
				{
					case ConsoleKey.W:
					case ConsoleKey.UpArrow:
						optionMenu.MoveSelectorUp();

						break;

					case ConsoleKey.S:
					case ConsoleKey.DownArrow:
						optionMenu.MoveSelectorDown();

						break;

					case ConsoleKey.Enter:
						OptionSelect(optionMenu.SelectorLocation);
						inMenu = false;

						break;

					default:
						break;
				}
			}
			while (inMenu);

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void BanditAmbush(Player player)
        {
            bool inMenu = true;
            Menu optionMenu = new Menu("", new List<string>(new string[] { "Attack", "Concede", "Refuse", "Persuade", "Retreat" }));

            const int ATTACK = 0;
            const int CONCEDE = 1;
            const int REFUSE = 2;
            const int PERSUADE = 3;
            const int RETREAT = 4;

            const int MIN_FOOD_REQUIRED = 30;
            const int MAX_FOOD_REQUIRED = 60;

            int foodRequired = randomGenerator.Next(MIN_FOOD_REQUIRED, MAX_FOOD_REQUIRED + 1);

            Console.ForegroundColor = ConsoleColor.White;

            void OptionSelect(int option)
            {
                switch (option)
                {
                    case ATTACK:
                        const int AMMO_MIN = 35;
                        const int AMMO_MAX = 45;

                        int ammoNeeded = randomGenerator.Next(AMMO_MIN, AMMO_MAX + 1);

                        if (ammoNeeded > player.Ammo)
                        {
                            const int DAMAGE_MIN = 20;
                            const int DAMAGE_MAX = 65;

                            int damage = randomGenerator.Next(DAMAGE_MIN, DAMAGE_MAX + 1);

                            if (damage > player.Health)
                            {
                                damage = player.Health;
                            }

                            Console.WriteLine("\nYou attempt to shoot at the bandits");
                            Console.WriteLine($"You do not have enough ammunition to keep fighting and take {damage} damage.");
                            Console.WriteLine("You also use all of your ammunition.");
                            Console.WriteLine("The bandits finally run off after an intense gun fight.");
                            
                            player.TakeDamage(damage);
                            player.Ammo = 0;
                        }
                        else
                        {
                            const int DAMAGE_MIN = 10;
                            const int DAMAGE_MAX = 35;

                            const int LOOTED_FOOD_MIN = 10;
                            const int LOOTED_FOOD_MAX = 40;
                            const int LOOTED_AMMO_MIN = 20;
                            const int LOOTED_AMMO_MAX = 30;
                            const int LOOTED_MEDKITS_MIN = 2;
                            const int LOOTED_MEDKITS_MAX = 4;

                            int damage = randomGenerator.Next(DAMAGE_MIN, DAMAGE_MAX + 1);

                            if (damage > player.Health)
                            {
                                damage = player.Health;
                            }

                            Console.WriteLine("\nYou deny their demands and begin firing upon them.");
                            Console.WriteLine("Despite you being outnumbered, you manage to defeat them all.");
                            Console.WriteLine($"You used {ammoNeeded} ammunition in the fight.");
                            Console.WriteLine($"You also took {damage} damage.");

                            player.TakeDamage(damage);
                            player.Ammo -= ammoNeeded;

                            if (player.IsAlive)
                            {
                                int lootedFood = randomGenerator.Next(LOOTED_FOOD_MIN, LOOTED_FOOD_MAX + 1);
                                int lootedAmmo = randomGenerator.Next(LOOTED_AMMO_MIN, LOOTED_AMMO_MAX + 1);
                                int lootedMedkits = randomGenerator.Next(LOOTED_MEDKITS_MIN, LOOTED_MEDKITS_MAX + 1);

                                Console.WriteLine($"You looted the bandits' supplies and found {lootedFood} food, {lootedAmmo} ammo and {lootedMedkits} medkits.");
                            }
                        }

                        break;

                    case CONCEDE:
                        if (player.Food >= foodRequired)
                        {
                            Console.WriteLine($"\n{player.Name} reluctanly gives {foodRequired} food to the bandits.");
                            Console.WriteLine("Satisfied with the outcome, the bandits leave the area.");

                            player.Food -= foodRequired;
                        }
                        else
                        {
                            const int DAMAGE_MIN = 20;
                            const int DAMAGE_MAX = 55;

                            int damage = randomGenerator.Next(DAMAGE_MIN, DAMAGE_MAX + 1);

                            if (damage > player.Health)
                            {
                                damage = player.Health;
                            }

                            Console.WriteLine($"\n{player.Name} explains that they do not have the required amount of food.");
                            Console.WriteLine($"Unpleased with this, the bandits shoot {player.Name} and steal all of their food.");
                            Console.WriteLine($"The shot does {damage} damage to you.");

                            player.TakeDamage(damage);
                            player.Food = 0;
                        }

                        break;

                    case REFUSE:
                        const int REFUSE_DAMAGE_MIN = 20;
                        const int REFUSE_DAMAGE_MAX = 55;

                        int refuseShotDamage = randomGenerator.Next(REFUSE_DAMAGE_MIN, REFUSE_DAMAGE_MAX + 1);

                        if (refuseShotDamage > player.Health)
                        {
                            refuseShotDamage = player.Health;
                        }

                        Console.WriteLine("\nYou refuse the deal.");
                        Console.WriteLine("Unpleased with this, the bandits shoot you and then drive off.");
                        Console.WriteLine($"The shot does {refuseShotDamage} damage to you.");

                        player.TakeDamage(refuseShotDamage);

                        break;

                    case PERSUADE:
                        Console.WriteLine("\nYou desperately ask the bandits not to go through with their demands.");

                        double outcome = randomGenerator.NextDouble();

                        if (outcome > 0.5)
                        {
                            Console.WriteLine("The bandits feel pity for you and drive off.");
                        }
                        else
                        {
                            const int DAMAGE_MIN = 20;
                            const int DAMAGE_MAX = 55;

                            int shotDamage = randomGenerator.Next(DAMAGE_MIN, DAMAGE_MAX + 1);

                            if (shotDamage > player.Health)
                            {
                                shotDamage = player.Health;
                            }

                            Console.WriteLine("The bandits do not care for you and shoot you before driving off.");
                            Console.WriteLine($"The shot does {shotDamage} damage to you.");

                            player.Health -= shotDamage;
                        }

                        break;

                    case RETREAT:
                        const int RETREAT_DAMAGE_MIN = 20;
                        const int RETREAT_DAMAGE_MAX = 55;

                        int retreatShotDamage = randomGenerator.Next(RETREAT_DAMAGE_MIN, RETREAT_DAMAGE_MAX + 1);

                        if (retreatShotDamage > player.Health)
                        {
                            retreatShotDamage = player.Health;
                        }

                        Console.WriteLine("\nYou attempt to retreat.");
                        Console.WriteLine("You don't make it far before the bandits shoot you and then drive off.");
                        Console.WriteLine($"The shot does {retreatShotDamage} damage to you.");

                        player.TakeDamage(retreatShotDamage);

                        break;

                    default:
                        break;
                }
            }

            do
            {
                Console.Clear();
                player.PrintStats();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ACTIVITY LOG");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($"{player.Name} is stopped in the middle of the road by some bandits.");
                Console.WriteLine($"The demand {foodRequired} food as payment to pass through.");
                Console.WriteLine("What do you want to do?\n");

                optionMenu.Print();

                ConsoleKey userInput = Console.ReadKey().Key;

                switch (userInput)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        optionMenu.MoveSelectorUp();

                        break;

                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        optionMenu.MoveSelectorDown();

                        break;

                    case ConsoleKey.Enter:
                        OptionSelect(optionMenu.SelectorLocation);
                        inMenu = false;

                        break;

                    default:
                        break;
                }
            }
            while (inMenu);

            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();
        }
    }
}
