using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
using LeagueSoldierDeathTeam.Site.Models.Account;
using Microsoft.Owin.Security;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[AllowAnonymous]
	public class AccountController : BaseController
	{
		private readonly IAccountService _accountService;

		private readonly IAuthenticationService _authenticationService;

		public AccountController(ServiceFactoryBase serviceFactory, IAuthenticationService authenticationService)
			: base(serviceFactory)
		{
			if (authenticationService == null)
				throw new ArgumentNullException("authenticationService");
			_authenticationService = authenticationService;
			_authenticationService.AuthenticationManager = HttpContextBase.GetOwinContext().Authentication;

			_accountService = ServiceFactory.CreateAccountService();
		}

		#region Actions

		#region Login

		[HttpPost]
		public ActionResult Login(LoginModel model)
		{
			if (ModelIsValid)
			{
				var user = Execute(() => _accountService.LogOn(model.Email, model.Password));
				if (ModelIsValid)
				{
					AppContext.CurrentUser = user;
					_authenticationService.SignIn(user.Email, model.RememberMe);
					return Json(model, JsonRequestBehavior.AllowGet);
				}
			}
			return View("_LoginPartial", model);
		}

		[HttpPost]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelIsValid)
			{
				var data = model.CopyTo();
				Execute(() => _accountService.Register(data));

				if (ModelIsValid)
					return Json(model, JsonRequestBehavior.AllowGet);
			}
			return View("_RegisterPartial", model);
		}

		public ActionResult LogOff()
		{
			_authenticationService.SignOut();
			AppContext.CurrentUser = null;
			return RedirectToAction<HomeController>(o => o.Index());
		}

		#endregion

		#region Extarnal Login

		//[HttpPost]
		//public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
		//{
		//	IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
		//	return RedirectToAction("Manage", new { Message = message });
		//}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}

		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
				return RedirectToAction<HomeController>(o => o.Index());

			// Sign in the user with this external login provider if the user already has a login
			//var user = await UserManager.FindAsync(loginInfo.Login);
			//if (user != null)
			//{
			//	await SignInAsync(user, isPersistent: false);
			//	return RedirectToAction(returnUrl);
			//}
			//else
			//{
			// If the user does not have an account, then prompt the user to create an account
			ViewBag.ReturnUrl = returnUrl;
			ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
			return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
			//}
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction<HomeController>(o => o.Index());

			if (ModelState.IsValid)
			{
				// Get the information about the user from the external login provider
				var info = await AuthenticationManager.GetExternalIdentityAsync("External");
				if (info == null)
				{
					return View("ExternalLoginFailure");
				}
				//var user = new ApplicationUser() { UserName = model.UserName };
				//var result = await UserManager.CreateAsync(user);
				//if (result.Succeeded)
				//{
				//	result = await UserManager.AddLoginAsync(user.Id, info.Login);
				//	if (result.Succeeded)
				//	{
				//		await SignInAsync(user, isPersistent: false);
				//		return RedirectToLocal(returnUrl);
				//	}
				//}
				//AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}

		//[ChildActionOnly]
		//public ActionResult RemoveAccountList()
		//{
		//	var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
		//	ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
		//	return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
		//}

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		//private async Task SignInAsync(ApplicationUser user, bool isPersistent)
		//{
		//	AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
		//	var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
		//	AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
		//}

		private const string XsrfKey = "XsrfId";

		private class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{ }

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			private string LoginProvider { get; set; }
			private string RedirectUri { get; set; }
			private string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}

		#endregion

		#endregion
	}
}