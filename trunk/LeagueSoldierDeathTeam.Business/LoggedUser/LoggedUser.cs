using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.LoggedUser;

namespace LeagueSoldierDeathTeam.Business.LoggedUser
{
	public class LoggedUser : ILoggedUser
	{
		public int CurrentUserId { get; set; }
	}
}
