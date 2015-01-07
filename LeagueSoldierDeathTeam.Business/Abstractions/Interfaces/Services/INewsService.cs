using System.Collections.Generic;
using LeagueSoldierDeathTeam.Business.Classes.Enums;
using LeagueSoldierDeathTeam.Business.Dto;

namespace LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services
{
	public interface INewsService
	{
		NewsData GetNews(int id);

		PageData<NewsData> GetNews(int? newsCategoryId, int? platformId, int newsSortId, int pageId, int pageSize);

		void SaveNews(NewsData data);

		IEnumerable<NewsCategoryData> GetNewsGategories();

		void ChangeCountViews(int id);

		void AddNewsComment(int newsId, CommentData data);

		void DeleteNewsComment(int id, bool haveRights, int currentUserId);

		IEnumerable<CommentData> GetNewsComments(int newsId, CommentSortEnum sortType);

		void DeleteNews(int id);
	}
}
