using System;
using System.Collections.ObjectModel;
using System.Web;
using Newtonsoft.Json;

namespace LeagueSoldierDeathTeam.Site.Classes.Serialization
{
	public static class ObjectSerializer
	{
		public const string MediaType = "application/json";
		public static bool IndentSerializedJson = false;

		public static string ToJson<T>(T value, bool ignoreNullValues = false)
		{
			var settings = GetJsonSerializerSettings(ignoreNullValues);
			return JsonConvert.SerializeObject(value, settings);
		}

		private static JsonSerializerSettings GetJsonSerializerSettings(bool ignoreNullValues)
		{
			return new JsonSerializerSettings
			{
				Formatting = IndentSerializedJson ? Formatting.Indented : Formatting.None,
				NullValueHandling = ignoreNullValues ? NullValueHandling.Ignore : NullValueHandling.Include
			};
		}

		public static string ToString<T>(T value, bool ignoreNullValues = true)
		{
			return ignoreNullValues ?
				JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) :
				JsonConvert.SerializeObject(value);
		}

		public static T ToObject<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}

		public static Collection<T> ToList<T>(string json)
		{
			if (string.IsNullOrEmpty(json))
				return (Collection<T>)Activator.CreateInstance(typeof(Collection<T>), new object[] { });
			return JsonConvert.DeserializeObject<Collection<T>>(HttpUtility.HtmlDecode(json));
		}
	}
}