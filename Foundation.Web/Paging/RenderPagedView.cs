using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Foundation.Web.Extensions;

namespace Foundation.Web.Paging
{
    public class RenderPagedView : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Controller.ViewData != null)
            {
                object model = filterContext.Controller.ViewData.Model;
                if (model != null)
                {
                    INavigationParameters pagingModel = null;

                    if (model.GetType().IsSubclassOf(typeof (INavigationParameters)))
                    {
                        pagingModel = (INavigationParameters) model;
                    }
                    else
                    {
                        PropertyInfo propertyInfo = model.GetType()
                            .GetProperties()
                            .FirstOrDefault(
                                x =>
                                    x.PropertyType.GetInterfaces()
                                        .Contains(typeof (INavigationParameters)));

                        if (propertyInfo != null)
                        {
                            pagingModel = (INavigationParameters) propertyInfo.GetValue(model);
                        }

                        if (pagingModel == null)
                        {
                            if (propertyInfo != null)
                            {
                                pagingModel =
                                    (INavigationParameters) Activator.CreateInstance(propertyInfo.PropertyType);
                                propertyInfo.SetValue(model, pagingModel);
                            }
                        }
                    }

                    INavigationParameters pagedModel = pagingModel;

                    string controllerName = filterContext.RouteData.Values["controller"].ToString();
                    string actionName = filterContext.RouteData.Values["action"].ToString();
                    var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

                    Func<object, string> actionFunction = x => urlHelper.Action(actionName, controllerName, x, true);

                    if (pagedModel != null) pagedModel.ActionFunc = actionFunction;
                }
            }

            base.OnResultExecuting(filterContext);
        }
    }
}