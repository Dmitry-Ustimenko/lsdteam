﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories
{
	public interface IRepository<TEntity>
		where TEntity : class
	{
		void Add(TEntity entity);

		void Delete(TEntity entity);

		IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter);

		IEnumerable<TD> GetData<TD>(Expression<Func<TEntity, TD>> func);

		IEnumerable<TD> GetData<TD>(Expression<Func<TEntity, TD>> func, Expression<Func<TEntity, bool>> filter);

		int GetDataCount(Expression<Func<TEntity, bool>> filter);
	}
}