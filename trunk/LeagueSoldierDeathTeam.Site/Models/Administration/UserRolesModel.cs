using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.Site.Models.Administration
{
	public class UserRolesModel : BasePagerModel<UserData>
	{
		public RoleEnum RoleType { get; set; }

		public string Term { get; set; }

		public UserRolesModel()
		{
			RoleType = RoleEnum.None;
		}
	}
}