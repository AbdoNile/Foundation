using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Web.Paging
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        internal PagedList(IEnumerable<T> source, int pageIndex, int pageSize) :
            this(source.GetPage(pageIndex, pageSize), pageIndex, pageSize, x => x.Count())
        {
        }

        internal PagedList(IEnumerable<T> source, int pageIndex, int pageSize, Func<IEnumerable<T>, int> totalFunc)
        {
            IList<T> sourceAsList = source as IList<T> ?? source.ToList();

            int totalCount = totalFunc(sourceAsList);

            AddRange(sourceAsList);

            int total = totalCount;

            var totalPages = (int) Math.Ceiling((decimal) total/pageSize);

            int showingFrom = (pageIndex*pageSize) + 1;

            int showingTo = totalPages == (pageIndex + 1) ? total : (showingFrom - 1) + pageSize;

            PagedQueryResults = new PagingResults(total, totalPages, showingFrom, showingTo, pageSize, pageIndex);

            PagingViewModel = new PagingInfoViewModel
            {
                ShowingFrom = showingFrom,
                ShowingTo = showingTo,
                TotalItems = total,
                TotalPages = totalPages,
                PageNumber = pageIndex,
                PageSize = pageSize
            };
        }

        public PagingResults PagedQueryResults { get; private set; }

        public PagingInfoViewModel PagingViewModel { get; private set; }
    }
}