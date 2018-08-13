using System;

namespace IttyBittyApocalypse
{
	public class Player
	{
		private const int INITIAL_HEALTH = 200;
		private const int FOOD_PER_DAY = 30;
		private const int STARVATION_DAMAGE = 40;

		private const int MEDKIT_MIN = 15;
		private const int MEDKIT_MAX = 25;

		public string Name { get; set; }

		public int Health { get; set; }
		public int Score { get; set; }
		public int KillCount { get; set; }

		public int X { get; set; }
		public int Y { get; set; }

		public int Food { get; set; }
		public int Medkits { get; set; }
		public int Ammo { get; set; }

		public bool IsAlive { get; set; } = true;
        public bool IsSafe { get; set; } = false;

		public Weapon MeleeWeapon { get; set; }
		public Weapon RangedWeapon { get; set; }
		public Shield ArmShield { get; set; }

		private static Random randomHealth = new Random();

		public Player()
		{
			Health = INITIAL_HEALTH;
			Score = 0;
			KillCount = 0;

			Food = 0;
			Medkits = 0;
			Ammo = 0;
		}

		public void TakeDamage(int damage)
		{
			if (Health - damage <= 0)
			{
				Health = 0;
				IsAlive = false;
			}
			else
			{
				Health -= damage;
			}
		}

		public bool UseMedkit()
		{
			int healthGained = randomHealth.Next(MEDKIT_MIN, MEDKIT_MAX + 1);

			if (Medkits > 0)
			{
				if (Health == INITIAL_HEALTH)
				{
					return false;
				}
				else if (Health + healthGained > INITIAL_HEALTH)
				{
					Health = INITIAL_HEALTH;
					Medkits--;

					return true;
				}
				else
				{
					Health += healthGained;
					Medkits--;

					return true;
				}
			}
			else
			{
				return false;
			}
		}

		public void PrintStats()
		{
			Console.Clear();

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(Name);

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"Health: {Health}");
			Console.WriteLine($"Food: {Food}");
			Console.WriteLine($"Ammo: {Ammo}");
			Console.WriteLine($"Medkits: {Medkits}\n");
		}

		public void Eat()
		{
			Console.ForegroundColor = ConsoleColor.White;

			if (Food > FOOD_PER_DAY)
			{
				Food -= FOOD_PER_DAY;

				Console.WriteLine($"{Name} sets down and eats a decent meal.");
				Console.WriteLine($"You eat {FOOD_PER_DAY} food.");
			}
			else if (Food == FOOD_PER_DAY)
			{
				Food -= FOOD_PER_DAY;

				Console.WriteLine($"{Name} sets down and eats a decent meal.");
				Console.WriteLine($"All of the food was eaten.");
			}
			else if (Food == 0)
			{
				Console.WriteLine($"{Name} has no food and starvation continues it's toll.");

				int damage = STARVATION_DAMAGE;

				if (damage > Health)
				{
					damage = Health;
				}

				Console.WriteLine($"{Name} takes {damage} starvation damage.");

				TakeDamage(damage);
			}
			else
			{
				Console.WriteLine($"{Name} eats what they have left but run out of food. Starvation begins to set in.");

				int damage = STARVATION_DAMAGE - Food;

				if (damage > Health)
				{
					damage = Health;
				}

				Console.WriteLine($"{Name} takes {damage} starvation damage.");

				TakeDamage(damage);
				Food = 0;
			}

			Console.WriteLine("\nPress enter to continue.");
			Console.ReadLine();
		}

		public void Shoot()
		{
			Ammo--;
		}

		public void AddToScore(int points)
		{
			Score += points;
		}
	}
}
