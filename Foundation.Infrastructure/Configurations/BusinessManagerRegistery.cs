using System;
using Foundation.Infrastructure.BL;
using StructureMap.Configuration.DSL;

namespace Foundation.Infrastructure.Configurations
{
    public class BusinessManagerRegistery : Registry , IBusinessManagerRegistery
    {
        public BusinessManagerRegistery()
        {
            this.For<IBusinessManagerContainer>().Use<BusinessManagerContainer>();
            var dir = AppDomain.CurrentDomain.BaseDirectory;

            this.Scan(x =>
            {
                var basePath = System.AppDomain.CurrentDomain.BaseDirectory;
                if (!basePath.ToLower().Contains("bin"))
                {
                    basePath = System.IO.Path.Combine(basePath, "bin");
                }
                
                x.AssembliesFromPath(basePath);
                x.With(new BusinessManagerRegisterationConvention());
                x.ConnectImplementationsToTypesClosing(typeof(BusinessManagerInterceptor<>));
            });
        }
    }
}
