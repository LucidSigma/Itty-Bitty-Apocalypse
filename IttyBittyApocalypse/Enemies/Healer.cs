using System;

namespace IttyBittyApocalypse
{
	internal class Healer : Enemy
	{
		private const int MIN_HEAL = 3;
		private const int MAX_HEAL = 6;

		private static Random healthGenerator = new Random();

		public int InitialHealth { get; private set; }
		public int HealCounter { get; set; } = 0;
		public int Cooldown { get; } = 4;

		public Healer(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Healer";
			InitialHealth = Health;
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} zombie claws at your torso.");
		}

		public int Heal()
		{
			int healthHealed = healthGenerator.Next(MIN_HEAL, MAX_HEAL + 1);

			if (healthHealed + Health > InitialHealth)
			{
				healthHealed = InitialHealth - Health;
			}


			Health += healthHealed;

			return healthHealed;
		}
	}
}
