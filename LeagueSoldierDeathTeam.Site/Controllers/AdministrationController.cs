using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services;
using LeagueSoldierDeathTeam.Site.Classes.Attributes;
using LeagueSoldierDeathTeam.Site.Classes.Extensions;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
using LeagueSoldierDeathTeam.Site.Models.Administration;
using LeagueSoldierDeathTeam.Site.Models.Mail;
using Microsoft.Owin.Security;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[UserAuthorize(UserRoles = Role.Administrator)]
	public class AdministrationController : BaseController
	{
		#region Private Fields

		private readonly IAccountService _accountService;

		private readonly IMailer _mailer;

		private readonly IAuthenticationService _authenticationService;

		private IAuthenticationManager AuthenticationManager { get { return HttpContextBase.GetOwinContext().Authentication; } }

		#endregion

		#region Constructors

		public AdministrationController(ServiceFactoryBase serviceFactory, IAuthenticationService authenticationService, IMailer mailer)
			: base(serviceFactory)
		{
			_accountService = serviceFactory.CreateAccountService();

			if (mailer == null)
				throw new ArgumentNullException("mailer");
			_mailer = mailer;

			if (authenticationService == null)
				throw new ArgumentNullException("authenticationService");
			_authenticationService = authenticationService;
			_authenticationService.AuthenticationManager = AuthenticationManager;
		}

		#endregion

		#region Actions

		#region Administration

		[Route("administration")]
		public ActionResult Index()
		{
			var users = GetUsers(SortEnum.Default, null).ToList();
			if (ModelIsValid)
			{
				var model = new AdministrationModel
				{
					UsersModel = users.CopyTo(),
					RoleManagementModel = users.Map()
				};
				return View(model);
			}
			return View(new AdministrationModel());
		}

		#endregion

		#region Users

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult DeleteUser(int? userId, SortEnum sortFilter, string term)
		{
			if (userId.HasValue)
				Execute(() => _accountService.DeleteUser(userId.GetValueOrDefault()));

			return ModelIsValid
				? (ActionResult)View("_UsersPartial", GetUsers(sortFilter, term).CopyTo())
				: JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult BanUser(int? userId, bool? isBanned, SortEnum sortFilter, string term)
		{
			if (userId.HasValue && isBanned.HasValue)
				Execute(() => _accountService.BanUser(userId.GetValueOrDefault(), isBanned.Value));

			return ModelIsValid
				? (ActionResult)View("_UsersPartial", GetUsers(sortFilter, term).CopyTo())
				: JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult SendMessageForActivate(int? userId, SortEnum sortFilter, string term)
		{
			if (userId.HasValue)
			{
				var user = Execute(() => _accountService.GetUser(userId.GetValueOrDefault()));

				if (ModelIsValid)
				{
					var activateModel = new MailActivateModel
					{
						Token = Execute(() => _accountService.GetUserActivateToken(user.Email)),
						Email = user.Email,
						Password = StringGeneration.Generate(8)
					};
					Execute(() => _accountService.SetPassword(user.Id, activateModel.Password));

					if (ModelIsValid && !string.IsNullOrEmpty(activateModel.Token))
						_mailer.SendMessageAsync("ActivateAccount", activateModel, user.Email);
				}
			}

			return ModelIsValid
				? (ActionResult)View("_UsersPartial", GetUsers(sortFilter, term).CopyTo())
				: JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult ActivateUser(int? userId, SortEnum sortFilter, string term)
		{
			if (userId.HasValue)
				Execute(() => _accountService.ActivateUser(userId.GetValueOrDefault()));

			return ModelIsValid
				? (ActionResult)View("_UsersPartial", GetUsers(sortFilter, term).CopyTo())
				: JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult FilterUsers(SortEnum sortFilter, string term)
		{
			return View("_UsersPartial", GetUsers(sortFilter, term).CopyTo());
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult ChangePage(SortEnum sortFilter, string term, int? pageId)
		{
			return View("_UsersPartial", GetUsers(sortFilter, term).CopyTo(pageId.GetValueOrDefault()));
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult LoginAs(int? userId)
		{
			if (CurrentUser.IsMainAdmin && userId.HasValue)
			{
				var user = _accountService.GetUser(userId.GetValueOrDefault());
				if (ModelIsValid && user != null && user.IsActive)
				{
					_authenticationService.SignOut();

					CurrentUser = user;
					_authenticationService.SignIn(user.Email, false);

					return RedirectToAction<HomeController>(o => o.Index());
				}
			}
			return RedirectToAction<AdministrationController>(o => o.Index());
		}

		#endregion

		#region Role Management

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult ChangeRole(int? roleId, int? userId)
		{
			Execute(() => _accountService.UpdateRole(roleId.GetValueOrDefault(), userId.GetValueOrDefault()));

			return ModelIsValid
				? Json(new { Id = userId }, JsonRequestBehavior.AllowGet)
				: JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult FilterRoles(string term)
		{
			return View("_RoleManagementPartial", GetUsers(SortEnum.Default, term).Map());
		}

		#endregion

		#endregion

		#region Internal Implementation

		private IEnumerable<UserData> GetUsers(SortEnum sortFilter, string term)
		{
			var users = Execute(() => _accountService.GetUsers(sortFilter, term));

			return CurrentUser.IsMainAdmin
				? users.Where(o => o.RoleId != (int)Role.MainAdministrator)
				: users.Where(o => o.RoleId != (int)Role.MainAdministrator && o.RoleId != (int)Role.Administrator);
		}

		#endregion
	}
}