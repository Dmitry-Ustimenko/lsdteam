using System.Linq;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.Site.Classes.Attributes;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
using LeagueSoldierDeathTeam.Site.Models.Administration;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class AdministrationController : BaseController
	{
		#region Private Fields

		private readonly IAccountService _accountService;

		#endregion

		#region Constructors

		public AdministrationController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{
			_accountService = serviceFactory.CreateAccountService();
		}

		#endregion

		#region Actions

		#region Index

		#region Administration

		[Route("administration")]
		[UserAuthorize(UserRoles = Role.Administrator)]
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

		public ActionResult DeleteUser(int userId)
		{
			var model = new UserEditModel();

			return PartialView("_UserEditPartial", model);
		}

		public ActionResult BanUser(int userId)
		{
			var model = new UserEditModel();

			return PartialView("_UserEditPartial", model);
		}

		public ActionResult ActivateUser(int userId)
		{
			var model = new UserEditModel();

			return PartialView("_UserEditPartial", model);
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