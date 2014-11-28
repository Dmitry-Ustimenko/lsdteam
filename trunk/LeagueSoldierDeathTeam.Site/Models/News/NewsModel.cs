using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Classes;

namespace LeagueSoldierDeathTeam.Site.Models.News
{
	public class NewsModel : BasePagerModel<NewsData>
	{
		public int? NewsCategoryId { get; set; }

		public IEnumerable<NewsCategoryData> NewsCategories { get; set; }

		public int? PlatformId { get; set; }

		public IEnumerable<PlatformData> Platforms { get; set; }

		public int SortId { get; set; }

		public NewsModel()
		{
			NewsCategories = new List<NewsCategoryData>();
			Platforms = new List<PlatformData>();
			SortId = (int)NewsSort.Date;
			Pager.PageSize = Constants.NewsPageSize;
		}
	}
}