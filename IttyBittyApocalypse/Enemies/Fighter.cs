using System;

namespace IttyBittyApocalypse
{
	internal class Fighter : Enemy
	{
		public Fighter(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Fighter";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} zombie ferociously claws at you.");
		}
	}
}