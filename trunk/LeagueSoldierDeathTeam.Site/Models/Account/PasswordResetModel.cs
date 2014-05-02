using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.Account
{
	public class PasswordResetModel
	{
		[Required(ErrorMessage = "Поле 'Пароль' не заполнено.")]
		[DisplayName("Пароль")]
		[DataType(DataType.Password)]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "Длина пароля от 8 до 20 символов.")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Поле 'Повторите пароль' не заполнено.")]
		[DisplayName("Повторите пароль")]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
		public string ConfirmNewPassword { get; set; }

		public bool PasswordWasChanged { get; set; }

		public string Token { get; set; }
	}
}