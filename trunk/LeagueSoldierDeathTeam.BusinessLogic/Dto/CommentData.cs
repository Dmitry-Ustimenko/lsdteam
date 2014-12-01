using System;

namespace LeagueSoldierDeathTeam.BusinessLogic.Dto
{
	public class CommentData
	{
		public int Id { get; set; }

		public string Description { get; set; }

		public int WriterId { get; set; }

		public DateTime CreateDate { get; set; }

		public DateTime ModifierDate { get; set; }

		public int Rate { get; set; }
	}
}
