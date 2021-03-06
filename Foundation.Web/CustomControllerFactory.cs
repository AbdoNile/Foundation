﻿using System;
using System.Globalization;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using StructureMap;

namespace Foundation.Web
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer container;

        private static readonly object NestedContainerKey = new object();
       
        public CustomControllerFactory(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;

        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var nestedContainer = this.container.GetNestedContainer();

            requestContext.HttpContext.Items[NestedContainerKey] = nestedContainer;

            ControllerBase controllerBase = null;

            Func<ControllerContext> ctxtCtor = () => controllerBase == null ? null : controllerBase.ControllerContext;
                //controllerBase == null ? null : controllerBase.ControllerContext;

            nestedContainer
                .Configure(cfg =>
                               {
                                   cfg.For<RequestContext>().Use(requestContext);
                                   cfg.For<HttpContextBase>().Use(requestContext.HttpContext);
                                   cfg.For<Func<ControllerContext>>().Use(ctxtCtor);
                                   cfg.For<IFlashMessenger>()
                                       .HybridHttpOrThreadLocalScoped()
                                       .Use(x =>
                                           {
                                               //var tempData = x.GetInstance<Func<ControllerContext>>()().Controller.TempData;
                                               var flashMessenger = new WebFlashMessenger(container.GetInstance<IResourcesLocator>(), x.GetInstance<Func<ControllerContext>>());
                                                 //   tempData["FlashMessenger"] = flashMessenger;
                                                    return flashMessenger;
                                                });
                               });

            var controller = nestedContainer.TryGetInstance<IController>(controllerName);

            controllerBase = controller as ControllerBase;

            if (controller == null)
            {
                throw new HttpException(
                    (int)HttpStatusCode.NotFound,
                    string.Format(CultureInfo.CurrentUICulture, "No controller found for {0} at path {1}.", new object[] { controllerName, requestContext.HttpContext.Request.Path }));
            }

       

            return controller;
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        public override void ReleaseController(IController controller)
        {
            var controllerBase = controller as Controller;

            if (controllerBase != null && controllerBase.ControllerContext != null)
            {
                var httpContextBase = controllerBase.ControllerContext.HttpContext;

                var nestedContainer = (IContainer)httpContextBase.Items[NestedContainerKey];

                if (nestedContainer != null)
                {
                    nestedContainer.Dispose();
                }
            }

            base.ReleaseController(controller);
        }
    }
}
