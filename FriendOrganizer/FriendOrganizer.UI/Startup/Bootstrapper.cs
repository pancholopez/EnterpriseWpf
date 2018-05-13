using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.ViewModel;
using Microsoft.EntityFrameworkCore;
using Prism.Events;

namespace FriendOrganizer.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            ConfigureRepository(builder);

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<FriendDataService>().As<IFriendDataService>();

            return builder.Build();
        }

        private static void ConfigureRepository(ContainerBuilder builder, bool useInMemoryDb = false)
        {
            if (!useInMemoryDb)
            {
                builder.RegisterType<FriendOrganizerDbContext>().AsSelf();
            }
            else
            {
                var options = new DbContextOptionsBuilder<FriendOrganizerDbContext>()
                    .UseInMemoryDatabase(databaseName: "testDB")
                    .Options;
                builder.Register(x => new FriendOrganizerDbContext(options))
                    .As<FriendOrganizerDbContext>();
            }
        }
    }
}