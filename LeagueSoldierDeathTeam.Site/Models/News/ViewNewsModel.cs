﻿using System;
using System.Collections.Generic;
using LeagueSoldierDeathTeam.Business.Dto;

namespace LeagueSoldierDeathTeam.Site.Models.News
{
	public class ViewNewsModel
	{
		public int? Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public int NewsCategoryId { get; set; }

		public string ImagePath { get; set; }

		public DateTime CreateDate { get; set; }

		public bool BlockComments { get; set; }

		public int WriterId { get; set; }

		public string WriterName { get; set; }

		public int CountViews { get; set; }

		public int CountComments { get; set; }

		public NewsCategoryData NewsCategory { get; set; }

		public IList<int> PlatformIds { get; set; }

		public IEnumerable<PlatformData> Platforms { get; set; }

		public IEnumerable<NewsData> PreviousNews { get; set; }

		public ViewNewsModel()
		{
			NewsCategory = new NewsCategoryData();
			Platforms = new List<PlatformData>();
			PlatformIds = new List<int>();
			PreviousNews = new List<NewsData>();
		}
	}
}