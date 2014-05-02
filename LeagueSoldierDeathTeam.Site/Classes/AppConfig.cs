using System.Configuration;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public static class AppConfig
	{
		public static string SystemEmailAddress { get { return ConfigurationManager.AppSettings["NoreplyEmail"]; } }
		
		public static string SystemEmailName { get { return ConfigurationManager.AppSettings["NoreplyName"]; } }

		public static string HostName { get { return ConfigurationManager.AppSettings["Host.Name"]; } }

		public static string CookieName { get { return ConfigurationManager.AppSettings["Cookie.Name"]; } }

		public static string MailAdmin { get { return ConfigurationManager.AppSettings["Mail.Admin"]; } }

		public static bool MailIsDebug { get { return bool.Parse(ConfigurationManager.AppSettings["Mail.IsDebug"]); } }
	}
}