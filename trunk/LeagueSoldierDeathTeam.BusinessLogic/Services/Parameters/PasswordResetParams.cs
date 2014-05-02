namespace LeagueSoldierDeathTeam.BusinessLogic.Services.Parameters
{
	public class PasswordResetParams
	{
		public string SessionId { get; set; }

		public string UserIp { get; set; }

		public string UserHostName { get; set; }

		public string PasswordResetToken { get; set; }

		public string NewPassword { get; set; }
	}
}
