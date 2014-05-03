using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Threading.Tasks;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes;
using Mvc.Mailer;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public sealed class Mailer : MailerBase, IMailer
	{
		public string SystemEmailAddress = AppConfig.SystemEmailAddress;
		public string SystemEmailName = AppConfig.SystemEmailName;

		public Mailer()
		{
			CurrentHttpContext = WebBuilder.GetHttpContextWrapper();
		}

		#region IMailer Members

		void IMailer.SendMessage(string mailTemplate, object model, string toAddress)
		{
			var message = BuildMessage(mailTemplate, model, toAddress);
			Send(message);
		}

		void IMailer.SendMessageAsync(string mailTemplate, object model, string toAddress)
		{
			var message = BuildMessage(mailTemplate, model, toAddress);
			SendAsync(message);
		}

		void IMailer.SendMessage(string mailTemplate, object model, IEnumerable<string> toAddresses)
		{
			var message = BuildMessage(mailTemplate, model, toAddresses);
			Send(message);
		}

		void IMailer.SendMessageAsync(string mailTemplate, object model, IEnumerable<string> toAddresses)
		{
			var message = BuildMessage(mailTemplate, model, toAddresses);
			SendAsync(message);
		}

		#endregion

		#region Internal Implementation

		private static void Send(MvcMailMessage message)
		{
			try
			{
				message.Send();
			}
			catch (Exception ex)
			{
				Logger.WriteEmergency(ex, ex.Message);
			}
		}

		private static void SendAsync(MvcMailMessage message)
		{
			try
			{
				var task = Task.Factory.StartNew(() => message.Send());
				task.ContinueWith(t =>
				{
					if (t.Exception == null) return;
					foreach (var innerEx in t.Exception.InnerExceptions)
						Logger.WriteEmergency(innerEx, innerEx.Message);
				});
			}
			catch (Exception ex)
			{
				Logger.WriteEmergency(ex, ex.Message);
			}
		}

		private MvcMailMessage BuildMessage(string mailTemplate, object model, string toAddress)
		{
			var message = CreateMessage(mailTemplate, model);
			message.To.Clear();
			message.To.Add(AppConfig.MailIsDebug ? AppConfig.MailAdmin : toAddress);
			return message;
		}

		private MvcMailMessage BuildMessage(string mailTemplate, object model, IEnumerable<string> toAddresses)
		{
			var message = CreateMessage(mailTemplate, model);
			message.To.Clear();
			if (AppConfig.MailIsDebug)
				message.To.Add(AppConfig.MailAdmin);
			else
				foreach (var address in toAddresses)
					message.To.Add(address);
			return message;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		private MvcMailMessage CreateMessage(string mailTemplate, object model)
		{
			mailTemplate = string.Format(CultureInfo.InvariantCulture, "~/Views/Mail/{0}.cshtml", mailTemplate);
			var mailMessage = new MvcMailMessage { IsBodyHtml = true };
			ViewBag.Model = model;
			PopulateBody(mailMessage, mailTemplate);
			PopulateSubject(mailMessage);
			PopulateFrom(mailMessage);
			return mailMessage;
		}

		private static void PopulateSubject(MailMessage mailMessage)
		{
			mailMessage.Subject = FindPart(mailMessage.Body, "<title>", "</title>");
		}

		private void PopulateFrom(MailMessage mailMessage)
		{
			var address = FindPart(mailMessage.Body, "<meta name=\"from\" content=\"", "\"");
			var isSystemMessage = string.IsNullOrEmpty(address);
			if (isSystemMessage)
				address = SystemEmailAddress;
			var displayName = FindPart(mailMessage.Body, "<meta name=\"fromName\" content=\"", "\"");
			if (string.IsNullOrEmpty(displayName) && isSystemMessage)
				displayName = SystemEmailName;
			mailMessage.From = new MailAddress(address, displayName);
		}

		private static string FindPart(string body, string start, string end)
		{
			var startIndex = body.IndexOf(start, StringComparison.OrdinalIgnoreCase);
			if (startIndex < 0)
				return null;
			startIndex += start.Length;
			var endIndex = body.IndexOf(end, startIndex, StringComparison.OrdinalIgnoreCase);
			return endIndex < 0 ? null : body.Substring(startIndex, endIndex - startIndex);
		}

		#endregion
	}
}