using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.AddDTO
{
    public class AddBusyDate
    {
        public string OrderId { get; set; }

        public DateTime Date { get; set; }
    }
}
