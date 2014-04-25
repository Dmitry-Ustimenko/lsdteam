using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LeagueSoldierDeathTeam.Site.Classes.Parsers.Xml
{
	public static class XmlParser<T>
		where T : class
	{
		public static IList<T> Parse(string xmlPath, string searchElement)
		{
			var xmlModels = new List<T>();

			using (var xml = XmlReader.Create(string.Concat(AppDomain.CurrentDomain.BaseDirectory, xmlPath)))
			{
				while (xml.Read())
				{
					switch (xml.NodeType)
					{
						case XmlNodeType.Element:
							if (xml.Name == searchElement)
							{
								var deserializeModel = Deserialize(xml.ReadOuterXml());
								if (deserializeModel != default(T))
									xmlModels.Add(deserializeModel);
							}
							break;
					}
				}
			}

			return xmlModels;
		}

		public static T Deserialize(string source)
		{
			try
			{
				var xmlSerializer = new XmlSerializer(typeof(T));
				using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(source)))
				{
					return (T)xmlSerializer.Deserialize(ms);
				}
			}
			catch (Exception)
			{
				return default(T);
			}
		}
	}
}