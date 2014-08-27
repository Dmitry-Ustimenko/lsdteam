using System.Data.Entity.Core.Objects;
using LeagueSoldierDeathTeam.DataBaseLayer.Abstractions.DataAccess;

namespace LeagueSoldierDeathTeam.DataBaseLayer.DataAccess
{
	public class ObjectContextProvider : IObjectContextProvider
	{
		public ObjectContext ObjectContext { get; set; }

		public ObjectContextProvider()
		{
			ObjectContext = new ObjectContext("name=Entities");
			ObjectContext.ContextOptions.LazyLoadingEnabled = true;
			ObjectContext.CommandTimeout = 600;
		}

		public void SaveChanges()
		{
			ObjectContext.SaveChanges();
		}
	}
}
