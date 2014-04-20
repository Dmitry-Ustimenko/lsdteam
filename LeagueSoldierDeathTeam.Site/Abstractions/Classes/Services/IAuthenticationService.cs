using LeagueSoldierDeathTeam.Site.Models.Account;

namespace LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services
{
	public interface IAuthenticationService
	{
		void SignOut();

		void SignIn(string userName, bool rememberMe);

		LoginModel GetSignInModel(string decryptValue);
	}
}
