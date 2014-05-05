using System;
using System.Globalization;
using System.Linq;
using System.Text;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Models.AccountProfile;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class UserProfileModelEx
	{
		public static void CopyFrom(this UserProfileModel model, UserInfoData data)
		{
			if (data == null)
				return;

			model.UserId = data.User.Id;
			model.UserName = data.User.UserName;
			model.UserRoles = string.Join(",", data.User.Roles.Select(o => o.Name));
			model.LastName = data.LastName;
			model.FirstName = data.FirstName;
			model.PhotoPath = data.PhotoPath;
			model.SexName = data.SexName;

			if (data.DateBirth.HasValue)
			{
				var dateNow = DateTime.Now;
				var dateBirthDay = data.DateBirth.Value;

				var years = dateNow.Year - dateBirthDay.Year;
				if (dateNow.Month < dateBirthDay.Month || dateNow.Month == dateBirthDay.Month && dateNow.Day < dateBirthDay.Day)
					years--;

				model.Age = years == default(int) ? string.Empty : years.ToString(CultureInfo.InvariantCulture);
			}

			model.Activity = data.Activity;
			model.AboutMe = data.AboutMe;
			model.DateBirth = data.DateBirth;

			var address = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(data.Country))
				address.Append(string.Format("{0}, ", data.Country));

			if (!string.IsNullOrWhiteSpace(data.Town))
				address.Append(string.Format("{0}, ", data.Town));

			if (!string.IsNullOrWhiteSpace(data.Street))
				address.Append(string.Format("{0}, ", data.Street));

			if (!string.IsNullOrWhiteSpace(data.HomeNumber))
				address.Append(string.Format("д. {0}", data.HomeNumber));

			model.Address = address.ToString();

			model.CreateDate = data.User.CreateDate;
			model.LastActivity = data.User.LastActivity;

			model.SiteLink = data.SiteLink;
			model.Icq = data.Icq;
			model.Skype = data.Skype;
			model.BattleLog = data.BattleLog;
			model.Steam = data.Steam;
		}
	}
}