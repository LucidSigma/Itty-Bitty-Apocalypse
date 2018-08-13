using System;
using System.Collections.Generic;

namespace IttyBittyApocalypse
{
	public static class ListExtensions
	{
		private static Random randomGenerator = new Random();

		public static void Shuffle<T>(this List<T> list)
		{
			int counter = list.Count;

			while (counter > 1)
			{
				int randomIndex = randomGenerator.Next(counter);
				counter--;

				T temp = list[randomIndex];
				list[randomIndex] = list[counter];
				list[counter] = temp;
			}
		}
	}
}
