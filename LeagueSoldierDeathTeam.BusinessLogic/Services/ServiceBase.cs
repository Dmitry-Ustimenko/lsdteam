using System;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;

namespace LeagueSoldierDeathTeam.BusinessLogic.Services
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
