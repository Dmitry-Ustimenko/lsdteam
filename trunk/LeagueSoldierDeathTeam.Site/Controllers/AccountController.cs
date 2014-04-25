using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
using LeagueSoldierDeathTeam.Site.Classes.Parsers.Xml;
using LeagueSoldierDeathTeam.Site.Models.Account;
using LeagueSoldierDeathTeam.Site.Models.Xml;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[AllowAnonymous]
	public class AccountController : BaseController
	{
		private readonly IAccountService _accountService;

		private readonly IAuthenticationService _authenticationService;

		private IAuthenticationManager AuthenticationManager { get { return HttpContextBase.GetOwinContext().Authentication; } }

		public AccountController(ServiceFactoryBase serviceFactory, IAuthenticationService authenticationService)
			: base(serviceFactory)
		{
			if (authenticationService == null)
				throw new ArgumentNullException("authenticationService");
			_authenticationService = authenticationService;
			_authenticationService.AuthenticationManager = AuthenticationManager;

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
				{
					model.ReturnUrl = WebBuilder.BuildActionUrl<AccountController>(o => o.RegisterSuccessfull(model.RegisterEmail));
					return Json(model, JsonRequestBehavior.AllowGet);
				}
			}
			return View("_RegisterPartial", model);
		}

		[HttpGet]
		[Route("register-successfully")]
		public ActionResult RegisterSuccessfull(string registerEmail)
		{
			if (string.IsNullOrWhiteSpace(registerEmail))
				return View();

			var index = registerEmail.IndexOf("@", StringComparison.Ordinal);
			if (index < 0)
				return View();

			var emailHost = registerEmail.Substring(index);
			if (string.IsNullOrWhiteSpace(emailHost))
				return View();

			var mailHostings = XmlParser<MailHosting>.Parse(Constants.XmlMailHostingPath, Constants.XmlMailHostingSearchName)
				.DistinctBy(o => o.HostAttribute).ToDictionary(o => o.HostAttribute, o => o.SiteAttribute);
			return View(new RegisterSuccessfullModel { MailHosting = mailHostings.ContainsKey(emailHost) ? mailHostings[emailHost] : string.Empty });
		}

		public ActionResult LogOff()
		{
			_authenticationService.SignOut();
			AppContext.CurrentUser = null;
			return RedirectToAction<HomeController>(o => o.Index());
		}

		#endregion

		#region Extarnal Login

		[HttpPost]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			return new ChallengeResult(provider, WebBuilder.BuildActionUrl<AccountController>(o => o.ExternalLoginCallback(returnUrl)));
		}

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
			return View("ExternalLoginConfirmation", new ExternalRegisterModel { UserName = loginInfo.DefaultUserName });
			//}
		}

		[HttpPost]
		public async Task<ActionResult> ExternalRegisterConfirmation(ExternalRegisterModel model, string returnUrl)
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

		#endregion

		#endregion
	}
}