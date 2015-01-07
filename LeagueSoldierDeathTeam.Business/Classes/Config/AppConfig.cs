using System;
using System.Configuration;

namespace LeagueSoldierDeathTeam.Business.Classes.Config
{
	public static class AppConfig
	{
		public static TimeSpan PasswordResetLinkLifetime
		{
			get
			{
				var rawValue = ConfigurationManager.AppSettings["PasswordResetLinkLifetimeMinutes"];
				int value;
				if (!int.TryParse(rawValue, out value))
					value = 1440;
				return new TimeSpan(0, value, 0);
			}
		}
	}
}
