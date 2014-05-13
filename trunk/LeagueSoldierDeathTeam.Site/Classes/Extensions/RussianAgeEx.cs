namespace LeagueSoldierDeathTeam.Site.Classes.Extensions
{
	public static class RussianAgeEx
	{
		public static string GetRussianAge(int age)
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
	}
}