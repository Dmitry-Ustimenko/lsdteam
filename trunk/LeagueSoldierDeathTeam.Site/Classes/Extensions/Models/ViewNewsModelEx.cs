using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Site.Models.News;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class ViewNewsModelEx
	{
		public static ViewNewsModel CopyFrom(this ViewNewsModel model, NewsData data)
		{
			model.Id = data.Id;
			model.Title = data.Title;
			model.Description = data.Description;
			model.WriterId = data.WriterId;
			model.WriterName = data.WriterName;
			model.NewsCategoryId = data.NewsCategory.Id;
			model.Platforms = data.Platforms;
			model.ImagePath = data.ImagePath;
			model.CreateDate = data.CreateDate;
			model.CountViews = ++data.CountViews;
			model.NewsCategory = data.NewsCategory;
			model.CountComments = data.CountComments;

			return model;
		}
	}
}