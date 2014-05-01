using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.Account
{
	public class ExternalRegisterModel
	{
		[Required]
		public string ProviderName { get; set; }

		[Required]
		public string ProviderKey { get; set; }

		[Required(ErrorMessage = "Поле 'Имя' не заполнено.")]
		[DisplayName("Имя")]
		public string ExternalUserName { get; set; }

		[Required(ErrorMessage = "Поле 'Email' не заполнено.")]
		[EmailAddress(ErrorMessage = "Email введен не верно.")]
		[DisplayName("Электронная почта")]
		public string ExternalEmail { get; set; }

		[DisplayName("* Пароль")]
		[DataType(DataType.Password)]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "Длина пароля от 8 до 20 символов.")]
		public string ExternalPassword { get; set; }

		[DisplayName("* Повторите пароль")]
		[DataType(DataType.Password)]
		[Compare("ExternalPassword", ErrorMessage = "Пароли не совпадают.")]
		public string ExternalConfirmPassword { get; set; }
	}
}
