﻿namespace LeagueSoldierDeathTeam.Site.Models.Administration
{
	public class AdministrationModel
	{
		public UsersModel UserEditModel { get; set; }

		public RoleManagementModel RoleManagementModel { get; set; }

		public AdministrationModel()
		{
			UserEditModel = new UsersModel();
			RoleManagementModel = new RoleManagementModel();
		}
	}
}