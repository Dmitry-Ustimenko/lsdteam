using LeagueSoldierDeathTeam.BusinessLogic.Dto;
using LeagueSoldierDeathTeam.Site.Models.Account;

namespace LeagueSoldierDeathTeam.Site.Classes.Extensions.Models
{
	public static class ExternalRegisterModelEx
	{
		public static UserData CopyTo(this ExternalRegisterModel model)
		{
			if (model == null)
				return new UserData();

			var password = model.ExternalPassword;
			if (string.IsNullOrWhiteSpace(model.ExternalPassword))
				password = StringGeneration.Generate(8);

			return new UserData
			{
				UserName = model.ExternalUserName,
				Password = password,
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