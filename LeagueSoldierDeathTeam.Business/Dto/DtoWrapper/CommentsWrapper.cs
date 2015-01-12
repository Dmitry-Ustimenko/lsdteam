using System.Collections.Generic;

namespace LeagueSoldierDeathTeam.Business.Dto.DtoWrapper
{
	public class CommentsWrapper
	{
		public int ContentId { get; set; }

		public bool BlockComments { get; set; }

		public IEnumerable<CommentData> Data { get; set; }

		public CommentsWrapper()
		{
			Data = new List<CommentData>();
		}
	}
}
