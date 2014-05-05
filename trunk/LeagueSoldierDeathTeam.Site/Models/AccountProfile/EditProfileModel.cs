using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.Site.Classes.Extensions;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class EditProfileModel
	{
		[Required]
		public int UserId { get; set; }

		[Required(ErrorMessage = "'Имя на сайте' должно быть введено.")]
		[DisplayName("Имя на сайте")]
		public string UserName { get; set; }

		[DisplayName("Имя")]
		public string FirstName { get; set; }

		[DisplayName("Фамилия")]
		public string LastName { get; set; }

		[DisplayName("Пол")]
		public int? SexId { get; set; }

		public IDictionary<int, string> Sexs { get; set; }

		public HttpPostedFileBase PhotoUploadFile { get; set; }

		public HttpPostedFileBase PhotoUploadFileName { get; set; }


		[DisplayName("Интересы")]
		public string AboutMe { get; set; }

		[DisplayName("Деятельность")]
		public string Activity { get; set; }

		[DisplayName("Дата рождения")]
		public DateTime? DateBirth { get; set; }

		[DisplayName("Страна")]
		public string Country { get; set; }

		[DisplayName("Населенный пункт")]
		public string Town { get; set; }

		[DisplayName("Улица")]
		public string Street { get; set; }

		[DisplayName("Номер дома")]
		public string HomeNum { get; set; }


		[DisplayName("Сайт")]
		public string SiteLink { get; set; }

		[DisplayName("ICQ")]
		public string Icq { get; set; }

		[DisplayName("Skype")]
		public string Skype { get; set; }

		[DisplayName("BattleLog")]
		public string BattleLog { get; set; }

		[DisplayName("Steam")]
		public string Steam { get; set; }

		public EditProfileModel()
		{
			Sexs = SexEnum.Man.GetItems();
		}
	}
}