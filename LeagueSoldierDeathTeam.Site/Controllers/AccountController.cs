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
using LeagueSoldierDeathTeam.Site.Classes.Attributes;
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
		#region Private Fields

		private readonly IAccountService _accountService;

		private readonly IAuthenticationService _authenticationService;

		private readonly IMailer _mailer;

		private IAuthenticationManager AuthenticationManager { get { return HttpContextBase.GetOwinContext().Authentication; } }

		#endregion

		#region Constructors

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

		#endregion

		#region Actions

		#region Login

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult Login(LoginModel model)
		{
			if (ModelIsValid)
			{
				var user = Execute(() => _accountService.LogOn(model.Email, model.Password));
				if (ModelIsValid)
				{
					CurrentUser = user;
					_authenticationService.SignIn(user.Email, model.RememberMe);

					//model.ReturnUrl = Request.QueryString.AllKeys.Contains("ReturnUrl")
					//	? Request.QueryString["ReturnUrl"]
					//	: WebBuilder.BuildActionUrl<HomeController>(o => o.Index());

					return Json(model, JsonRequestBehavior.AllowGet);
				}
			}
			return View("_LoginPartial", model);
		}

		public ActionResult LogOff()
		{
			_authenticationService.SignOut();
			CurrentUser = null;
			return RedirectToAction<HomeController>(o => o.Index());
		}

		#endregion

		#region Register

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

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelIsValid)
			{
				var data = model.CopyTo();
				Execute(() => _accountService.Create(data));

				if (ModelIsValid)
				{
					var activateModel = new MailActivateModel
					{
						Token = Execute(() => _accountService.GetUserActivateToken(data.Email)),
						Email = data.Email,
						Password = data.Password
					};

					if (!string.IsNullOrEmpty(activateModel.Token))
						_mailer.SendMessageAsync("ActivateAccount", activateModel, model.RegisterEmail);

					model.ReturnUrl = WebBuilder.BuildActionUrl<AccountController>(o => o.RegisterSuccessfull(model.RegisterEmail));
					return Json(model, JsonRequestBehavior.AllowGet);
				}
			}
			return View("_RegisterPartial", model);
		}

		#endregion

		#region External Login

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			return new ChallengeResult(provider, WebBuilder.BuildActionUrl<AccountController>(o => o.ExternalLoginCallback(returnUrl, provider)));
		}

		public async Task<ActionResult> ExternalLoginCallback(string returnUrl, string provider)
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
				return View("ExternalLoginError", new ExternalLoginErrorModel
				{
					ProviderName = provider,
					Message = "Не удалось получить данные с сервиса. Попробуйте авторизироваться немного позднее."
				});


			var externalUser = Execute(() => _accountService.GetExternalUser(loginInfo.Login.LoginProvider, loginInfo.Login.ProviderKey));
			if (externalUser != null)
			{
				var user = Execute(() => _accountService.GetUser(externalUser.UserId));
				if (user != null)
				{
					if (!user.IsActive)
						return View("ExternalLoginError", new ExternalLoginErrorModel
						{
							ProviderName = provider,
							Message = "Данный аккаунт не был активирован. Пожалуйста активируйте его по ссылке, которая была выслана на указанный при регистрации e-mail."
						});

					CurrentUser = user;
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
				{
					var activateModel = new MailActivateModel
					{
						Token = Execute(() => _accountService.GetUserActivateToken(data.Email)),
						Email = data.Email,
						Password = data.Password
					};

					if (string.IsNullOrWhiteSpace(model.ExternalPassword))
						activateModel.ProviderName = model.ProviderName;

					if (!string.IsNullOrEmpty(activateModel.Token))
						_mailer.SendMessageAsync("ActivateAccount", activateModel, model.ExternalEmail);

					return RedirectToAction<AccountController>(o => o.RegisterSuccessfull(model.ExternalEmail));
				}
			}

			return View(model);
		}

		#endregion

		#region Password Recovery

		[HttpGet]
		[Route("password-recovery")]
		public ActionResult PasswordRecovery()
		{
			return View(new PasswordRecoveryModel());
		}

		[HttpPost]
		[Route("password-recovery")]
		public ActionResult PasswordRecovery(PasswordRecoveryModel model)
		{
			if (ModelIsValid)
			{
				var resetModel = new MailTokenModel { Token = Execute(() => _accountService.GetUserResetToken(model.RecoveryEmail)) };
				if (!string.IsNullOrEmpty(resetModel.Token))
					_mailer.SendMessageAsync("PasswordReset", resetModel, model.RecoveryEmail);

				return View(new PasswordRecoveryModel { EmailWasSend = true });
			}
			return View(new PasswordRecoveryModel());
		}

		#endregion

		#region Password Reset

		[HttpGet]
		[Route("password-reset/{token?}")]
		public ActionResult PasswordReset(string token)
		{
			if (!Execute(() => _accountService.VerifyUserResetToken(token)))
				return RedirectToAction<AccountController>(o => o.PasswordRecovery());
			return View(new PasswordResetModel { Token = token });
		}

		[HttpPost]
		[Route("password-reset/{token?}")]
		public ActionResult PasswordReset(PasswordResetModel model)
		{
			if (ModelIsValid)
			{
				if (!Execute(() => _accountService.VerifyUserResetToken(model.Token)))
					return RedirectToAction<AccountController>(o => o.PasswordRecovery());

				var parameters = new PasswordResetParams
				{
					PasswordResetToken = model.Token,
					NewPassword = model.NewPassword,
					SessionId = Session.SessionID,
					UserIp = Request.UserHostAddress,
					UserHostName = Request.UserHostName
				};

				if (Execute(() => _accountService.PasswordReset(parameters)))
					return View(new PasswordResetModel { PasswordWasChanged = true });
			}
			return View(model);
		}

		#endregion

		#region Activate Account

		[HttpGet]
		[Route("activate-account/{token?}")]
		public ActionResult ActivateAccount(string token)
		{
			if (Execute(() => _accountService.ActivateAccount(token)))
				return View(new ActivateAccountModel { AccountWasActivated = true });
			return View(new ActivateAccountModel());
		}

		#endregion

		#endregion
	}
}