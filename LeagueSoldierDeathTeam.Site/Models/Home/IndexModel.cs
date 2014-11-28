using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Classes;

namespace LeagueSoldierDeathTeam.Site.Models.Home
{
	public class IndexModel
	{
		public IEnumerable<ImageModel> SliderImages { get; set; }

		public IEnumerable<NewsData> NewsData { get; set; }

		public IndexModel()
		{
			SliderImages = StaticResourse.GetSliderImages();
			NewsData = new List<NewsData>();
		}
	}
}