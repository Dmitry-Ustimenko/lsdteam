using System;
using LeagueSoldierDeathTeam.Business.Classes.Enums;

namespace LeagueSoldierDeathTeam.Business.Dto
{
	public class UserMessageData
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsRead { get; set; }
		public int? SenderId { get; set; }
		public string SenderName { get; set; }
		public string SenderPhoto { get; set; }
		public int? RecipientId { get; set; }
		public string RecipientName { get; set; }
		public DateTime CreateDate { get; set; }
		public bool IsRecipientSaved { get; set; }
		public bool IsSenderSaved { get; set; }

		public int TypeId { get; set; }
		public MessageTypeEnum Type
		{
			get { return (MessageTypeEnum)TypeId; }
		}
	}
}
