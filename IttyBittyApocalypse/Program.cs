namespace IttyBittyApocalypse
{
	internal class Program
	{
		public static int Main(string[] args)
		{
			Game game = new Game();

			if (!game.Initialise())
			{
				return 1;
			}

			game.Play();

			return 0;
		}
	}
}
