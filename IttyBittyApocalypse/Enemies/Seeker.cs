using System;

namespace IttyBittyApocalypse
{
	internal class Seeker : Enemy
	{
		public Seeker(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Seeker";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} zombie grabs at and lacerates you.");
		}
	}
}
