using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.Site.Models
{
	public class BaseCommentModel
	{
		public IEnumerable<CommentData> CommentData { get; set; }

		public BaseCommentModel()
		{
			CommentData = new List<CommentData>();
		}
	}
}