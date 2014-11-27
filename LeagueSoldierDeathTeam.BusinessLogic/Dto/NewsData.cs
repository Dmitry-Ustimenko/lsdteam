using System;
using System.Collections.Generic;

namespace LeagueSoldierDeathTeam.BusinessLogic.Dto
{
	public class NewsData
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string ShortDescription { get; set; }

		public string Description { get; set; }

		public DateTime CreateDate { get; set; }

		public int CountViews { get; set; }

		public int NewsCategoryId { get; set; }

		public string NewsCategoryName { get; set; }

		public int WriterId { get; set; }

		public string WriterName { get; set; }

		public IEnumerable<int> PlatformIds { get; set; }

		public IEnumerable<PlatformData> Platforms { get; set; }

		public string ImagePath { get; set; }
	}
}
