using System.Collections.Generic;

namespace LeagueSoldierDeathTeam.Site.Models.Administration
{
	public class UsersModel
	{
		public IEnumerable<UserEditItemModel> Items { get; set; }

		public UsersModel()
		{
			Items = new List<UserEditItemModel>();
		}
	}
}