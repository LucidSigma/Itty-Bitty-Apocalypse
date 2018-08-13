using System;

namespace IttyBittyApocalypse
{
	internal class Tank : Enemy
	{
		public Tank(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Tank";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} zombie throws itself at you.");
		}
	}
}
