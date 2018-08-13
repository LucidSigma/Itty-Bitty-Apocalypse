using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttyBittyApocalypse
{
	public struct Shield
	{
		public string name;

		public int minDefence;
		public int maxDefence;

		public Shield(string name, int minDefence, int maxDefence)
		{
			this.name = name;
			this.minDefence = minDefence;
			this.maxDefence = maxDefence;
		}
	}
}