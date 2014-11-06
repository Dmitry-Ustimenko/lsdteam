using System;
using System.Text;
using System.Text.RegularExpressions;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions
{
	public static class StringGeneration
	{
		public static string Generate(int length)
		{
			var builder = new StringBuilder();
			var random = new Random();

			for (var i = 0; i < length; i++)
				builder.Append(Constants.Chars[random.Next(0, Constants.Chars.Length - 1)]);
			return builder.ToString();
		}

		public static string QuoteTitleBuilder(string title)
		{
			var reRgx = new Regex("^Re\\[[0-9]+\\]:");
			var re = reRgx.Match(title);

			if (!string.IsNullOrWhiteSpace(re.Value))
			{
				var reCountRgx = new Regex("[0-9]+");
				var reCount = reCountRgx.Match(re.Value);
				int count;

				if (int.TryParse(reCount.Value, out count) && count > 0)
					return string.Format("Re[{0}]:{1}", count + 1, title.Remove(0, re.Length));
			}

			return string.Format("Re[{0}]: {1}", 1, title);
		}

		public static string QuoteDescriptionBuilder(string description, string userName)
		{
			userName = !string.IsNullOrWhiteSpace(userName) ? string.Concat("=\"", userName, "\"") : string.Empty;
			return string.Format("[quote{0}]{1}[/quote]\n", userName, description);
		}
	}
}