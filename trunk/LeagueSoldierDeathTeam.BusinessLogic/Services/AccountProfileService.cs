using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

namespace LeagueSoldierDeathTeam.BusinessLogic.Services
{
	public class AccountProfileService : ServiceBase, IAccountProfileService
	{
		#region Private Fields

		private readonly IRepository<UserMessage> _userMessageRepository;

		#endregion

		#region Constructors

		public AccountProfileService(IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
			: base(unitOfWork)
		{
			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");

			_userMessageRepository = repositoryFactory.CreateRepository<UserMessage>();
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
				&& (o.SenderId == userId || o.Recipient != null && o.Recipient.Id == userId)).ToList();

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

		IEnumerable<UserMessageData> IAccountProfileService.GetUserMessages(int userId, int typeId)
		{
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

			return filter != null ? _userMessageRepository.GetData(o => new UserMessageData
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
			}, filter).OrderByDescending(o => o.CreateDate).ToList() : new List<UserMessageData>();
		}

		int IAccountProfileService.GetUserMessageCount(int userId)
		{
			return _userMessageRepository.GetDataCount(o => o.RecipientId == userId && !o.IsRecipientDeleted && !o.IsRecipientSaved);
		}

		#endregion

		#region Internal Implementation


		#endregion
	}
}
