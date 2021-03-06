﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid? ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public Guid? CustomerId { get; set; }
        public User Customer { get; set; }

        public decimal? TotalCoast { get; set; }

        public HashSet<BusyDate> Dates { get; set; }

        public DateTime? Update { get; set; } = DateTime.UtcNow;
    }
}