using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.Account
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Поле 'Имя' не заполнено.")]
		[DisplayName("Имя")]
		public string RegisterUserName { get; set; }

		[Required(ErrorMessage = "Поле 'Email' не заполнено.")]
		[EmailAddress(ErrorMessage = "Email введен не верно.")]
		[DisplayName("Электронная почта")]
		public string RegisterEmail { get; set; }

		[Required(ErrorMessage = "Поле 'Пароль' не заполнено.")]
		[DisplayName("Пароль")]
		[DataType(DataType.Password)]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "Длина пароля от 8 до 20 символов.")]
		public string RegisterPassword { get; set; }

		[Required(ErrorMessage = "Поле 'Повторите пароль' не заполнено.")]
		[DisplayName("Повторите пароль")]
		[DataType(DataType.Password)]
		[Compare("RegisterPassword", ErrorMessage = "Пароли не совпадают.")]
		public string ConfirmPassword { get; set; }

		public string ReturnUrl { get; set; }
	}
}