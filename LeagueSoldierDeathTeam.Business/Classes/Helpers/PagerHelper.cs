using System.Linq;

namespace LeagueSoldierDeathTeam.Business.Classes.Helpers
{
	public static class PagerHelper
	{
		public static IQueryable<T> Page<T>(this IQueryable<T> items, int pageId, int pageSize)
		{
			return pageId != 0
				? items.Skip((pageId - 1) * pageSize).Take(pageSize)
				: items.Skip(pageId * pageSize).Take(pageSize);
		}
	}
}
