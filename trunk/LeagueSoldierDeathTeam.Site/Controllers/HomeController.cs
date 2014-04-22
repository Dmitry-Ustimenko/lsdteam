using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class HomeController : BaseController
	{
		public HomeController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{ }

		public ActionResult Index()
		{
			return View();
		}

		[Route("forum")]
		public ActionResult Forum()
		{
			return View();
		}
	}
}