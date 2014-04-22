using System;
using System.Linq;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess.Repositories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Security;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.DataBaseLayer.Model;

namespace LeagueSoldierDeathTeam.BusinessLogic.Services
{
	public class AccountService : IAccountService
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IRepository<User> _userRepository;

		public AccountService(IUnitOfWork unitOfWork, RepositoryFactoryBase repositoryFactory)
		{
			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");
			_unitOfWork = unitOfWork;

			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");
			_userRepository = repositoryFactory.CreateUserRepository();
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

		void IAccountService.Register(UserData data)
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
				DateBirth = DateTime.Today,
				IsActive = true
			};
			_userRepository.Add(user);
			_unitOfWork.Commit();
		}

		UserData IAccountService.GetUser(string email)
		{
			return _userRepository.GetData(o => new UserData
			{
				Id = o.Id,
				UserName = o.UserName,
				FirstName = o.FirstName,
				LastName = o.LastName,
				Email = o.Email,
				IsActive = o.IsActive,
				Activity = o.Activity,
				Address = o.Address,
				DateBirth = o.DateBirth,
				SexId = o.SexId,
				Password = o.Password
			}, o => o.Email == email).SingleOrDefault();
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

		#endregion
	}
}
