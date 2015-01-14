using System;

namespace LeagueSoldierDeathTeam.Business.Dto
{
	public class CommentData
	{
		public int Id { get; set; }

		public string Description { get; set; }

		public UserData Writer { get; set; }

		public int WriterId { get; set; }

		public DateTime CreateDate { get; set; }

		public DateTime? ModifierDate { get; set; }

		public int Rate { get; set; }

		public string RateString
		{
			get
			{
				return string.Format("{0}{1}", Rate > 0 ? "+" : Rate < 0 ? "-" : string.Empty, Math.Abs(Rate));
			}
		}

		public int ContentId { get; set; }

		public CommentData()
		{
			Writer = new UserData();
		}
	}
}
