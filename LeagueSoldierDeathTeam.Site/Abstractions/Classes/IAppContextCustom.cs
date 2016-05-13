using LeagueSoldierDeathTeam.Business.Dto;

namespace LeagueSoldierDeathTeam.Site.Abstractions.Classes
{
	public interface IAppContextCustom
	{
		UserData CurrentUser { get; set; }
	}
}