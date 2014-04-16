namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services
{
	public interface IAccountService
	{
		bool LogOn(string login, string password);
	}
}
