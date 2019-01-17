using System;

namespace Namegiver.Models
{
	[Flags]
	public enum SuperTypeEnum : byte
	{
		None = 0,
		SuperHero = 1,
		SuperVillain = 3,
	}
}