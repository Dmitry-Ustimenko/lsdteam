namespace LeagueSoldierDeathTeam.Site.Models
{
	public class BaseOverlayModel
	{
		public string Title { get; set; }
		public string YesLabel { get; set; }
		public string NoLabel { get; set; }

		public BaseOverlayModel()
		{
			YesLabel = "Ok";
			NoLabel = "Cancel";
		}
	}
}