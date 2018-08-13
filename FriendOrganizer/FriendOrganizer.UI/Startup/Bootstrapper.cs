using Autofac;
using FriendOrganizer.Core.Repositories;
using FriendOrganizer.Core.Services;
using FriendOrganizer.DataAccess;
using FriendOrganizer.DataAccess.Lookups;
using FriendOrganizer.DataAccess.Repositories;
using FriendOrganizer.UI.View.Services;
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

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(FriendDetailViewModel));
            builder.RegisterType<MeetingDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));
            builder.RegisterType<ProgrammingLanguageDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(ProgrammingLanguageDetailViewModel));

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<FriendRepository>().As<IFriendRepository>();
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();
            builder.RegisterType<ProgrammingLanguageRepository>()
                .As<IProgrammingLanguageRepository>();

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