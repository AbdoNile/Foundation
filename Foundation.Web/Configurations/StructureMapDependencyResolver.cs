using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace Foundation.Web.Configurations
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer container;

        public StructureMapDependencyResolver(IContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            object instance = container.TryGetInstance(serviceType);

            if (instance == null && !serviceType.IsAbstract)
            {
                container.Configure(c => c.AddType(serviceType, serviceType));
                instance = container.TryGetInstance(serviceType);
            }

            return instance;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}