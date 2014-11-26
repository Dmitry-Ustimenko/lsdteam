﻿using System.Collections.Generic;
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
		public IEnumerable<int> PlatformsIds { get; set; }

		public IEnumerable<PlatformData> Platforms { get; set; }

		public EditNewsModel()
		{
			NewsCategories = new List<NewsCategoryData>();
			Platforms = new List<PlatformData>();
			PlatformsIds = new[] { 1, 4, 5 };
		}
	}
}