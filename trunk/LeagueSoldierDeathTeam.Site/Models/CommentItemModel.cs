using LeagueSoldierDeathTeam.Business.Dto;

namespace LeagueSoldierDeathTeam.Site.Models
{
	public class CommentItemModel
	{
		public bool BlockComments { get; set; }

		public CommentData CommentItem { get; set; }

		public CommentItemModel()
		{
			CommentItem = new CommentData();
		}
	}
}