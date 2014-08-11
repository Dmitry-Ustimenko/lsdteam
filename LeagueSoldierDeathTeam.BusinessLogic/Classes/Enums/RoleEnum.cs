using System.ComponentModel;

namespace LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums
{
	public enum RoleEnum
	{
		None = 0,

		[Description("Главный aдминистратор")]
		MainAdministrator = 1,

		[Description("Администратор")]
		Administrator = 2,

		[Description("Модератор")]
		Moderator = 4,

		[Description("Пользователь")]
		User = 8
	}
}
