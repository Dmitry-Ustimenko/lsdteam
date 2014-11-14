using System.Collections.Generic;
using System.Linq;

namespace LeagueSoldierDeathTeam.Site.Classes.Helpers
{
	public static class PagerHelper
	{
		public static IEnumerable<T> Page<T>(this IEnumerable<T> items, int pageId, int pageSize)
		{
			return pageId != 0
				? items.Skip((pageId - 1) * pageSize).Take(pageSize)
				: items.Skip(pageId * pageSize).Take(pageSize);
		}
	}
}