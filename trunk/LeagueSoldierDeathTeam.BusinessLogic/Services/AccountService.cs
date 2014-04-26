using System;
using System.Linq;
using System.Linq.Expressions;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Security;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

namespace LeagueSoldierDeathTeam.BusinessLogic.Services
{
	public class AccountService : IAccountService
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IRepository<User> _userRepository;

		private readonly IRepository<Role> _roleRepository;

		private readonly IRepository<UserExternalInfo> _userExternalInfoRepository;

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
		}

		#region IAccountService Members

		UserData IAccountService.LogOn(string login, string password)
		{
			var user = (this as IAccountService).GetUser(login);
			if (user == null)
				throw new ArgumentException("Логин или пароль введены не верно.");

			if (!VerifyPassword(password, user.Password))
				throw new ArgumentException("Логин или пароль введены не верно.");

			return user;
		}

		void IAccountService.Create(UserData data)
		{
			var users = _userRepository.Query(o => o.UserName == data.UserName);
			if (users.Any())
				throw new ArgumentException("Пользователь с таким именем существует.");

			users = _userRepository.Query(o => o.Email == data.Email);
			if (users.Any())
				throw new ArgumentException("Пользователь с такой электронной почтой существует.");

			var hashing = new Hashing(data.Password);
			var user = new User
			{
				UserName = data.UserName,
				Email = data.Email,
				Password = string.Concat(hashing.Salt, hashing.Hash),
				CreateDate = DateTime.Today,
				IsActive = true,
				LastActivity = DateTime.Today,
			};

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

		UserData IAccountService.GetUser(string email)
		{
			return GetUser(o => o.Email == email);
		}

		UserData IAccountService.GetUser(int userExternalInfoId)
		{
			return GetUser(o => o.UserExternalInfoId == userExternalInfoId);
		}

		public UserExternalInfoData GetExternalUser(string providerName, string providerKey)
		{
			return _userExternalInfoRepository.GetData(o => new UserExternalInfoData
			{
				Id = o.Id,
				ProviderKey = o.ProviderKey,
				ProviderName = o.ProviderName,
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

		#endregion

		#region Internal Implementation

		private static bool VerifyPassword(string password, string userPassword)
		{
			if (string.IsNullOrEmpty(userPassword) || userPassword.Length < 8)
				return false;
			var hashing = new Hashing(userPassword.Substring(0, 8), userPassword.Substring(8));
			return hashing.Verify(password);
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
				LastActivity = o.LastActivity,
				UserExternalInfoId = o.UserExternalInfoId
			}, filter).SingleOrDefault();
		}

		#endregion
	}
}
