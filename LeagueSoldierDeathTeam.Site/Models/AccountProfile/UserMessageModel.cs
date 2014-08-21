using System.ComponentModel;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class UserMessageModel
	{
		public int? Id { get; set; }

		[DisplayName("Заголовок")]
		public string Title { get; set; }

		[DisplayName("Сообщение")]
		public string Description { get; set; }

		public int? RecipientId { get; set; }

		[DisplayName("Кому")]
		public string RecipientName { get; set; }
	}
}