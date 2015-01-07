using LeagueSoldierDeathTeam.Business.Dto;
using LeagueSoldierDeathTeam.Site.Models;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class BasePagerModelEx
	{
		public static BasePagerModel<T> CopyFrom<T>(this BasePagerModel<T> model, PageData<T> data)
			where T : class
		{
			model.Pager.PageId = data.PageId;
			model.Pager.Count = data.Count;
			model.Data = data.Data;

			return model;
		}
	}
}