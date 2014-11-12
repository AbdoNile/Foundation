using System;
using System.Web.Mvc;

namespace Foundation.Web.Extensions
{
    internal class BasePagingExtensions
    {
        public static MvcHtmlString CreatePageLink(Func<object, string> pageUrl,
            object pagingInfo,
            string linkText,
            string title = "",
            string cssClass = "")
        {
            title = title == string.Empty ? linkText : title;
            string href = pageUrl(pagingInfo);
            TagBuilder tagBuilder = TagBuilder(href, title, cssClass, linkText);
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        private static TagBuilder TagBuilder(string href, string title, string cssClass, string innerHtml)
        {
            var tag = new TagBuilder("a");
            tag.MergeAttribute("href", href);
            tag.MergeAttribute("title", title);
            tag.MergeAttribute("class", cssClass);

            tag.GenerateId("paging");
            tag.InnerHtml = innerHtml;
            return tag;
        }
    }
}