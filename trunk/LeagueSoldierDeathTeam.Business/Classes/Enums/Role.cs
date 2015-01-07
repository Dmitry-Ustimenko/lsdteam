using System;

namespace LeagueSoldierDeathTeam.Business.Classes.Enums
{
	[Flags]
	public enum Role
	{
		None = 0,
		MainAdministrator = 1,
		Administrator = 2,
		Moderator = 4,
		User = 8
	}
}
