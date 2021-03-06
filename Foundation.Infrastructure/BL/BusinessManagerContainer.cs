﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Foundation.Configuration;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Foundation.Infrastructure.BL
{
    public class BusinessManagerContainer : IBusinessManagerContainer
    {
        private IContainer container;
        private readonly IFoundationConfigurator foundationConfigurator;

        public BusinessManagerContainer(IContainer container, IFoundationConfigurator foundationConfigurator )
        {
            this.container = container;
            this.foundationConfigurator = foundationConfigurator;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public T Get<T>() where T : class , IBusinessManager
        {
            var proxyGenerator = new ProxyGenerator();
            
            new ProxyGenerationOptions {Hook = new BusinessManagerContainerHook()};

            var constructorArguments = new List<object>();
            var constructor = typeof(T).GetConstructors().FirstOrDefault();
            if (constructor != null)
            {
                var constructorParameters = constructor.GetParameters();
                constructorArguments.AddRange(constructorParameters.Select(parameterInfo => container.GetInstance(parameterInfo.ParameterType)));
            }

            var interceptors = container.GetAllInstances<BusinessManagerInterceptor<T>>().Select(mi => (IInterceptor)mi).ToList();
            
            if (foundationConfigurator.UsePresistence)
            {
                var transactionInterceptor = container.GetInstance<TransactionInterceptor<T>>() as IInterceptor;
                interceptors.Add(transactionInterceptor);

            }
            
            var objectToReturn = proxyGenerator.CreateClassProxy(typeof(T), constructorArguments.ToArray(), interceptors.ToArray());
            return (T)objectToReturn;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.container != null)
                {
                    this.container.Dispose();
                    this.container = null;
                }
            }
        }
    }

    internal class BusinessManagerContainerHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodInfo.IsPublic;
        }
    }
}
