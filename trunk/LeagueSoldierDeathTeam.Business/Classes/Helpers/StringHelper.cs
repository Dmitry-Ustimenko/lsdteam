namespace LeagueSoldierDeathTeam.Business.Classes.Helpers
{
	public static class StringHelper
	{
		public static string GetShortDescription(this string description, int lenghtCutLetters)
		{
			return !string.IsNullOrWhiteSpace(description)
				? (description.Length > lenghtCutLetters
					? string.Format("{0}...", description.Substring(0, lenghtCutLetters))
					: description)
				: string.Empty;
		}
	}
}
