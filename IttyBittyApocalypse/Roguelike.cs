using System;
using System.Collections.Generic;

namespace IttyBittyApocalypse
{
	public class Roguelike
	{
		private Level level = new Level();
		private Player player;

		private readonly List<Weapon> meleeWeapons;
		private readonly List<Weapon> rangedWeapons;
		private readonly List<Shield> shields;

		private readonly int[,] enemyData;

		private int targetX;
		private int targetY;

		private bool playerMoved = false;
		private bool playerDidAction = false;

		public Roguelike(string levelFile, Player player, int[,] enemyData, List<Weapon> meleeWeapons, List<Weapon> rangedWeapons, List<Shield> shields)
		{
			this.enemyData = enemyData;
			this.meleeWeapons = meleeWeapons;
			this.rangedWeapons = rangedWeapons;
			this.shields = shields;
			this.player = player;

			Initialise(levelFile);
			level.Scan(player);
		}

		public void Initialise(string levelFile)
		{
			level.Load(levelFile, enemyData, meleeWeapons, rangedWeapons, shields);
		}

		public void Play()
		{
			level.Print(player);
			
			while (!level.IsComplete)
			{
				playerMoved = MovePlayer(GetInput());

				if (!level.IsComplete)
				{
					if (playerMoved || playerDidAction)
					{
						MoveEntities();
					}

					playerMoved = false;
					playerDidAction = false;
				}
			}
		}

		private bool MovePlayer(Direction direction)
		{
			switch (direction)
			{
				case Direction.Up:
					targetX = player.X;
					targetY = player.Y - 1;

					break;

				case Direction.Down:
					targetX = player.X;
					targetY = player.Y + 1;

					break;

				case Direction.Left:
					targetX = player.X - 1;
					targetY = player.Y;

					break;

				case Direction.Right:
					targetX = player.X + 1;
					targetY = player.Y;

					break;

				case Direction.None:
				default:
					level.UpdateStats(player);

					return false;
			}

			return level.MovePlayer(player, targetX, targetY);
		}

		private Direction GetInput()
		{
			ConsoleKey userInput = Console.ReadKey().Key;

			level.ClearPrompt();

			switch (userInput)
			{
				case ConsoleKey.W:
				case ConsoleKey.UpArrow:
					return Direction.Up;

				case ConsoleKey.S:
				case ConsoleKey.DownArrow:
					return Direction.Down;

				case ConsoleKey.A:
				case ConsoleKey.LeftArrow:
					return Direction.Left;

				case ConsoleKey.D:
				case ConsoleKey.RightArrow:
					return Direction.Right;

				case ConsoleKey.I:
					level.ShootBullet(Direction.Up, player);
					PlayerShoot();

					return Direction.None;

				case ConsoleKey.K:
					level.ShootBullet(Direction.Down, player);
					PlayerShoot();

					return Direction.None;

				case ConsoleKey.J:
					level.ShootBullet(Direction.Left, player);
					PlayerShoot();

					return Direction.None;

				case ConsoleKey.L:
					level.ShootBullet(Direction.Right, player);
					PlayerShoot();

					return Direction.None;

				case ConsoleKey.H:
					playerDidAction = player.UseMedkit();

					if (playerDidAction)
					{
						playerMoved = true;
					}

					return Direction.None;

				default:
					break;
			}

			return Direction.None;
		}

		private void MoveEntities()
		{
			level.MoveEnemies(player);
			level.MoveProjectiles(player);
		}

		private void PlayerShoot()
		{
			if (player.Ammo > 0)
			{
				playerDidAction = true;
			}
		}
	}
}
