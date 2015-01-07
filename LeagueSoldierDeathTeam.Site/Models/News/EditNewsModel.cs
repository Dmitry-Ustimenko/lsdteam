using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using LeagueSoldierDeathTeam.Business.Dto;

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

		public HttpPostedFileBase ImageUploadFile { get; set; }

		public string ImagePath { get; set; }

		[Required(ErrorMessage = "'Аннотация' должна быть введена")]
		[MaxLength(150, ErrorMessage = "'Аннотация' не должна превышать 150 символов")]
		[DisplayName("Аннотация")]
		public string Annotation { get; set; }

		public string ImageUploadFileName { get; set; }

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
			Annotation = string.Empty;
			Description = string.Empty;
		}
	}
}