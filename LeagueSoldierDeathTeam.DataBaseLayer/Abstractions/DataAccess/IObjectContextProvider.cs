using System.Data.Entity.Core.Objects;

namespace LeagueSoldierDeathTeam.DataBaseLayer.Abstractions.DataAccess
{
	public interface IObjectContextProvider
	{
		ObjectContext ObjectContext { get; set; }

		void SaveChanges();
	}
}
