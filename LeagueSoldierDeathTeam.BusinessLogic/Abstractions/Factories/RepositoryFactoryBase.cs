using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories
{
	public abstract class RepositoryFactoryBase
	{
		public abstract IRepository<User> CreateUserRepository();
	}
}
