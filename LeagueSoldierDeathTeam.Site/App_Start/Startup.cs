using System;
using System.Security.Claims;
using Duke.Owin.VkontakteMiddleware;
using Duke.Owin.VkontakteMiddleware.Provider;
using LeagueSoldierDeathTeam.Site.App_Start;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
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
				ExpireTimeSpan = new TimeSpan(3, 0, 0, 0),
				LoginPath = new PathString(WebBuilder.BuildActionUrl<HomeController>(o => o.Index()))
			});

			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			app.UseFacebookAuthentication(GetFacebookOptions());
			app.UseVkontakteAuthentication(GetVKontakteOptions());
		}

		private static FacebookAuthenticationOptions GetFacebookOptions()
		{
			var facebookOptions = new FacebookAuthenticationOptions
			{
				AppId = "716319181763981",
				AppSecret = "48e5e982f464d6a9ce610fdca6d2eaa2"
			};
			facebookOptions.Scope.Add("email");
			facebookOptions.Provider = new FacebookAuthenticationProvider
			{
				OnAuthenticated = async context =>
				{
					context.Identity.AddClaim(new Claim("AccessToken", context.AccessToken));
					context.Identity.AddClaim(new Claim("Id", context.Id));
					context.Identity.AddClaim(new Claim("FullName", context.User["name"].ToString()));
					context.Identity.AddClaim(new Claim("FirstName", context.User["first_name"].ToString()));
					context.Identity.AddClaim(new Claim("LastName", context.User["last_name"].ToString()));
					context.Identity.AddClaim(new Claim("Nickname", string.Empty));
					context.Identity.AddClaim(new Claim("PhotoLink", string.Format("https://graph.facebook.com/{0}/picture", context.Id)));
					context.Identity.AddClaim(new Claim("Email", context.Email));
				}
			};

			return facebookOptions;
		}

		private static VkAuthenticationOptions GetVKontakteOptions()
		{
			return new VkAuthenticationOptions
			{
				AppId = "4325286",
				AppSecret = "nplOLs4MEAQ8ElhCyHAO",
				Provider = new VkAuthenticationProvider
				{
					OnAuthenticated = async context =>
					{
						context.Identity.AddClaim(new Claim("AccessToken", context.AccessToken));
						context.Identity.AddClaim(new Claim("Id", context.Id));
						context.Identity.AddClaim(new Claim("FullName", context.FullName));
						context.Identity.AddClaim(new Claim("FirstName", context.Name));
						context.Identity.AddClaim(new Claim("LastName", context.LastName));
						context.Identity.AddClaim(new Claim("Nickname", context.Nickname));
						context.Identity.AddClaim(new Claim("PhotoLink", context.Link));
						context.Identity.AddClaim(new Claim("Email", string.Empty));
					}
				},
			};
		}
	}
}