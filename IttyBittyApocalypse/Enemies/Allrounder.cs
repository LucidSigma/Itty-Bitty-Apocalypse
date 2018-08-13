using System;

namespace IttyBittyApocalypse
{
	internal class Allrounder : Enemy
	{
		public Allrounder(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Allrounder";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} zombie fiercely tries to eviscerate you.");
		}
	}
}