using LeagueSoldierDeathTeam.BusinessLogic.Dto;
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
			model.NewsCategoryId = data.NewsCategoryId;
			model.PlatformsIds = data.PlatformIds;

			return model;
		}
	}
}