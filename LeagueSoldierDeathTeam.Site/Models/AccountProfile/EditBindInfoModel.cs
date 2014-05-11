using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class EditBindInfoModel
	{
		[Required]
		public int UserId { get; set; }

		[DisplayName("Сайт")]
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