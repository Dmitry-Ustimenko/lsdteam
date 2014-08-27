using System;
using System.Data.Entity.Core.Objects;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess
{
	public interface IUnitOfWork: IDisposable
	{
		void Commit();

		ObjectContext ObjectContext { get; }
	}
}
