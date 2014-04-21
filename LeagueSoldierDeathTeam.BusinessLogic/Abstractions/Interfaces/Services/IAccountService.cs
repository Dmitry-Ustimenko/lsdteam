using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services
{
	public interface IAccountService
	{
		UserData LogOn(string login, string password);

		void Register(string userName, string email, string password);

		UserData GetUser(string userName);
	}
}
