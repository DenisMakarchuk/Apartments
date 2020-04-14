using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.DTO
{
    /// <summary>
    /// Model for working with an existing BusyDate
    /// </summary>
    public class BusyDateDTO
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }
    }
}