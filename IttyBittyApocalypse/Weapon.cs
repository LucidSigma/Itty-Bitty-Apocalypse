using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttyBittyApocalypse
{
	public struct Weapon
	{
		public string name;

		public int minDamage;
		public int maxDamage;

		public Weapon(string name, int minDamage, int maxDamage)
		{
			this.name = name;
			this.minDamage = minDamage;
			this.maxDamage = maxDamage;
		}
	}
}
