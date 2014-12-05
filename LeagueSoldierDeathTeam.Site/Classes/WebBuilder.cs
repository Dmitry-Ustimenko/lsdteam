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

		public static string BuildActionUrl<TController>(Expression<Action<TController>> action, bool fullUrl = false)
			where TController : Controller
		{
			var actionInfo = ControllerActionInfo.Create(action);
			var url = BuildUrl(actionInfo.ActionName, actionInfo.ControllerName, actionInfo.RouteValues);
			return fullUrl ? string.Concat(AppConfig.HostName, url) : url;
		}

		public static string BuildActionUrl<TController>(Expression<Action<TController>> action, IDictionary<string, object> routeValues, bool fullUrl = false)
			where TController : Controller
		{
			var actionInfo = ControllerActionInfo.Create(action);
			var url = BuildUrl(actionInfo.ActionName, actionInfo.ControllerName, routeValues);
			return fullUrl ? string.Concat(AppConfig.HostName, url) : url;
		}

		public static string BuildActionUrl<TController>(Expression<Action<TController>> action, IDictionary<string, object> routeValues, string fragment, bool fullUrl = false)
			where TController : Controller
		{
			var actionInfo = ControllerActionInfo.Create(action);
			var url = BuildUrl(actionInfo.ActionName, actionInfo.ControllerName, routeValues);
			var fullFragment = !string.IsNullOrWhiteSpace(fragment) ? string.Concat("#", fragment) : string.Empty;

			return fullUrl ? string.Concat(AppConfig.HostName, url, fullFragment) : string.Concat(url, fullFragment);
		}

		private static string BuildUrl(string actionName, string controllerName, IDictionary<string, object> routeValues, IDictionary<string, string> queryParameters = null)
		{
			return BuildUrl(actionName, controllerName, new RouteValueDictionary(routeValues ?? new Dictionary<string, object>()), queryParameters);
		}

		private static string BuildUrl(string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, string> queryParameters)
		{
			string cacheKey = null;
			string result = null;
			if (routeValues == null || routeValues.Count == 0)
			{
				cacheKey = GetUrlCacheKey(null, actionName, controllerName);
				UrlCache.TryGetValue(cacheKey, out result);
			}

			if (result == null)
			{
				var request = ContextFactory.GetHttpContext().Request;
				if (request != null)
				{
					result = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues, RouteTable.Routes, request.RequestContext, false);

					if (cacheKey != null)
						UrlCache.TryAdd(cacheKey, result);
				}
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

		#region HttpContext

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

		#endregion
	}
}