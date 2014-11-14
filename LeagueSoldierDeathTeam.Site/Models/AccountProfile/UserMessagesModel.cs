using System.Collections.Generic;
using System.Web.UI;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Extensions;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class UserMessagesModel
	{
		public int MessageTypeId { get; set; }

		public MessageTypeEnum MessageType
		{
			get { return (MessageTypeEnum)MessageTypeId; }
		}

		public IDictionary<int, string> MessageTypes { get; set; }

		public IEnumerable<UserMessageData> Data { get; set; }

		public PagerModel Pager { get; set; }

		public UserMessagesModel()
		{
			MessageTypeId = (int)MessageTypeEnum.Inbox;
			Data = new List<UserMessageData>();
			MessageTypes = EnumEx.ToDictionary<MessageTypeEnum>();
			Pager = new PagerModel();
		}
	}
}