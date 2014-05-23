using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;

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