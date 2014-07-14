using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.Site.Classes;

namespace LeagueSoldierDeathTeam.Site.Models.Administration
{
	public class RoleManagementModel
	{
		public IDictionary<RoleEnum, IEnumerable<UserEditItemModel>> Items { get; set; }

		public RoleManagementModel()
		{
			Items = new Dictionary<RoleEnum, IEnumerable<UserEditItemModel>>
			{
				{RoleEnum.Moderator,new List<UserEditItemModel>()},
				{RoleEnum.User,new List<UserEditItemModel>()} 
			};

			if (AppContext.Current.CurrentUser.IsMainAdmin)
				Items.Add(RoleEnum.Administrator, new List<UserEditItemModel>());
		}
	}
}