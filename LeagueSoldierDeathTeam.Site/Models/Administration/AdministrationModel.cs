namespace LeagueSoldierDeathTeam.Site.Models.Administration
{
	public class AdministrationModel
	{
		public UserEditModel UserEditModel { get; set; }

		public AdministrationModel()
		{
			UserEditModel = new UserEditModel();
		}
	}
}