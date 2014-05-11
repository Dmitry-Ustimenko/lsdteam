using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class EditAdvanceInfoModel
	{
		[Required]
		public int UserId { get; set; }

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
	}
}