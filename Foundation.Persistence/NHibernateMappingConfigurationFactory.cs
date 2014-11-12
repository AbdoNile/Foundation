﻿using System;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;

namespace Foundation.Persistence
{
    public class NHibernateMappingConfigurationFactory : INHibernateMappingConfigurationFactory
    {
        public FluentConfiguration BuildMapping(FluentConfiguration configuration, Type type, string nameSpace)
        {
            configuration = configuration
               .Mappings(m =>
               {
                   m.AutoMappings.Add(
                       AutoMap.Assemblies(Assembly.GetAssembly(type))
                       .UseOverridesFromAssembly(Assembly.GetAssembly(type))
                       .Conventions.AddAssembly(Assembly.GetAssembly(type))
                       .Where(t => t.Namespace != null && t.Namespace.StartsWith(nameSpace)));
                   m.HbmMappings.AddFromAssembly(Assembly.GetAssembly(type)); // for stored procedures
               });

            return configuration;
        }
    }
}