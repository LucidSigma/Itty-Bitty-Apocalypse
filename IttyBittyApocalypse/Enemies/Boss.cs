using System;

namespace IttyBittyApocalypse
{
	internal class Boss : Enemy
	{
		public Boss(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Boss";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} zombie throws you across the room.");
		}
	}
}
