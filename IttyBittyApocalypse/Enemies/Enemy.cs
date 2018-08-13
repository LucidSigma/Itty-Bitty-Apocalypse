namespace IttyBittyApocalypse
{
	public abstract class Enemy
	{
		public int X { get; set; }
		public int Y { get; set; }

		public int Range { get; private set; }
		public int Health { get; set; }
		public int Score { get; private set; }

		public int MinDamage { get; private set; }
		public int MaxDamage { get; private set; }

		public string Name { get; set; }
		public char Tile { get; private set; }

		public bool IsAlive { get; set; } = true;

		public Enemy(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile)
		{
			X = x;
			Y = y;
			Health = health;
			Score = score;
			Range = range;
			MinDamage = minDamage;
			MaxDamage = maxDamage;
			Tile = tile;
		}

		public virtual void TakeDamage(int damage)
		{
			if (Health - damage <= 0)
			{
				Health = 0;
				IsAlive = false;
			}
			else
			{
				Health -= damage;
			}
		}

		public abstract void Attack();
	}
}
