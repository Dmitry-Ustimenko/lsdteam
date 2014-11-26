using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.Site.Models.News
{
	public class EditNewsModel
	{
		public int? Id { get; set; }

		[Required(ErrorMessage = "'Заголовок' должен быть введен")]
		[DisplayName("Заголовок")]
		public string Title { get; set; }

		[Required(ErrorMessage = "'Описание' должно быть введено")]
		[DisplayName("Описание")]
		public string Description { get; set; }

		[Required(ErrorMessage = "'Категория' должна быть введена")]
		[DisplayName("Категория")]
		public int NewsCategoryId { get; set; }

		public int WriterId { get; set; }

		public string WriterName { get; set; }

		public IEnumerable<NewsCategoryData> NewsCategories { get; set; }

		[DisplayName("Платформы")]
		public IList<int> PlatformIds { get; set; }

		public string HiddenPlatformIds { get; set; }

		public IEnumerable<PlatformData> Platforms { get; set; }

		public EditNewsModel()
		{
			NewsCategories = new List<NewsCategoryData>();
			Platforms = new List<PlatformData>();
			PlatformIds = new List<int>();
			HiddenPlatformIds = string.Empty;
		}
	}
}