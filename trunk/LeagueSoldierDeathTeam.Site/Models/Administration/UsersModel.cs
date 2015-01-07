using LeagueSoldierDeathTeam.Business.Classes.Enums;
using LeagueSoldierDeathTeam.Business.Dto;

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