using System.ComponentModel;

namespace LeagueSoldierDeathTeam.Site.Models.Account
{
	public class LoginModel
	{
		[DisplayName("Логин")]
		public string UserName { get; set; }

		[DisplayName("Пароль")]
		public string Password { get; set; }

		[DisplayName("Запомнить меня")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }
	}
}