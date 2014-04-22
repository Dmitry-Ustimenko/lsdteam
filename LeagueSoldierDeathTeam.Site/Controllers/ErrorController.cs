using System;
using System.Web.Mvc;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class ErrorController : Controller
	{
		[Route("error/{id}")]
		public ActionResult Error(string id)
		{
			var model = new HandleErrorInfo(new Exception(id), "Error", "Error");
			return View(model);
		}
	}
}