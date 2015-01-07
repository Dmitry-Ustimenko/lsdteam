using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess.Repositories;

namespace LeagueSoldierDeathTeam.Business.Abstractions.Factories
{
	public abstract class RepositoryFactoryBase
	{
		public abstract IRepository<TEntity> CreateRepository<TEntity>()
			where TEntity : class;
	}
}
