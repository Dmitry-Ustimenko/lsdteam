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

			if (string.IsNullOrWhiteSpace(model.ExternalPassword))
				model.ExternalPassword = StringGeneration.Generate(8);

			return new UserData
			{
				UserName = model.ExternalUserName,
				Password = model.ExternalPassword,
				Email = model.ExternalEmail,
				UserExternalInfo = new UserExternalInfoData
				{
					ProviderName = model.ProviderName,
					ProviderKey = model.ProviderKey
				}
			};
		}
	}
}