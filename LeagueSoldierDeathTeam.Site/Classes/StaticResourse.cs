using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using LeagueSoldierDeathTeam.Site.Models;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public static class StaticResourse
	{
		public static string GetBackgroundImages()
		{
			var backgrounds = new List<BackgroundModel>();
			var path = string.Concat(AppDomain.CurrentDomain.BaseDirectory, Constants.BackgroundDirectoryPath);

			if (Directory.Exists(path))
			{
				var images = Directory.GetFiles(path, "*.jpg");
				backgrounds.AddRange(images.Select(image => new BackgroundModel
				{
					Src = string.Concat(Constants.BackgroundDirectoryPath, image.Substring(image.LastIndexOf('\\')))
				}));
				if (backgrounds.Any())
					backgrounds.Add(new BackgroundModel { Src = backgrounds[0].Src });
			}

			return Json.Encode(backgrounds);
		}
	}
}