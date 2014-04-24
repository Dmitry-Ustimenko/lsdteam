using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models.Account
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }
	}

	public class ExternalLoginViewModel
	{
		public string Action { get; set; }
		public string ReturnUrl { get; set; }
	}
}
