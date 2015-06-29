using System;
using LeagueSoldierDeathTeam.Business.Abstractions.Factories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.LoggedUser;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.Business.Services;

namespace LeagueSoldierDeathTeam.Business.Factories
{
	public class ServiceFactory : ServiceFactoryBase
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly RepositoryFactoryBase _repositoryFactory;

		private readonly ILoggedUserProvider _loggedUserProvider;

		public ServiceFactory(ILoggedUserProvider loggedUserProvider, IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
		{
			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");
			_unitOfWork = unitOfWork;

			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");
			_repositoryFactory = repositoryFactory;

			if (loggedUserProvider == null)
				throw new ArgumentNullException("loggedUserProvider");
			_loggedUserProvider = loggedUserProvider;
		}

		public override IAccountService CreateAccountService()
		{
			return new AccountService(_loggedUserProvider, _unitOfWork, _repositoryFactory);
		}

		public override IAccountProfileService CreateAccountProfileService()
		{
			return new AccountProfileService(_loggedUserProvider, _unitOfWork, _repositoryFactory);
		}

		public override INewsService CreateNewsService()
		{
			return new NewsService(_loggedUserProvider, _unitOfWork, _repositoryFactory);
		}

		public override IResourceService CreateResourceService()
		{
			return new ResourceService(_loggedUserProvider, _unitOfWork, _repositoryFactory);
		}
	}
}
