﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.DTO
{
    public class BusyDateDTO
    {
        public string Id { get; set; }

        public string OrderId { get; set; }

        public DateTime Date { get; set; }
    }
}
