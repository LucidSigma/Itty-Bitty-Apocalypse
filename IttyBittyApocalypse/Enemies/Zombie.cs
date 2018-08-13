using System;

namespace IttyBittyApocalypse
{
	internal class Zombie : Enemy
	{
		public Zombie(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Zombie";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} lunges at you and scratches you.");
		}
	}
}
