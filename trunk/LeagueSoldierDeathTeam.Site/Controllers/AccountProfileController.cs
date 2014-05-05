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

		[Route("user-profile-info/{userId:int}")]
		public ActionResult ProfileInfo(int userId)
		{
			var model = new UserProfileModel { UserId = userId };
			FillUserProfileModel(model);
			return View(model);
		}

		[Route("edit-user-profile/{userId:int}")]
		public ActionResult EditProfile(int userId)
		{
			return View();
		}

		[Route("change-password")]
		public ActionResult ChangePassword()
		{
			return View();
		}

		#endregion

		#region Internal Implementation

		private void FillUserProfileModel(UserProfileModel model)
		{
			var userProfile = Execute(() => _accountService.GetUserProfile(model.UserId));
			model.CopyFrom(userProfile);
		}

		#endregion
	}
}