using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Attributes;
using LeagueSoldierDeathTeam.Site.Classes.Extensions;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
using LeagueSoldierDeathTeam.Site.Models.Administration;
using LeagueSoldierDeathTeam.Site.Models.Mail;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[UserAuthorize(UserRoles = Role.Administrator)]
	public class AdministrationController : BaseController
	{
		#region Private Fields

		private readonly IAccountService _accountService;

		private readonly IMailer _mailer;

		#endregion

		#region Constructors

		public AdministrationController(ServiceFactoryBase serviceFactory, IMailer mailer)
			: base(serviceFactory)
		{
			_accountService = serviceFactory.CreateAccountService();

			if (mailer == null)
				throw new ArgumentNullException("mailer");
			_mailer = mailer;
		}

		#endregion

		#region Actions

		#region Administration

		[Route("administration")]
		public ActionResult Index()
		{
			var users = GetUsers(SortEnum.Default, null);
			if (ModelIsValid)
			{
				var model = new AdministrationModel
				{
					UserEditModel = users.CopyTo(),
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

			return View("_UserEditPartial", GetUsers(sortFilter, term).CopyTo());
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult BanUser(int? userId, bool? isBanned, SortEnum sortFilter, string term)
		{
			if (userId.HasValue && isBanned.HasValue)
				Execute(() => _accountService.BanUser(userId.GetValueOrDefault(), isBanned.Value));

			return View("_UserEditPartial", GetUsers(sortFilter, term).CopyTo());
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

			return View("_UserEditPartial", GetUsers(sortFilter, term).CopyTo());
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult ActivateUser(int? userId, SortEnum sortFilter, string term)
		{
			if (userId.HasValue)
				Execute(() => _accountService.ActivateUser(userId.GetValueOrDefault()));

			return View("_UserEditPartial", GetUsers(sortFilter, term).CopyTo());
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult FilterUsers(SortEnum sortFilter, string term)
		{
			return View("_UserEditPartial", GetUsers(sortFilter, term).CopyTo());
		}

		#endregion

		#region Role Management

		public ActionResult RoleManagement()
		{
			return View();
		}

		#endregion

		#endregion

		#region Internal Implementation

		private IEnumerable<UserData> GetUsers(SortEnum sortFilter, string term)
		{
			var users = Execute(() => _accountService.GetUsers(sortFilter, term));

			return AppContext.CurrentUser.IsMainAdmin
				? users.Where(o => o.RoleId != (int)Role.MainAdministrator)
				: users.Where(o => o.RoleId != (int)Role.MainAdministrator && o.RoleId != (int)Role.Administrator);
		}

		#endregion
	}
}