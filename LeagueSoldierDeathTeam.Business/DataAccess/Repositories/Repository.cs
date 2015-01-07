using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess.Repositories;

namespace LeagueSoldierDeathTeam.Business.DataAccess.Repositories
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

		void IRepository<TEntity>.Add(TEntity entity)
		{
			GetObjectSet<TEntity>().AddObject(entity);
		}

		void IRepository<TEntity>.Delete(TEntity entity)
		{
			GetObjectSet<TEntity>().DeleteObject(entity);
		}

		IEnumerable<TEntity> IRepository<TEntity>.Query(Expression<Func<TEntity, bool>> filter)
		{
			return GetObjectSet<TEntity>().Where(filter);
		}

		IEnumerable<TD> IRepository<TEntity>.GetData<TD>(Expression<Func<TEntity, TD>> func)
		{
			return GetObjectSet<TEntity>().Select(func);
		}

		IEnumerable<TD> IRepository<TEntity>.GetData<TD>(Expression<Func<TEntity, TD>> func, Expression<Func<TEntity, bool>> filter)
		{
			return GetObjectSet<TEntity>().Where(filter).Select(func);
		}

		IQueryable<TEntity> IRepository<TEntity>.GetQueryableData()
		{
			return GetObjectSet<TEntity>().AsQueryable();
		}

		IQueryable<TEntity> IRepository<TEntity>.GetQueryableData(Expression<Func<TEntity, bool>> filter)
		{
			return GetObjectSet<TEntity>().Where(filter);
		}

		int IRepository<TEntity>.GetDataCount(Expression<Func<TEntity, bool>> filter)
		{
			return GetObjectSet<TEntity>().Where(filter).Count();
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
