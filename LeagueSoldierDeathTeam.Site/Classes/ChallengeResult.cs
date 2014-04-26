using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public class ChallengeResult : HttpUnauthorizedResult
	{
		private string LoginProvider { get; set; }
		private string RedirectUri { get; set; }

		public ChallengeResult(string provider, string redirectUri)
		{
			LoginProvider = provider;
			RedirectUri = redirectUri;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
			context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
		}
	}
}