using System.ComponentModel.DataAnnotations;

namespace LeagueSoldierDeathTeam.Site.Models
{
	public class CommentEditModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Комментарий должен быть введен")]
		public string Description { get; set; }
	}
}