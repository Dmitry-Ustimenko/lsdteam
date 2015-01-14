using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSoldierDeathTeam.Business.Abstractions.Factories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.DataBase.Model;

namespace LeagueSoldierDeathTeam.Business.Services
{
	public class ResourceService : ServiceBase, IResourceService
	{
		#region Private Fields

		private readonly IRepository<Platform> _platformRepository;
		private readonly IRepository<Comment> _commentRepository;
		private readonly IRepository<User> _userRepository;

		#endregion

		#region Constructors

		public ResourceService(IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
			: base(unitOfWork)
		{
			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");

			_platformRepository = repositoryFactory.CreateRepository<Platform>();
			_commentRepository = repositoryFactory.CreateRepository<Comment>();
			_userRepository = repositoryFactory.CreateRepository<User>();
		}

		#endregion

		#region IAccountService Members

		IEnumerable<PlatformData> IResourceService.GetPlatforms()
		{
			return _platformRepository.GetData(o => new PlatformData
			{
				Id = o.Id,
				Name = o.Name,
				ShortName = o.ShortName
			}).ToList();
		}

		CommentData IResourceService.GetComment(int id)
		{
			var comment = _commentRepository.GetData(o => new CommentData
			{
				Id = o.Id,
				CreateDate = o.CreateDate,
				Description = o.Description,
				ModifierDate = o.ModifierDate,
				Rate = o.Rate,
				WriterId = o.WriterId
			}, o => o.Id == id).SingleOrDefault();

			if (comment == null)
				throw new ArgumentException("Данного комментария не существует.");

			return comment;
		}

		public CommentData SaveComment(CommentData data)
		{
			var entity = _commentRepository.Query(o => o.Id == data.Id).SingleOrDefault();
			if (entity == null)
				throw new ArgumentException("Данного комментария не существует.");

			entity.Description = data.Description;
			entity.ModifierDate = DateTime.UtcNow;

			UnitOfWork.Commit();

			var comment = ((IResourceService)this).GetComment(entity.Id);
			var writer = _userRepository.GetData(o => new UserData
			{
				Id = o.Id,
				UserName = o.UserName,
				PhotoPath = o.PhotoPath,
				RoleId = o.RoleId
			}, o => o.Id == comment.WriterId).SingleOrDefault();

			if (writer == null)
				throw new ArgumentException("Данного пользователя не существует.");

			comment.Writer = writer;

			return comment;
		}

		#endregion

		#region Internal Implementation



		#endregion
	}
}
