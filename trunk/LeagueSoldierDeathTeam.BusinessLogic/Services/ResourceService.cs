using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

namespace LeagueSoldierDeathTeam.BusinessLogic.Services
{
	public class ResourceService : ServiceBase, IResourceService
	{
		#region Private Fields

		private readonly IRepository<Platform> _platformRepository;

		#endregion

		#region Constructors

		public ResourceService(IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
			: base(unitOfWork)
		{
			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");

			_platformRepository = repositoryFactory.CreateRepository<Platform>();
		}

		#endregion

		#region IAccountService Members

		IEnumerable<PlatformData> IResourceService.GetPlatforms()
		{
			return _platformRepository.GetData(o => new PlatformData
			{
				Id = o.Id,
				Name = o.Name,
				ShortName = o.ShortName
			}).ToList();
		}

		#endregion

		#region Internal Implementation



		#endregion
	}
}
