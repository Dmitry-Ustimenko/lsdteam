using System.Collections.Generic;
using LeagueSoldierDeathTeam.Site.Classes;

namespace LeagueSoldierDeathTeam.Site.Models.Home
{
	public class IndexModel
	{
		public IEnumerable<ImageModel> SliderImages { get; set; }

		public IndexModel()
		{
			SliderImages = StaticResourse.GetSliderImages();
		}
	}
}