using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;

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

		public bool IsAdmin { get { return Roles.Select(o => o.Id).Contains((int)RoleEnum.Administrator); } }
		public bool IsModerator { get { return Roles.Select(o => o.Id).Contains((int)RoleEnum.Moderator); } }
		public bool IsUser { get { return Roles.Select(o => o.Id).Contains((int)RoleEnum.User); } }

		public UserExternalInfoData UserExternalInfo { get; set; }
		public IEnumerable<RoleData> Roles { get; set; }

		public UserData()
		{
			CreateDate = DateTime.Today;
			LastActivity = DateTime.Today;
			Password = "secret";
		}
	}
}
