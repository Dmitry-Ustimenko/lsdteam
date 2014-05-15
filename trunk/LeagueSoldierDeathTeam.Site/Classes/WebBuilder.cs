using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LeagueSoldierDeathTeam.Site.Classes.Factories;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public static class WebBuilder
	{
		private static readonly ConcurrentDictionary<string, string> UrlCache = new ConcurrentDictionary<string, string>();

		public static string GetFullUrl<TController>(Expression<Action<TController>> action) where TController : Controller
		{
			var actionInfo = ControllerActionInfo.Create(action);
			var data = BuildUrl(actionInfo.ActionName, actionInfo.ControllerName, actionInfo.RouteValues);
			return string.Concat(AppConfig.HostName, data);
		}
		public static string BuildActionUrl<TController>(Expression<Action<TController>> action, bool? secure = null) where TController : Controller
		{
			var actionInfo = ControllerActionInfo.Create(action);
			return BuildUrl(actionInfo.ActionName, actionInfo.ControllerName, actionInfo.RouteValues, secure);
		}

		public static string BuildUrl(string actionName, string controllerName, IDictionary<string, object> routeValues, bool? secure = false, IDictionary<string, string> queryParameters = null)
		{
			return BuildUrl(null, secure, actionName, controllerName, new RouteValueDictionary(routeValues ?? new Dictionary<string, object>()), queryParameters);
		}

		public static string BuildUrl(string routeName, bool? secure, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, string> queryParameters)
		{
			string cacheKey = null;
			string result = null;
			if (routeValues == null || routeValues.Count == 0)
			{
				cacheKey = GetUrlCacheKey(routeName, actionName, controllerName);
				UrlCache.TryGetValue(cacheKey, out result);
			}

			if (result == null)
			{
				result = UrlHelper.GenerateUrl(routeName, actionName, controllerName, routeValues, RouteTable.Routes, ContextFactory.GetHttpContext().Request.RequestContext, false);
				if (cacheKey != null)
					UrlCache.TryAdd(cacheKey, result);
			}

			if (result == null)
				return string.Empty;


			if (queryParameters == null || queryParameters.Count == 0)
				return result;

			var url = new StringBuilder(result);
			url.Append(result.Contains("?") ? '&' : '?');
			foreach (var pair in queryParameters)
				url.Append(pair.Key).Append('=').Append(HttpUtility.UrlEncode(pair.Value)).Append('&');
			url.Remove(url.Length - 1, 1);
			return url.ToString().ToLowerInvariant();
		}

		private static string GetUrlCacheKey(string routeName, string actionName, string controllerName)
		{
			if (actionName == null && controllerName == null)
				return routeName;
			var sb = new StringBuilder();
			return sb.Append(routeName).Append('.').Append(controllerName).Append('.').Append(actionName).ToString();
		}

		public static HttpContext GetHttpContext()
		{
			var uriBuilder = new UriBuilder(AppConfig.HostName);
			return new HttpContext(new HttpRequest(null, uriBuilder.Uri.AbsoluteUri, null), new HttpResponse(new StringWriter(CultureInfo.InvariantCulture)));
		}

		public static HttpContextBase GetHttpContextWrapper()
		{
			return new HttpContextWrapper(GetHttpContext());
		}

		public static HttpRequestWrapper GetHttpRequestWrapper(HttpRequest httpRequest)
		{
			return httpRequest != null ? new HttpRequestWrapper(httpRequest) : null;
		}
	}
}