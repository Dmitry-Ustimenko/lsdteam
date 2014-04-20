using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Factories;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public class AppContext : IAppContext
	{
		public static IAppContext Current
		{
			get { return (AppContext)ContextFactory.GetHttpContext().Items["AppContextKey"]; }
			set { ContextFactory.GetHttpContext().Items["AppContextKey"] = value; }
		}

		public UserData CurrentUser
		{
			get { return SessionManager.Contains(SessionKeys.User) ? SessionManager.Get<UserData>(SessionKeys.User) : null; }
			set { SessionManager.Set(SessionKeys.User, value); }
		}
	}
}