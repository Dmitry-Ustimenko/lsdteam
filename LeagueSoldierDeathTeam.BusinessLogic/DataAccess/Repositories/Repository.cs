using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;

namespace LeagueSoldierDeathTeam.BusinessLogic.DataAccess.Repositories
{
	public class Repository<TEntity> : IRepository<TEntity>
		where TEntity : class
	{
		private readonly ObjectContext _objectContext;

		private readonly object _synchObj = new object();

		private object ObjSet { get; set; }

		public Repository(IUnitOfWork unitOfWork)
		{
			_objectContext = unitOfWork.ObjectContext;
		}

		public void Add(TEntity entity)
		{
			GetObjectSet<TEntity>().AddObject(entity);
		}

		public void Delete(TEntity entity)
		{
			GetObjectSet<TEntity>().DeleteObject(entity);
		}

		public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
		{
			return GetObjectSet<TEntity>().Where(filter);
		}

		public IEnumerable<TD> GetData<TD>(Expression<Func<TEntity, TD>> func)
		{
			return GetObjectSet<TEntity>().Select(func);
		}

		public IEnumerable<TD> GetData<TD>(Expression<Func<TEntity, TD>> func, Expression<Func<TEntity, bool>> filter)
		{
			return GetObjectSet<TEntity>().Where(filter).Select(func);
		}

		protected virtual IObjectSet<TEntity> GetObjectSet<TEntity>()
			where TEntity : class
		{
			if (ObjSet != null)
				return (IObjectSet<TEntity>)ObjSet;

			lock (_synchObj)
			{
				if (ObjSet != null)
					return (IObjectSet<TEntity>)ObjSet;

				ObjSet = _objectContext.CreateObjectSet<TEntity>();
			}

			return (IObjectSet<TEntity>)ObjSet;
		}
	}
}
