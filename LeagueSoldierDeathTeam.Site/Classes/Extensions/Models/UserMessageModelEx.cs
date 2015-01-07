using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Site.Models.AccountProfile;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class UserMessageModelEx
	{
		public static UserMessageData CopyTo(this UserMessageModel model)
		{
			return model == null ? new UserMessageData()
				: new UserMessageData
				{
					Id = model.MessageId.GetValueOrDefault(),
					Title = model.Title,
					Description = model.Description,
					RecipientName = model.RecipientName
				};
		}
	}
}