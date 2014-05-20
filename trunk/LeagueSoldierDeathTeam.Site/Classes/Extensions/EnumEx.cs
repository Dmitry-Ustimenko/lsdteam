using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions
{
	public static class EnumEx
	{
		public static string GetDisplayName(this Enum value)
		{
			return GetName(value, false);
		}

		public static string GetDescriptionName(this Enum value)
		{
			return GetName(value, true);
		}

		public static IDictionary<int, string> GetItems(this Enum source, bool showDescriptionName = true)
		{
			var e = source.GetType();
			var results = Enum.GetValues(e).Cast<int>().ToDictionary(o => o, o => GetName((Enum)Enum.Parse(e, o.ToString(CultureInfo.InvariantCulture)), showDescriptionName));
			return results.Where(o => !string.IsNullOrEmpty(o.Value)).ToDictionary(o => o.Key, o => o.Value);
		}

		public static ICollection<TEnum> GetItems<TEnum>(int value)
		{
			return Enum.GetValues(typeof(TEnum)).Cast<int>().Where(o => IsItemSet(value, o)).Cast<TEnum>().ToList();
		}

		private static string GetName(Enum value, bool showDescriptionName)
		{
			if (value == null)
				return string.Empty;

			var data = value.ToString();
			var fi = value.GetType().GetField(data);

			if (showDescriptionName)
			{
				var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (attributes.Any())
					return attributes[0].Description;
			}
			return data;
		}

		private static bool IsItemSet(int value, int item)
		{
			return (value & item) == item;
		}
	}
}