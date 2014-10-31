
using Foundation.Configuration;
using Foundation.Infrastructure;
using Foundation.Infrastructure.BL;
using Foundation.Infrastructure.Configurations;
using Foundation.Infrastructure.Notifications;
using Foundation.Persistence;
using Foundation.Persistence.Configurations;
using Foundation.Web;
using Kafala.BusinessManagers;
using Kafala.BusinessManagers.Donor;
using NHibernate;
using StructureMap;
using StructureMap.Configuration.DSL.Expressions;

namespace Kafala.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            NHibernateConfigurationFactory.ExportSchema = true;
            NHibernateConfigurationFactory.ExportedSchemaLocation = "KafalaDB.sql";
            var container = ConfigureDependencies();

            var session = container.GetInstance<ISession>();

           var businessManagerContainer = container.GetInstance<IBusinessManagerContainer>();

            var donorBusinessManager = businessManagerContainer.Get<DonorBusinessManager>();

            //donorBusinessManager.Add("Abdo", "1234", null, null);
        }

        private static IContainer ConfigureDependencies()
        {
            var container = ObjectFactory.Container;
            Configure();
            
            container.Configure(x => x.For<IConnectionString>().Use(new ConnectionString("KafalaDB")));
            container.Configure(x => x.For<IFlashMessenger>().Use<SwallowFlashMessneger>());
            return container;
        }

        private static void Configure()
        {
            var config = new FoundationConfigurator
            {

                Business =
                {
                    BusinessInvocationLogger =
                        typeof(Kafala.BusinessManagers.SqlProcBusinessManagerInvocationLogger),
                    EmailLogger = typeof(Foundation.Infrastructure.Notifications.EmailLogger)
                },

                Persistence =
                {
                    PocoPointer = typeof(Kafala.Entities.Donor),
                    ConnectionStringKeyName = "Kafaladb"
                },

                UseBuseinssManagers = true,
                UseEmailing = false,
                UsePresistence = true,
                UseQueryContainer = false,
                UseSecurity = false,
                UseWeb = false,
            };

            FoundationKickStart.Configure(config);

            ObjectFactory.Configure(cfg => new Foundation.Persistence.Configurations.PersistenceConfigurator().Configure(cfg, config));

            ObjectFactory.Configure(cfg => new Foundation.Infrastructure.Configurations.InfrastructureConfigurator().Configure(cfg, config));
        }
    }
}
