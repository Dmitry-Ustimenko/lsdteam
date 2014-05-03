using System;
using System.Linq;
using System.Linq.Expressions;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Config;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Security;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.BusinessLogic.Services.Parameters;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

namespace LeagueSoldierDeathTeam.BusinessLogic.Services
{
	public class AccountService : IAccountService
	{
		#region Private Fields

		private readonly IUnitOfWork _unitOfWork;

		private readonly IRepository<User> _userRepository;

		private readonly IRepository<Role> _roleRepository;

		private readonly IRepository<UserExternalInfo> _userExternalInfoRepository;

		private readonly IRepository<UserResetToken> _userResetTokenRepository;

		private readonly IRepository<UserActivateToken> _userActivateTokenRepository;

		#endregion

		#region Constructors

		public AccountService(IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
		{
			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");
			_unitOfWork = unitOfWork;

			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");

			_userRepository = repositoryFactory.CreateUserRepository();
			_roleRepository = repositoryFactory.CreateRoleRepository();
			_userExternalInfoRepository = repositoryFactory.CreateUserExternalInfoRepository();
			_userResetTokenRepository = repositoryFactory.CreateUserResetTokenRepository();
			_userActivateTokenRepository = repositoryFactory.CreateUserActivateTokenRepository();
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

			user.UserRoles.Add(new UserRole { Role = role, User = user });

			_userRepository.Add(user);
			_unitOfWork.Commit();
		}

		void IAccountService.Update(UserData data)
		{
			throw new NotImplementedException();
		}

		void IAccountService.UpdateLastActivity(int userId)
		{
			var user = _userRepository.Query(o => o.Id == userId).SingleOrDefault();
			if (user == null)
				throw new ArgumentNullException(string.Format("Данного пользователя не существует."));

			user.LastActivity = DateTime.Now;

			_unitOfWork.Commit();
		}

		UserData IAccountService.GetUser(string email)
		{
			return GetUser(o => o.Email == email);
		}

		UserData IAccountService.GetUser(int id)
		{
			return GetUser(o => o.Id == id);
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

		string IAccountService.GetUserResetToken(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("E-mail не заполнен.");

			var user = _userRepository.Query(o => o.Email == email).SingleOrDefault();
			if (user == null) return string.Empty;

			var token = CryptingHelper.GenerateEncodedUniqueToken();
			var resetToken = _userResetTokenRepository.Query(o => o.UserId == user.Id).SingleOrDefault();

			if (resetToken == null)
				_userResetTokenRepository.Add(new UserResetToken { CreateDate = DateTime.Now, Token = token, User = user });
			else
				resetToken.Token = token;

			_unitOfWork.Commit();

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

			_unitOfWork.Commit();

			return token;
		}

		bool IAccountService.VerifyUserResetToken(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				throw new ArgumentNullException("token");

			var resetPasswordToken = _userResetTokenRepository.Query(o => o.Token == token).SingleOrDefault();
			return resetPasswordToken != null && ValidatePasswordResetToken(resetPasswordToken);
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
			_unitOfWork.Commit();

			DeleteUserResetToken(userResetToken);
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
			_unitOfWork.Commit();

			DeleteUserActivateToken(userActivateToken);

			return true;
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

			if (userResetToken.CreateDate.Add(AppConfig.PasswordResetLinkLifetime) < DateTime.Now)
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
			_unitOfWork.Commit();
		}

		private void DeleteUserActivateToken(UserActivateToken userActivateToken)
		{
			if (userActivateToken == null)
				return;

			_userActivateTokenRepository.Delete(userActivateToken);
			_unitOfWork.Commit();
		}

		private UserData GetUser(Expression<Func<User, bool>> filter)
		{
			return _userRepository.GetData(o => new UserData
			{
				Id = o.Id,
				UserName = o.UserName,
				Email = o.Email,
				IsActive = o.IsActive,
				Password = o.Password,
				CreateDate = o.CreateDate,
				LastActivity = o.LastActivity
			}, filter).SingleOrDefault();
		}

		private static string GetHashingPassword(string password)
		{
			var hashing = new Hashing(password);
			return string.Concat(hashing.Salt, hashing.Hash);
		}

		#endregion
	}
}
