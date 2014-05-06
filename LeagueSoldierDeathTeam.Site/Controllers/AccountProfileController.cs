using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
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
		[Route("edit-user-profile/{userId:int}")]
		public ActionResult EditProfile(EditProfileModel model)
		{
			return View(model);
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