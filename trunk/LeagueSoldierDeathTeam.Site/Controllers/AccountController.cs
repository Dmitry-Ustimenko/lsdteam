using System;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
using LeagueSoldierDeathTeam.Site.Models.Account;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
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

			_accountService = ServiceFactory.CreateAccountService();
		}

		#region Actions

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
	}
}