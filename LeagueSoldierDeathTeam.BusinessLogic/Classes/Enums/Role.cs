﻿using System;

namespace LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums
{
	[Flags]
	public enum Role
	{
		SuperAdministrator = 1,
		Administrator = 2,
		Moderator = 4,
		User = 8
	}
}
