namespace LeagueSoldierDeathTeam.Site.Models.Administration
{
	public class AdministrationModel
	{
		public UsersModel UsersModel { get; set; }

		public UserRolesModel UserRolesModel { get; set; }

		public AdministrationModel()
		{
			UsersModel = new UsersModel();
			UserRolesModel = new UserRolesModel();
		}
	}
}