using System;
using LeagueSoldierDeathTeam.Site.App_Start;
using LeagueSoldierDeathTeam.Site.Classes;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace LeagueSoldierDeathTeam.Site.App_Start
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				CookieName = AppConfig.CookieName,
				ExpireTimeSpan = new TimeSpan(3, 0, 0, 0)
			});

			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			app.UseVkontakteAuthentication("4323709", "AIwB4s6cpn4sYNgOzprk", "");
		}
	}
}