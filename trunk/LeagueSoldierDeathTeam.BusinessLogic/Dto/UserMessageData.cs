using System;

namespace LeagueSoldierDeathTeam.BusinessLogic.Dto
{
	public class UserMessageData
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsRead { get; set; }
		public int TypeId { get; set; }
		public int? SenderId { get; set; }
		public string SenderName { get; set; }
		public int? RecipientId { get; set; }
		public string RecipientName { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
