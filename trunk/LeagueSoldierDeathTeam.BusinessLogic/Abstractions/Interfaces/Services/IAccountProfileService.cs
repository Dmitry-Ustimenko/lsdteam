using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services
{
	public interface IAccountProfileService
	{
		IEnumerable<UserMessageData> GetUserMessages(int userId, int typeId);

		int GetUserMessageCount(int userId);

		void SaveAsRead(int userId, IEnumerable<int> messagesIds);

		void SaveAsDraft(int userId, IEnumerable<int> messagesIds);

		void DeleteMessages(int userId, IEnumerable<int> messagesIds);
	}
}
