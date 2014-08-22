using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class UserMessageModel
	{
		public int? Id { get; set; }

		[DisplayName("Заголовок")]
		[Required(ErrorMessage = "'Заголовок' должен быть введен")]
		public string Title { get; set; }

		[DisplayName("Сообщение")]
		[Required(ErrorMessage = "'Сообщение' должно быть введено")]
		public string Description { get; set; }

		[DisplayName("Кому")]
		[Required(ErrorMessage = "'Получатель' должен быть введен")]
		public string RecipientName { get; set; }
	}
}