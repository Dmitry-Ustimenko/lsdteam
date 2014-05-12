using System;
using System.Globalization;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
using LeagueSoldierDeathTeam.Site.Models.AccountProfile;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[Authorize]
	public class AccountProfileController : BaseController
	{
		private readonly IAccountService _accountService;

		public AccountProfileController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{
			_accountService = serviceFactory.CreateAccountService();
		}

		#region Actions

		#region Profile Info

		[Route("user-profile-info/{userId:int}")]
		public ActionResult ProfileInfo(int userId)
		{
			var model = new UserProfileModel { UserId = userId };
			FillUserProfileModel(model);
			return View(model);
		}

		#endregion

		#region Edit Profile

		[Route("edit-user-profile/{userId:int}")]
		public ActionResult EditProfile(int userId)
		{
			var model = new EditProfileModel { UserId = userId };
			FillEditProfileModel(model);
			return View(model);
		}

		[HttpPost]
		public ActionResult EditMainInfo(EditMainInfoModel model)
		{
			if (ModelIsValid)
			{
				var data = new UserInfoData
				{
					UserId = model.UserId,
					UserName = model.UpdateUserName,
					UserEmail = model.UpdateUserEmail,
					FirstName = model.FirstName,
					LastName = model.LastName,
					SexId = model.SexId
				};

				Execute(() => _accountService.UpdateMainInfo(data));
			}

			return View("_EditMainInfoPartial", model);
		}

		[HttpPost]
		public ActionResult EditAdvanceInfo(EditAdvanceInfoModel model)
		{
			if (ModelIsValid)
			{
				var dateBirth = default(DateTime?);
				if (!string.IsNullOrWhiteSpace(model.DateBirth))
				{
					DateTime date;
					if (DateTime.TryParse(model.DateBirth, out date))
						dateBirth = date;
					else
						ModelState.AddModelError(string.Empty, "Неверный формат даты.");
				}

				if (ModelIsValid)
				{
					var data = new UserInfoData
					{
						UserId = model.UserId,
						AboutMe = model.AboutMe,
						Activity = model.Activity,
						DateBirth = dateBirth,
						Country = model.Country,
						Town = model.Town,
						Street = model.Street,
						HomeNumber = model.HomeNum
					};

					Execute(() => _accountService.UpdateAdvanceInfo(data));
				}
			}

			return View("_EditAdvanceInfoPartial", model);
		}

		[HttpPost]
		public ActionResult EditBindInfo(EditBindInfoModel model)
		{
			if (ModelIsValid)
			{
				var data = new UserInfoData
				{
					UserId = model.UserId,
					SiteLink = model.SiteLink,
					Icq = model.Icq,
					Skype = model.Skype,
					BattleLog = model.BattleLog,
					Steam = model.Steam
				};

				Execute(() => _accountService.UpdateBindInfo(data));
			}

			return View("_EditBindInfoPartial", model);
		}

		#endregion

		#region Change Password

		[HttpPost]
		[Route("change-password")]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelIsValid)
				Execute(() => _accountService.ChangePassword(model.OldPassword, model.NewPassword, model.UserId));
			return View("_ChangePasswordPartial", model);
		}

		#endregion

		#endregion

		#region Internal Implementation

		private void FillUserProfileModel(UserProfileModel model)
		{
			var userProfile = Execute(() => _accountService.GetUserProfile(model.UserId));
			model.CopyFrom(userProfile);
		}

		private void FillEditProfileModel(EditProfileModel model)
		{
			var userProfile = Execute(() => _accountService.GetUserProfile(model.UserId));
			model.CopyFrom(userProfile);
		}

		#endregion
	}
}