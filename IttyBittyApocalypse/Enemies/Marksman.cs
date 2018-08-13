using System;

namespace IttyBittyApocalypse
{
	internal class Marksman : Enemy
	{
		public int ShotCounter { get; set; } = 0;
		public int Cooldown { get; } = 3;

		public Marksman(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Marksman";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} zombie spits diseased saliva at you.");
		}
	}
}
