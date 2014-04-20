using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.Site.Abstractions.Classes
{
	public interface IAppContext
	{
		UserData CurrentUser { get; set; }
	}
}