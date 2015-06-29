using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.LoggedUser;

namespace LeagueSoldierDeathTeam.Business.LoggedUser
{
	public class LoggedUserProvider : ILoggedUserProvider
	{
		public ILoggedUser LoggedUser { get; set; }
	}
}
