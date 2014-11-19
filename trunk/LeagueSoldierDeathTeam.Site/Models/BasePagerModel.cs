using System.Collections.Generic;

namespace LeagueSoldierDeathTeam.Site.Models
{
	public class BasePagerModel<T>
		where T : class
	{
		public IEnumerable<T> Data { get; set; }

		public PagerModel Pager { get; set; }

		public BasePagerModel()
		{
			Data = new List<T>();
			Pager = new PagerModel();
		}
	}
}