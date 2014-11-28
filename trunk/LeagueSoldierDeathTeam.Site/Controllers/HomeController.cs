using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Classes;
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
			var model = new IndexModel();
			FillIndexModel(model);
			return View(model);
		}
		#endregion

		#region Internal Implementation

		private void FillIndexModel(IndexModel model)
		{
			var pageData = (Execute(() => _newsService.GetNews(null, null, (int)NewsSort.Date, 150, 1, Constants.LastNewsPageSize)))
				?? new PageData<NewsData>();
			model.NewsData = pageData.Data;
		}

		#endregion
	}
}