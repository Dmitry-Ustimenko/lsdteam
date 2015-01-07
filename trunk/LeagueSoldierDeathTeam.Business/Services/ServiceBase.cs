using System;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;

namespace LeagueSoldierDeathTeam.Business.Services
{
	public class ServiceBase
	{
		protected IUnitOfWork UnitOfWork { get; set; }

		public ServiceBase(IUnitOfWork unitOfWork)
		{
			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");
			UnitOfWork = unitOfWork;
		}
	}
}
