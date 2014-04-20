using System.Web;
using LeagueSoldierDeathTeam.Site.Classes.Factories;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public static class SessionManager
	{
		public static HttpSessionStateBase Session
		{
			get { return ContextFactory.GetHttpContext().Session; }
		}

		public static T Get<T>(string key)
		{
			return Session[key] != null ? (T)Session[key] : default(T);
		}

		public static T GetAndClear<T>(string key)
		{
			var value = Get<T>(key);
			Clear(key);
			return value;
		}

		public static void Set(string key, object value)
		{
			Session[key] = value;
		}

		public static void Clear(string key)
		{
			Session[key] = null;
		}

		public static bool Contains(string key)
		{
			return Session[key] != null;
		}

	}
}