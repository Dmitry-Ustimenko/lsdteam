using System.ComponentModel;

namespace LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums
{
	public enum RoleEnum
	{
		[Description("Администратор")]
		Administrator = 1,

		[Description("Модератор")]
		Moderator = 2,

		[Description("Пользователь")]
		User = 3
	}
}
