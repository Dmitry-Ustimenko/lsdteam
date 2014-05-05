using System;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.BusinessLogic.DataAccess.Repositories;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

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

		public override IRepository<User> CreateUserRepository()
		{
			return new Repository<User>(_unitOfWork);
		}

		public override IRepository<Role> CreateRoleRepository()
		{
			return new Repository<Role>(_unitOfWork);
		}

		public override IRepository<UserInfo> CreateUserInfoRepository()
		{
			return new Repository<UserInfo>(_unitOfWork);
		}

		public override IRepository<UserExternalInfo> CreateUserExternalInfoRepository()
		{
			return new Repository<UserExternalInfo>(_unitOfWork);
		}

		public override IRepository<UserResetToken> CreateUserResetTokenRepository()
		{
			return new Repository<UserResetToken>(_unitOfWork);
		}

		public override IRepository<UserActivateToken> CreateUserActivateTokenRepository()
		{
			return new Repository<UserActivateToken>(_unitOfWork);
		}
	}
}
