using System;
using System.Text;
using System.Web;
using log4net;
using log4net.Config;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public static class Logger
	{
		private static readonly ILog EventLogger = LogManager.GetLogger("Event");
		private static readonly ILog EmergencyLogger = LogManager.GetLogger("Emergency");

		public static void InitLogger()
		{
			XmlConfigurator.Configure();
		}

		public static void WriteEvent(string message)
		{
			EventLogger.Info(message);
		}

		public static void WriteEvent(Exception ex, string message)
		{
			EventLogger.Info(message, ex);
		}

		public static void WriteEmergency(Exception ex)
		{
			EmergencyLogger.Error(string.Empty, ex);
		}
		public static void WriteEmergency(Exception ex, string message)
		{
			var msg = GetUserContext();
			if (!string.IsNullOrEmpty(message))
				msg += message;
			EmergencyLogger.Error(msg, ex);
		}

		private static string GetUserContext()
		{
			var httpContext = HttpContext.Current;
			if (httpContext == null)
				return "No HttpContext";

			var request = httpContext.Request;
			var userHostAddress = request.UserHostAddress;
			var rawUrl = request.RawUrl;
			var httpMethod = request.HttpMethod;
			var urlReferrer = request.UrlReferrer != null ? request.UrlReferrer.AbsoluteUri : string.Empty;
			var userAgent = request.UserAgent;

			var sb = new StringBuilder();
			sb.Append('\t');
			if (!string.IsNullOrWhiteSpace(userHostAddress))
			{
				sb.Append(userHostAddress);
				sb.Append('\t');
			}
			if (!string.IsNullOrWhiteSpace(rawUrl))
			{
				sb.Append(rawUrl);
				sb.Append('\t');
			}
			if (!string.IsNullOrWhiteSpace(httpMethod))
			{
				sb.Append(httpMethod);
				sb.Append('\t');
			}
			if (!string.IsNullOrWhiteSpace(urlReferrer))
			{
				sb.Append(urlReferrer);
				sb.Append('\t');
			}
			if (!string.IsNullOrWhiteSpace(userAgent))
			{
				sb.Append(userAgent);
				sb.Append('\t');
			}
			return sb.ToString();
		}
	}
}