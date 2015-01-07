using System.Collections.Generic;
using LeagueSoldierDeathTeam.Business.Dto;

namespace LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services
{
	public interface IAccountProfileService
	{
		PageData<UserMessageData> GetUserMessages(int userId, int typeId, int pageId, int pageSize);

		int GetUserMessageCount(int userId);

		void SaveAsRead(int userId, IEnumerable<int> messagesIds);

		void SaveAsDraft(int userId, IEnumerable<int> messagesIds);

		void DeleteMessages(int userId, IEnumerable<int> messagesIds);

		UserMessageData GetUserMessage(int userId, int id);

		void SaveMessage(UserMessageData data);
	}
}
