using System;
using System.Data.Entity.Core.Objects;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.DataBaseLayer.Abstractions.DataAccess;

namespace LeagueSoldierDeathTeam.BusinessLogic.DataAccess
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly IObjectContextProvider _objectContextProvider;
		private bool _disposed;

		public UnitOfWork(IObjectContextProvider objectContextProvider)
		{
			if (objectContextProvider == null)
				throw new ArgumentNullException("objectContextProvider");
			_objectContextProvider = objectContextProvider;
		}

		public void Commit()
		{
			_objectContextProvider.SaveChanges();
		}

		public ObjectContext ObjectContext
		{
			get { return _objectContextProvider.ObjectContext; }
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
					ObjectContext.Dispose();
			}
			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
