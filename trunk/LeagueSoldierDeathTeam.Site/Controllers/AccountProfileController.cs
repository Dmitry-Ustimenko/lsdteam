using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[Authorize]
	public class AccountProfileController : BaseController
	{
		public AccountProfileController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{ }

		#region Actions

		[Route("user-profile")]
		public ActionResult Index()
		{
			return View();
		}

		[Route("change-password")]
		public ActionResult ChangePassword()
		{
			return View();
		}

		#endregion
	}
}