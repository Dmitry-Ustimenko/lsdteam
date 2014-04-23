using System;
using System.Configuration;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public static class AppConfig
	{
		public static string HostName { get { return ConfigurationManager.AppSettings["Host.Name"]; } }

		public static string CookieName { get { return ConfigurationManager.AppSettings["Cookie.Name"]; } }
	}
}