using System;
using LeagueSoldierDeathTeam.Site.Classes;

namespace LeagueSoldierDeathTeam.Site.Models
{
	public class PagerModel
	{
		private int _pageId = 1;

		public int Count { get; set; }
		public int PageSize { get; set; }
		public int PageCount { get { return (int)Math.Ceiling(Count == 0 ? 1 : Count / (double)PageSize); } }

		public int PageId
		{
			get { return _pageId <= PageCount ? _pageId : 1; }
			set { _pageId = _pageId != default(int) ? value : 1; }
		}

		public PagerModel()
		{
			PageSize = Constants.DefaultPageSize;
			PageId = 1;
		}
	}
}