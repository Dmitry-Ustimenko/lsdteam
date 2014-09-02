using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.Account
{
	public class LoginModel : BaseModel
	{
		[Required(ErrorMessage = "Поле 'Логин' не заполнено.")]
		[DisplayName("Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Поле 'Пароль' не заполнено.")]
		[DataType(DataType.Password)]
		[DisplayName("Пароль")]
		public string Password { get; set; }

		[DisplayName("Запомнить меня")]
		public bool RememberMe { get; set; }
	}
}