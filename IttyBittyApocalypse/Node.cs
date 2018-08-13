namespace IttyBittyApocalypse
{
	public class Node
	{
		private const char EMPTY_SPACE = ' ';
		private const char PLAYER = '@';

		public int X { get; private set; }
		public int Y { get; private set; }

		public int GCost { get; set; }
		public int HCost { get; set; }

		public int FCost
		{
			get
			{
				return GCost + HCost;
			}
		}

		public bool IsWalkable { get; private set; }

		public Node Parent { get; set; }

		public Node(int x, int y, char tile)
		{
			X = x;
			Y = y;

			IsWalkable = ((tile == EMPTY_SPACE) || (tile == PLAYER));
		}
	}
}
