using System;
using System.Web;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
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
			return View(new AdministrationModel());
		}

		#endregion

		#region Users

		[AjaxOrChildActionOnly]
		public ActionResult UsersData(UsersModel model)
		{
			FillUsersModel(model);
			return View(model);
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult DeleteUser(int? userId, UsersModel model)
		{
			if (userId.HasValue)
				Execute(() => _accountService.DeleteUser(userId.GetValueOrDefault()));

			FillUsersModel(model);

			return ModelIsValid
				? (ActionResult)View("UsersData", model)
				: JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult BanUser(int? userId, bool? isBanned, UsersModel model)
		{
			if (userId.HasValue && isBanned.HasValue)
				Execute(() => _accountService.BanUser(userId.GetValueOrDefault(), isBanned.Value));

			FillUsersModel(model);

			return ModelIsValid
				? (ActionResult)View("UsersData", model)
				: JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult SendMessageForActivate(int? userId, UsersModel model)
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

			FillUsersModel(model);

			return ModelIsValid
				? (ActionResult)View("UsersData", model)
				: JsonErrorResult();
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult ActivateUser(int? userId, UsersModel model)
		{
			if (userId.HasValue)
				Execute(() => _accountService.ActivateUser(userId.GetValueOrDefault()));

			FillUsersModel(model);

			return ModelIsValid
				? (ActionResult)View("UsersData", model)
				: JsonErrorResult();
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

		//#region Role Management

		//[HttpPost]
		//[AjaxOrChildActionOnly]
		//public ActionResult ChangeRole(int? roleId, int? userId)
		//{
		//	Execute(() => _accountService.UpdateRole(roleId.GetValueOrDefault(), userId.GetValueOrDefault()));

		//	return ModelIsValid
		//		? Json(new { Id = userId }, JsonRequestBehavior.AllowGet)
		//		: JsonErrorResult();
		//}

		//[HttpPost]
		//[AjaxOrChildActionOnly]
		//public ActionResult FilterRoles(string term)
		//{
		//	return View("_RoleManagementPartial", GetUsers(SortEnum.Default, term).Map());
		//}

		//#endregion

		#endregion

		#region Internal Implementation

		private void FillUsersModel(UsersModel model)
		{
			var users = Execute(() => _accountService.GetUsers(model.SortType, model.Term, model.Pager.PageId, model.Pager.PageSize));
			model.CopyFrom(users);
		}

		//private IEnumerable<UserData> GetUsers(SortEnum sortFilter, string term)
		//{
		//	var users = Execute(() => _accountService.GetUsers(sortFilter, term));

		//	return CurrentUser.IsMainAdmin
		//		? users.Where(o => o.RoleId != (int)Role.MainAdministrator)
		//		: users.Where(o => o.RoleId != (int)Role.MainAdministrator && o.RoleId != (int)Role.Administrator);
		//}

		#endregion
	}
}