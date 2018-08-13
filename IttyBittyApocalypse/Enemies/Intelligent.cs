using System;
using System.Collections.Generic;
using System.Text;

namespace IttyBittyApocalypse
{
	internal class Intelligent : Enemy
	{
		private int gridSizeX;
		private int gridSizeY;

		public Intelligent(int x, int y, int health, int score, int range, int minDamage, int maxDamage, char tile) : base(x, y, health, score, range, minDamage, maxDamage, tile)
		{
			Name = "Intelligent";
		}

		public override void Attack()
		{
			Console.WriteLine($"The {Name.ToLower()} zombie hones in and stabs you.");
		}

		public void Pathfind(ref int targetX, ref int targetY, List<StringBuilder> levelMap, Player player)
		{
			Node[,] grid = CreateNodeGrid(levelMap);

			Node startNode = new Node(X, Y, levelMap[Y][X]);
			Node targetNode = new Node(player.X, player.Y, levelMap[player.Y][player.X]);

			List<Node> openNodes = new List<Node>();
			HashSet<Node> closedNodes = new HashSet<Node>();

			openNodes.Add(startNode);

			while (openNodes.Count > 0)
			{
				Node currentNode = openNodes[0];

				for (int i = 1; i < openNodes.Count; i++)
				{
					if ((openNodes[i].FCost < currentNode.FCost) || ((openNodes[i].FCost == currentNode.FCost) && (openNodes[i].HCost < currentNode.HCost)))
					{
						currentNode = openNodes[i];
					}
				}

				openNodes.Remove(currentNode);
				closedNodes.Add(currentNode);

				if ((currentNode.X == targetNode.X) && (currentNode.Y == targetNode.Y))
				{
					Node nextNode = RetracePath(startNode, currentNode);

					GetDirection(ref targetX, ref targetY, startNode, nextNode);

					return;
				}

				List<Node> neighbours = GetNeighbours(currentNode, grid);

				foreach (Node neighbour in neighbours)
				{
					if (!neighbour.IsWalkable || closedNodes.Contains(neighbour))
					{
						continue;
					}

					int costToMoveToNeighbour = currentNode.GCost + ManhattanDistance(currentNode, neighbour);

					if ((costToMoveToNeighbour < neighbour.GCost) || (!openNodes.Contains(neighbour)))
					{
						neighbour.GCost = costToMoveToNeighbour;
						neighbour.HCost = ManhattanDistance(neighbour, targetNode);

						neighbour.Parent = currentNode;

						if (!openNodes.Contains(neighbour))
						{
							openNodes.Add(neighbour);
						}
					}
				}
			}
		}

		private Node[,] CreateNodeGrid(List<StringBuilder> levelMap)
		{
			gridSizeX = levelMap[0].Length;
			gridSizeY = levelMap.Count;

			Node[,] grid = new Node[gridSizeX, gridSizeY];

			for (int i = 0; i < gridSizeY; i++)
			{
				for (int j = 0; j < gridSizeX; j++)
				{
					Node currentNode = new Node(j, i, levelMap[i][j]);

					grid[j, i] = currentNode;
				}
			}

			return grid;
		}

		private List<Node> GetNeighbours(Node node, Node[,] grid)
		{
			List<Node> neighbours = new List<Node>();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (Math.Abs(x) == Math.Abs(y))
					{
						continue;
					}

					int neighbourX = node.X + x;
					int neighbourY = node.Y + y;

					if (((neighbourX >= 0) && (neighbourX < gridSizeX)) && ((neighbourY >= 0) && (neighbourY < gridSizeY)))
					{
						neighbours.Add(grid[neighbourX, neighbourY]);
					}
				}
			}

			return neighbours;
		}

		private int ManhattanDistance(Node source, Node destination)
		{
			int xDifference = Math.Abs(destination.X - source.X);
			int yDifference = Math.Abs(destination.Y - source.Y);

			return xDifference + yDifference;
		}
		
		private Node RetracePath(Node start, Node end)
		{
			List<Node> path = new List<Node>();
			Node currentNode = end;

			while (currentNode != start)
			{
				path.Add(currentNode);				

				currentNode = currentNode.Parent;
			}

			path.Reverse();

			return path[0];
		}

		private void GetDirection(ref int targetX, ref int targetY, Node currentNode, Node nextNode)
		{
			if (currentNode.Y == nextNode.Y)
			{
				if (currentNode.X > nextNode.X)
				{
					targetX--;
				}
				else
				{
					targetX++;
				}
			}
			else
			{
				if (currentNode.Y > nextNode.Y)
				{
					targetY--;
				}
				else
				{
					targetY++;
				}
			}
		}
	}
}
