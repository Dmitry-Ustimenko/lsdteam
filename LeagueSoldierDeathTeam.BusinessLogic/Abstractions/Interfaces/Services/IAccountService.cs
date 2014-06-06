﻿using System.Collections.Generic;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.BusinessLogic.Services.Parameters;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services
{
	public interface IAccountService
	{
		UserData LogOn(string login, string password);

		void Create(UserData data, bool isExternal = false);

		void UpdateMainInfo(UserInfoData data);

		void UpdateAdvanceInfo(UserInfoData data);

		void UpdateBindInfo(UserInfoData data);

		void UpdateLastActivity(int userId);

		void UpdateUserPhoto(int userId, string photoPath);

		void DeleteUserPhoto(int userId);

		UserData GetUser(string email);

		UserData GetUser(int userExternalInfoId);

		IEnumerable<UserData> GetUsers();

		UserExternalInfoData GetExternalUser(string providerName, string providerKey);

		RoleData GetRole(int id);

		string GetUserResetToken(string email);

		string GetUserActivateToken(string email);

		bool VerifyUserResetToken(string token);

		void PasswordReset(PasswordResetParams resetContext);

		void SetPassword(int userId, string password);

		bool ActivateAccount(string token);

		UserInfoData GetUserProfile(int userId);

		void ChangePassword(string oldPassword, string newPassword, int userId);

		void DeleteUser(int userId);

		void BanUser(int userId, bool isBanned);

		void ActivateUser(int userId);
	}
}
