using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services
{
	public interface IResourceService
	{
		IEnumerable<PlatformData> GetPlatforms();
	}
}
