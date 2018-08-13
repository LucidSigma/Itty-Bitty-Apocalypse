using System;

namespace IttyBittyApocalypse
{
    internal class Reinforced : Enemy
    {
		private const int MIN_DEFENCE = 3;
		private const int MAX_DEFENCE = 8;

        private static Random blockGenerator = new Random();

        public int Block { get; set; }

        public Reinforced(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
        {
            Name = "Reinforced";
        }

        public override void TakeDamage(int damage)
        {
            int currentBlock = blockGenerator.Next(MIN_DEFENCE, MAX_DEFENCE + 1);

            if (currentBlock >= damage)
            {
                currentBlock = damage - 1;
            }

            damage -= currentBlock;
            Block = currentBlock;

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

        public override void Attack()
        {
            Console.WriteLine($"The {Name.ToLower()} barges you with its armour.");
        }
    }
}
