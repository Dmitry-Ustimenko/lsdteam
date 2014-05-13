using System;

namespace LeagueSoldierDeathTeam.BusinessLogic.Dto
{
	public class UserInfoData
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhotoPath { get; set; }
		public string Activity { get; set; }
		public DateTime? DateBirth { get; set; }
		public string Country { get; set; }
		public string Town { get; set; }
		public string Street { get; set; }
		public string HomeNumber { get; set; }
		public string SiteLink { get; set; }
		public string Icq { get; set; }
		public string Skype { get; set; }
		public string BattleLog { get; set; }
		public string Steam { get; set; }
		public string AboutMe { get; set; }
		public int? SexId { get; set; }
		public string SexName { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserEmail { get; set; }
		public bool ShowUserEmail { get; set; }

		public UserData User { get; set; }
	}
}
