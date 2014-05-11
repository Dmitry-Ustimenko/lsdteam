using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.Site.Classes.Extensions;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class EditMainInfoModel
	{
		[Required]
		public int UserId { get; set; }

		[Required(ErrorMessage = "'Имя на сайте' должно быть введено.")]
		[DisplayName("Имя на сайте")]
		public string UpdateUserName { get; set; }

		[Required(ErrorMessage = "'Email' должен быть введен.")]
		[EmailAddress(ErrorMessage = "Email введен не верно.")]
		[DisplayName("Email")]
		public string UpdateUserEmail { get; set; }

		[DisplayName("Имя")]
		public string FirstName { get; set; }

		[DisplayName("Фамилия")]
		public string LastName { get; set; }

		[DisplayName("Пол")]
		public int? SexId { get; set; }

		public IDictionary<int, string> Sexs { get; set; }

		public HttpPostedFileBase PhotoUploadFile { get; set; }

		public HttpPostedFileBase PhotoUploadFileName { get; set; }

		public EditMainInfoModel()
		{
			Sexs = SexEnum.Man.GetItems();
		}
	}
}