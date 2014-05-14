using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class UserProfileModel
	{
		public int UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhotoPath { get; set; }
		public string Activity { get; set; }
		public string Age { get; set; }
		public DateTime? DateBirth { get; set; }
		public string Address { get; set; }
		public string SiteLink { get; set; }
		public string Icq { get; set; }
		public string Skype { get; set; }
		public string BattleLog { get; set; }
		public string Steam { get; set; }
		public string UserName { get; set; }
		public string UserEmail { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime LastActivity { get; set; }
		public string UserRoles { get; set; }
		public string SexName { get; set; }
		public string AboutMe { get; set; }
		public bool ShowEmail { get; set; }
	}
}