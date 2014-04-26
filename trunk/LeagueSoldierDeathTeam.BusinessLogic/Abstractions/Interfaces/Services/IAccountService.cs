using System;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;

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
	}
}
