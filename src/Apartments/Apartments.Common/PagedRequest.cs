using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Common
{
    public class PagedRequest
    {
        public PagedRequest(int pageNumber = 1, int pageSize = 20)
        {
            PageNumber = (pageNumber >= 1 ? pageNumber : 1);
            PageSize = (pageSize >= 1 ? pageSize : 20);
        }

        public int PageNumber { get; }

        public int PageSize { get; }
    }

    public class PagedRequest<T> : PagedRequest 
    {
        public PagedRequest(T data, int pageNumber = 1, int pageSize = 20) : base(pageNumber, pageSize)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
