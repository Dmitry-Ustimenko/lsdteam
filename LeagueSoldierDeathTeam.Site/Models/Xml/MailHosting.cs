using System;
using System.Xml.Serialization;
using LeagueSoldierDeathTeam.Site.Classes;

namespace LeagueSoldierDeathTeam.Site.Models.Xml
{
	[Serializable]
	[XmlRoot(Constants.XmlMailHostingSearchName, Namespace = "", IsNullable = false)]
	public class MailHosting
	{
		[XmlAttribute("host")]
		public string HostAttribute { get; set; }

		[XmlAttribute("site")]
		public string SiteAttribute { get; set; }
	}
}