using System;
using System.Data.Entity.Core.Objects;
using System.Transactions;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.DataBase.Abstractions.DataAccess;

namespace LeagueSoldierDeathTeam.Business.DataAccess
{
	public class UnitOfWork : IUnitOfWork
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
			using (var transaction = new TransactionScope())
			{
				_objectContextProvider.SaveChanges();
				transaction.Complete();
			}
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
