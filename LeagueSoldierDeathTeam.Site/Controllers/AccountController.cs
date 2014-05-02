using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Services.Parameters;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
using LeagueSoldierDeathTeam.Site.Classes.Parsers.Xml;
using LeagueSoldierDeathTeam.Site.Models.Account;
using LeagueSoldierDeathTeam.Site.Models.Mail;
using LeagueSoldierDeathTeam.Site.Models.Xml;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security;
using Constants = LeagueSoldierDeathTeam.Site.Classes.Constants;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[AllowAnonymous]
	public class AccountController : BaseController
	{
		private readonly IAccountService _accountService;

		private readonly IAuthenticationService _authenticationService;

		private readonly IMailer _mailer;

		private IAuthenticationManager AuthenticationManager { get { return HttpContextBase.GetOwinContext().Authentication; } }

		public AccountController(ServiceFactoryBase serviceFactory, IAuthenticationService authenticationService, IMailer mailer)
			: base(serviceFactory)
		{
			if (mailer == null)
				throw new ArgumentNullException("mailer");
			_mailer = mailer;

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
				Execute(() => _accountService.Create(data));

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

		#region External Login

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

			var externalUser = Execute(() => _accountService.GetExternalUser(loginInfo.Login.LoginProvider, loginInfo.Login.ProviderKey));
			if (externalUser != null)
			{
				var user = Execute(() => _accountService.GetUser(externalUser.Id));
				if (user != null)
				{
					AppContext.CurrentUser = user;
					_authenticationService.SignIn(user.Email, true);
					return RedirectToAction<HomeController>(o => o.Index());
				}
			}

			return View("ExternalRegisterConfirmation", new ExternalRegisterModel
			{
				ExternalUserName = loginInfo.DefaultUserName,
				ExternalEmail = loginInfo.Email,
				ProviderName = loginInfo.Login.LoginProvider,
				ProviderKey = loginInfo.Login.ProviderKey
			});
		}

		[HttpPost]
		[Route("external-register-confirmation")]
		public ActionResult ExternalRegisterConfirmation(ExternalRegisterModel model)
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction<HomeController>(o => o.Index());

			if (ModelState.IsValid)
			{
				var data = model.CopyTo();
				Execute(() => _accountService.Create(data, true));

				if (ModelIsValid)
					return RedirectToAction<AccountController>(o => o.RegisterSuccessfull(model.ExternalEmail));
			}

			return View(model);
		}

		#endregion

		#region Password Recovery

		[HttpGet]
		[AllowAnonymous]
		[Route("password-recovery")]
		public ActionResult ForgotPassword()
		{
			return View(new PasswordRecoveryModel());
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("password-recovery")]
		public ActionResult ForgotPassword(PasswordRecoveryModel model)
		{
			if (ModelState.IsValid)
			{
				var resetModel = new MailPasswordResetModel
				{
					Token = Execute(() => _accountService.GetResetPasswordToken(model.Email)),
					Email = model.Email,
				};
				if (!string.IsNullOrEmpty(resetModel.Token))
					_mailer.SendMessageAsync("PasswordReset", resetModel, model.Email);

				return View(new PasswordRecoveryModel { EmailWasSend = true });
			}
			return View(new PasswordRecoveryModel());
		}

		#endregion

		#region Password Reset

		[HttpGet]
		[AllowAnonymous]
		[Route("password-reset/{token?}")]
		public ActionResult PasswordReset(string token)
		{
			if (!Execute(() => _accountService.VerifyPasswordResetToken(token)))
				return RedirectToAction<AccountController>(o => o.ForgotPassword());
			return View(new PasswordResetModel { Token = token });
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("password-reset/{token?}")]
		public ActionResult PasswordReset(PasswordResetModel model)
		{
			if (!Execute(() => _accountService.VerifyPasswordResetToken(model.Token)))
				return RedirectToAction<AccountController>(o => o.ForgotPassword());

			var resetContext = new PasswordResetParams
			{
				PasswordResetToken = model.Token,
				NewPassword = model.NewPassword,
				SessionId = Session.SessionID,
				UserIp = Request.UserHostAddress,
				UserHostName = Request.UserHostName
			};

			if (Execute(() => _accountService.PasswordReset(resetContext)))
				return View(new PasswordResetModel { PasswordWasChanged = true });
			return View(model);
		}

		#endregion

		#endregion
	}
}