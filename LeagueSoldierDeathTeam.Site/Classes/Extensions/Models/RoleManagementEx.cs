using System.Collections.Generic;
using System.Linq;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Models.Administration;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class RoleManagementEx
	{
		public static RoleManagementModel Map(this IEnumerable<UserData> data)
		{
			var model = new RoleManagementModel();

			if (data == null || !data.Any())
				return model;

			foreach (var group in data.OrderBy(o => o.UserName).ToLookup(o => o.RoleId))
			{
				var items = group.Select(o => new UserEditItemModel
				{
					UserId = o.Id,
					UserName = o.UserName,
					UserEmail = o.Email,
					UserPhoto = o.PhotoPath,
					IsActive = o.IsActive,
					IsBanned = o.IsBanned,
					RoleId = o.RoleId
				});

				if (model.Items.ContainsKey((RoleEnum)group.Key))
					model.Items[(RoleEnum)group.Key] = items;
			}

			return model;
		}
	}
}