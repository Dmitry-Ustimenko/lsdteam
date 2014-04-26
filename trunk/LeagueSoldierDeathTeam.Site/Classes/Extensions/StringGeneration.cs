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
				builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));
			return builder.ToString();
		}
	}
}