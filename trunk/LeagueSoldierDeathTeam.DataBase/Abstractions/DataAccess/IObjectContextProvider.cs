using System.Data.Entity.Core.Objects;

namespace LeagueSoldierDeathTeam.DataBase.Abstractions.DataAccess
{
	public interface IObjectContextProvider
	{
		ObjectContext ObjectContext { get; set; }

		void SaveChanges();
	}
}
