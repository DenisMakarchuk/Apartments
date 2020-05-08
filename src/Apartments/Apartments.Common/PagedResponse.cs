using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Common
{
    public class PagedResponse<T> where T : class
    {
        public PagedResponse(IEnumerable<T> data, int count, int pageNumber, int pageSize)
        {
            Data = data;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public IEnumerable<T> Data { get; }

        public int PageNumber { get; }
        public int TotalPages { get; }


        public bool HasNextPage => (PageNumber < TotalPages);

        public bool HasPreviousPage => (PageNumber > 1);
    }
}
