﻿using System;
using LeagueSoldierDeathTeam.Business.Classes.Enums;

namespace LeagueSoldierDeathTeam.Business.Dto
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
		public bool ShowEmail { get; set; }
		public string PhotoPath { get; set; }
		public RoleEnum Role { get { return (RoleEnum)RoleId; } }
		public int RoleId { get; set; }
		public string RoleName { get; set; }
		public bool IsBanned { get; set; }
		public int InboxMessageCount { get; set; }

		public bool IsMainAdmin { get { return RoleId == (int)RoleEnum.MainAdministrator; } }
		public bool IsAdmin { get { return RoleId == (int)RoleEnum.Administrator; } }
		public bool IsModerator { get { return RoleId == (int)RoleEnum.Moderator; } }
		public bool IsUser { get { return RoleId == (int)RoleEnum.User; } }
		public bool IsMainRole { get { return RoleId != (int)RoleEnum.User; } }

		public UserExternalInfoData UserExternalInfo { get; set; }

		public UserData()
		{
			CreateDate = DateTime.UtcNow;
			LastActivity = DateTime.UtcNow;
		}

		public bool IsMe(int userId)
		{
			return userId == Id;
		}
	}
}
