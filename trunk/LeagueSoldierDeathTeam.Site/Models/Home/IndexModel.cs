using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.Site.Classes;

namespace LeagueSoldierDeathTeam.Site.Models.Home
{
	public class IndexModel
	{
		public IEnumerable<ImageModel> SliderImages { get; set; }

		public int NewsSortId { get; set; }

		public IndexModel()
		{
			SliderImages = StaticResourse.GetSliderImages();
			NewsSortId = (int)NewsSort.Date;
		}
	}
}