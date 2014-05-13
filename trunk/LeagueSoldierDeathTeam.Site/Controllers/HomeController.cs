﻿using System.Web.Mvc;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Models.Home;

namespace LeagueSoldierDeathTeam.Site.Controllers
{
	public class HomeController : BaseController
	{
		public HomeController(ServiceFactoryBase serviceFactory)
			: base(serviceFactory)
		{ }

		#region Actions

		public ActionResult Index()
		{
			return View(new IndexModel());
		}

		[Route("forum")]
		public ActionResult Forum()
		{
			return View();
		}

		#endregion

		#region Internal Implementation

		#endregion
	}
}