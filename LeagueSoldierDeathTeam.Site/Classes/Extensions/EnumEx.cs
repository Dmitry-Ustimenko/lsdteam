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
			return GetDisplayName(value, false);
		}

		public static string GetDisplayName(Enum value, bool showEmpty)
		{
			if (value == null)
				return string.Empty;

			var data = value.ToString();

			var fi = value.GetType().GetField(data);
			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes.Any())
				return attributes[0].Description;
			return showEmpty ? string.Empty : data;
		}

		public static IDictionary<int, string> GetItems(this System.Enum source)
		{
			return GetItems(source, false);
		}

		public static IDictionary<int, string> GetItems(this System.Enum source, bool showEmpty)
		{
			var e = source.GetType();
			var results = System.Enum.GetValues(e)
				.Cast<int>()
				.ToDictionary(o => o,
					o => GetDisplayName((System.Enum)System.Enum.Parse(e, o.ToString(CultureInfo.InvariantCulture)), showEmpty));
			return results.Where(o => !string.IsNullOrEmpty(o.Value)).ToDictionary(o => o.Key, o => o.Value);
		}

		public static ICollection<TEnum> GetItems<TEnum>(int value)
		{
			return Enum.GetValues(typeof(TEnum)).Cast<int>().Where(o => IsItemSet(value, o)).Cast<TEnum>().ToList();
		}

		public static bool IsItemSet(int value, int item)
		{
			return (value & item) == item;
		}
	}
}