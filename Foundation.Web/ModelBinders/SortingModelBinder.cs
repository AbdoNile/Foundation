using System.Web.Mvc;
using Foundation.Web.Paging;

namespace Foundation.Web.ModelBinders
{
    public class SortingModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object model = base.BindModel(controllerContext, bindingContext);
            var pagedModel = (ISortingParameters) model;

            if (pagedModel == null)
            {
                pagedModel = new SortingParameters();
            }

            return pagedModel;
        }
    }
}