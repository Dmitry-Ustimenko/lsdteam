using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Models.Account;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class RegisterModelEx
	{
		public static UserData CopyTo(this RegisterModel model)
		{
			return model == null ? new UserData()
				: new UserData
				{
					UserName = model.RegisterUserName,
					Password = model.RegisterPassword,
					Email = model.RegisterEmail
				};
		}
	}
}