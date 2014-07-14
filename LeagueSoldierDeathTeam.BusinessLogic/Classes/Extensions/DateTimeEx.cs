using System;

namespace LeagueSoldierDeathTeam.BusinessLogic.Classes.Extensions
{
	public static class DateTimeEx
	{
		public static DateTime ToUtc(this DateTime date)
		{
			return date.ToUniversalTime();
		}

		public static DateTime? ToUtc(this DateTime? date)
		{
			return date.HasValue ? date.Value.ToUniversalTime() : default(DateTime?);
		}

		public static DateTime UtcToTimeZone(this DateTime date, string timeZone)
		{
			var zone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
			return TimeZoneInfo.ConvertTimeFromUtc(date, zone);

		}

		public static DateTime? UtcToTimeZone(this DateTime? date, string timeZone)
		{
			if (!date.HasValue) return default(DateTime?);

			var zone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
			return TimeZoneInfo.ConvertTimeFromUtc(date.Value, zone);
		}
	}
}
