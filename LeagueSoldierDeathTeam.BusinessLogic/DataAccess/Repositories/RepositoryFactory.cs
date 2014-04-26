using System;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

namespace LeagueSoldierDeathTeam.BusinessLogic.DataAccess.Repositories
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

		public override IRepository<User> CreateUserRepository()
		{
			return new Repository<User>(_unitOfWork);
		}

		public override IRepository<Role> CreateRoleRepository()
		{
			return new Repository<Role>(_unitOfWork);
		}

		public override IRepository<UserExternalInfo> CreateUserExternalInfoRepository()
		{
			return new Repository<UserExternalInfo>(_unitOfWork);
		}
	}
}
