namespace LeagueSoldierDeathTeam.Site.Models.Administration
{
	public class UserEditItemModel
	{
		public int UserId { get; set; }

		public string UserName { get; set; }

		public string UserEmail { get; set; }

		public string UserPhoto { get; set; }

		public bool IsBanned { get; set; }

		public bool IsActive { get; set; }
	}
}