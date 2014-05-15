using System.Web.Mvc;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class ErrorController : Controller
	{
		[Route("error/{httpCode:int}/{message?}")]
		public ActionResult Error(int httpCode, string message)
		{
			ViewBag.Message = httpCode == 404 ? "Страница не найдена" : message;
			ViewBag.HttpCode = httpCode;
			return View();
		}
	}
}