using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class Apartment
    {
        public Guid Id { get; set; }

        public Guid AddressId { get; set; }
        public Address Address { get; set; }

        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        public bool? IsOpen { get; set; } = false;
        public decimal? Price { get; set; }

        public int? NumberOfRooms { get; set; }
        public int? Area { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public HashSet<Order> Orders { get; set; }

        public HashSet<Comment> Comments { get; set; }

        public DateTime? Update { get; set; } = DateTime.Now;
    }
}
