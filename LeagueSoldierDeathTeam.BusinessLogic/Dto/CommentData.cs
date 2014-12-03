using System;

namespace LeagueSoldierDeathTeam.BusinessLogic.Dto
{
	public class CommentData
	{
		public int Id { get; set; }

		public string Description { get; set; }

		public UserData Writer { get; set; }

		public DateTime CreateDate { get; set; }

		public DateTime? ModifierDate { get; set; }

		public int Rate { get; set; }

		public int ContentId { get; set; }

		public CommentData()
		{
			Writer = new UserData();
		}
	}
}
