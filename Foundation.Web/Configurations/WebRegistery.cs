using System.Reflection;
using System.Web;
using Foundation.Configuration;
using Foundation.Web.Caching;
using StructureMap.Configuration.DSL;

namespace Foundation.Web.Configurations
{
    public class WebRegistery : Registry, IWebRegistery
    {
        public WebRegistery(IFoundationConfigurator configurator)
        {
            For<ICacheService>()
                .HybridHttpOrThreadLocalScoped()
                .Use<InMemoryCache>();
            Scan(x =>
            {
                x.Assembly(Assembly.GetAssembly(configurator.Web.ControllersAssemblyHookType).GetName().Name);
                x.With(new ControllerRegistrationConvention());
            });
            For<HttpSessionStateBase>().Use(ctx => new HttpSessionStateWrapper(HttpContext.Current.Session));
        }
    }
}