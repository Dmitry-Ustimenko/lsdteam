using System;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Services;

namespace LeagueSoldierDeathTeam.BusinessLogic.Factories
{
	public class ServiceFactory : ServiceFactoryBase
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly RepositoryFactoryBase _repositoryFactory;

		public ServiceFactory(IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
		{
			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");
			_unitOfWork = unitOfWork;

			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");
			_repositoryFactory = repositoryFactory;
		}

		public override IAccountService CreateAccountService()
		{
			return new AccountService(_unitOfWork, _repositoryFactory);
		}

		public override IAccountProfileService CreateAccountProfileService()
		{
			return new AccountProfileService(_unitOfWork, _repositoryFactory);
		}

		public override INewsService CreateNewsService()
		{
			return new NewsService(_unitOfWork, _repositoryFactory);
		}

		public override IResourceService CreateResourceService()
		{
			return new ResourceService(_unitOfWork, _repositoryFactory);
		}
	}
}
