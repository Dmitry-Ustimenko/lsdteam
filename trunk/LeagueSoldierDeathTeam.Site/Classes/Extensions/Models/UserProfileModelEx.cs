using System;
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
			model.UserEmail = data.User.Email;
			model.UserRole = data.User.RoleName;
			model.LastName = data.LastName;
			model.FirstName = data.FirstName;
			model.PhotoPath = data.User.PhotoPath;
			model.SexName = data.SexName;
			model.ShowEmail = data.User.ShowEmail;

			var dateNow = DateTime.Now;
			if (data.DateBirth.HasValue)
			{
				var dateBirthDay = data.DateBirth.Value;
				if (dateNow.Date > dateBirthDay.Date)
				{
					var years = dateNow.Year - dateBirthDay.Year;
					if (dateNow.Month < dateBirthDay.Month || dateNow.Month == dateBirthDay.Month && dateNow.Day < dateBirthDay.Day)
						years--;

					model.Age = years.GetRussianYears();
				}
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
				address.Append(string.Format("д. {0},", data.HomeNumber));

			var str = address.ToString().Trim();
			var index = str.LastIndexOf(',');
			if (index >= 0)
				model.Address = str.Substring(0, index);

			var dateCreate = data.User.CreateDate;
			if (dateNow.Date > dateCreate.Date)
			{
				var days = (int)(dateNow - dateCreate).TotalDays;
				if (days < 32)
					model.Experience = string.Format("{0}", days.GetRussianDays()).Trim();
				else
				{
					var years = dateNow.Year - dateCreate.Year;
					var months = dateNow.Month - dateCreate.Month;

					if (dateNow.Month < dateCreate.Month || dateNow.Month == dateCreate.Month && dateNow.Day < dateCreate.Day)
						years--;

					if (dateNow.Month < dateCreate.Month)
						months = 12 - (dateCreate.Month - dateNow.Month);

					model.Experience = string.Format("{0} {1}", years.GetRussianYears(), months.GetRussianMonths()).Trim();
				}
			}

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