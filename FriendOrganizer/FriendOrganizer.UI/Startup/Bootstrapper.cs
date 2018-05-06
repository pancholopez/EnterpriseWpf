using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace FriendOrganizer.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            var options = new DbContextOptionsBuilder<FriendOrganizerDbContext>()
                .UseInMemoryDatabase(databaseName: "testDB")
                .Options;
            builder.Register(x => new FriendOrganizerDbContext(options))
                .As<FriendOrganizerDbContext>();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<FriendDataService>().As<IFriendDataService>();

            return builder.Build();
        }
    }
}