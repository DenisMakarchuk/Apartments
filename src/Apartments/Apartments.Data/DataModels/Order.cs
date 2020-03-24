using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class Order
    {
        public string Id { get; set; }

        public string ApartmentId { get; set; }
        public string CustomerId { get; set; }

        public HashSet<DateTime> Dates { get; set; }

        public DateTime? Update { get; set; } = DateTime.Now;
    }
}
