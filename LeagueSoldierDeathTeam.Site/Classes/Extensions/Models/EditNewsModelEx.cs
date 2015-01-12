using System.Linq;
using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Site.Models.News;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class EditNewsModelEx
	{
		public static EditNewsModel CopyFrom(this EditNewsModel model, NewsData data)
		{
			model.Id = data.Id;
			model.Title = data.Title;
			model.Description = data.Description;
			model.WriterName = data.WriterName;
			model.NewsCategoryId = data.NewsCategory.Id;
			model.PlatformIds = data.PlatformIds.ToList();
			model.HiddenPlatformIds = string.Join(",", data.PlatformIds);
			model.ImagePath = data.ImagePath;
			model.Annotation = data.Annotation;
			model.BlockComments = data.BlockComments;

			return model;
		}
	}
}