using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services;

namespace LeagueSoldierDeathTeam.Business.Abstractions.Factories
{
	public abstract class ServiceFactoryBase
	{
		public abstract IAccountService CreateAccountService();

		public abstract IAccountProfileService CreateAccountProfileService();

		public abstract INewsService CreateNewsService();

		public abstract IResourceService CreateResourceService();
	}
}
