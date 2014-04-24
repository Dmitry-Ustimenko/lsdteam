using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public class ChallengeResult : HttpUnauthorizedResult
	{
		private const string XsrfKey = "XsrfId";
		private string LoginProvider { get; set; }
		private string RedirectUri { get; set; }
		private string UserId { get; set; }

		public ChallengeResult(string provider, string redirectUri)
			: this(provider, redirectUri, null)
		{ }

		public ChallengeResult(string provider, string redirectUri, string userId)
		{
			LoginProvider = provider;
			RedirectUri = redirectUri;
			UserId = userId;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
			if (UserId != null)
				properties.Dictionary[XsrfKey] = UserId;

			context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
		}
	}
}