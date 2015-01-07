using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.Business.Classes.Enums;
using LeagueSoldierDeathTeam.Site.Controllers;

namespace LeagueSoldierDeathTeam.Site.Classes.Attributes
{
	public class UserAuthorize : AuthorizeAttribute
	{
		public Role UserRoles { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			var currentUser = AppContext.Current.CurrentUser;
			if (!httpContext.Request.IsAuthenticated || currentUser == null)
				return false;

			if (currentUser.IsMainAdmin)
				return true;

			return UserRoles == 0 || UserRoles.HasFlag((Role)currentUser.RoleId);
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			if (filterContext == null)
				return;

			if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
				filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.NoContent);
			else
				filterContext.Result = BaseController.RedirectToAction<HomeController>(c => c.Index());
		}
	}
}