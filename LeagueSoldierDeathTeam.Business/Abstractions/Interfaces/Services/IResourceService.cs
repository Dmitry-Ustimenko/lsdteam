using System.Collections.Generic;
using LeagueSoldierDeathTeam.Business.Dto;

namespace LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services
{
	public interface IResourceService
	{
		IEnumerable<PlatformData> GetPlatforms();

		CommentData GetComment(int id);

		CommentData SaveComment(CommentData data);

		int IncCommentRate(int commentId);

		int DecCommentRate(int commentId);
	}
}
