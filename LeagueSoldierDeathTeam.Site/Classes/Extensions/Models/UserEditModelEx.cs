using System.Collections.Generic;
using System.Linq;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Models.Administration;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class UserEditModelEx
	{
		public static UserEditModel CopyTo(this IEnumerable<UserData> data)
		{
			var model = new UserEditModel();

			if (data == null || !data.Any())
				return model;

			model.Items = data.Select(o => new UserEditItemModel
			{
				UserId = o.Id,
				UserName = o.UserName,
				UserEmail = o.Email,
				UserPhoto = o.PhotoPath,
				IsActive = o.IsActive,
				IsBanned = o.IsBanned
			});

			return model;
		}
	}
}