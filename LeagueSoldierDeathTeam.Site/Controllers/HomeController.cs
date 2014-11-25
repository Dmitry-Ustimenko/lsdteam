using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.Site.Classes.Attributes;
using LeagueSoldierDeathTeam.Site.Models.Home;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[AllowAnonymous]
	public class HomeController : BaseController
	{
		public HomeController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{ }

		#region Actions

		public ActionResult Index()
		{
			return View(new IndexModel());
		}

		[AjaxOrChildActionOnly]
		public ActionResult NewsData()
		{
			return View();
		}

		#endregion

		#region Internal Implementation

		#endregion
	}
}