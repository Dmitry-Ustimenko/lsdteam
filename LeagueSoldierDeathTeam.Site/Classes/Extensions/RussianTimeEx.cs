namespace LeagueSoldierDeathTeam.Site.Classes.Extensions
{
	public static class RussianTimeEx
	{
		public static string GetRussianYears(this int years)
		{
			if (years == default(int))
				return string.Empty;

			var t1 = years % 10;
			var t2 = years % 100;

			if (t1 == 1 && t2 != 11)
				return string.Format("{0} год", years);

			return t1 >= 2 && t1 <= 4 && (t2 < 10 || t2 >= 20)
				? string.Format("{0} года", years)
				: string.Format("{0} лет", years);
		}

		public static string GetRussianMonths(this int months)
		{
			if (months == default(int))
				return string.Empty;

			if (months == 1)
				return string.Format("{0} месяц", months);

			return months > 1 && months < 5
				? string.Format("{0} месяца", months)
				: string.Format("{0} месяцев", months);
		}

		public static string GetRussianDays(this int days)
		{
			if (days == default(int))
				return string.Empty;

			var lastNum = days % 10;

			if (days >= 11 && days <= 19 || lastNum == 0 || lastNum >= 5 && lastNum <= 9)
				return string.Format("{0} дней", days);

			return lastNum == 1
				? string.Format("{0} день", days)
				: string.Format("{0} дня", days);
		}
	}
}