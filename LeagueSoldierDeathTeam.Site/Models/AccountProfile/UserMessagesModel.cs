using System.Collections.Generic;
using LeagueSoldierDeathTeam.Business.Classes.Enums;
using LeagueSoldierDeathTeam.Business.Classes.Extensions;
using LeagueSoldierDeathTeam.Business.Dto;

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