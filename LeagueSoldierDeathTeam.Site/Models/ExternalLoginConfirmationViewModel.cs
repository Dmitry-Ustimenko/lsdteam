using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }
	}
}