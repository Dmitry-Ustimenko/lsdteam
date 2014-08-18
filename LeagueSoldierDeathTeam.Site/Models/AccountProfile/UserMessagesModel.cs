using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Extensions;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class UserMessagesModel
	{
		public MessageTypeEnum MessageType { get; set; }

		public int MessageTypeId { get; set; }

		public IDictionary<int, string> MessageTypes { get; set; }

		public IEnumerable<UserMessageData> Data { get; set; }

		public UserMessagesModel()
		{
			MessageType = MessageTypeEnum.Inbox;
			Data = new List<UserMessageData>();
			MessageTypes = EnumEx.ToDictionary<MessageTypeEnum>();
		}
	}
}