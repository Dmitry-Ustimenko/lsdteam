using System;

namespace LeagueSoldierDeathTeam.BusinessLogic.Dto
{
	public class UserData
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Address { get; set; }
		public bool IsActive { get; set; }
		public string Activity { get; set; }
		public DateTime DateBirth { get; set; }
		public int? SexId { get; set; }

		public UserData()
		{
			IsActive = true;
		}
	}
}
