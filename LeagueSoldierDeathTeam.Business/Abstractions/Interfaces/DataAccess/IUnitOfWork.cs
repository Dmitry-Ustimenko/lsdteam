using System;
using System.Data.Entity.Core.Objects;

namespace LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess
{
	public interface IUnitOfWork: IDisposable
	{
		void Commit();

		ObjectContext ObjectContext { get; }
	}
}
