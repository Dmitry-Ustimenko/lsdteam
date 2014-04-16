using System;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class HomeController : Controller
	{
		private readonly ServiceFactoryBase _serviceFactory;

		public HomeController(ServiceFactoryBase serviceFactory)
		{
			if (serviceFactory == null)
				throw new ArgumentNullException();
			_serviceFactory = serviceFactory;
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}