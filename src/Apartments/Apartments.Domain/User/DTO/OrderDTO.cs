﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.User.DTO
{
    /// <summary>
    /// Model for working with an existing order
    /// </summary>
    public class OrderDTO
    {
        public string Id { get; set; }

        public IEnumerable<DateTime> Dates { get; set; }

        public DateTime Update { get; set; }
    }
}
