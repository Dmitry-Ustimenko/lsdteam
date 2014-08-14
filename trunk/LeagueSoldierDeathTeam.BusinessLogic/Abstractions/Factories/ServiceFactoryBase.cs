using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories
{
	public abstract class ServiceFactoryBase
	{
		public abstract IAccountService CreateAccountService();

		public abstract IAccountProfileService CreateAccountProfileService();
	}
}
