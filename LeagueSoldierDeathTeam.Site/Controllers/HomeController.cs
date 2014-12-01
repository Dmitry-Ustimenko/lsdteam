using System.Collections.Generic;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Attributes;
using LeagueSoldierDeathTeam.Site.Models.Home;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[AllowAnonymous]
	public class HomeController : BaseController
	{
		#region Private Fields

		private readonly INewsService _newsService;

		#endregion

		#region Constructors

		public HomeController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{
			_newsService = serviceFactory.CreateNewsService();
		}

		#endregion

		#region Actions

		public ActionResult Index()
		{
			return View(new IndexModel());
		}

		[AjaxOrChildActionOnly]
		public ActionResult TopNewsData(int? newsSortId)
		{
			return View(GetNewsData(newsSortId.GetValueOrDefault()));
		}

		#endregion

		#region Internal Implementation

		private IEnumerable<NewsData> GetNewsData(int newsSortId)
		{
			var pageData = (Execute(() => _newsService.GetNews(null, null, newsSortId, 1, Constants.LastNewsPageSize)))
				?? new PageData<NewsData>();
			return pageData.Data;
		}

		#endregion
	}
}