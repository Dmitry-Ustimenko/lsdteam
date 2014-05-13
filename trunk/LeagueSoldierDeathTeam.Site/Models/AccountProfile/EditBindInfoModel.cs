using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class EditBindInfoModel
	{
		[Required]
		public int UserId { get; set; }

		[DisplayName("Сайт")]
		[RegularExpression(@"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$",
			ErrorMessage = "Неверный формат ссылки на сайт. Используйте: (http|https|ftp)://*.(com|net|ru|...)")]
		public string SiteLink { get; set; }

		[DisplayName("ICQ")]
		public string Icq { get; set; }

		[DisplayName("Skype")]
		public string Skype { get; set; }

		[DisplayName("BattleLog")]
		public string BattleLog { get; set; }

		[DisplayName("Steam")]
		public string Steam { get; set; }
	}
}