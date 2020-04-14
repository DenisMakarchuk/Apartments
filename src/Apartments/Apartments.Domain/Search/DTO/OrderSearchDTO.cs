using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Search.DTO
{
    public class OrderSearchDTO
    {
        public string Id { get; set; }

        public IEnumerable<BusyDateDTO> Dates { get; set; }
    }
}