using System.Web;
using Autofac;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.DataAccess;
using LeagueSoldierDeathTeam.BusinessLogic.Factories;
using LeagueSoldierDeathTeam.DataBaseLayer.Abstractions.DataAccess;
using LeagueSoldierDeathTeam.DataBaseLayer.DataAccess;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes;
using LeagueSoldierDeathTeam.Site.Abstractions.Classes.Services;
using LeagueSoldierDeathTeam.Site.Classes;
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

			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();

			builder.RegisterType<RepositoryFactory>().As<RepositoryFactoryBase>().SingleInstance();

			builder.RegisterType<ObjectContextProvider>().As<IObjectContextProvider>().SingleInstance();

			builder.RegisterType<Mailer>().As<IMailer>().SingleInstance();
		}

		#endregion
	}
}