using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.BusinessLogic.Services
{
	public class AccountService : IAccountService
	{
		#region IAccountService Members

		UserData IAccountService.LogOn(string login, string password)
		{
			return new UserData { UserName = "Admin", Email = "admin@gmail.com" };
		}

		void IAccountService.Register(string userName, string email, string password)
		{
		}

		UserData IAccountService.GetUser(string userName)
		{
			return new UserData { UserName = "Admin", Email = "admin@gmail.com" };
		}

		#endregion
	}
}
