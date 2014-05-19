using System;
using System.ComponentModel;

namespace LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums
{
	[Flags]
	public enum Role
	{
		[Description("Администратор")]
		Administrator = 1,

		[Description("Модератор")]
		Moderator = 2,

		[Description("Пользователь")]
		User = 4
	}
}
