﻿using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services
{
	public interface INewsService
	{
		NewsData GetNews(int id);

		PageData<NewsData> GetNews(int pageId, int pageSize);

		void SaveNews(NewsData data);

		IEnumerable<NewsCategoryData> GetNewsGategories();
	}
}