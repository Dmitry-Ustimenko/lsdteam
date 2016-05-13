using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Factories;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public class AppContextCustom : IAppContextCustom
	{
		public static IAppContextCustom Current
		{
			get { return (AppContextCustom)ContextFactory.GetHttpContext().Items["AppContextKey"]; }
			set { ContextFactory.GetHttpContext().Items["AppContextKey"] = value; }
		}

		public UserData CurrentUser
		{
			get { return SessionManager.Contains(SessionKeys.User) ? SessionManager.Get<UserData>(SessionKeys.User) : null; }
			set { SessionManager.Set(SessionKeys.User, value); }
		}
	}
}