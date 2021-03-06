﻿using System.Web.Mvc;

namespace Foundation.FormBuilder.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static FormBuilder<TModel> FormBuilder<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new FormBuilder<TModel>(htmlHelper);
        }
    }
}