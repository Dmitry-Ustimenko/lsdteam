using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class ChangePasswordModel
	{
		[Required]
		public int UserId { get; set; }

		[Required(ErrorMessage = "Поле 'Старый пароль' не заполнено.")]
		[DataType(DataType.Password)]
		[DisplayName("Старый пароль")]
		public string OldPassword { get; set; }

		[Required(ErrorMessage = "Поле 'Пароль' не заполнено.")]
		[DisplayName("Новый пароль")]
		[DataType(DataType.Password)]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "Длина пароля от 8 до 20 символов.")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Поле 'Повторите пароль' не заполнено.")]
		[DisplayName("Повторите пароль")]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
		public string ConfirmNewPassword { get; set; }

		public bool PasswordWasChanged { get; set; }
	}
}