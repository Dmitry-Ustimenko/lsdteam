namespace LeagueSoldierDeathTeam.Site.Models.Mail
{
	public class ResetPasswordModel
	{
		public string Token { get; set; }
		public string Email { get; set; }
	}
}