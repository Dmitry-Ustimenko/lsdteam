﻿using System.ComponentModel;

namespace LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums
{
	public enum RoleEnum
	{
		[Description("Super Admin")]
		SuperAdministrator = 1,

		[Description("Администратор")]
		Administrator = 2,

		[Description("Модератор")]
		Moderator = 4,

		[Description("Пользователь")]
		User = 8
	}
}
