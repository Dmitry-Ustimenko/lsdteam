using System;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
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

		#region Index

		#region Administration

		[Route("administration")]
		public ActionResult Index()
		{
			var users = Execute(() => _accountService.GetUsers());
			if (ModelIsValid)
			{
				var model = new AdministrationModel { UserEditModel = users.CopyTo() };
				return View(model);
			}
			return View(new AdministrationModel());
		}

		#endregion

		#region Users

		[HttpPost]
		public ActionResult DeleteUser(int? userId)
		{
			if (userId.HasValue)
				Execute(() => _accountService.DeleteUser(userId.GetValueOrDefault()));

			var users = Execute(() => _accountService.GetUsers());
			return ModelIsValid ? View("_UserEditPartial", users.CopyTo()) : View(new UserEditModel());
		}

		[HttpPost]
		public ActionResult BanUser(int? userId, bool? isBanned)
		{
			if (userId.HasValue)
				Execute(() => _accountService.BanUser(userId.GetValueOrDefault(), isBanned.GetValueOrDefault()));

			var users = Execute(() => _accountService.GetUsers());
			return ModelIsValid ? View("_UserEditPartial", users.CopyTo()) : View(new UserEditModel());
		}

		[HttpPost]
		public ActionResult SendMessageForActivate(int? userId)
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

			var users = Execute(() => _accountService.GetUsers());
			return users != null ? View("_UserEditPartial", users.CopyTo()) : View(new UserEditModel());
		}

		[HttpPost]
		public ActionResult ActivateUser(int? userId)
		{
			if (userId.HasValue)
				Execute(() => _accountService.ActivateUser(userId.GetValueOrDefault()));

			var users = Execute(() => _accountService.GetUsers());
			return users != null ? View("_UserEditPartial", users.CopyTo()) : View(new UserEditModel());
		}

		#endregion

		#region Role Management

		public ActionResult RoleManagement()
		{
			return View();
		}

		#endregion

		#endregion

		#endregion
	}
}