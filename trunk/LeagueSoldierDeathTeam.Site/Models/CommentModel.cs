using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LeagueSoldierDeathTeam.Business.Classes.Enums;
using LeagueSoldierDeathTeam.Business.Dto;

namespace LeagueSoldierDeathTeam.Site.Models
{
	public class CommentModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Комментарий должен быть введен")]
		public string CommentDescription { get; set; }

		public int WriterId { get; set; }

		public int ContentId { get; set; }

		public int Rate { get; set; }

		public CommentSortEnum SortType { get; set; }

		public IEnumerable<CommentData> Data { get; set; }

		public CommentModel()
		{
			Data = new List<CommentData>();
			SortType = CommentSortEnum.New;
		}
	}
}