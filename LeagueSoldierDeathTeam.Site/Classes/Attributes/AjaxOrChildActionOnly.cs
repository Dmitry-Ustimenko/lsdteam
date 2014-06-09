using System;
using System.Net;
using System.Web.Mvc;

namespace LeagueSoldierDeathTeam.Site.Classes.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class AjaxOrChildActionOnly : FilterAttribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext == null)
				throw new ArgumentNullException("filterContext");

			if (!filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() && !filterContext.IsChildAction)
				filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.NotFound);
		}
	}
}