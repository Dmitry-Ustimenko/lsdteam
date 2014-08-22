using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LeagueSoldierDeathTeam.BusinessLogic.Classes.Extensions
{
	public static class EnumEx
	{
		public static string GetName(Enum value)
		{
			return value == null ? string.Empty : value.ToString();
		}

		public static string GetDescription(Enum value, bool showEmpty = true)
		{
			if (value == null)
				return string.Empty;

			var data = value.ToString();

			var fi = value.GetType().GetField(data);
			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes.Length > 0)
				return attributes[0].Description;
			return !showEmpty ? string.Empty : data;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static Dictionary<int, string> ToDictionary<TEnum>(bool showEmpty = false) where TEnum : struct
		{
			var results = (from TEnum e in Enum.GetValues(typeof(TEnum))
						   select new KeyValuePair<int, string>
						   (
							   (int)Enum.Parse(typeof(TEnum), e.ToString()),
								   GetDescription((Enum)Enum.Parse(typeof(TEnum), e.ToString()), showEmpty)
						   )).ToDictionary(o => o.Key, o => o.Value);
			return results.Where(o => !string.IsNullOrEmpty(o.Value)).ToDictionary(o => o.Key, o => o.Value);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static Dictionary<string, string> ToStringDictionary<TEnum>(bool showEmpty = false) where TEnum : struct
		{
			var results = (from TEnum e in Enum.GetValues(typeof(TEnum))
						   select new KeyValuePair<string, string>
						   (
							   e.ToString(),
							   GetDescription((Enum)Enum.Parse(typeof(TEnum), e.ToString()), showEmpty)
						   )).ToDictionary(o => o.Key, o => o.Value);
			return results.Where(o => !string.IsNullOrEmpty(o.Value)).ToDictionary(o => o.Key, o => o.Value);
		}
	}
}