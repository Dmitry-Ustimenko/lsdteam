using LeagueSoldierDeathTeam.BusinessLogic.Dto;
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
					Title = model.Title,
					Description = model.Description,
					RecipientName = model.RecipientName
				};
		}
	}
}