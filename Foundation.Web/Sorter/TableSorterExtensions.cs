using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Foundation.Web.Configurations;
using Foundation.Web.Extensions;
using Foundation.Web.Paging;

namespace Foundation.Web.Sorter
{
    public static class TableSorterExtensions
    {
        public static MvcHtmlString SortableHeader(this HtmlHelper row, string currentSort, string currentDirection,
            string columnId, string title, Func<object, string> actionFunc, object htmlAttributes = null)
        {
            var sortingInfo = new SortingParameters();
            sortingInfo.ActionFunc = actionFunc;
            sortingInfo.Sort = currentSort;
            sortingInfo.SortDirection = currentDirection;
            return row.SortableHeader(sortingInfo, columnId, title, htmlAttributes);
        }

        public static MvcHtmlString SortableHeader(this HtmlHelper row, object queryObject, string columnId,
            string title, object htmlAttributes = null)
        {
            string properties = string.Empty;
            IDictionary<string, object> attributes = new RouteValueDictionary(htmlAttributes);

            ISortingParameters sortingInfo = queryObject as ISortingParameters ?? new SortingParameters();

            string currentSort = sortingInfo.Sort;
            string currentDirection = sortingInfo.SortDirection;
            Func<object, string> urlActionDelegate = sortingInfo.ActionFunc;

            const string direction = "asc";
            string newSortDirection = direction;
            foreach (var attr in attributes)
            {
                properties += " " + attr.Key + "=\"" + attr.Value + "\" ";
            }

            string sortableHeaderCssClass = WebConfigurations.PagingConfigurations.SortableHeaderCssClass;
            string sortableHeader = sortableHeaderCssClass;
            string cssClass = sortableHeader;
            string sortIcon = "";

            if (!string.IsNullOrEmpty(currentSort) && currentSort == columnId)
            {
                string sorted = WebConfigurations.PagingConfigurations.SortedHeaderCssClass;
                cssClass += string.Format(" {0}", sorted);
                if (currentDirection.ToLower().StartsWith("d"))
                {
                    string sortedIcondDescending = WebConfigurations.PagingConfigurations.SortedIcondDescending;
                    sortIcon = sortedIcondDescending;
                    newSortDirection = direction;
                }
                else
                {
                    string sortedIcondAscending = WebConfigurations.PagingConfigurations.SortedIcondAscending;
                    sortIcon = sortedIcondAscending;
                    newSortDirection = "desc";
                }
            }
            else
            {
                // default (initial sort) is ascending
                newSortDirection = direction;
            }

            string iconSpan = string.Format("<span class=\"glyphicon glyph{0}\"></span>", sortIcon);

            sortingInfo.Sort = columnId;
            sortingInfo.SortDirection = newSortDirection;

            MvcHtmlString link = BasePagingExtensions.CreatePageLink(urlActionDelegate, queryObject, title, title);

            sortingInfo.Sort = currentSort;
            sortingInfo.SortDirection = currentDirection;

            return
                MvcHtmlString.Create(string.Format("<th class=\"{0}\" id=\"{1}\" {2}>{3} {4} </th>", cssClass, columnId,
                    properties, link, iconSpan));
        }
    }
}