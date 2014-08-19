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
			var messages = _userMessageRepository.Query(o => messagesIds.Contains(o.Id)
				&& !o.IsRead
				&& o.TypeId == (int)MessageTypeEnum.Inbox
				&& o.Recipient != null && o.Recipient.Id == userId).ToList();

			if (!messages.Any()) return;

			foreach (var message in messages)
				message.IsRead = true;

			UnitOfWork.Commit();
		}

		void IAccountProfileService.SaveAsDraft(int userId, IEnumerable<int> messagesIds)
		{
			var messages = _userMessageRepository.Query(o => messagesIds.Contains(o.Id)
				&& o.TypeId != (int)MessageTypeEnum.Draft
				&& (o.SenderId == userId || o.Recipient != null && o.Recipient.Id == userId)).ToList();

			if (!messages.Any()) return;

			foreach (var message in messages)
			{
				message.TypeId = (int)MessageTypeEnum.Draft;
				message.IsRead = true;
			}

			UnitOfWork.Commit();
		}

		void IAccountProfileService.DeleteMessages(int userId, IEnumerable<int> messagesIds)
		{
			var messages = _userMessageRepository.Query(o => messagesIds.Contains(o.Id)
				&& (o.SenderId == userId || o.Recipient != null && o.Recipient.Id == userId)).ToList();

			if (!messages.Any()) return;

			foreach (var message in messages)
				_userMessageRepository.Delete(message);

			UnitOfWork.Commit();
		}

		IEnumerable<UserMessageData> IAccountProfileService.GetUserMessages(int userId, int typeId)
		{
			Expression<Func<UserMessage, bool>> filter = null;
			switch ((MessageTypeEnum)typeId)
			{
				case MessageTypeEnum.Inbox:
					filter = o => o.TypeId == typeId && o.Recipient != null && o.Recipient.Id == userId;
					break;
				case MessageTypeEnum.Sent:
					filter = o => o.TypeId == typeId && o.SenderId == userId;
					break;
				case MessageTypeEnum.Draft:
					filter = o => o.TypeId == typeId && (o.SenderId == userId || o.Recipient != null && o.Recipient.Id == userId);
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
				SenderId = o.SenderId,
				SenderName = o.Sender != null ? o.Sender.UserName : string.Empty,
				TypeId = o.TypeId
			}, filter).OrderByDescending(o => o.CreateDate).ToList() : new List<UserMessageData>();
		}

		int IAccountProfileService.GetUserMessageCount(int userId)
		{
			return _userMessageRepository.GetDataCount(o => o.TypeId == (int)MessageTypeEnum.Inbox && o.Recipient != null && o.Recipient.Id == userId);
		}

		#endregion

		#region Internal Implementation


		#endregion
	}
}
