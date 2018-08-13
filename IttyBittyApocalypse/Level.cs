using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IttyBittyApocalypse
{
	public class Level
	{
		private const char PLAYER = '@';
		private const char DEAD_PLAYER = 'X';

		private const char SPIKE = '^';
		private const char WALL = '#';
		private const char EMPTY_SPACE = ' ';
		private const char WATER = '~';
		private const char BREAKABLE = ',';
		private const char EXIT = '/';
        private const char SAFE_EXIT = '&';

		private const char ZOMBIE = 'Z';
		private const char TANK = 'T';
		private const char FIGHTER = 'F';
		private const char SEEKER = 'S';
		private const char ALLROUNDER = 'A';
		private const char MARKSMAN = 'M';
		private const char WRITHER = 'W';
        private const char REINFORCED = 'R';
		private const char JUGGERNAUT = 'J';
		private const char HEALER = 'H';
		private const char INTELLIGENT = 'I';
		private const char DESTROYER = 'D';
		private const char BOSS = 'B';

		private const char CRATE = 'O';
		private const char LOOTED_CRATE = 'C';
		private const char CABINET = '0';
		private const char LOOTED_CABINET = 'Q';
		private const char MEDICAL_BOX = '+';
		private const char LOOTED_MEDICAL_BOX = '-';

		private const char BULLET = '.';
		private const char MARKSMAN_SPIT = '*';

		private enum StatLines
		{
			LevelName = 0,
			PlayerName = 1,
			Health = 2,
			Score = 3,
			Kills = 4,
			Food = 6,
			Ammo = 7,
			Medkits = 8,
			WeaponTitle = 10,
			MeleeWeapon = 11,
			MeleeDamage = 12,
			RangedWeapon = 14,
			RangedDamage = 15,
			Shield = 17,
			ShieldDefence = 18,
			EnemyTitle = 20,
			EnemyName = 21,
			EnemyHealth = 22
		}

		private enum ChestType
		{
			Crate,
			Cabinet,
			MedicalBox
		}

		private readonly List<Enemy> enemies = new List<Enemy>();
		private readonly List<Projectile> projectiles = new List<Projectile>();

		private int[,] enemyData;

		private List<Weapon> meleeWeapons;
		private List<Weapon> rangedWeapons;
		private List<Shield> shields;

		private bool inBattle = false;
		private Enemy battleEnemy = null;

		private static Random randomGenerator = new Random();

		public List<StringBuilder> LevelMap { get; private set; } = new List<StringBuilder>();

        public string Name { get; private set; }
        public bool IsComplete { get; private set; }

		public void Load(string filename, int[,] enemyData, List<Weapon> meleeWeapons, List<Weapon> rangedWeapons, List<Shield> shields)
		{
			string[] levelContents = File.ReadAllLines(filename);
			StringBuilder[] conversionList = new StringBuilder[levelContents.Length];

			for (int i = 0; i < levelContents.Length; i++)
			{
				conversionList[i] = new StringBuilder(levelContents[i]);
			}

			LevelMap = conversionList.ToList<StringBuilder>();

            Name = filename.ToUpper();

			try
			{
				Name = Name.Split('\\')[1];
			}
			catch (IndexOutOfRangeException)
			{
				Name = Name.Split('/')[1];
			}

			Name = Name.Split('.')[0];
			Name = Name.Replace('_', ' ');

			this.enemyData = enemyData;
			this.meleeWeapons = meleeWeapons;
			this.rangedWeapons = rangedWeapons;
			this.shields = shields;					
		}

		public void Print(Player player)
		{
			Console.Clear();

			for (int i = 0; i < LevelMap.Count; i++)
			{
				for (int j = 0; j < LevelMap[i].Length; j++)
				{
					switch (LevelMap[i][j])
					{
						case PLAYER:
							ChangeConsoleColour(ConsoleColor.Yellow);
							break;

						case DEAD_PLAYER:
							ChangeConsoleColour(ConsoleColor.DarkRed);
							break;

						case WALL:
							ChangeConsoleColour(ConsoleColor.Gray, ConsoleColor.DarkGray);
							break;

						case SPIKE:
							ChangeConsoleColour(ConsoleColor.DarkGray);
							break;

						case WATER:
							ChangeConsoleColour(ConsoleColor.Blue, ConsoleColor.Blue);
							break;

						case BREAKABLE:
							ChangeConsoleColour(ConsoleColor.DarkGray);
							break;

						case EXIT:
							ChangeConsoleColour(ConsoleColor.Cyan);
							break;

                        case SAFE_EXIT:
                            ChangeConsoleColour(ConsoleColor.Cyan, ConsoleColor.DarkBlue);
                            break;

						case ZOMBIE:
						case TANK:
						case FIGHTER:
						case SEEKER:
						case ALLROUNDER:
						case MARKSMAN:
						case WRITHER:
                        case REINFORCED:
						case JUGGERNAUT:
						case HEALER:
						case INTELLIGENT:
						case DESTROYER:
						case BOSS:
							ChangeConsoleColour(ConsoleColor.Green);
							break;

						case CRATE:
						case LOOTED_CRATE:
							ChangeConsoleColour(ConsoleColor.DarkYellow);
							break;

						case CABINET:
						case LOOTED_CABINET:
							ChangeConsoleColour(ConsoleColor.Black, ConsoleColor.DarkGray);
							break;

						case MEDICAL_BOX:
							ChangeConsoleColour(ConsoleColor.Red, ConsoleColor.White);
							break;

						case LOOTED_MEDICAL_BOX:
							ChangeConsoleColour(ConsoleColor.Gray, ConsoleColor.White);
							break;

						case MARKSMAN_SPIT:
							ChangeConsoleColour(ConsoleColor.DarkGreen);
							break;

						case BULLET:
							ChangeConsoleColour(ConsoleColor.Red);
							break;

						case EMPTY_SPACE:
						default:
							ChangeConsoleColour();
							break;
					}

					Console.Write(LevelMap[i][j]);
				}

				ChangeConsoleColour(ConsoleColor.Red);

				switch (i)
				{
					case (int)StatLines.LevelName:
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write($" {Name}");

						break;

					case (int)StatLines.PlayerName:
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write($" {player.Name}");

						break;

					case (int)StatLines.Health:
						Console.Write(" Health:  ");
						break;

					case (int)StatLines.Score:
						Console.Write(" Score:   ");
						break;

					case (int)StatLines.Kills:
						Console.Write(" Kills:   ");
						break;

					case (int)StatLines.Food:
						Console.Write(" Food:    ");
						break;

					case (int)StatLines.Ammo:
						Console.Write(" Ammo:    ");
						break;

					case (int)StatLines.Medkits:
						Console.Write(" Medkits: ");
						break;

					case (int)StatLines.WeaponTitle:
						ChangeConsoleColour(ConsoleColor.White);
						Console.Write(" Weapons");

						break;
					
					case (int)StatLines.MeleeWeapon:
						Console.Write(" Melee:   ");
						break;

					case (int)StatLines.MeleeDamage:
						Console.Write(" Damage:  ");
						break;

					case (int)StatLines.RangedWeapon:
						Console.Write(" Ranged:  ");
						break;

					case (int)StatLines.RangedDamage:
						Console.Write(" Damage:  ");
						break;

					case (int)StatLines.Shield:
						Console.Write(" Shield:  ");
						break;

					case (int)StatLines.ShieldDefence:
						Console.Write(" Defence: ");
						break;

					default:
						break;
				}

				ChangeConsoleColour();
				Console.WriteLine();
			}

			UpdateStats(player);

			ChangeConsoleColour();
		}

		public void Scan(Player player)
		{
			for (int y = 0; y < LevelMap.Count; y++)
			{
				for (int x = 0; x < LevelMap[y].Length; x++)
				{
					int index;

					switch (LevelMap[y][x])
					{
						case PLAYER:
							player.X = x;
							player.Y = y;

							break;

						case ZOMBIE:
							index = (int)EnemyIndices.Zombie;
							enemies.Add(new Zombie(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], ZOMBIE));

							break;

						case FIGHTER:
							index = (int)EnemyIndices.Fighter;
							enemies.Add(new Fighter(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], FIGHTER));

							break;

						case TANK:
							index = (int)EnemyIndices.Tank;
							enemies.Add(new Tank(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], TANK));

							break;

						case SEEKER:
							index = (int)EnemyIndices.Seeker;
							enemies.Add(new Seeker(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], SEEKER));

							break;

						case ALLROUNDER:
							index = (int)EnemyIndices.Allrounder;
							enemies.Add(new Allrounder(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], ALLROUNDER));

							break;

						case MARKSMAN:
							index = (int)EnemyIndices.Marksman;
							enemies.Add(new Marksman(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], MARKSMAN));

							break;

						case WRITHER:
							index = (int)EnemyIndices.Writher;
							enemies.Add(new Writher(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], WRITHER));

							break;

                        case REINFORCED:
                            index = (int)EnemyIndices.Reinforced;
							enemies.Add(new Reinforced(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], REINFORCED));

							break;

						case JUGGERNAUT:
							index = (int)EnemyIndices.Juggernaut;
							enemies.Add(new Juggernaut(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], JUGGERNAUT));

							break;

						case HEALER:
							index = (int)EnemyIndices.Healer;
							enemies.Add(new Healer(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], HEALER));

							break;

						case INTELLIGENT:
							index = (int)EnemyIndices.Intelligent;
							enemies.Add(new Intelligent(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], INTELLIGENT));

							break;

						case DESTROYER:
							index = (int)EnemyIndices.Destroyer;
							enemies.Add(new Destroyer(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], DESTROYER));

							break;

						case BOSS:
							index = (int)EnemyIndices.Boss;
							enemies.Add(new Boss(x, y, enemyData[index, (int)EnemyAttributes.Health], enemyData[index, (int)EnemyAttributes.Score], enemyData[index, (int)EnemyAttributes.Range], enemyData[index, (int)EnemyAttributes.MinDamage], enemyData[index, (int)EnemyAttributes.MaxDamage], BOSS));

							break;

						default:
							break;
					}
				}
			}
		}

		public bool MovePlayer(Player player, int targetX, int targetY)
		{
			switch (LevelMap[targetY][targetX])
			{
				case SPIKE:
					const int MIN_SPIKE_DAMAGE = 8;
					const int MAX_SPIKE_DAMAGE = 19;

					int spikeDamage = randomGenerator.Next(MIN_SPIKE_DAMAGE, MAX_SPIKE_DAMAGE);

					player.TakeDamage(spikeDamage);

					UpdateStats(player);

					goto case EMPTY_SPACE;

				case MARKSMAN_SPIT:
					Projectile impact = FindProjectile(targetX, targetY);
					int damage = randomGenerator.Next(impact.MinDamage, impact.MaxDamage + 1);

					player.TakeDamage(damage);
					impact.Collided = true;

					ClearInactiveObjects();
					UpdateStats(player);

					goto case EMPTY_SPACE;

				case EMPTY_SPACE:
					Console.SetCursorPosition(targetX, targetY);
					ChangeConsoleColour(ConsoleColor.Yellow);
					Console.Write(PLAYER);

					Console.SetCursorPosition(player.X, player.Y);
					ChangeConsoleColour();
					Console.Write(EMPTY_SPACE);

					Console.SetCursorPosition(0, LevelMap.Count);

					LevelMap[targetY][targetX] = PLAYER;
					LevelMap[player.Y][player.X] = EMPTY_SPACE;

					player.X = targetX;
					player.Y = targetY;

					if (!player.IsAlive)
					{
						Console.SetCursorPosition(player.X, player.Y);
						ChangeConsoleColour(ConsoleColor.DarkRed);
						Console.Write(DEAD_PLAYER);

						ChangeConsoleColour();
						Console.SetCursorPosition(0, LevelMap.Count);

						LevelMap[player.Y][player.X] = DEAD_PLAYER;
						IsComplete = true;

						Console.WriteLine("You were killed by a spike.");

						Console.ReadLine();
					}

					return true;

				case BREAKABLE:
					BreakTileWithMelee(targetX, targetY, player.MeleeWeapon.minDamage);

					return true;

				case EXIT:
					IsComplete = true;

					return true;

				case SAFE_EXIT:
					IsComplete = true;
					player.IsSafe = true;

					return true;

				case ZOMBIE:
				case FIGHTER:
				case TANK:
				case SEEKER:
				case ALLROUNDER:
				case MARKSMAN:
				case WRITHER:
                case REINFORCED:
				case JUGGERNAUT:
				case HEALER:
				case INTELLIGENT:
				case DESTROYER:
				case BOSS:
					FightEnemy(FindEnemy(targetX, targetY), player, true);

					ClearInactiveObjects();

					return true;

				case CRATE:
					OpenChest(ChestType.Crate, player, targetX, targetY);

					return true;

				case CABINET:
					OpenChest(ChestType.Cabinet, player, targetX, targetY);

					return true;

				case MEDICAL_BOX:
					OpenChest(ChestType.MedicalBox, player, targetX, targetY);

					return true;

				default:
					return false;
			}
		}

		public void MoveEnemies(Player player)
		{
			foreach (Enemy enemy in enemies)
			{
				if (enemy.IsAlive)
				{
					int enemyTargetX;
					int enemyTargetY;

					Direction direction;

					enemyTargetX = enemy.X;
					enemyTargetY = enemy.Y;

					int xDifference = player.X - enemy.X;
					int yDifference = player.Y - enemy.Y;

					int distanceToPlayer = (int)Math.Sqrt((Math.Pow(xDifference, 2) + Math.Pow(yDifference, 2)));

					if (distanceToPlayer <= enemy.Range)
					{
						if (enemy is Intelligent)
						{
							Intelligent currentEnemy = enemy as Intelligent;

							currentEnemy.Pathfind(ref enemyTargetX, ref enemyTargetY, LevelMap, player);
						}
						else if (Math.Abs(xDifference) < Math.Abs(yDifference))
						{
							if (xDifference > 0)
							{
								enemyTargetX++;
							}
							else if (xDifference < 0)
							{
								enemyTargetX--;
							}
							else if (enemy is Marksman)
							{
								Marksman currentEnemy = enemy as Marksman;

								if (currentEnemy.ShotCounter == 0)
								{
									if (yDifference < 0)
									{
										projectiles.Add(new Projectile(enemy.X, enemy.Y, enemy.MinDamage, enemy.MaxDamage, Direction.Up, MARKSMAN_SPIT));
									}
									else
									{
										projectiles.Add(new Projectile(enemy.X, enemy.Y, enemy.MinDamage, enemy.MaxDamage, Direction.Down, MARKSMAN_SPIT));
									}

									currentEnemy.ShotCounter = currentEnemy.Cooldown;
								}
							}
							else
							{
								if (yDifference > 0)
								{
									enemyTargetY++;
								}
								else
								{
									enemyTargetY--;
								}
							}
						}
						else
						{
							if (yDifference > 0)
							{
								enemyTargetY++;
							}
							else if (yDifference < 0)
							{
								enemyTargetY--;
							}
							else if (enemy is Marksman)
							{
								Marksman currentEnemy = enemy as Marksman;

								if (currentEnemy.ShotCounter == 0)
								{
									if (xDifference < 0)
									{
										projectiles.Add(new Projectile(enemy.X, enemy.Y, enemy.MinDamage, enemy.MaxDamage, Direction.Left, MARKSMAN_SPIT));
									}
									else
									{
										projectiles.Add(new Projectile(enemy.X, enemy.Y, enemy.MinDamage, enemy.MaxDamage, Direction.Right, MARKSMAN_SPIT));
									}

									currentEnemy.ShotCounter = currentEnemy.Cooldown;
								}
							}
							else
							{
								if (xDifference > 0)
								{
									enemyTargetX++;
								}
								else
								{
									enemyTargetX--;
								}
							}
						}
					}
					else
					{
						direction = (Direction)(randomGenerator.Next() % Enum.GetNames(typeof(Direction)).Length);

						switch (direction)
						{
							case Direction.Up:
								enemyTargetY--;

								break;

							case Direction.Down:
								enemyTargetY++;

								break;

							case Direction.Left:
								enemyTargetX--;

								break;

							case Direction.Right:
								enemyTargetX++;

								break;

							case Direction.None:
							default:
								break;
						}
					}

					if (enemy is Marksman)
					{
						Marksman currentEnemy = enemy as Marksman;

						if (currentEnemy.ShotCounter > 0)
						{
							currentEnemy.ShotCounter--;
						}
					}

					if (enemy is Healer)
					{
						Healer currentEnemy = enemy as Healer;

						if (currentEnemy.HealCounter > 0)
						{
							currentEnemy.HealCounter--;
						}
						else
						{
							if (currentEnemy.Health < currentEnemy.InitialHealth)
							{
								currentEnemy.Heal();

								currentEnemy.HealCounter = currentEnemy.Cooldown;
							}
						}
					}

					switch (LevelMap[enemyTargetY][enemyTargetX])
					{
						case EMPTY_SPACE:
							Console.SetCursorPosition(enemyTargetX, enemyTargetY);
							ChangeConsoleColour(ConsoleColor.Green);
							Console.Write(enemy.Tile);

							Console.SetCursorPosition(enemy.X, enemy.Y);
							ChangeConsoleColour();
							Console.Write(EMPTY_SPACE);

							Console.SetCursorPosition(0, LevelMap.Count);

							LevelMap[enemyTargetY][enemyTargetX] = enemy.Tile;
							LevelMap[enemy.Y][enemy.X] = EMPTY_SPACE;

							enemy.X = enemyTargetX;
							enemy.Y = enemyTargetY;

							break;

						case BREAKABLE:
							if (enemy is Destroyer)
							{
								BreakTileWithMelee(enemyTargetX, enemyTargetY, enemy.MinDamage);
							}

							break;

						case PLAYER:
							FightEnemy(enemy, player, false);

							break;

						default:
							break;
					}
				}

				if (!player.IsAlive)
				{
					break;
				}
			}

			ClearInactiveObjects();
		}

		public void UpdateStats(Player player)
		{
			const int TEXT_WIDTH = 10;

			void ClearLine(int initialX, int initialY)
			{
				const int LINES_TO_CLEAR = 40;

				for (int i = 0; i < LINES_TO_CLEAR; i++)
				{
					Console.Write(' ');
				}

				Console.SetCursorPosition(initialX, initialY);
			}
			
			int beginColumn = LevelMap[0].Length + 1 + TEXT_WIDTH;

			for (int i = 0; i < LevelMap.Count; i++)
			{
				Console.SetCursorPosition(beginColumn, i);
				ChangeConsoleColour(ConsoleColor.White);

				switch (i)
				{
					case (int)StatLines.Health:
						ClearLine(beginColumn, i);

						Console.Write(player.Health);

						break;

					case (int)StatLines.Score:
						ClearLine(beginColumn, i);

						Console.Write(player.Score);

						break;

					case (int)StatLines.Kills:
						ClearLine(beginColumn, i);

						Console.Write(player.KillCount);

						break;

					case (int)StatLines.Food:
						ClearLine(beginColumn, i);

						Console.Write(player.Food);

						break;

					case (int)StatLines.Ammo:
						ClearLine(beginColumn, i);

						Console.Write(player.Ammo);

						break;

					case (int)StatLines.Medkits:
						ClearLine(beginColumn, i);

						Console.Write(player.Medkits);

						break;

					case (int)StatLines.MeleeWeapon:
						ClearLine(beginColumn, i);

						Console.Write(player.MeleeWeapon.name);

						break;

					case (int)StatLines.MeleeDamage:
						ClearLine(beginColumn, i);

						if (player.MeleeWeapon.minDamage == player.MeleeWeapon.maxDamage)
						{
							Console.Write(player.MeleeWeapon.minDamage);
						}
						else
						{
							Console.Write($"{player.MeleeWeapon.minDamage}-{player.MeleeWeapon.maxDamage}");
						}

						break;

					case (int)StatLines.RangedWeapon:
						ClearLine(beginColumn, i);

						Console.Write(player.RangedWeapon.name);

						break;

					case (int)StatLines.RangedDamage:
						ClearLine(beginColumn, i);

						if (player.RangedWeapon.minDamage == player.RangedWeapon.maxDamage)
						{
							Console.Write(player.RangedWeapon.minDamage);
						}
						else
						{
							Console.Write($"{player.RangedWeapon.minDamage}-{player.RangedWeapon.maxDamage}");
						}

						break;

					case (int)StatLines.Shield:
						ClearLine(beginColumn, i);

						Console.Write(player.ArmShield.name);

						break;

					case (int)StatLines.ShieldDefence:
						ClearLine(beginColumn, i);

						if (player.ArmShield.minDefence == player.ArmShield.maxDefence)
						{
							Console.Write(player.ArmShield.minDefence);
						}
						else
						{
							Console.Write($"{player.ArmShield.minDefence}-{player.ArmShield.maxDefence}");
						}

						break;

					case (int)StatLines.EnemyTitle:
						if (inBattle)
						{
							Console.SetCursorPosition(beginColumn - TEXT_WIDTH, i);
							Console.Write("Enemy");
						}
						else
						{
							Console.SetCursorPosition(beginColumn - TEXT_WIDTH, i);
							ClearLine(beginColumn - TEXT_WIDTH, i);
						}

						break;

					case (int)StatLines.EnemyName:
						if (inBattle)
						{
							Console.SetCursorPosition(beginColumn - TEXT_WIDTH, i);
							ClearLine(beginColumn - TEXT_WIDTH, i);

							ChangeConsoleColour(ConsoleColor.Red);
							Console.Write("Type:");

							Console.SetCursorPosition(beginColumn, i);

							ChangeConsoleColour(ConsoleColor.White);
							Console.Write(battleEnemy.Name);

						}
						else
						{
							Console.SetCursorPosition(beginColumn - TEXT_WIDTH, i);
							ClearLine(beginColumn - TEXT_WIDTH, i);
						}

						break;

					case (int)StatLines.EnemyHealth:
						if (inBattle)
						{
							Console.SetCursorPosition(beginColumn - TEXT_WIDTH, i);
							ClearLine(beginColumn - TEXT_WIDTH, i);

							ChangeConsoleColour(ConsoleColor.Red);
							Console.Write("Health:");

							Console.SetCursorPosition(beginColumn, i);

							ChangeConsoleColour(ConsoleColor.White);
							Console.Write(battleEnemy.Health);
						}
						else
						{
							Console.SetCursorPosition(beginColumn - TEXT_WIDTH, i);
							ClearLine(beginColumn - TEXT_WIDTH, i);
						}

						break;

					default:
						break;
				}
			}

			Console.SetCursorPosition(0, LevelMap.Count);
			ChangeConsoleColour();
		}

		private void FightEnemy(Enemy enemy, Player player, bool playerAttackFirst)
		{
			inBattle = true;
			battleEnemy = enemy;

			int playerDamageMin = player.MeleeWeapon.minDamage;
			int playerDamageMax = player.MeleeWeapon.maxDamage;

			int playerAttack;

			Random attackRoller = new Random();

			UpdateStats(player);

            if (enemy is Zombie)
            {
                Console.WriteLine($"You encounter {enemy.Name.GetArticle()} {enemy.Name.SentenceCase()}.");
            }
            else
            {
                Console.WriteLine($"You encounter {enemy.Name.GetArticle()} {enemy.Name.SentenceCase()} zombie.");
            }

			Console.ReadLine();
			ClearPrompt();

			if (!playerAttackFirst)
			{
				EnemyAttack(enemy, player);
			}

			while ((enemy.Health > 0) && (player.Health > 0))
			{
				playerAttack = attackRoller.Next(playerDamageMin, playerDamageMax + 1);

                int currentHealth;
                
                if (enemy is Reinforced)
                {
                    Reinforced currentEnemy = enemy as Reinforced;

                    currentHealth = currentEnemy.Health;

                    currentEnemy.TakeDamage(playerAttack);

                    if (!currentEnemy.IsAlive)
                    {
                        playerAttack = currentHealth + currentEnemy.Block;
                    }
                }
                else
                {
                    if (enemy.Health < playerAttack)
                    {
                        playerAttack = enemy.Health;

                        enemy.TakeDamage(playerAttack);
                    }
                    else
                    {
                        enemy.TakeDamage(playerAttack);
                    }
                }

                Console.WriteLine($"You attack the {enemy.Name.SentenceCase()} with your {player.MeleeWeapon.name.SentenceCase()}.");
                Console.WriteLine($"You deal {playerAttack} damage to it.");

                if (enemy is Reinforced)
                {
                    Reinforced currentEnemy = enemy as Reinforced;

					if (currentEnemy.Block > 0)
					{
						Console.Write($"The reinforced zombie's armour blocks {currentEnemy.Block} of the damage.");
					}
					else
					{
						Console.Write($"The reinforced zombie's armour doesn't block any damage.");
					}
                }

				Console.ReadLine();

				UpdateStats(player);
				ClearPrompt();

				if (enemy.Health > 0)
				{
					EnemyAttack(enemy, player);
				}
				else
				{
                    Console.SetCursorPosition(enemy.X, enemy.Y);
					Console.Write(EMPTY_SPACE);

					Console.SetCursorPosition(0, LevelMap.Count);

                    if (enemy is Zombie)
                    {
                        Console.WriteLine($"You killed the {enemy.Name.SentenceCase()}.");
                    }
                    else
                    {
                        Console.WriteLine($"You killed the {enemy.Name.SentenceCase()} zombie.");
                    }

                    LevelMap[enemy.Y][enemy.X] = EMPTY_SPACE;
					enemy.IsAlive = false;

                    player.KillCount++;
					player.AddToScore(enemy.Score);

					Console.ReadLine();

					UpdateStats(player);
					ClearPrompt();
				}
			}

			if (!player.IsAlive)
			{
				Console.SetCursorPosition(player.X, player.Y);
				ChangeConsoleColour(ConsoleColor.DarkRed);
				Console.Write(DEAD_PLAYER);

				ChangeConsoleColour();
				Console.SetCursorPosition(0, LevelMap.Count);

				LevelMap[player.Y][player.X] = DEAD_PLAYER;
				IsComplete = true;

				UpdateStats(player);

                if (enemy is Zombie)
                {
                    Console.WriteLine($"You were killed by the {enemy.Name.SentenceCase()}.");
                }
                else
                {
                    Console.WriteLine($"You were killed by the {enemy.Name.SentenceCase()} zombie.");
                }

				Console.ReadLine();
			}
			else
			{
				inBattle = false;
				battleEnemy = null;

				UpdateStats(player);
			}
		}

		public void ClearPrompt()
		{
			const int LINES_TO_CLEAR = 3;
			const int SPACES_PER_LINE = 90;

			Console.SetCursorPosition(0, LevelMap.Count);

			for (int i = 0; i < LINES_TO_CLEAR; i++)
			{
				for (int j = 0; j < SPACES_PER_LINE; j++)
				{
					Console.Write(' ');
				}

				Console.WriteLine();
			}

			Console.SetCursorPosition(0, LevelMap.Count);
		}

		private void EnemyAttack(Enemy enemy, Player player)
		{
			int playerDefenceMin = player.ArmShield.minDefence;
			int playerDefenceMax = player.ArmShield.maxDefence;

			int enemyDamageMin = enemy.MinDamage;
			int enemyDamageMax = enemy.MaxDamage;

			Random attackRoller = new Random();

			int enemyAttack = attackRoller.Next(enemyDamageMin, enemyDamageMax + 1);
			int playerBlock = attackRoller.Next(playerDefenceMin, playerDefenceMax + 1);

			if (player.ArmShield.name != "None")
			{
				if (player.Health < (enemyAttack - playerBlock))
				{
					enemyAttack = player.Health + playerBlock;

					player.TakeDamage(enemyAttack);
				}
				else if (playerBlock < enemyAttack)
				{
					player.TakeDamage(enemyAttack - playerBlock);
				}
			}
			else
			{
				if (player.Health < enemyAttack)
				{
					enemyAttack = player.Health;

					player.TakeDamage(enemyAttack);
				}
				else
				{
					player.TakeDamage(enemyAttack);
				}
			}

			enemy.Attack();
			Console.WriteLine($"It does {enemyAttack} damage to you.");

			if (player.ArmShield.name != "None")
			{
				if (playerBlock < enemyAttack)
				{
					Console.Write($"Your {player.ArmShield.name.SentenceCase()} blocks {playerBlock} damage.");
				}
				else
				{
					Console.Write($"Your {player.ArmShield.name.SentenceCase()} blocks all of the damage.");
				}
			}

			Console.ReadLine();

			UpdateStats(player);
			ClearPrompt();

			if (enemy is Healer)
			{
				Healer currentEnemy = enemy as Healer;

				if (currentEnemy.IsAlive)
				{
					if (currentEnemy.HealCounter == 0)
					{
						if (currentEnemy.Health < currentEnemy.InitialHealth)
						{
							int healthGained = currentEnemy.Heal();
							currentEnemy.HealCounter = currentEnemy.Cooldown;

							if (healthGained > 0)
							{
								Console.WriteLine($"The healer zombie healed {healthGained} of its health.");
							}
							else
							{
								Console.WriteLine("The healer zombie doesn't heal any health.");
							}

							Console.ReadLine();
						}
					}
					else
					{
						currentEnemy.HealCounter--;
					}
				}
			}

			UpdateStats(player);
			ClearPrompt();
		}

		public void ShootBullet(Direction direction, Player player)
		{
			if (player.Ammo > 0)
			{
				int targetX = player.X;
				int targetY = player.Y;

				switch (direction)
				{
					case Direction.Up:
						targetY--;

						break;

					case Direction.Down:
						targetY++;

						break;

					case Direction.Left:
						targetX--;

						break;

					case Direction.Right:
						targetX++;

						break;

					default:
						break;
				}

				Enemy targetEnemy = FindEnemy(targetX, targetY);

				if (targetEnemy != null)
				{
					int damage = randomGenerator.Next(player.RangedWeapon.minDamage, player.RangedWeapon.maxDamage + 1);

					targetEnemy.TakeDamage(damage);
				}
				else
				{
					projectiles.Add(new Projectile(player.X, player.Y, player.RangedWeapon.minDamage, player.RangedWeapon.maxDamage, direction, BULLET));
				}

				player.Shoot();
			}
		}

		public void MoveProjectiles(Player player)
		{
			int targetX;
			int targetY;

            bool killedByMarksman = false;

			foreach (Projectile projectile in projectiles)
			{
				targetX = projectile.X;
				targetY = projectile.Y;

				switch (projectile.ShotDirection)
				{
					case Direction.Up:
						targetY--;

						break;

					case Direction.Down:
						targetY++;

						break;

					case Direction.Left:
						targetX--;

						break;

					case Direction.Right:
						targetX++;

						break;

					default:
						break;
				}

				switch (LevelMap[targetY][targetX])
				{
					case EMPTY_SPACE:
						Console.SetCursorPosition(targetX, targetY);

						if (projectile.Tile == BULLET)
						{
							ChangeConsoleColour(ConsoleColor.Red);
						}
						else if (projectile.Tile == MARKSMAN_SPIT)
						{
							ChangeConsoleColour(ConsoleColor.DarkGreen);
						}
						
						Console.Write(projectile.Tile);

						Console.SetCursorPosition(projectile.X, projectile.Y);
						ChangeConsoleColour();

						if ((LevelMap[projectile.Y][projectile.X] != PLAYER) && (LevelMap[projectile.Y][projectile.X] != MARKSMAN))
						{
							Console.Write(EMPTY_SPACE);

							LevelMap[projectile.Y][projectile.X] = EMPTY_SPACE;
						}

						LevelMap[targetY][targetX] = projectile.Tile;

						Console.SetCursorPosition(0, LevelMap.Count);

						projectile.X = targetX;
						projectile.Y = targetY;

						break;

					case BREAKABLE:
						BreakTileWithProjectile(targetX, targetY, projectile.MinDamage);

						if ((LevelMap[projectile.Y][projectile.X] != PLAYER) && (LevelMap[projectile.Y][projectile.X] != MARKSMAN))
						{
							Console.SetCursorPosition(projectile.X, projectile.Y);
							ChangeConsoleColour();
							Console.Write(EMPTY_SPACE);

							LevelMap[projectile.Y][projectile.X] = EMPTY_SPACE;
						}

						projectile.Collided = true;

						Console.SetCursorPosition(0, LevelMap.Count);

						break;

					case ZOMBIE:
					case FIGHTER:
					case TANK:
					case SEEKER:
					case ALLROUNDER:
					case MARKSMAN:
					case WRITHER:
                    case REINFORCED:
					case JUGGERNAUT:
					case HEALER:
					case INTELLIGENT:
					case DESTROYER:
					case BOSS:
					case PLAYER:
						if ((projectile.Tile == BULLET) && (LevelMap[targetY][targetX] != PLAYER))
						{
							Enemy targetEnemy = FindEnemy(targetX, targetY);
							int damage = randomGenerator.Next(projectile.MinDamage, projectile.MaxDamage + 1);

							targetEnemy.TakeDamage(damage);

							if (!targetEnemy.IsAlive)
							{
								player.AddToScore(targetEnemy.Score);
								player.KillCount++;

								UpdateStats(player);
							}

							if (LevelMap[projectile.Y][projectile.X] != PLAYER)
							{
								Console.SetCursorPosition(projectile.X, projectile.Y);
								ChangeConsoleColour();
								Console.Write(EMPTY_SPACE);

								LevelMap[projectile.Y][projectile.X] = EMPTY_SPACE;

								Console.SetCursorPosition(0, LevelMap.Count);
							}

							projectile.Collided = true;

							ClearDeadEnemies();

							break;
						}				
						else if ((projectile.Tile == MARKSMAN_SPIT) && (LevelMap[targetY][targetX] == PLAYER))
						{
							int damage = randomGenerator.Next(projectile.MinDamage, projectile.MaxDamage + 1);
							int defence = randomGenerator.Next(player.ArmShield.minDefence, player.ArmShield.maxDefence + 1);

							if (defence > damage)
							{
								damage = 0;
							}
							else
							{
								damage -= defence;
							}

							player.TakeDamage(damage);

                            if (!player.IsAlive)
                            {
                                killedByMarksman = true;

                                break;
                            }

							if (LevelMap[projectile.Y][projectile.X] != MARKSMAN)
							{
								Console.SetCursorPosition(projectile.X, projectile.Y);
								ChangeConsoleColour();
								Console.Write(EMPTY_SPACE);

								LevelMap[projectile.Y][projectile.X] = EMPTY_SPACE;

								Console.SetCursorPosition(0, LevelMap.Count);
							}

							projectile.Collided = true;
							UpdateStats(player);

							break;
						}

						goto default;

					default:
						if ((LevelMap[projectile.Y][projectile.X] != PLAYER) && (LevelMap[projectile.Y][projectile.X] != MARKSMAN))
						{
							Console.SetCursorPosition(projectile.X, projectile.Y);
							ChangeConsoleColour();
							Console.Write(EMPTY_SPACE);

							LevelMap[projectile.Y][projectile.X] = EMPTY_SPACE;

							Console.SetCursorPosition(0, LevelMap.Count);
						}

						projectile.Collided = true;

						break;
				}
			}

			if (killedByMarksman)
			{
				Console.SetCursorPosition(player.X, player.Y);
				ChangeConsoleColour(ConsoleColor.DarkRed);
				Console.Write(DEAD_PLAYER);

				ChangeConsoleColour();
				Console.SetCursorPosition(0, LevelMap.Count);

				LevelMap[player.Y][player.X] = DEAD_PLAYER;
				IsComplete = true;

				Console.WriteLine("You were taken out by a marksman zombie.");

				Console.ReadLine();
			}

			ClearInactiveObjects();
		}

		private void OpenChest(ChestType chest, Player player, int chestX, int chestY)
		{
			const int MAX_LOOT_ROLL = 15;

			int lootRoll = randomGenerator.Next() % MAX_LOOT_ROLL;

			const int LOW_FOOD_MIN = 3;
			const int LOW_FOOD_MAX = 7;
			const int MID_FOOD_MIN = 8;
			const int MID_FOOD_MAX = 12;
			const int BIG_FOOD_MIN = 14;
			const int BIG_FOOD_MAX = 19;
			int foodGained;

			const int LOW_AMMO_MIN = 2;
			const int LOW_AMMO_MAX = 7;
			const int MID_AMMO_MIN = 9;
			const int MID_AMMO_MAX = 17;
			const int BIG_AMMO_MIN = 18;
			const int BIG_AMMO_MAX = 27;
			int ammoGained;

			const int MEDKITS_MIN = 1;
			const int MEDKITS_MAX = 5;
			int medkitsGained;
			
			switch (chest)
			{
				case ChestType.Crate:
					switch (lootRoll)
					{
						case 0:
						case 1:
							foodGained = randomGenerator.Next(LOW_FOOD_MIN, LOW_FOOD_MAX);

							Console.WriteLine($"The container had {foodGained} food in it.");

							Console.ReadLine();

							player.Food += foodGained;

							UpdateStats(player);
							ClearPrompt();

							break;

						case 2:
							foodGained = randomGenerator.Next(MID_FOOD_MIN, MID_FOOD_MAX);

							Console.WriteLine($"The container had {foodGained} food in it.");

							Console.ReadLine();

							player.Food += foodGained;

							UpdateStats(player);
							ClearPrompt();

							break;

						case 3:
							foodGained = randomGenerator.Next(BIG_FOOD_MIN, BIG_FOOD_MAX);

							Console.WriteLine($"The container had {foodGained} food in it.");

							Console.ReadLine();

							player.Food += foodGained;

							UpdateStats(player);
							ClearPrompt();

							break;

						case 4:
						case 5:
						case 6:
						case 7:
							Console.WriteLine("The container didn't have anything useful in it.");

							Console.ReadLine();
							ClearPrompt();

							break;

						case 8:
						case 9:
							Console.WriteLine("The container had a medkit.");

							Console.ReadLine();

							player.Medkits++;

							UpdateStats(player);
							ClearPrompt();

							break;

						case 10:
							ammoGained = randomGenerator.Next(LOW_AMMO_MIN, LOW_AMMO_MAX);

							Console.WriteLine($"The container had {ammoGained} bullets in it.");

							Console.ReadLine();

							player.Ammo += ammoGained;

							UpdateStats(player);
							ClearPrompt();

							break;

						case 11:
						case 12:
							FindMeleeWeapon(player);

							break;

						case 13:
							FindRangedWeapon(player);

							break;

						case 14:
							FindShield(player);

							break;

						default:
							break;
					}

					Console.SetCursorPosition(chestX, chestY);
					ChangeConsoleColour(ConsoleColor.DarkYellow);
					Console.Write(LOOTED_CRATE);

					LevelMap[chestY][chestX] = LOOTED_CRATE;

					ChangeConsoleColour();
					Console.SetCursorPosition(0, LevelMap.Count);

					break;

				case ChestType.Cabinet:
					switch (lootRoll)
					{
						case 0:
							ammoGained = randomGenerator.Next(LOW_AMMO_MIN, LOW_AMMO_MAX);

							Console.WriteLine($"The container had {ammoGained} bullets in it.");

							Console.ReadLine();

							player.Ammo += ammoGained;

							UpdateStats(player);
							ClearPrompt();

							break;

						case 1:
						case 2:
						case 3:
							ammoGained = randomGenerator.Next(MID_AMMO_MIN, MID_AMMO_MAX);

							Console.WriteLine($"The container had {ammoGained} bullets in it.");

							Console.ReadLine();

							player.Ammo += ammoGained;

							UpdateStats(player);
							ClearPrompt();

							break;

						case 4:
						case 5:
						case 6:
							ammoGained = randomGenerator.Next(BIG_AMMO_MIN, BIG_AMMO_MAX);

							Console.WriteLine($"The container had {ammoGained} bullets in it.");

							Console.ReadLine();

							player.Ammo += ammoGained;

							UpdateStats(player);
							ClearPrompt();

							break;

						case 7:
						case 8:
						case 9:
							Console.WriteLine("The container didn't have anything useful in it.");

							Console.ReadLine();
							ClearPrompt();

							break;

						case 10:
							Console.WriteLine("The container had a medkit.");

							Console.ReadLine();

							player.Medkits++;

							UpdateStats(player);
							ClearPrompt();

							break;

						case 11:
							FindMeleeWeapon(player);

							break;

						case 12:
						case 13:
						case 14:
							FindRangedWeapon(player);

							break;

						default:
							break;
					}

					Console.SetCursorPosition(chestX, chestY);
					ChangeConsoleColour(ConsoleColor.Black, ConsoleColor.DarkGray);
					Console.Write(LOOTED_CABINET);

					LevelMap[chestY][chestX] = LOOTED_CABINET;

					ChangeConsoleColour();
					Console.SetCursorPosition(0, LevelMap.Count);

					break;

				case ChestType.MedicalBox:
					switch (lootRoll)
					{
						case 0:
						case 1:
							foodGained = randomGenerator.Next(LOW_FOOD_MIN, LOW_FOOD_MAX);

							Console.WriteLine($"The container had {foodGained} food in it.");

							Console.ReadLine();

							player.Food += foodGained;

							UpdateStats(player);
							ClearPrompt();

							break;

						case 2:
						case 3:
						case 4:
						case 5:
						case 6:
						case 7:
						case 8:
							Console.WriteLine("The container had already been looted.");

							Console.ReadLine();
							ClearPrompt();

							break;

						case 9:
						case 10:
						case 11:
						case 12:
						case 13:
						case 14:
							medkitsGained = randomGenerator.Next(MEDKITS_MIN, MEDKITS_MAX);

							if (medkitsGained == 1)
							{
								Console.WriteLine($"The container had a medkit in it.");
							}
							else
							{
								Console.WriteLine($"The container had {medkitsGained} medkits in it.");
							}

							Console.ReadLine();

							player.Medkits += medkitsGained;

							UpdateStats(player);
							ClearPrompt();

							break;

						default:
							break;
					}

					Console.SetCursorPosition(chestX, chestY);
					ChangeConsoleColour(ConsoleColor.Gray, ConsoleColor.White);
					Console.Write(LOOTED_MEDICAL_BOX);

					LevelMap[chestY][chestX] = LOOTED_MEDICAL_BOX;

					ChangeConsoleColour();
					Console.SetCursorPosition(0, LevelMap.Count);

					break;

				default:
					break;
			}
		}

		private void FindMeleeWeapon(Player player)
		{
			Weapon newMeleeWeapon = meleeWeapons[randomGenerator.Next() % meleeWeapons.Count];

			if (newMeleeWeapon.name == player.MeleeWeapon.name)
			{
				Console.WriteLine($"You find {newMeleeWeapon.name.SentenceCase().GetArticle()} {newMeleeWeapon.name.SentenceCase()}, but you already have one.");
			}
			else
			{
				if (newMeleeWeapon.minDamage > player.MeleeWeapon.minDamage)
				{
					Console.WriteLine($"You find {newMeleeWeapon.name.SentenceCase().GetArticle()} {newMeleeWeapon.name.SentenceCase()} and replace your {player.MeleeWeapon.name.SentenceCase()}.");

					player.MeleeWeapon = newMeleeWeapon;
				}
				else if (newMeleeWeapon.minDamage == player.MeleeWeapon.minDamage)
				{
					if (newMeleeWeapon.maxDamage > player.MeleeWeapon.maxDamage)
					{
						Console.WriteLine($"You find {newMeleeWeapon.name.SentenceCase().GetArticle()} {newMeleeWeapon.name.SentenceCase()} and replace your {player.MeleeWeapon.name.SentenceCase()}.");

						player.MeleeWeapon = newMeleeWeapon;
					}
					else
					{
						Console.WriteLine($"You find {newMeleeWeapon.name.SentenceCase().GetArticle()} {newMeleeWeapon.name.SentenceCase()}, but your {player.MeleeWeapon.name.SentenceCase()} is a better weapon.");
					}
				}
				else
				{
					Console.WriteLine($"You find {newMeleeWeapon.name.SentenceCase().GetArticle()} {newMeleeWeapon.name.SentenceCase()}, but your {player.MeleeWeapon.name.SentenceCase()} is a better weapon.");
				}
			}

			Console.ReadLine();

			UpdateStats(player);
			ClearPrompt();
		}

		private void FindRangedWeapon(Player player)
		{
			Weapon newRangedWeapon = rangedWeapons[randomGenerator.Next() % rangedWeapons.Count];

			if (newRangedWeapon.name == player.RangedWeapon.name)
			{
				Console.WriteLine($"You find {newRangedWeapon.name.SentenceCase().GetArticle()} {newRangedWeapon.name.SentenceCase()}, but you already have one.");
			}
			else
			{
				if (newRangedWeapon.minDamage > player.RangedWeapon.minDamage)
				{
					Console.WriteLine($"You find {newRangedWeapon.name.SentenceCase().GetArticle()} {newRangedWeapon.name.SentenceCase()} and replace your {player.RangedWeapon.name.SentenceCase()}.");

					player.RangedWeapon = newRangedWeapon;
				}
				else if (newRangedWeapon.minDamage == player.RangedWeapon.minDamage)
				{
					if (newRangedWeapon.maxDamage > player.RangedWeapon.maxDamage)
					{
						Console.WriteLine($"You find {newRangedWeapon.name.SentenceCase().GetArticle()} {newRangedWeapon.name.SentenceCase()} and replace your {player.RangedWeapon.name.SentenceCase()}.");

						player.RangedWeapon = newRangedWeapon;
					}
					else
					{
						Console.WriteLine($"You find {newRangedWeapon.name.SentenceCase().GetArticle()} {newRangedWeapon.name.SentenceCase()}, but your {player.RangedWeapon.name.SentenceCase()} is a better weapon.");
					}
				}
				else
				{
					Console.WriteLine($"You find {newRangedWeapon.name.SentenceCase().GetArticle()} {newRangedWeapon.name.SentenceCase()}, but your {player.RangedWeapon.name.SentenceCase()} is a better weapon.");
				}
			}

			Console.ReadLine();

			UpdateStats(player);
			ClearPrompt();
		}

		private void FindShield(Player player)
		{
			Shield newShield = shields[randomGenerator.Next() % shields.Count];

			if (player.ArmShield.name == "None")
			{
				Console.WriteLine($"You find {newShield.name.SentenceCase().GetArticle()} {newShield.name.SentenceCase()}.");

				player.ArmShield = newShield;
			}
			else if (newShield.name == player.ArmShield.name)
			{
				Console.WriteLine($"You find {newShield.name.SentenceCase().GetArticle()} {newShield.name.SentenceCase()}, but you already have one.");
			}
			else
			{
				if (newShield.minDefence > player.ArmShield.minDefence)
				{
					Console.WriteLine($"You find {newShield.name.SentenceCase().GetArticle()} {newShield.name.SentenceCase()} and replace your {player.ArmShield.name.SentenceCase()}.");

					player.ArmShield = newShield;
				}
				else if (newShield.minDefence == player.ArmShield.minDefence)
				{
					if (newShield.maxDefence > player.ArmShield.maxDefence)
					{
						Console.WriteLine($"You find {newShield.name.SentenceCase().GetArticle()} {newShield.name.SentenceCase()} and replace your {player.ArmShield.name.SentenceCase()}.");

						player.ArmShield = newShield;
					}
					else
					{
						Console.WriteLine($"You find {newShield.name.SentenceCase().GetArticle()} {newShield.name.SentenceCase()}, but your {player.ArmShield.name.SentenceCase()} is a better shield.");
					}
				}
				else
				{
					Console.WriteLine($"You find {newShield.name.SentenceCase().GetArticle()} {newShield.name.SentenceCase()}, but your {player.ArmShield.name.SentenceCase()} is a better shield.");
				}
			}

			Console.ReadLine();

			UpdateStats(player);
			ClearPrompt();
		}

		private void BreakTileWithMelee(int x, int y, int damage)
		{
			const int MIN_BREAKING_DAMAGE = 4;

			if (damage >= MIN_BREAKING_DAMAGE)
			{
				Console.SetCursorPosition(x, y);
				Console.Write(EMPTY_SPACE);

				LevelMap[y][x] = EMPTY_SPACE;

				Console.SetCursorPosition(0, LevelMap.Count);
			}
			else
			{
				Console.SetCursorPosition(0, LevelMap.Count);
				Console.WriteLine("Your weapon is not strong enough to smash through this.");

				Console.ReadLine();

				ClearPrompt();
			}
		}

		private void BreakTileWithProjectile(int x, int y, int damage)
		{
			const int MIN_BREAKING_DAMAGE = 4;

			if (damage >= MIN_BREAKING_DAMAGE)
			{
				Console.SetCursorPosition(x, y);
				Console.Write(EMPTY_SPACE);

				LevelMap[y][x] = EMPTY_SPACE;

				Console.SetCursorPosition(0, LevelMap.Count);
			}
		}

		private Enemy FindEnemy(int x, int y)
		{
			foreach (Enemy enemy in enemies)
			{
				if ((enemy.X == x) && (enemy.Y == y))
				{
					return enemy;
				}
			}

			return null;
		}

		private Projectile FindProjectile(int x, int y)
		{
			foreach (Projectile projectile in projectiles)
			{
				if ((projectile.X == x) && (projectile.Y == y))
				{
					return projectile;
				}
			}

			return null;
		}

		private void ClearInactiveObjects()
		{
			ClearDeadEnemies();
			ClearCollidedProjectiles();
		}

		private void ClearDeadEnemies()
		{
			foreach (Enemy enemy in enemies)
			{
				if (!enemy.IsAlive)
				{
					Console.SetCursorPosition(enemy.X, enemy.Y);
					ChangeConsoleColour();
					Console.Write(EMPTY_SPACE);
					LevelMap[enemy.Y][enemy.X] = EMPTY_SPACE;
				}
			}

			Console.SetCursorPosition(0, LevelMap.Count);

			enemies.RemoveAll(enemy => !enemy.IsAlive);
		}

		private void ClearCollidedProjectiles()
		{
			projectiles.RemoveAll(projectile => projectile.Collided);
		}

		private void ChangeConsoleColour(ConsoleColor foreground = ConsoleColor.Gray, ConsoleColor background = ConsoleColor.Black)
		{
			Console.ForegroundColor = foreground;
			Console.BackgroundColor = background;
		}
	}
}
