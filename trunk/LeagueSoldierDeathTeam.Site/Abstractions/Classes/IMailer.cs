using System.Collections.Generic;

namespace LeagueSoldierDeathTeam.Site.Abstractions.Classes
{
	public interface IMailer
	{
		void SendMessage(string mailTemplate, object model, string toAddress);

		void SendMessageAsync(string mailTemplate, object model, string toAddress);

		void SendMessage(string mailTemplate, object model, IEnumerable<string> toAddresses);

		void SendMessageAsync(string mailTemplate, object model, IEnumerable<string> toAddresses);
	}
}
