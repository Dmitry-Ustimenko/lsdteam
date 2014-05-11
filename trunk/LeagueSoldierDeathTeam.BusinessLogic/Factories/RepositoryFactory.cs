using System;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.BusinessLogic.DataAccess.Repositories;

namespace LeagueSoldierDeathTeam.BusinessLogic.Factories
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
