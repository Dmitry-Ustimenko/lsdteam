using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.Services;
using LeagueSoldierDeathTeam.BusinessLogic.Classes.Enums;
using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Classes.Attributes;
using LeagueSoldierDeathTeam.Site.Classes.Extensions;
using LeagueSoldierDeathTeam.Site.Classes.Extensions.Models;
using LeagueSoldierDeathTeam.Site.Models;
using LeagueSoldierDeathTeam.Site.Models.News;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	[AllowAnonymous]
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

		[HttpGet]
		[Route("news")]
		public ActionResult News()
		{
			var model = new NewsModel();
			FillResourceNewsModel(model);
			return View(model);
		}

		[HttpGet]
		[Route("news/{id?}")]
		public ActionResult NewsFilter(string id)
		{
			var model = new NewsModel();
			FillResourceNewsModel(model);
			FillFilterParams(model, id);

			return View("News", model);
		}

		[AjaxOrChildActionOnly]
		[Route("news-data")]
		public ActionResult NewsData(NewsModel model)
		{
			FillNewsModel(model);
			return View(model);
		}

		[Route("view-news/{id:int?}")]
		public ActionResult ViewNews(int? id)
		{
			if (id.HasValue)
			{
				var model = new ViewNewsModel { Id = id.GetValueOrDefault() };

				var news = Execute(() => _newsService.GetNews(model.Id.GetValueOrDefault()));
				if (news == null)
					return RedirectToAction<NewsController>(o => o.News());

				//Execute(() => _newsService.ChangeCountViews(news.Id));
				model.CopyFrom(news);

				return View(model);
			}

			return RedirectToAction<NewsController>(o => o.News());
		}

		[AjaxOrChildActionOnly]
		[Route("news-comment-data")]
		public ActionResult NewsCommentData(int? id)
		{
			var model = new CommentModel
			{
				ContentId = id.GetValueOrDefault()
			};
			FillCommentModel(model);
			return View(model);
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		[Route("add-news-comment")]
		public ActionResult AddNewComment(CommentModel model)
		{
			var comment = new CommentData
			{
				CreateDate = DateTime.UtcNow,
				Description = model.CommentDescription,
				ModifierDate = DateTime.UtcNow,
				Rate = default(int),
				Writer = new UserData { Id = CurrentUser.Id }
			};

			Execute(() => _newsService.AddNewsComment(model.ContentId, comment));

			FillCommentModel(model);

			return View("NewsCommentData", model);
		}

		[HttpPost]
		[AjaxOrChildActionOnly]
		public ActionResult DeleteNews(int? id)
		{
			if (id.HasValue)
			{


				return RedirectToAction<NewsController>(o => o.News());
			}

			return RedirectToAction<NewsController>(o => o.News());
		}

		[Route("create-news")]
		[UserAuthorize(UserRoles = Role.Administrator | Role.Moderator)]
		public ActionResult CreateNews()
		{
			var model = new EditNewsModel();
			FillResourceEditNewsModel(model);

			return View("EditNews", model);
		}

		[HttpGet]
		[Route("edit-news/{id:int?}")]
		[UserAuthorize(UserRoles = Role.Administrator | Role.Moderator)]
		public ActionResult EditNews(int? id)
		{
			if (id.HasValue)
			{
				var model = new EditNewsModel { Id = id.GetValueOrDefault() };

				var news = Execute(() => _newsService.GetNews(model.Id.GetValueOrDefault()));
				if (news == null)
					return RedirectToAction<NewsController>(o => o.News());

				model.CopyFrom(news);
				FillResourceEditNewsModel(model);

				return View(model);
			}

			return RedirectToAction<NewsController>(o => o.CreateNews());
		}

		[HttpPost]
		[Route("edit-news/{id:int?}")]
		[UserAuthorize(UserRoles = Role.Administrator | Role.Moderator)]
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

				ValidateUploadFile(model);

				if (ModelIsValid)
				{
					var data = new NewsData
					{
						Id = model.Id.GetValueOrDefault(),
						Description = model.Description,
						Title = model.Title,
						NewsCategory = new NewsCategoryData { Id = model.NewsCategoryId },
						WriterId = CurrentUser.Id,
						PlatformIds = model.PlatformIds,
						ImagePath = model.ImagePath,
						Annotation = model.Annotation
					};

					Execute(() => _newsService.SaveNews(data));

					if (ModelIsValid)
						return RedirectToAction<NewsController>(o => o.News());
				}
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

		private void FillResourceNewsModel(NewsModel model)
		{
			model.NewsCategories = Execute(() => _newsService.GetNewsGategories()) ?? new List<NewsCategoryData>();
			model.Platforms = Execute(() => _resourceService.GetPlatforms()) ?? new List<PlatformData>();
		}

		private void FillNewsModel(NewsModel model)
		{
			var pagerData = (Execute(() => _newsService.GetNews(model.NewsCategoryId, model.PlatformId, model.SortId, model.Pager.PageId, model.Pager.PageSize))
				?? new PageData<NewsData>());
			model.CopyFrom(pagerData);
		}

		private void FillCommentModel(CommentModel model)
		{
			model.Data = Execute(() => _newsService.GetNewsComments(model.ContentId)) ?? new List<CommentData>();
		}

		private void ValidateUploadFile(EditNewsModel model)
		{
			if (model.ImageUploadFile != null && model.ImageUploadFile.FileName != null)
			{
				try
				{
					if (model.ImageUploadFile.ContentLength > 102400)
						ModelState.AddModelError(string.Empty, "Размер файла не выше 100кб");

					if (!Constants.AcceptImage.Contains(model.ImageUploadFile.ContentType))
						ModelState.AddModelError(string.Empty, "Требуемые форматы: *.jpg, *.jpeg, *.png, *.gif");

					if (ModelIsValid)
					{
						var image = Image.FromStream(model.ImageUploadFile.InputStream);
						if (image.Width > 150 || image.Height > 150)
							ModelState.AddModelError(string.Empty, "Разрешение фото не должно превышать 150х150");
					}

					if (ModelIsValid)
					{
						var fileName = string.Concat(StringGeneration.Generate(20), Path.GetExtension(model.ImageUploadFile.FileName));
						var path = Path.Combine(Server.MapPath(Constants.NewsPreviewsPath), Path.GetFileName(fileName));
						while (System.IO.File.Exists(path))
						{
							fileName = string.Concat(StringGeneration.Generate(20), Path.GetExtension(model.ImageUploadFile.FileName));
						}

						model.ImageUploadFile.SaveAs(path);

						if (!string.IsNullOrWhiteSpace(model.ImagePath) && System.IO.File.Exists(model.ImagePath))
							System.IO.File.Delete(model.ImagePath);

						model.ImagePath = string.Concat(Constants.NewsPreviewsPath, fileName);
					}
				}
				catch (Exception)
				{
					ModelState.AddModelError(string.Empty, "Ошибка при сохранении файла");
				}
			}
		}

		private static void FillFilterParams(NewsModel model, string shortId)
		{
			if (string.IsNullOrWhiteSpace(shortId))
				return;

			var equalsPlatforms = model.Platforms.Where(platform => String.Equals(platform.ShortName, shortId, StringComparison.OrdinalIgnoreCase)).ToList();
			if (equalsPlatforms.Any())
			{
				model.PlatformId = equalsPlatforms.First().Id;
			}
			else
			{
				var equalsCategories = model.NewsCategories.Where(category => String.Equals(category.ShortName, shortId, StringComparison.OrdinalIgnoreCase)).ToList();
				if (equalsCategories.Any())
				{
					model.NewsCategoryId = equalsCategories.First().Id;
				}
			}
		}

		#endregion
	}
}