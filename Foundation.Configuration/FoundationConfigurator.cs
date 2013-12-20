﻿using Foundation.Infrastructure;

namespace Foundation.Configuration
{
    public class FoundationConfigurator : IFoundationConfigurator
    {
        public FoundationConfigurator()
        {
            this.Business = new BusinessConfigurations();

            this.Persistence = new PersistenceConfigurations();

            this.Web = new WebConfigurations();

            this.UseBuseinssManagers = true;
            this.UseEmailing = true;
            this.UsePresistence = true;
            this.UseQueryContainer = true;
            this.UseSecurity = true;
            this.UseWeb = true;
        }

        public WebConfigurations Web { get; set; }
        public BusinessConfigurations Business { get; set; }
        public PersistenceConfigurations Persistence { get; set; }
        
        public bool UsePresistence { get; set; }
        
        public bool UseQueryContainer { get; set; }
        
        public bool UseBuseinssManagers { get; set; }
        
        public bool UseWeb { get; set; }
        
        public bool UseSecurity { get; set; }
        
        public bool UseEmailing { get; set; }
    }
}