using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;

namespace LeagueSoldierDeathTeam.BusinessLogic.Services
{
	public class AccountService : IAccountService
	{
		#region IAccountService Members

		public bool LogOn(string login, string password)
		{
			return true;
		}

		#endregion
	}
}
