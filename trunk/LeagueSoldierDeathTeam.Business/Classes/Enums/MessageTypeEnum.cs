using System.ComponentModel;

namespace LeagueSoldierDeathTeam.Business.Classes.Enums
{
	public enum MessageTypeEnum
	{
		None = 0,

		[Description("Входящие")]
		Inbox = 1,

		[Description("Отправленные")]
		Sent = 2,

		[Description("Сохраненные")]
		Draft = 3
	}
}
