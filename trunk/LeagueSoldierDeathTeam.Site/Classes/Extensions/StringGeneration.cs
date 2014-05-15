using System;
using System.Text;

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
	}
}