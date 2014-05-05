using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Models.AccountProfile;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class EditProfileModelEx
	{
		public static void CopyFrom(this EditProfileModel model, UserInfoData data)
		{
			if (data == null)
				return;

			model.UserId = data.User.Id;
			model.UserName = data.User.UserName;
			model.LastName = data.LastName;
			model.FirstName = data.FirstName;
			model.SexId = data.SexId;

			model.Activity = data.Activity;
			model.AboutMe = data.AboutMe;
			model.DateBirth = data.DateBirth;
			model.Country = data.Country;
			model.Town = data.Town;
			model.Street = data.Street;
			model.HomeNum = data.HomeNumber;

			model.SiteLink = data.SiteLink;
			model.Icq = data.Icq;
			model.Skype = data.Skype;
			model.BattleLog = data.BattleLog;
			model.Steam = data.Steam;
		}
	}
}