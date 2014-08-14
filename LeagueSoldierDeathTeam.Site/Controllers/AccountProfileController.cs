using System;
using System.Drawing;
using System.Globalization;
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
		#region Private Fields

		private readonly IAccountService _accountService;

		#endregion

		#region Constructors

		public AccountProfileController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{
			_accountService = serviceFactory.CreateAccountService();
		}

		#endregion

		#region Actions

		#region Profile Info

		[Route("user-profile-info/{userId:int}")]
		public ActionResult ProfileInfo(int userId)
		{
			var model = new UserProfileModel { UserId = userId };
			FillUserProfileModel(model);

			if (ModelIsValid)
				return View(model);
			return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(CurrentUser.Id));
		}

		[HttpGet]
		public ActionResult EditPhoto()
		{
			return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(CurrentUser.Id));
		}

		[HttpPost]
		public ActionResult EditPhoto(int userId, HttpPostedFileBase photoUploadFile)
		{
			Private(userId, true);

			if (ModelIsValid)
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

							if (CurrentUser.IsMe(userId))
								Execute(() => CurrentUser = _accountService.GetUser(userId));

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
			}

			var model = new UserProfileModel { UserId = userId };
			FillUserProfileModel(model);
			return View("ProfileInfo", model);
		}

		[HttpGet]
		public ActionResult DeletePhoto()
		{
			return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(CurrentUser.Id));
		}

		[HttpPost]
		public ActionResult DeletePhoto(int userId)
		{
			Private(userId, true);

			if (ModelIsValid)
			{
				var user = Execute(() => _accountService.GetUser(userId));
				var path = string.Concat(AppDomain.CurrentDomain.BaseDirectory, user.PhotoPath);

				Execute(() => _accountService.DeleteUserPhoto(userId));

				if (CurrentUser.IsMe(userId))
					Execute(() => CurrentUser = _accountService.GetUser(userId));

				if (ModelIsValid && System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);
					return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(userId));
				}
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

			if (ModelIsValid)
				return View(model);
			return RedirectToAction<AccountProfileController>(o => o.ProfileInfo(CurrentUser.Id));
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

				if (CurrentUser.IsMe(model.UserId))
					Execute(() => CurrentUser = _accountService.GetUser(model.UserId));
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
					if (DateTime.TryParse(model.DateBirth, CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
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

		#region Message

		[Route("messages/{userId:int}")]
		public ActionResult Messages(int userId)
		{


			return View();
		}

		[Route("create-message/{userId:int}")]
		public ActionResult CreateMessage(int userId)
		{


			return View();
		}

		public ActionResult MessagesData(int userId)
		{
			return View();
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