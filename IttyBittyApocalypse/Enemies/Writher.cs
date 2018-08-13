using System;

namespace IttyBittyApocalypse
{
	internal class Writher : Enemy
	{
		public Writher(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Writher";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} jumps up and attempts to gouge you.");
		}
	}
}
