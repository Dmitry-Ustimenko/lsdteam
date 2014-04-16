using Autofac;
using LeagueSoldierDeathTeam.BusinessLogic.Abstractions.Factories;
using LeagueSoldierDeathTeam.BusinessLogic.Factories;

namespace LeagueSoldierDeathTeam.Site.Modules.Autofac
{
	public class LibraryModule : Module
	{
		#region Override Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<ServiceFactory>().As<ServiceFactoryBase>().SingleInstance();
		}

		#endregion
	}
}