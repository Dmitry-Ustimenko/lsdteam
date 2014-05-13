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
			var images = GetImages(Constants.BackgroundDirectoryPath).ToList();
			if (images.Any())
				images.Add(new ImageModel { Src = images[0].Src });

			return Json.Encode(images);
		}

		public static IEnumerable<ImageModel> GetSliderImages()
		{
			return GetImages(Constants.SliderDirectoryPath);
		}

		private static IEnumerable<ImageModel> GetImages(string folderPath)
		{
			var images = new List<ImageModel>();
			var path = string.Concat(AppDomain.CurrentDomain.BaseDirectory, folderPath);

			if (Directory.Exists(path))
			{
				var files = Directory.GetFiles(path, "*.jpg");
				images.AddRange(files.Select(image => new ImageModel
				{
					Src = string.Concat(folderPath, image.Substring(image.LastIndexOf('\\')))
				}));
			}

			return images;
		}
	}
}