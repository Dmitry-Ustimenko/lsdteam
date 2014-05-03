using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.BusinessLogic.Services.Parameters;

namespace LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services
{
	public interface IAccountService
	{
		UserData LogOn(string login, string password);

		void Create(UserData data, bool isExternal = false);

		void Update(UserData data);

		void UpdateLastActivity(int userId);

		UserData GetUser(string email);

		UserData GetUser(int userExternalInfoId);

		UserExternalInfoData GetExternalUser(string providerName, string providerKey);

		RoleData GetRole(int id);

		string GetUserResetToken(string email);

		string GetUserActivateToken(string email);

		bool VerifyUserResetToken(string token);

		void PasswordReset(PasswordResetParams resetContext);

		bool ActivateAccount(string token);
	}
}
