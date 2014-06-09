﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Extensions;
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

		[HttpPost]
		public ActionResult EditPhoto(int userId, HttpPostedFileBase photoUploadFile)
		{
			if (photoUploadFile != null && photoUploadFile.FileName != null)
			{
				try
				{
					if (photoUploadFile.ContentLength > 102400)
						ModelState.AddModelError(string.Empty, "Размер файла не выше 100кб");

					if (!Constants.AcceptImage.Contains(photoUploadFile.ContentType))
						ModelState.AddModelError(string.Empty, "Требуемые форматы: *.jpg, *.jpeg, *.png, *.gif");

					if (ModelIsValid)
					{
						var image = Image.FromStream(photoUploadFile.InputStream);
						if (image.Width > 200 || image.Height > 200)
							ModelState.AddModelError(string.Empty, "Разрешение фото не должно превышать 200х200");
					}

					if (ModelIsValid)
					{
						var user = Execute(() => _accountService.GetUser(userId));
						var oldPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, user.PhotoPath);

						var fileName = string.Concat(StringGeneration.Generate(20), Path.GetExtension(photoUploadFile.FileName));
						var path = Path.Combine(Server.MapPath(Constants.PhotoDirectoryPath), Path.GetFileName(fileName));
						while (System.IO.File.Exists(path))
							fileName = string.Concat(StringGeneration.Generate(20), Path.GetExtension(photoUploadFile.FileName));

						photoUploadFile.SaveAs(path);

						Execute(() => _accountService.UpdateUserPhoto(userId, string.Concat(Constants.PhotoDirectoryPath, fileName)));

						if (AppContext.CurrentUser.IsMe(userId))
							Execute(() => AppContext.CurrentUser = _accountService.GetUser(userId));

						if (ModelIsValid)
						{
							if (System.IO.File.Exists(oldPath))
								System.IO.File.Delete(oldPath);

							return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(userId));
						}
					}
				}
				catch (Exception)
				{
					ModelState.AddModelError(string.Empty, "Ошибка при сохранении файла");
				}
			}
			else
				ModelState.AddModelError(string.Empty, "Файл не выбран");

			var model = new UserProfileModel { UserId = userId };
			FillUserProfileModel(model);
			return View("ProfileInfo", model);
		}

		[HttpPost]
		public ActionResult DeletePhoto(int userId)
		{
			var user = Execute(() => _accountService.GetUser(userId));
			var path = string.Concat(AppDomain.CurrentDomain.BaseDirectory, user.PhotoPath);

			Execute(() => _accountService.DeleteUserPhoto(userId));

			if (AppContext.CurrentUser.IsMe(userId))
				Execute(() => AppContext.CurrentUser = _accountService.GetUser(userId));

			if (ModelIsValid && System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
				return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(userId));
			}

			var model = new UserProfileModel { UserId = userId };
			FillUserProfileModel(model);
			return View("ProfileInfo", model);
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
					SexId = model.SexId,
					ShowUserEmail = model.ShowEmail
				};

				Execute(() => _accountService.UpdateMainInfo(data));

				if (AppContext.CurrentUser.IsMe(model.UserId))
					Execute(() => AppContext.CurrentUser = _accountService.GetUser(model.UserId));
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