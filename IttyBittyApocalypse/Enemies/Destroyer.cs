using System;

namespace IttyBittyApocalypse
{
	internal class Destroyer : Enemy
	{
		public Destroyer(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Destroyer";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} pounds you with it's fists.");
		}
	}
}
