using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.Site.Models.Administration
{
	public class UsersModel : BasePagerModel<UserData>
	{
		public SortEnum SortType { get; set; }

		public string Term { get; set; }

		public UsersModel()
		{
			SortType = SortEnum.Default;
		}
	}
}