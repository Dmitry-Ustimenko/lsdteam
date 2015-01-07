using System.Collections.Generic;
using LeagueSoldierDeathTeam.Business.Classes.Enums;
using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Business.Services.Parameters;

namespace LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services
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

		UserData GetUser(int userId);

		IEnumerable<UserData> GetUsers();

		PageData<UserData> GetUsers(SortEnum sortType, string term, int pageId, int pageSize, RoleEnum currentUserRole);

		PageData<UserData> GetUsers(RoleEnum roleType, string term, int pageId, int pageSize, RoleEnum currentUserRole);

		UserExternalInfoData GetExternalUser(string providerName, string providerKey);

		RoleData GetRole(int id);

		void UpdateRole(int id, int userId);

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
