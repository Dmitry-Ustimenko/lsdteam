using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Models.Account;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class ExternalRegisterModelEx
	{
		public static UserData CopyTo(this ExternalRegisterModel model)
		{
			if (model == null)
				return new UserData(); ;

			if (string.IsNullOrWhiteSpace(model.RegisterPassword))
				model.RegisterPassword = StringGeneration.Generate(8);

			return new UserData
			{
				UserName = model.UserName,
				Password = model.RegisterPassword,
				Email = model.Email,
				UserExternalInfo = new UserExternalInfoData
				{
					ProviderName = model.ProviderName,
					ProviderKey = model.ProviderKey
				}
			};
		}
	}
}