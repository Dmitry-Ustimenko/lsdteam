﻿using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories
{
	public abstract class RepositoryFactoryBase
	{
		public abstract IRepository<User> CreateUserRepository();

		public abstract IRepository<Role> CreateRoleRepository();

		public abstract IRepository<UserInfo> CreateUserInfoRepository();

		public abstract IRepository<UserExternalInfo> CreateUserExternalInfoRepository();

		public abstract IRepository<UserResetToken> CreateUserResetTokenRepository();

		public abstract IRepository<UserActivateToken> CreateUserActivateTokenRepository();
	}
}
