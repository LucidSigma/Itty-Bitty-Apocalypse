using System;
using System.Collections.Generic;

namespace IttyBittyApocalypse
{
	public class Menu
	{
		private const char SELECTOR = '>';
		private const char NO_SELECTOR = ' ';

		private readonly string title;

		private readonly List<char> selectIcons = new List<char>();

		public List<string> MenuOptions { get; private set; }
		public int SelectorLocation { get; private set; }
		public int Size { get; private set; }

		public Menu(string title, List<string> options)
		{
			this.title = title;
			MenuOptions = options;

			Size = MenuOptions.Count;
			SelectorLocation = 0;

			for (int i = 0; i < Size; i++)
			{
				selectIcons.Add(NO_SELECTOR);
			}

			selectIcons[SelectorLocation] = SELECTOR;
		}

		public void Print()
		{
			if (title != "")
			{
				Console.ForegroundColor = ConsoleColor.Green;

				Console.WriteLine(title);
				Console.WriteLine();
			}

			for (int i = 0; i < Size; i++)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($"{selectIcons[i]} ");

				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine(MenuOptions[i]);
			}

			Console.ForegroundColor = ConsoleColor.White;
		}

		public void MoveSelectorUp()
		{
			if (SelectorLocation > 0)
			{
				SelectorLocation--;

				selectIcons[SelectorLocation] = SELECTOR;
				selectIcons[SelectorLocation + 1] = NO_SELECTOR;
			}
		}

		public void MoveSelectorDown()
		{
			if (SelectorLocation < (Size - 1))
			{
				SelectorLocation++;

				selectIcons[SelectorLocation] = SELECTOR;
				selectIcons[SelectorLocation - 1] = NO_SELECTOR;
			}
		}
	}
}
