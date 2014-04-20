using Autofac;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Factories;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services;
using LeagueSoldierDeathTeam.Site.Classes.Services;

namespace LeagueSoldierDeathTeam.Site.Modules.Autofac
{
	public class LibraryModule : Module
	{
		#region Override Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<ServiceFactory>().As<ServiceFactoryBase>().SingleInstance();

			builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();
		}

		#endregion
	}
}