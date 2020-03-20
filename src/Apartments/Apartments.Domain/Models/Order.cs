using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain
{
    public class Order
    {
        public string Id { get; set; }

        public string ApartmentId { get; set; }
        public string CustomerId { get; set; }

        public ICollection<DateTime> Dates { get; set; }
    }
}
