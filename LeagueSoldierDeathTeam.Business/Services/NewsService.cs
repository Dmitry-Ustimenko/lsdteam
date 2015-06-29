using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSoldierDeathTeam.Business.Abstractions.Factories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.LoggedUser;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.Business.Classes.Enums;
using LeagueSoldierDeathTeam.Business.Classes.Helpers;
using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Business.Dto.DtoWrapper;
using LeagueSoldierDeathTeam.DataBase.Model;

namespace LeagueSoldierDeathTeam.Business.Services
{
	public class NewsService : ServiceBase, INewsService
	{
		#region Private Fields

		private readonly IRepository<User> _userRepository;
		private readonly IRepository<News> _newsRepository;
		private readonly IRepository<Platform> _platformRepository;
		private readonly IRepository<NewsCategory> _newsCategoryRepository;
		private readonly IRepository<NewsPlatform> _newsPlatformRepository;
		private readonly IRepository<Comment> _commentRepository;
		private readonly IRepository<NewsComment> _newsCommentRepository;
		private readonly IRepository<UserComment> _userCommentRepository;

		#endregion

		#region Constructors

		public NewsService(ILoggedUserProvider loggedUserProvider, IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
			: base(loggedUserProvider, unitOfWork)
		{
			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");

			_newsRepository = repositoryFactory.CreateRepository<News>();
			_userRepository = repositoryFactory.CreateRepository<User>();
			_newsCategoryRepository = repositoryFactory.CreateRepository<NewsCategory>();
			_platformRepository = repositoryFactory.CreateRepository<Platform>();
			_newsPlatformRepository = repositoryFactory.CreateRepository<NewsPlatform>();
			_commentRepository = repositoryFactory.CreateRepository<Comment>();
			_newsCommentRepository = repositoryFactory.CreateRepository<NewsComment>();
			_userCommentRepository = repositoryFactory.CreateRepository<UserComment>();
		}

		#endregion

		#region IAccountService Members

		PageData<NewsData> INewsService.GetNews(int? newsCategoryId, int? platformId, int newsSortId, int pageId, int pageSize)
		{
			var pageData = new PageData<NewsData> { PageId = pageId, PageSize = pageSize };

			var data = _newsRepository.GetQueryableData(o => (!newsCategoryId.HasValue || newsCategoryId.Value == o.NewsCategoryId)
				&& (!platformId.HasValue || o.NewsPlatforms.Select(p => p.PlatformId).Contains(platformId.Value)));

			if (data != null)
			{
				pageData.Count = data.Count();

				data = (NewsSort)newsSortId == NewsSort.Popularity ? data.OrderByDescending(o => o.CountViews) : data.OrderByDescending(o => o.CreateDate);

				pageData.Data = data.Page(pageData.PageId, pageData.PageSize).Select(o => new NewsData
				{
					Id = o.Id,
					Title = o.Title,
					Description = o.Description,
					CreateDate = o.CreateDate,
					CountViews = o.CountViews,
					WriterId = o.Writer != null ? o.Writer.Id : default(int),
					WriterName = o.Writer != null ? o.Writer.UserName : string.Empty,
					NewsCategory = new NewsCategoryData { Id = o.NewsCategory.Id, Name = o.NewsCategory.Name, ShortName = o.NewsCategory.ShortName },
					Platforms = o.NewsPlatforms.Select(p => new PlatformData { Id = p.Platform.Id, Name = p.Platform.Name, ShortName = p.Platform.ShortName }),
					ImagePath = o.ImagePath,
					Annotation = o.Annotation,
					CountComments = o.NewsComments.Count,
					BlockComments = o.BlockComments
				}).ToList();
			}

			return pageData;
		}

		public IEnumerable<NewsData> GetPreviousNews(int newsId, int newsCount)
		{
			var entity = _newsRepository.Query(o => o.Id == newsId).SingleOrDefault();
			if (entity == null)
				throw new ArgumentException("Данной новости не существует");

			var data = _newsRepository.GetQueryableData(o => o.CreateDate < entity.CreateDate).OrderByDescending(o => o.CreateDate).Take(newsCount);

			return data.Select(o => new NewsData
			{
				Id = o.Id,
				Title = o.Title,
				CreateDate = o.CreateDate,
				CountViews = o.CountViews,
				ImagePath = o.ImagePath,
				Annotation = o.Annotation,
				CountComments = o.NewsComments.Count,
				WriterName = o.Writer != null ? o.Writer.UserName : string.Empty,
				WriterId = o.WriterId
			}).ToList();
		}

		NewsData INewsService.GetNews(int id)
		{
			return _newsRepository.GetData(o => new NewsData
			{
				Id = o.Id,
				Title = o.Title,
				Description = o.Description,
				CreateDate = o.CreateDate,
				CountViews = o.CountViews,
				WriterId = o.Writer != null ? o.Writer.Id : default(int),
				WriterName = o.Writer != null ? o.Writer.UserName : string.Empty,
				NewsCategory = new NewsCategoryData { Id = o.NewsCategory.Id, Name = o.NewsCategory.Name, ShortName = o.NewsCategory.ShortName },
				PlatformIds = o.NewsPlatforms.Select(p => p.PlatformId),
				Platforms = o.NewsPlatforms.Select(p => new PlatformData { Id = p.Platform.Id, Name = p.Platform.Name, ShortName = p.Platform.ShortName }),
				ImagePath = o.ImagePath,
				Annotation = o.Annotation,
				CountComments = o.NewsComments.Count,
				BlockComments = o.BlockComments
			}, o => o.Id == id).SingleOrDefault();
		}

		void INewsService.SaveNews(NewsData data)
		{
			var entity = data.Id != default(int) ? _newsRepository.Query(o => o.Id == data.Id).SingleOrDefault() : new News();
			if (entity == null)
				throw new ArgumentException("Данной новости не существует.");

			var newsCategory = _newsCategoryRepository.Query(o => o.Id == data.NewsCategory.Id).SingleOrDefault();
			if (newsCategory == null)
				throw new ArgumentException("Данной категории не существует.");

			entity.Title = data.Title;
			entity.Description = data.Description;
			entity.NewsCategory = newsCategory;
			entity.ImagePath = data.ImagePath;
			entity.Annotation = data.Annotation;
			entity.BlockComments = data.BlockComments;

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
						_newsPlatformRepository.Add(new NewsPlatform { News = entity, Platform = platform });
					}
				}

				_newsRepository.Add(entity);
			}
			else
			{
				if (data.PlatformIds.Any())
				{
					var validePlatforms = _platformRepository.Query(o => data.PlatformIds.Contains(o.Id)).ToList();
					var validePlatformIds = validePlatforms.Select(o => o.Id).ToArray();

					var platforms = entity.NewsPlatforms.Select(o => o.Platform).ToList();
					var platformIds = platforms.Select(o => o.Id).ToArray();

					var newPlatforms = validePlatforms.Where(o => !platformIds.Contains(o.Id)).ToList();
					var deletePlatforms = entity.NewsPlatforms.Where(o => !validePlatformIds.Contains(o.Platform.Id)).ToList();

					foreach (var newPlatform in newPlatforms)
					{
						_newsPlatformRepository.Add(new NewsPlatform { News = entity, Platform = newPlatform });
					}

					foreach (var deletePlatform in deletePlatforms)
					{
						_newsPlatformRepository.Delete(deletePlatform);
					}
				}
			}

			UnitOfWork.Commit();
		}

		IEnumerable<NewsCategoryData> INewsService.GetNewsGategories()
		{
			return _newsCategoryRepository.GetData(o => new NewsCategoryData
			{
				Id = o.Id,
				Name = o.Name,
				ShortName = o.ShortName
			}).ToList();
		}

		void INewsService.ChangeCountViews(int id)
		{
			var entity = _newsRepository.Query(o => o.Id == id).SingleOrDefault();
			if (entity == null) return;

			var count = entity.CountViews + 1;
			if (count > Int32.MaxValue) return;

			entity.CountViews = count;
			UnitOfWork.Commit();
		}

		void INewsService.AddNewsComment(int newsId, CommentData data)
		{
			var news = _newsRepository.Query(o => o.Id == newsId).SingleOrDefault();
			if (news == null)
				throw new ArgumentException("Данной новости не существует");

			var writer = _userRepository.Query(o => o.Id == data.Writer.Id).SingleOrDefault();
			if (writer == null)
				throw new ArgumentException("Данного пользователя не существует");

			if (news.BlockComments)
				throw new ArgumentException("Добавление комментариев заблокировано");

			var comment = new Comment
			{
				CreateDate = data.CreateDate,
				Description = data.Description,
				Rate = data.Rate,
				Writer = writer
			};

			_commentRepository.Add(comment);
			_newsCommentRepository.Add(new NewsComment { News = news, Comment = comment });

			UnitOfWork.Commit();
		}

		public void DeleteNewsComment(int id, bool haveRights, int currentUserId)
		{
			var comment = _commentRepository.Query(o => o.Id == id).SingleOrDefault();
			if (comment == null)
				throw new ArgumentException("Данного комментария не существует");

			if (!haveRights && comment.WriterId != currentUserId)
				return;

			_commentRepository.Delete(comment);
			UnitOfWork.Commit();
		}

		CommentsWrapper INewsService.GetNewsComments(int newsId, CommentSortEnum sortType)
		{
			var news = _newsRepository.Query(o => o.Id == newsId).SingleOrDefault();
			if (news == null)
				throw new ArgumentException("Данной новости не существует");

			var commentsWrap = new CommentsWrapper { ContentId = news.Id, BlockComments = news.BlockComments };

			var comments = news.NewsComments.Select(o => new CommentData
			{
				Id = o.Comment.Id,
				Description = o.Comment.Description,
				CreateDate = o.Comment.CreateDate,
				ModifierDate = o.Comment.ModifierDate,
				Rate = o.Comment.Rate,
				Writer = o.Comment.Writer != null
					? new UserData { Id = o.Comment.Writer.Id, UserName = o.Comment.Writer.UserName, PhotoPath = o.Comment.Writer.PhotoPath, RoleId = o.Comment.Writer.RoleId }
					: new UserData(),
				ContentId = o.NewsId
			}).OrderByDescending(o => o.CreateDate).ToList();

			var commentIds = comments.Select(o => o.Id).ToArray();
			var userComments = _userCommentRepository.GetData(o => new UserCommentData
			{
				UserId = o.UserId,
				CommentId = o.CommentId,
				IsInc = o.IsIncrement
			}, o => o.UserId == CurrentUserId && commentIds.Contains(o.CommentId)).ToDictionary(o => o.CommentId, o => o.IsInc);

			foreach (var comment in comments.Where(comment => userComments.ContainsKey(comment.Id)))
			{
				comment.IsInc = userComments[comment.Id];
			}

			switch (sortType)
			{
				case CommentSortEnum.Old:
					commentsWrap.Data = comments.OrderBy(o => o.CreateDate).ToList();
					break;
				case CommentSortEnum.Popular:
					commentsWrap.Data = comments.OrderByDescending(o => o.Rate);
					break;
				default:
					commentsWrap.Data = comments;
					break;
			}

			return commentsWrap;
		}

		public void DeleteNews(int id)
		{
			var news = _newsRepository.Query(o => o.Id == id).SingleOrDefault();
			if (news == null)
				throw new ArgumentException("Данной новости не существует");

			_newsRepository.Delete(news);
			UnitOfWork.Commit();
		}

		#endregion

		#region Internal Implementation



		#endregion
	}
}
