namespace LeagueSoldierDeathTeam.Site.Classes.Extensions
{
	public static class RussianAgeEx
	{
		public static string GetRussianAge(this int age)
		{
			if (age == default(int))
				return string.Empty;

			var t1 = age % 10;
			var t2 = age % 100;

			if (t1 == 1 && t2 != 11)
				return string.Format("{0} год", age);

			return t1 >= 2 && t1 <= 4 && (t2 < 10 || t2 >= 20)
				? string.Format("{0} года", age)
				: string.Format("{0} лет", age);
		}

		public static string GetRussianMonth(this int month)
		{
			if (month == default(int))
				return string.Empty;

			if (month == 1)
				return string.Format("{0} месяц", month);

			return month > 1 && month < 5
				? string.Format("{0} месяца", month)
				: string.Format("{0} месяцев", month);
		}
	}
}