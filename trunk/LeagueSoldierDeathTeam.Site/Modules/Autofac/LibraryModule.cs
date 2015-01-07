using Autofac;
using LeagueSoldierDeathTeam.Business.Abstractions.Factories;
using LeagueSoldierDeathTeam.Business.Abstractions.Interfaces.DataAccess;
using LeagueSoldierDeathTeam.Business.DataAccess;
using LeagueSoldierDeathTeam.Business.Factories;
using LeagueSoldierDeathTeam.DataBase.Abstractions.DataAccess;
using LeagueSoldierDeathTeam.DataBase.DataAccess;
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

			// Single tone
			builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();

			builder.RegisterType<Mailer>().As<IMailer>().SingleInstance();

			// Instance per dependency
			builder.RegisterType<ObjectContextProvider>().As<IObjectContextProvider>().InstancePerLifetimeScope();

			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

			builder.RegisterType<RepositoryFactory>().As<RepositoryFactoryBase>().InstancePerLifetimeScope();

			builder.RegisterType<ServiceFactory>().As<ServiceFactoryBase>().InstancePerLifetimeScope();
		}

		#endregion
	}
}