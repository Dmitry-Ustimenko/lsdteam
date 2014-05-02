using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.Account
{
	public class PasswordRecoveryModel
	{
		[Required(ErrorMessage = "Поле 'Email' не заполнено.")]
		[EmailAddress(ErrorMessage = "Email введен не верно.")]
		[DisplayName("Электронная почта")]
		public string Email { get; set; }

		public bool EmailWasSend { get; set; }
	}
}