namespace IttyBittyApocalypse
{
	internal class Projectile
	{
		public int X { get; set; }
		public int Y { get; set; }

		public int MinDamage { get; private set; }
		public int MaxDamage { get; private set; }

		public Direction ShotDirection { get; private set; }

		public char Tile { get; private set; }
		public bool Collided { get; set; }

		public Projectile(int x, int y, int minDamage, int maxDamage, Direction direction, char tile)
		{
			X = x;
			Y = y;
			MinDamage = minDamage;
			MaxDamage = maxDamage;
			ShotDirection = direction;
			Tile = tile;

			Collided = false;
		}
	}
}
