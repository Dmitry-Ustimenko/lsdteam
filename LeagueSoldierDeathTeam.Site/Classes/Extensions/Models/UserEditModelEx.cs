using System.Collections.Generic;
using System.Linq;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Classes.Helpers;
using LeagueSoldierDeathTeam.Site.Models.Administration;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class UsersModelEx
	{
		public static UsersModel CopyTo(this IEnumerable<UserData> data, int pageId = 1)
		{
			var model = new UsersModel();

			var users = data as IList<UserData> ?? data.ToList();
			if (!users.Any())
				return model;

			model.Pager.PageId = pageId;
			model.Pager.Count = users.Count();

			model.Items = users.Select(o => new UserEditItemModel
			{
				UserId = o.Id,
				UserName = o.UserName,
				UserEmail = o.Email,
				UserPhoto = o.PhotoPath,
				IsActive = o.IsActive,
				IsBanned = o.IsBanned
			}).Page(model.Pager.PageId, model.Pager.PageSize);

			return model;
		}
	}
}