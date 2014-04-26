using System;

namespace LeagueSoldierDeathTeam.BusinessLogic.Dto
{
	public class UserData
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime LastActivity { get; set; }
		public int? UserExternalInfoId { get; set; }

		public UserData()
		{
			IsActive = true;
			CreateDate = DateTime.Today;
			Password = "secret";
		}
	}
}
