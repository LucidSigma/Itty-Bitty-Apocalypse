using System;

namespace IttyBittyApocalypse
{
	internal class Juggernaut : Enemy
	{
		public Juggernaut(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Juggernaut";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} smashes itself at you.");
		}
	}
}
