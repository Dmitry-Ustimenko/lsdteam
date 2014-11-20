using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Helpers;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

namespace LeagueSoldierDeathTeam.BusinessLogic.Services
{
	public class AccountProfileService : ServiceBase, IAccountProfileService
	{
		#region Private Fields

		private readonly IRepository<UserMessage> _userMessageRepository;
		private readonly IRepository<User> _userRepository;

		#endregion

		#region Constructors

		public AccountProfileService(IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
			: base(unitOfWork)
		{
			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");

			_userMessageRepository = repositoryFactory.CreateRepository<UserMessage>();
			_userRepository = repositoryFactory.CreateRepository<User>();
		}

		#endregion

		#region IAccountService Members

		void IAccountProfileService.SaveAsRead(int userId, IEnumerable<int> messagesIds)
		{
			var messages = _userMessageRepository.Query(o => messagesIds.Contains(o.Id) && !o.IsRead && o.RecipientId == userId).ToList();
			if (!messages.Any()) return;

			foreach (var message in messages)
				message.IsRead = true;

			UnitOfWork.Commit();
		}

		void IAccountProfileService.SaveAsDraft(int userId, IEnumerable<int> messagesIds)
		{
			var messages = _userMessageRepository.Query(o => messagesIds.Contains(o.Id)).ToList();
			if (!messages.Any()) return;

			foreach (var message in messages)
			{
				if (message.SenderId == userId && !message.IsSenderDeleted)
				{
					message.IsSenderSaved = true;
				}
				else if (message.RecipientId == userId && !message.IsRecipientDeleted)
				{
					message.IsRecipientSaved = true;
					message.IsRead = true;
				}
			}

			UnitOfWork.Commit();
		}

		void IAccountProfileService.DeleteMessages(int userId, IEnumerable<int> messagesIds)
		{
			var messages = _userMessageRepository.Query(o => messagesIds.Contains(o.Id)
				&& (o.SenderId == userId || o.RecipientId == userId)).ToList();

			if (!messages.Any()) return;

			foreach (var message in messages)
			{
				if (message.SenderId == userId)
				{
					message.IsSenderDeleted = true;
				}
				else if (message.RecipientId == userId)
				{
					message.IsRecipientDeleted = true;
				}
			}

			UnitOfWork.Commit();
		}

		public UserMessageData GetUserMessage(int userId, int id)
		{
			var message = _userMessageRepository.GetData(o => new UserMessageData
			{
				Id = o.Id,
				CreateDate = o.CreateDate,
				Description = o.Description,
				Title = o.Title,
				IsRead = o.IsRead,
				SenderId = o.Sender != null ? o.Sender.Id : default(int?),
				SenderName = o.Sender != null ? o.Sender.UserName : string.Empty,
				SenderPhoto = o.Sender != null ? o.Sender.PhotoPath : string.Empty,
				RecipientId = o.Recipient != null ? o.Recipient.Id : default(int?),
				RecipientName = o.Recipient != null ? o.Recipient.UserName : string.Empty,
				IsSenderSaved = o.IsSenderSaved,
				IsRecipientSaved = o.IsRecipientSaved
			}, o => o.Id == id && (o.RecipientId == userId && !o.IsRecipientDeleted || o.SenderId == userId && !o.IsSenderDeleted)).SingleOrDefault();

			if (message != null)
			{
				if (message.RecipientId.GetValueOrDefault() == userId)
				{
					message.TypeId = message.IsRecipientSaved ? (int)MessageTypeEnum.Draft : (int)MessageTypeEnum.Inbox;
				}
				else if (message.SenderId.GetValueOrDefault() == userId)
				{
					message.TypeId = message.IsSenderSaved ? (int)MessageTypeEnum.Draft : (int)MessageTypeEnum.Sent;
				}
			}

			return message;
		}

		public void SaveMessage(UserMessageData data)
		{
			var entity = data.Id == default(int) ? new UserMessage() : _userMessageRepository.Query(o => o.Id == data.Id).SingleOrDefault();
			if (entity == null)
				throw new ArgumentException("Сообщение не существует", string.Format("Message"));

			var recipient = _userRepository.Query(o => o.UserName == data.RecipientName).SingleOrDefault();
			if (recipient == null)
				throw new ArgumentException("Пользователь с таким именем не найден", string.Format("RecipientName"));

			if (data.Id == default(int))
			{
				entity.Title = data.Title;
				entity.Description = data.Description;
				entity.CreateDate = DateTime.UtcNow;
				entity.RecipientId = recipient.Id;
				entity.SenderId = data.SenderId;
				_userMessageRepository.Add(entity);
			}
			else if (!entity.IsRead && entity.SenderId.GetValueOrDefault() == data.SenderId && !entity.IsSenderSaved)
			{
				entity.Title = data.Title;
				entity.Description = data.Description;
			}

			UnitOfWork.Commit();
		}

		PageData<UserMessageData> IAccountProfileService.GetUserMessages(int userId, int typeId, int pageId, int pageSize)
		{
			var pageData = new PageData<UserMessageData> { PageId = pageId, PageSize = pageSize };

			Expression<Func<UserMessage, bool>> filter = null;
			switch ((MessageTypeEnum)typeId)
			{
				case MessageTypeEnum.Inbox:
					filter = o => o.RecipientId == userId && !o.IsRecipientDeleted && !o.IsRecipientSaved;
					break;
				case MessageTypeEnum.Sent:
					filter = o => o.SenderId == userId && !o.IsSenderDeleted && !o.IsSenderSaved;
					break;
				case MessageTypeEnum.Draft:
					filter = o => o.SenderId == userId && o.IsSenderSaved && !o.IsSenderDeleted
						|| o.RecipientId == userId && o.IsRecipientSaved && !o.IsRecipientDeleted;
					break;
			}

			var data = filter != null ? _userMessageRepository.GetQueryableData(filter) : null;
			if (data != null)
			{
				pageData.Count = data.Count();

				pageData.Data = data.OrderByDescending(o => o.CreateDate).Page(pageData.PageId, pageData.PageSize).Select(o => new UserMessageData
				{
					Id = o.Id,
					Title = o.Title,
					Description = o.Description,
					IsRead = o.IsRead,
					CreateDate = o.CreateDate,
					RecipientId = o.Recipient != null ? o.Recipient.Id : default(int?),
					RecipientName = o.Recipient != null ? o.Recipient.UserName : string.Empty,
					SenderId = o.Sender != null ? o.Sender.Id : default(int?),
					SenderName = o.Sender != null ? o.Sender.UserName : string.Empty
				}).ToList();
			}

			return pageData;
		}

		int IAccountProfileService.GetUserMessageCount(int userId)
		{
			return _userMessageRepository.GetDataCount(o => o.RecipientId == userId && !o.IsRecipientDeleted && !o.IsRecipientSaved && !o.IsRead);
		}

		#endregion

		#region Internal Implementation


		#endregion
	}
}
