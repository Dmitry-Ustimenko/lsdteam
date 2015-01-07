using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSoldierDeathTeam.Business.Abstractions.Factories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.DataBase.Model;

namespace LeagueSoldierDeathTeam.Business.Services
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
