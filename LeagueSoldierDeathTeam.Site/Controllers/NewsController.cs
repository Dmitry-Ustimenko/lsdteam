using System.Collections.Generic;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Models.News;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class NewsController : BaseController
	{
		#region Private Fields

		private readonly INewsService _newsService;

		private readonly IResourceService _resourceService;

		#endregion

		#region Constructors

		public NewsController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{
			_newsService = ServiceFactory.CreateNewsService();
			_resourceService = serviceFactory.CreateResourceService();
		}

		#endregion

		#region Actions

		[Route("news")]
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult ViewNews()
		{
			return View();
		}

		[HttpGet]
		public ActionResult CreateNews()
		{
			var model = new EditNewsModel();
			FillResourceEditNewsModel(model);

			return View("EditNews", model);
		}

		[HttpGet]
		public ActionResult EditNews(int id)
		{
			var model = new EditNewsModel { Id = id };
			FillEditNewsModel(model);
			FillResourceEditNewsModel(model);

			return View(model);
		}

		[HttpPost]
		public ActionResult EditNews(EditNewsModel model)
		{
			if (ModelIsValid)
			{

				return RedirectToAction<NewsController>(o => o.Index());
			}

			FillResourceEditNewsModel(model);

			return View(model);
		}

		#endregion

		#region Internal Implementation

		private void FillEditNewsModel(EditNewsModel model)
		{

		}

		private void FillResourceEditNewsModel(EditNewsModel model)
		{
			model.NewsCategories = Execute(() => _newsService.GetNewsGategories()) ?? new List<NewsCategoryData>();
			model.Platforms = Execute(() => _resourceService.GetPlatforms()) ?? new List<PlatformData>();
		}

		#endregion
	}
}