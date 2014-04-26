using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.Account
{
	public class ExternalRegisterModel
	{
		public string LoginProvider { get; set; }

		[Required]
		[Display(Name = "Имя")]
		public string UserName { get; set; }

		[Required]
		[Display(Name = "Электронная почта")]
		public string Email { get; set; }

		[DisplayName("* Пароль")]
		[DataType(DataType.Password)]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "Длина пароля от 8 до 20 символов.")]
		public string RegisterPassword { get; set; }

		[DisplayName("* Повторите пароль")]
		[DataType(DataType.Password)]
		[Compare("RegisterPassword", ErrorMessage = "Пароли не совпадают.")]
		public string ConfirmPassword { get; set; }
	}
}
