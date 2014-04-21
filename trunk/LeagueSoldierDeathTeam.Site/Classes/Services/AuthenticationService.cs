using System.Web.Security;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services;
using LeagueSoldierDeathTeam.Site.Models.Account;

namespace LeagueSoldierDeathTeam.Site.Classes.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		void IAuthenticationService.SignOut()
		{
			FormsAuthentication.SignOut();
		}

		void IAuthenticationService.SignIn(string userName, bool rememberMe)
		{
			FormsAuthentication.SetAuthCookie(userName, rememberMe);
		}

		LoginModel IAuthenticationService.GetSignInModel(string decryptValue)
		{
			var model = new LoginModel();

			if (!string.IsNullOrWhiteSpace(decryptValue))
			{
				var ticket = FormsAuthentication.Decrypt(decryptValue);
				if (ticket != null)
				{
					model.RememberMe = ticket.IsPersistent;
					model.Email = ticket.Name;
				}
			}

			return model;
		}
	}
}
