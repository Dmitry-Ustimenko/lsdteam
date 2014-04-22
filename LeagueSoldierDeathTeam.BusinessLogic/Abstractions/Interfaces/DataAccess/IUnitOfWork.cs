using System.Data.Entity.Core.Objects;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess
{
	public interface IUnitOfWork
	{
		void Commit();

		ObjectContext ObjectContext { get; }
	}
}
