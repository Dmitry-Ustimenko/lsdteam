using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Services;

namespace LeagueSoldierDeathTeam.BusinessLogic.Factories
{
	public class ServiceFactory : ServiceFactoryBase
	{
		public override IAccountService CreateAccountService()
		{
			return new AccountService();
		}
	}
}
