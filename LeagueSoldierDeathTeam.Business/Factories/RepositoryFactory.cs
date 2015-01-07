using System;
using LeagueSoldierDeathTeam.Business.Abstractions.Factories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.Business.DataAccess.Repositories;

namespace LeagueSoldierDeathTeam.Business.Factories
{
	public class RepositoryFactory : RepositoryFactoryBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public RepositoryFactory(IUnitOfWork unitOfWork)
		{
			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");
			_unitOfWork = unitOfWork;
		}

		public override IRepository<TEntity> CreateRepository<TEntity>()
		{
			return new Repository<TEntity>(_unitOfWork);
		}
	}
}
