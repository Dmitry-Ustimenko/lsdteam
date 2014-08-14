using System;
using System.Collections.Generic;
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

		IEnumerable<UserMessageData> IAccountProfileService.GetUserMessages(int userId, MessageTypeEnum type)
		{
			return _userMessageRepository.GetData(o => new UserMessageData
			{
				Id = o.Id,
				Title = o.Title,
				Description = o.Description,
				IsRead = o.IsRead,
				RecipientId = o.RecipientId,
				RecipientName = o.Recipient != null ? o.Recipient.UserName : string.Empty,
				SenderId = o.SenderId,
				SenderName = o.Sender != null ? o.Sender.UserName : string.Empty,
				TypeId = o.TypeId
			});
		}

		#endregion

		#region Internal Implementation



		#endregion
	}
}
