using System;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.LoggedUser;

namespace LeagueSoldierDeathTeam.Business.Services
{
	public class ServiceBase
	{
		private int? _currentAccountId;
		private readonly object _locker = new Object();
		protected readonly ILoggedUserProvider LoggedUserProvider;

		public int CurrentUserId
		{
			get
			{
				if (!_currentAccountId.HasValue)
				{
					lock (_locker)
					{
						if (!_currentAccountId.HasValue)
						{
							var loggedUser = LoggedUserProvider.LoggedUser;
							_currentAccountId = loggedUser.CurrentUserId;
						}
					}
				}

				return _currentAccountId.GetValueOrDefault();
			}

			set { _currentAccountId = value; }
		}

		protected IUnitOfWork UnitOfWork { get; set; }

		public ServiceBase(ILoggedUserProvider loggedUserProvider, IUnitOfWork unitOfWork)
		{
			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");
			UnitOfWork = unitOfWork;

			LoggedUserProvider = loggedUserProvider;
		}
	}
}
