using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services
{
	public interface IAccountService
	{
		UserData LogOn(string login, string password);

		UserData GetUser(string userName);
	}
}
