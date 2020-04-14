using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.AddDTO
{
    /// <summary>
    /// Model for adding a BusyDate
    /// </summary>
    public class AddBusyDate
    {
        public string ApartmentId { get; set; }

        public DateTime Date { get; set; }
    }
}