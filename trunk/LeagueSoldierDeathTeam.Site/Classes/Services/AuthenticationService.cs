using System.Security.Claims;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace LeagueSoldierDeathTeam.Site.Classes.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		public IAuthenticationManager AuthenticationManager { get; set; }

		void IAuthenticationService.SignOut()
		{
			AuthenticationManager.SignOut();
		}

		void IAuthenticationService.SignIn(string userName, bool rememberMe)
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
			var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) }, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);
			AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
		}
	}
}
