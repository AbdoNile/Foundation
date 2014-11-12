using System.Web.Mvc;

namespace Foundation.Web
{
    public abstract class BaseController : Controller
    {
        protected ActionResult PageNotFound()
        {
            return View("NotFound");
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            if (viewName == null && model != null)
            {
                string modelName = model.GetType().Name;
                viewName = modelName.Replace("ViewModel", string.Empty);
            }

            return base.View(viewName, masterName, model);
        }

        protected override PartialViewResult PartialView(string viewName, object model)
        {
            if (viewName == null && model != null)
            {
                string modelName = model.GetType().Name;
                viewName = string.Format("Partials/_{0}", modelName.Replace("PartialViewModel", string.Empty));
            }

            return base.PartialView(viewName, model);
        }

        protected ViewResultBase AdaptiveView(string viewName, object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName, model);
            }
            return View(viewName, model);
        }
    }
}