using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<Apartment> Apartments { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
