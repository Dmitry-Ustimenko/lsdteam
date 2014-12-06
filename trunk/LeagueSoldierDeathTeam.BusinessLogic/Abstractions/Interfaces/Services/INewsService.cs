using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services
{
	public interface INewsService
	{
		NewsData GetNews(int id);

		PageData<NewsData> GetNews(int? newsCategoryId, int? platformId, int newsSortId, int pageId, int pageSize);

		void SaveNews(NewsData data);

		IEnumerable<NewsCategoryData> GetNewsGategories();

		void ChangeCountViews(int id);

		void AddNewsComment(int newsId, CommentData data);

		IEnumerable<CommentData> GetNewsComments(int newsId);

		void DeleteNews(int id);
	}
}
