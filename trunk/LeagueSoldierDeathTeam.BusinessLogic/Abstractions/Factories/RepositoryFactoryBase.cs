using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories
{
	public abstract class RepositoryFactoryBase
	{
		public abstract IRepository<TEntity> CreateRepository<TEntity>()
			where TEntity : class;
	}
}
