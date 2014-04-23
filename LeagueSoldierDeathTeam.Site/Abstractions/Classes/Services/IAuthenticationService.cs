using Microsoft.Owin.Security;

namespace LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services
{
	public interface IAuthenticationService
	{
		IAuthenticationManager AuthenticationManager { get; set; }

		void SignOut();

		void SignIn(string userName, bool rememberMe);
	}
}
