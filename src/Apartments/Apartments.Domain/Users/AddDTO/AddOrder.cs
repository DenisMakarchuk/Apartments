using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.AddDTO
{
    /// <summary>
    /// Model for order adding
    /// </summary>
    public class AddOrder
    {
        public string ApartmentId { get; set; }

        public string CustomerId { get; set; }

        public IEnumerable<DateTime> Dates { get; set; }
    }
}
