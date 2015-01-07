using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Site.Models.AccountProfile;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class EditProfileModelEx
	{
		public static void CopyFrom(this EditProfileModel model, UserInfoData data)
		{
			if (data == null)
				return;

			model.EditMainInfoModel = new EditMainInfoModel
			{
				UserId = data.User.Id,
				UpdateUserName = data.User.UserName,
				UpdateUserEmail = data.User.Email,
				LastName = data.LastName,
				FirstName = data.FirstName,
				SexId = data.SexId,
				ShowEmail = data.User.ShowEmail
			};

			model.EditAdvanceInfoModel = new EditAdvanceInfoModel
			{
				Activity = data.Activity,
				AboutMe = data.AboutMe,
				DateBirth = data.DateBirth.HasValue ? data.DateBirth.Value.ToString(Constants.DateFormat) : string.Empty,
				Country = data.Country,
				Town = data.Town,
				Street = data.Street,
				HomeNum = data.HomeNumber
			};

			model.EditBindInfoModel = new EditBindInfoModel
			{
				SiteLink = data.SiteLink,
				Icq = data.Icq,
				Skype = data.Skype,
				BattleLog = data.BattleLog,
				Steam = data.Steam
			};
		}
	}
}