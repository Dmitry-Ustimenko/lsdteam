using Microsoft.AspNet.Identity.EntityFramework;

namespace LeagueSoldierDeathTeam.Site.Models.Account
{
	public class ApplicationUser : IdentityUser
	{
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext()
			: base("DefaultConnection")
		{ }
	}
}