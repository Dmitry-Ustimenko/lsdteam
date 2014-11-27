using System.Collections.Generic;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
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
		[Route("create-news")]
		public ActionResult CreateNews()
		{
			var model = new EditNewsModel();
			FillResourceEditNewsModel(model);

			return View("EditNews", model);
		}

		[HttpGet]
		[Route("edit-news/{id:int?}")]
		public ActionResult EditNews(int? id)
		{
			if (id.HasValue)
			{
				var model = new EditNewsModel { Id = id.GetValueOrDefault() };

				var news = Execute(() => _newsService.GetNews(model.Id.GetValueOrDefault()));
				if (news == null)
					return RedirectToAction<NewsController>(o => o.Index());

				model.CopyFrom(news);
				FillResourceEditNewsModel(model);

				return View(model);
			}

			return RedirectToAction<NewsController>(o => o.CreateNews());
		}

		[HttpPost]
		[Route("edit-news/{id:int?}")]
		public ActionResult EditNews(EditNewsModel model)
		{
			if (ModelIsValid)
			{
				model.PlatformIds.Clear();

				if (!string.IsNullOrWhiteSpace(model.HiddenPlatformIds))
				{
					var platformIds = model.HiddenPlatformIds.Split(',');
					foreach (var platformId in platformIds)
					{
						int id;
						if (int.TryParse(platformId, out id))
							model.PlatformIds.Add(id);
					}
				}

				var data = new NewsData
				{
					Id = model.Id.GetValueOrDefault(),
					Description = model.Description,
					Title = model.Title,
					NewsCategoryId = model.NewsCategoryId,
					WriterId = CurrentUser.Id,
					PlatformIds = model.PlatformIds
				};

				Execute(() => _newsService.SaveNews(data));

				if (ModelIsValid)
					return RedirectToAction<NewsController>(o => o.Index());
			}

			FillResourceEditNewsModel(model);

			return View(model);
		}

		#endregion

		#region Internal Implementation

		private void FillResourceEditNewsModel(EditNewsModel model)
		{
			model.NewsCategories = Execute(() => _newsService.GetNewsGategories()) ?? new List<NewsCategoryData>();
			model.Platforms = Execute(() => _resourceService.GetPlatforms()) ?? new List<PlatformData>();
		}

		#endregion
	}
}