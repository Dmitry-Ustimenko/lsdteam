using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.Site.Classes.Attributes;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class AdministrationController : BaseController
	{
		#region Constructors

		public AdministrationController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{ }

		#endregion

		#region Actions

		#region Index

		[Route("administration")]
		[UserAuthorize(UserRoles = Role.Administrator)]
		public ActionResult Index()
		{
			return View();
		}

		#endregion

		#region Role Management

		public ActionResult RoleManagement()
		{
			return View();
		}

		#endregion

		#endregion
	}
}