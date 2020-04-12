using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.DTO
{
    /// <summary>
    /// Model for working with an existing order
    /// </summary>
    public class OrderDTO
    {
        public string Id { get; set; }

        public string ApartmentId { get; set; }

        public string CustomerId { get; set; }

        public decimal TotalCoast { get; set; }

        public IEnumerable<DateTime> Dates { get; set; }

        public DateTime Update { get; set; }
    }
}
