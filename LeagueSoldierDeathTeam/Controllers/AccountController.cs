using System.Web.Mvc;
using LeagueSoldierDeathTeam.Models.Account;

namespace LeagueSoldierDeathTeam.Controllers
{
	public class AccountController : Controller
	{
		[HttpGet]
		public ActionResult LogOn()
		{
			return View();
		}

		[HttpPost]
		public ActionResult LogOn(LogOnModel model)
		{
			return View();
		}
	}
}