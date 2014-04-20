using System.Web;

namespace LeagueSoldierDeathTeam.Site.Classes.Factories
{
	public class ContextFactory
	{
		public static HttpContextBase GetHttpContext()
		{
			return (HttpContext.Current != null ? new HttpContextWrapper(HttpContext.Current) : null);
		}
	}
}
