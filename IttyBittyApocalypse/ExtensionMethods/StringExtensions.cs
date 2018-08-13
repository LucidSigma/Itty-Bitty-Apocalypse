namespace IttyBittyApocalypse
{
	public static class StringExtensions
	{
		public static string SentenceCase(this string noun)
		{
			if ((noun != "IV drip") && (noun != "RPG")) {
				return noun.ToLower();
			}

			return noun;
		}

		public static string GetArticle(this string noun)
		{
			if ((noun.ToLower() == "rpg") || (noun.ToLower() == "aegis"))
			{
				return "an";
			}
			else if (((noun[noun.Length - 1] == 's') && (noun[noun.Length - 2] != 's')) || (noun.ToLower() == "corrugated iron"))
			{
				return "some";
			}

			switch (noun[0])
			{
				case 'a':
				case 'A':
				case 'e':
				case 'E':
				case 'i':
				case 'I':
				case 'o':
				case 'O':
				case 'u':
				case 'U':
					return "an";

				default:
					return "a";
			}
		}
	}
}
