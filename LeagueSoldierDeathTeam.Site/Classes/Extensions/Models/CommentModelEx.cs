using LeagueSoldierDeathTeam.Business.Dto.DtoWrapper;
using LeagueSoldierDeathTeam.Site.Models;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class CommentModelEx
	{
		public static CommentModel CopyFrom(this CommentModel model, CommentsWrapper wrapper)
		{
			model.BlockComments = wrapper.BlockComments;
			model.ContentId = wrapper.ContentId;
			model.Data = wrapper.Data;

			return model;
		}
	}
}