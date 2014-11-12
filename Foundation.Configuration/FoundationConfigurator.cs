namespace Foundation.Configuration
{
    public class FoundationConfigurator : IFoundationConfigurator
    {
        public FoundationConfigurator()
        {
            Business = new BusinessConfigurations();

            Persistence = new PersistenceConfigurations();

            Mongo = new MongoConfigurations();

            Web = new WebConfigurations();

            UseBuseinssManagers = true;
            UseEmailing = false;
            UsePresistence = false;
            UseQueryContainer = true;
            UseSecurity = true;
            UseWeb = true;
            UseMongo = false;
        }

        public WebConfigurations Web { get; set; }
        public BusinessConfigurations Business { get; set; }
        public PersistenceConfigurations Persistence { get; set; }
        public MongoConfigurations Mongo { get; set; }

        public bool UsePresistence { get; set; }

        public bool UseQueryContainer { get; set; }

        public bool UseBuseinssManagers { get; set; }

        public bool UseWeb { get; set; }

        public bool UseSecurity { get; set; }

        public bool UseEmailing { get; set; }
        public bool UseMongo { get; set; }
    }
}