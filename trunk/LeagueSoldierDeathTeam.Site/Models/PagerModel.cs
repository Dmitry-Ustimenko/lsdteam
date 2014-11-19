using System;
using LeagueSoldierDeathTeam.Site.Classes;

namespace LeagueSoldierDeathTeam.Site.Models
{
	public class PagerModel
	{
		public int Count { get; set; }
		public int PageSize { get; set; }
		public int PageCount { get { return (int)Math.Ceiling(Count == 0 ? 1 : Count / (double)PageSize); } }
		public int PageId { get; set; }

		public PagerModel()
		{
			PageSize = Constants.DefaultPageSize;
			PageId = 1;
		}
	}
}