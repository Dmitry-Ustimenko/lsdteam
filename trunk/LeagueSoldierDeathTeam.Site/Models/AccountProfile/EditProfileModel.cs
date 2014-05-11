namespace LeagueSoldierDeathTeam.Site.Models.AccountProfile
{
	public class EditProfileModel
	{
		public int UserId { get; set; }

		public EditMainInfoModel EditMainInfoModel { get; set; }

		public EditAdvanceInfoModel EditAdvanceInfoModel { get; set; }

		public EditBindInfoModel EditBindInfoModel { get; set; }

		public ChangePasswordModel ChangePasswordModel { get; set; }

		public EditProfileModel()
		{
			ChangePasswordModel = new ChangePasswordModel();
		}
	}
}