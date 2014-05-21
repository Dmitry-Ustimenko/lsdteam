﻿using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
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

			if (UserRoles == 0)
				return true;

			var roles = currentUser.Roles.Select(o => (Role)o.Id);
			return roles.Any(role => UserRoles.HasFlag(role));
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