using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.Account
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Поле 'Логин' не заполнено.")]
		[DisplayName("Логин")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Поле 'Пароль' не заполнено.")]
		[DisplayName("Пароль")]
		public string Password { get; set; }

		[DisplayName("Запомнить меня")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }
	}
}