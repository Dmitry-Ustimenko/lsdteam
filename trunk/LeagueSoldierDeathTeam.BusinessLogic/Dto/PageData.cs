using System;
using System.Collections.Generic;

namespace LeagueSoldierDeathTeam.BusinessLogic.Dto
{
	public class PageData<T>
		where T : class
	{
		private int _pageId = 1;

		public int PageId
		{
			get { return _pageId <= PageCount ? _pageId : 1; }
			set { _pageId = _pageId != default(int) ? value : 1; }
		}

		public int Count { get; set; }

		public int PageSize { get; set; }

		public int PageCount { get { return (int)Math.Ceiling(Count == 0 ? 1 : Count / (double)PageSize); } }

		public IEnumerable<T> Data { get; set; }

		public PageData()
		{
			Data = new List<T>();
		}
	}
}
