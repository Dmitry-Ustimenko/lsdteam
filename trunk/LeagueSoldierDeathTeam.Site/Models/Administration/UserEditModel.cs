using System.Collections.Generic;

namespace LeagueSoldierDeathTeam.Site.Models.Administration
{
	public class UserEditModel
	{
		public IEnumerable<UserEditItemModel> Items { get; set; }

		public UserEditModel()
		{
			Items = new List<UserEditItemModel>();
		}
	}
}