using System;

namespace LeagueSoldierDeathTeam.Business.Classes.Extensions
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

		public static string TodayYesterday(this DateTime date, string timeZone)
		{
			if (date.Date == DateTime.UtcNow.UtcToTimeZone(timeZone).Date)
				return date.ToString("сегодня в HH:mm");

			if (date.Date == DateTime.UtcNow.UtcToTimeZone(timeZone).Date.AddDays(-1))
				return date.ToString("вчера в HH:mm");

			return date.ToString("d MMMM yyyy в HH:mm");
		}
	}
}
