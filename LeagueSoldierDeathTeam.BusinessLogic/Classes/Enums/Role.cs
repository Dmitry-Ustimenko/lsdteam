using System;

namespace LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums
{
	[Flags]
	public enum Role
	{
		Administrator = 1,
		Moderator = 2,
		User = 4
	}
}
