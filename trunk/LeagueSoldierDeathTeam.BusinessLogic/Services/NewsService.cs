using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

namespace LeagueSoldierDeathTeam.BusinessLogic.Services
{
	public class NewsService : ServiceBase, INewsService
	{
		#region Private Fields

		private readonly IRepository<User> _userRepository;

		private readonly IRepository<News> _newsRepository;

		private readonly IRepository<Platform> _platformRepository;

		private readonly IRepository<NewsCategory> _newsCategoryRepository;

		#endregion

		#region Constructors

		public NewsService(IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
			: base(unitOfWork)
		{
			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");

			_newsRepository = repositoryFactory.CreateRepository<News>();
			_userRepository = repositoryFactory.CreateRepository<User>();
			_newsCategoryRepository = repositoryFactory.CreateRepository<NewsCategory>();
			_platformRepository = repositoryFactory.CreateRepository<Platform>();
		}

		#endregion

		#region IAccountService Members

		void INewsService.SaveNews(NewsData data)
		{
			var entity = data.Id != default(int) ? _newsRepository.Query(o => o.Id == data.Id).SingleOrDefault() : new News();
			if (entity == null)
				throw new ArgumentException("Данной новости не существует.");

			var newsCategory = _newsCategoryRepository.Query(o => o.Id == data.NewsCategoryId).SingleOrDefault();
			if (newsCategory == null)
				throw new ArgumentException("Данной категории не существует.");

			entity.Title = data.Title;
			entity.Description = data.Description;
			entity.NewsCategory = newsCategory;

			if (data.Id == default(int))
			{
				var writer = _userRepository.Query(o => o.Id == data.WriterId).SingleOrDefault();
				if (writer == null)
					throw new ArgumentException("Данного пользователя не существует.");

				entity.CreateDate = DateTime.UtcNow;
				entity.Writer = writer;

				if (data.PlatformIds.Any())
				{
					var validePlatforms = _platformRepository.Query(o => data.PlatformIds.Contains(o.Id)).ToList();
					foreach (var platform in validePlatforms)
					{
						entity.NewsPlatforms.Add(new NewsPlatform { News = entity, Platform = platform });
					}
				}

				_newsRepository.Add(entity);
			}

			UnitOfWork.Commit();
		}

		IEnumerable<NewsCategoryData> INewsService.GetNewsGategories()
		{
			return _newsCategoryRepository.GetData(o => new NewsCategoryData
			{
				Id = o.Id,
				Name = o.Name
			}).ToList();
		}

		#endregion

		#region Internal Implementation



		#endregion
	}
}
