using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class NewsController : BaseController
	{
		#region Constructors

		public NewsController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{ }

		#endregion

		#region Actions

		[Route("news")]
		public ActionResult Index()
		{
			return View();
		}

		#endregion
	}
}