using System.Web.Mvc;
using System.Web.Routing;
using LeagueSoldierDeathTeam.App_Start;

namespace LeagueSoldierDeathTeam
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
	}
}
