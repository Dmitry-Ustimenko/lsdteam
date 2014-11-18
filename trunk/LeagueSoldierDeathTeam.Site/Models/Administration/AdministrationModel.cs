namespace LeagueSoldierDeathTeam.Site.Models.Administration
{
	public class AdministrationModel
	{
		public UsersModel UsersModel { get; set; }

		public RoleManagementModel RoleManagementModel { get; set; }

		public AdministrationModel()
		{
			UsersModel = new UsersModel();
			RoleManagementModel = new RoleManagementModel();
		}
	}
}