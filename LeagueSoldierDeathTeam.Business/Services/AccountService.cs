﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LeagueSoldierDeathTeam.Business.Abstractions.Factories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.LoggedUser;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.Business.Classes.Config;
using LeagueSoldierDeathTeam.Business.Classes.Enums;
using LeagueSoldierDeathTeam.Business.Classes.Helpers;
using LeagueSoldierDeathTeam.Business.Classes.Security;
using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Business.Services.Parameters;
using LeagueSoldierDeathTeam.DataBase.Model;
using Role = LeagueSoldierDeathTeam.DataBase.Model.Role;

namespace LeagueSoldierDeathTeam.Business.Services
{
	public class AccountService : ServiceBase, IAccountService
	{
		#region Private Fields

		private readonly IRepository<User> _userRepository;

		private readonly IRepository<Role> _roleRepository;

		private readonly IRepository<UserExternalInfo> _userExternalInfoRepository;

		private readonly IRepository<UserResetToken> _userResetTokenRepository;

		private readonly IRepository<UserActivateToken> _userActivateTokenRepository;

		private readonly IRepository<UserInfo> _userInfoRepository;

		private readonly IRepository<Sex> _sexRepository;

		#endregion

		#region Constructors

		public AccountService(ILoggedUserProvider loggedUserProvider, IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
			: base(loggedUserProvider, unitOfWork)
		{
			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");

			_userRepository = repositoryFactory.CreateRepository<User>();
			_roleRepository = repositoryFactory.CreateRepository<Role>();
			_userExternalInfoRepository = repositoryFactory.CreateRepository<UserExternalInfo>();
			_userActivateTokenRepository = repositoryFactory.CreateRepository<UserActivateToken>();
			_userInfoRepository = repositoryFactory.CreateRepository<UserInfo>();
			_userResetTokenRepository = repositoryFactory.CreateRepository<UserResetToken>();
			_sexRepository = repositoryFactory.CreateRepository<Sex>();
		}

		#endregion

		#region IAccountService Members

		UserData IAccountService.LogOn(string login, string password)
		{
			var user = (this as IAccountService).GetUser(login);
			if (user == null)
				throw new ArgumentException("Логин или пароль введены не верно.");

			if (!VerifyPassword(password, user.Password))
				throw new ArgumentException("Логин или пароль введены не верно.");

			if (!user.IsActive)
				throw new ArgumentException("Данный аккаунт не активирован.");

			return user;
		}

		void IAccountService.Create(UserData data, bool isExternal)
		{
			var users = _userRepository.Query(o => o.UserName == data.UserName);
			if (users.Any())
				throw new ArgumentException("Пользователь с таким именем существует.");

			users = _userRepository.Query(o => o.Email == data.Email);
			if (users.Any())
				throw new ArgumentException("Пользователь с такой электронной почтой существует.");

			var user = new User
			{
				UserName = data.UserName,
				Email = data.Email,
				Password = GetHashingPassword(data.Password),
				CreateDate = data.CreateDate,
				IsActive = data.IsActive,
				LastActivity = data.LastActivity,
			};

			if (isExternal)
			{
				if (data.UserExternalInfo == null)
					throw new ArgumentException("Данные о провайдере и внешнем ключе отсутсвуют.");

				var userExternalInfo = new UserExternalInfo
				{
					ProviderKey = data.UserExternalInfo.ProviderKey,
					ProviderName = data.UserExternalInfo.ProviderName,
					User = user
				};

				_userExternalInfoRepository.Add(userExternalInfo);
			}

			var role = _roleRepository.Query(o => o.Id == (int)RoleEnum.User).SingleOrDefault();
			if (role == null)
				throw new ArgumentNullException(string.Format("role"));

			user.Role = role;

			_userRepository.Add(user);
			UnitOfWork.Commit();
		}

		void IAccountService.UpdateMainInfo(UserInfoData data)
		{
			var users = _userRepository.Query(o => o.UserName == data.UserName && o.Id != data.UserId);
			if (users.Any())
				throw new ArgumentException("Пользователь с таким именем существует.");

			users = _userRepository.Query(o => o.Email == data.UserEmail && o.Id != data.UserId);
			if (users.Any())
				throw new ArgumentException("Пользователь с такой электронной почтой существует.");

			var user = _userRepository.Query(o => o.Id == data.UserId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("user"));

			user.UserName = data.UserName;
			user.Email = data.UserEmail;
			user.ShowEmail = data.ShowUserEmail;

			var entityUserInfo = _userInfoRepository.Query(o => o.UserId == data.UserId).SingleOrDefault();

			var userInfo = entityUserInfo ?? new UserInfo { User = user };
			userInfo.FirstName = data.FirstName;
			userInfo.LastName = data.LastName;

			var sex = _sexRepository.Query(o => o.Id == data.SexId.Value).SingleOrDefault();
			userInfo.SexId = sex != null ? sex.Id : (int?)null;

			if (entityUserInfo == null)
				_userInfoRepository.Add(userInfo);

			UnitOfWork.Commit();
		}

		void IAccountService.UpdateAdvanceInfo(UserInfoData data)
		{
			var user = _userRepository.Query(o => o.Id == data.UserId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("user"));

			var entityUserInfo = _userInfoRepository.Query(o => o.UserId == data.UserId).SingleOrDefault();
			var userInfo = entityUserInfo ?? new UserInfo { User = user };

			userInfo.AboutMe = data.AboutMe;
			userInfo.Activity = data.Activity;
			userInfo.DateBirth = data.DateBirth;
			userInfo.Country = data.Country;
			userInfo.Town = data.Town;
			userInfo.Street = data.Street;
			userInfo.HomeNumber = data.HomeNumber;

			if (entityUserInfo == null)
				_userInfoRepository.Add(userInfo);

			UnitOfWork.Commit();
		}

		void IAccountService.UpdateBindInfo(UserInfoData data)
		{
			var user = _userRepository.Query(o => o.Id == data.UserId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("user"));

			var entityUserInfo = _userInfoRepository.Query(o => o.UserId == data.UserId).SingleOrDefault();
			var userInfo = entityUserInfo ?? new UserInfo { User = user };

			userInfo.SiteLink = data.SiteLink;
			userInfo.ICQ = data.Icq;
			userInfo.Skype = data.Skype;
			userInfo.BattleLog = data.BattleLog;
			userInfo.Steam = data.Steam;

			if (entityUserInfo == null)
				_userInfoRepository.Add(userInfo);

			UnitOfWork.Commit();
		}

		void IAccountService.UpdateLastActivity(int userId)
		{
			var user = _userRepository.Query(o => o.Id == userId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("Данного пользователя не существует."));

			user.LastActivity = DateTime.UtcNow;

			UnitOfWork.Commit();
		}

		void IAccountService.UpdateUserPhoto(int userId, string photoPath)
		{
			var user = _userRepository.Query(o => o.Id == userId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("Данного пользователя не существует."));

			user.PhotoPath = photoPath;
			UnitOfWork.Commit();
		}

		void IAccountService.DeleteUserPhoto(int userId)
		{
			var user = _userRepository.Query(o => o.Id == userId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("Данного пользователя не существует."));

			user.PhotoPath = string.Empty;
			UnitOfWork.Commit();
		}

		UserData IAccountService.GetUser(string email)
		{
			return GetUser(o => o.Email == email);
		}

		UserData IAccountService.GetUser(int id)
		{
			var user = GetUser(o => o.Id == id);
			if (user == null)
				throw new ArgumentException("Такого пользователя не существует.");

			return user;
		}

		IEnumerable<UserData> IAccountService.GetUsers()
		{
			return _userRepository.GetData(o => new UserData
			{
				Id = o.Id,
				UserName = o.UserName,
				Email = o.Email,
				IsActive = o.IsActive,
				PhotoPath = o.PhotoPath,
				IsBanned = o.IsBanned,
				RoleId = o.RoleId
			});
		}

		PageData<UserData> IAccountService.GetUsers(SortEnum sortType, string term, int pageId, int pageSize, RoleEnum currentUserRole)
		{
			var pageData = new PageData<UserData> { PageId = pageId, PageSize = pageSize };

			var data = !string.IsNullOrWhiteSpace(term)
				? _userRepository.GetQueryableData(o => o.UserName.Contains(term))
				: _userRepository.GetQueryableData();

			data = data.Where(o => o.RoleId != (int)RoleEnum.MainAdministrator);

			if (currentUserRole == RoleEnum.Administrator)
				data = data.Where(o => o.RoleId != (int)RoleEnum.Administrator);

			pageData.Count = data.Count();

			if (pageData.Count != 0)
			{
				switch (sortType)
				{
					case SortEnum.Default:
						data = data.OrderBy(o => !o.IsActive).ThenBy(o => o.IsBanned).ThenBy(o => o.UserName);
						break;
					case SortEnum.Name:
						data = data.OrderBy(o => o.UserName);
						break;
					case SortEnum.Email:
						data = data.OrderBy(o => o.Email);
						break;
					case SortEnum.Actived:
						data = data.OrderBy(o => !o.IsActive).ThenBy(o => o.IsBanned).ThenBy(o => o.UserName);
						break;
					case SortEnum.Banned:
						data = data.OrderBy(o => !o.IsBanned).ThenBy(o => !o.IsActive).ThenBy(o => o.UserName);
						break;
				}

				pageData.Data = data.Page(pageData.PageId, pageData.PageSize).Select(o => new UserData
				{
					Id = o.Id,
					UserName = o.UserName,
					Email = o.Email,
					IsActive = o.IsActive,
					PhotoPath = o.PhotoPath,
					IsBanned = o.IsBanned,
					RoleId = o.RoleId
				}).ToList();
			}

			return pageData;
		}

		PageData<UserData> IAccountService.GetUsers(RoleEnum roleType, string term, int pageId, int pageSize, RoleEnum currentUserRole)
		{
			var pageData = new PageData<UserData> { PageId = pageId, PageSize = pageSize };

			var data = !string.IsNullOrWhiteSpace(term)
				? _userRepository.GetQueryableData(o => o.UserName.Contains(term) && ((int)roleType == (int)RoleEnum.None || o.RoleId == (int)roleType))
				: _userRepository.GetQueryableData(o => ((int)roleType == (int)RoleEnum.None || o.RoleId == (int)roleType));

			data = data.Where(o => o.RoleId != (int)RoleEnum.MainAdministrator);

			if (currentUserRole == RoleEnum.Administrator)
				data = data.Where(o => o.RoleId != (int)RoleEnum.Administrator);

			pageData.Count = data.Count();

			if (pageData.Count != 0)
			{
				pageData.Data = data.OrderBy(o => o.UserName).Page(pageData.PageId, pageData.PageSize).Select(o => new UserData
				{
					Id = o.Id,
					UserName = o.UserName,
					Email = o.Email,
					IsActive = o.IsActive,
					PhotoPath = o.PhotoPath,
					IsBanned = o.IsBanned,
					RoleId = o.RoleId
				}).ToList();
			}

			return pageData;
		}

		UserInfoData IAccountService.GetUserProfile(int userId)
		{
			var user = GetUser(o => o.Id == userId);
			if (user == null)
				throw new ArgumentNullException(string.Format("user"));

			var userInfo = _userInfoRepository.GetData(o => new UserInfoData
			{
				Id = o.Id,
				FirstName = o.FirstName,
				LastName = o.LastName,
				Activity = o.Activity,
				Country = o.Country,
				DateBirth = o.DateBirth,
				HomeNumber = o.HomeNumber,
				Town = o.Town,
				SiteLink = o.SiteLink,
				Skype = o.Skype,
				Street = o.Street,
				Icq = o.ICQ,
				BattleLog = o.BattleLog,
				Steam = o.Steam,
				AboutMe = o.AboutMe,
				SexId = o.SexId,
				SexName = o.Sex != null ? o.Sex.Name : string.Empty
			}, o => o.UserId == userId).SingleOrDefault();

			if (userInfo == null)
				return new UserInfoData { User = user };

			userInfo.User = user;
			return userInfo;
		}

		public UserExternalInfoData GetExternalUser(string providerName, string providerKey)
		{
			return _userExternalInfoRepository.GetData(o => new UserExternalInfoData
			{
				Id = o.Id,
				ProviderKey = o.ProviderKey,
				ProviderName = o.ProviderName,
				UserId = o.UserId
			}, o => o.ProviderName == providerName && o.ProviderKey == providerKey).SingleOrDefault();
		}

		RoleData IAccountService.GetRole(int id)
		{
			return _roleRepository.GetData(o => new RoleData
			{
				Id = o.Id,
				Name = o.Name
			}, o => o.Id == id).SingleOrDefault();
		}

		void IAccountService.UpdateRole(int id, int userId)
		{
			var user = _userRepository.Query(o => o.Id == userId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("user"));

			var role = _roleRepository.Query(o => o.Id == id).SingleOrDefault();
			if (role == null)
				throw new ArgumentNullException(string.Format("role"));

			user.Role = role;

			UnitOfWork.Commit();
		}

		string IAccountService.GetUserResetToken(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("E-mail не заполнен.");

			var user = _userRepository.Query(o => o.Email == email).SingleOrDefault();
			if (user == null) return string.Empty;

			var token = CryptingHelper.GenerateEncodedUniqueToken();
			var resetToken = _userResetTokenRepository.Query(o => o.UserId == user.Id).SingleOrDefault();

			if (resetToken == null)
				_userResetTokenRepository.Add(new UserResetToken { CreateDate = DateTime.UtcNow, Token = token, User = user });
			else
			{
				resetToken.CreateDate = DateTime.UtcNow;
				resetToken.Token = token;
			}

			UnitOfWork.Commit();

			return token;
		}

		string IAccountService.GetUserActivateToken(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("E-mail не заполнен.");

			var user = _userRepository.Query(o => o.Email == email).SingleOrDefault();
			if (user == null) return string.Empty;

			var token = CryptingHelper.GenerateEncodedUniqueToken();
			var activateToken = _userActivateTokenRepository.Query(o => o.UserId == user.Id).SingleOrDefault();

			if (activateToken == null)
				_userActivateTokenRepository.Add(new UserActivateToken { Token = token, User = user });
			else
				activateToken.Token = token;

			UnitOfWork.Commit();

			return token;
		}

		bool IAccountService.VerifyUserResetToken(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				throw new ArgumentNullException("token");

			var resetPasswordToken = _userResetTokenRepository.Query(o => o.Token == token).SingleOrDefault();
			return resetPasswordToken != null && ValidatePasswordResetToken(resetPasswordToken);
		}

		public void SetPassword(int userId, string password)
		{
			var user = _userRepository.Query(o => o.Id == userId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("user"));

			user.Password = GetHashingPassword(password);
			UnitOfWork.Commit();
		}

		bool IAccountService.ActivateAccount(string token)
		{
			var userActivateToken = _userActivateTokenRepository.Query(o => o.Token == token).SingleOrDefault();
			if (userActivateToken == null)
				return false;

			var user = _userRepository.Query(o => o.Id == userActivateToken.UserId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("user"));

			user.IsActive = true;
			UnitOfWork.Commit();

			DeleteUserActivateToken(userActivateToken);

			return true;
		}

		void IAccountService.PasswordReset(PasswordResetParams parameters)
		{
			if (parameters == null)
				return;

			var userResetToken = _userResetTokenRepository.Query(o => o.Token == parameters.PasswordResetToken).SingleOrDefault();
			if (userResetToken == null || !ValidatePasswordResetToken(userResetToken))
				return;

			var user = _userRepository.Query(o => o.Id == userResetToken.UserId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("user"));

			user.Password = GetHashingPassword(parameters.NewPassword);
			UnitOfWork.Commit();

			DeleteUserResetToken(userResetToken);
		}

		void IAccountService.ChangePassword(string oldPassword, string newPassword, int userId)
		{
			var user = _userRepository.Query(o => o.Id == userId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("user"));

			if (!VerifyPassword(oldPassword, user.Password))
				throw new ArgumentException("Старый пароль введен не верно.");

			user.Password = GetHashingPassword(newPassword);
			UnitOfWork.Commit();
		}

		void IAccountService.DeleteUser(int userId)
		{
			var user = GetUser(userId);

			_userRepository.Delete(user);
			UnitOfWork.Commit();
		}

		void IAccountService.BanUser(int userId, bool isBanned)
		{
			var user = GetUser(userId);

			user.IsBanned = !isBanned;
			UnitOfWork.Commit();
		}

		void IAccountService.ActivateUser(int userId)
		{
			var user = GetUser(userId);

			user.IsActive = true;
			UnitOfWork.Commit();
		}

		#endregion

		#region Internal Implementation

		private static bool VerifyPassword(string password, string userPassword)
		{
			if (string.IsNullOrEmpty(userPassword) || userPassword.Length < 8)
				return false;
			var hashing = new Hashing(userPassword.Substring(0, 8), userPassword.Substring(8));
			return hashing.Verify(password);
		}

		private bool ValidatePasswordResetToken(UserResetToken userResetToken)
		{
			if (userResetToken == null)
				throw new ArgumentNullException("userResetToken");

			if (userResetToken.CreateDate.Add(AppConfig.PasswordResetLinkLifetime) < DateTime.UtcNow)
			{
				DeleteUserResetToken(userResetToken);
				return false;
			}
			return true;
		}

		private void DeleteUserResetToken(UserResetToken userResetToken)
		{
			if (userResetToken == null)
				return;

			_userResetTokenRepository.Delete(userResetToken);
			UnitOfWork.Commit();
		}

		private void DeleteUserActivateToken(UserActivateToken userActivateToken)
		{
			if (userActivateToken == null)
				return;

			_userActivateTokenRepository.Delete(userActivateToken);
			UnitOfWork.Commit();
		}

		private UserData GetUser(Expression<Func<User, bool>> filter)
		{
			return _userRepository.GetData(o => new UserData
			{
				Id = o.Id,
				UserName = o.UserName,
				Email = o.Email,
				ShowEmail = o.ShowEmail,
				IsActive = o.IsActive,
				Password = o.Password,
				CreateDate = o.CreateDate,
				LastActivity = o.LastActivity,
				PhotoPath = o.PhotoPath,
				RoleId = o.Role != null ? o.Role.Id : (int)RoleEnum.User,
				RoleName = o.Role != null ? o.Role.Name : "Пользователь",
				IsBanned = o.IsBanned
			}, filter).SingleOrDefault();
		}

		private User GetUser(int id)
		{
			var user = _userRepository.Query(o => o.Id == id).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("user"));

			return user;
		}

		private static string GetHashingPassword(string password)
		{
			var hashing = new Hashing(password);
			return string.Concat(hashing.Salt, hashing.Hash);
		}

		#endregion
	}
}
