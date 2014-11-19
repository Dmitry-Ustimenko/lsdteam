using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Extensions;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class UserMessagesModel : BasePagerModel<UserMessageData>
	{
		public int MessageTypeId { get; set; }

		public MessageTypeEnum MessageType { get { return (MessageTypeEnum)MessageTypeId; } }

		public IDictionary<int, string> MessageTypes { get; set; }

		public UserMessagesModel()
		{
			MessageTypeId = (int)MessageTypeEnum.Inbox;
			MessageTypes = EnumEx.ToDictionary<MessageTypeEnum>();
		}
	}
}