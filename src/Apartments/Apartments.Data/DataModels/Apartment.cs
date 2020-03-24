using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    class Apartment
    {
        public string Id { get; set; }

        public string OwnerId { get; set; }
        public bool? IsOpen { get; set; } = false;
        public int? Price { get; set; }

        public int? NumberOfRooms { get; set; }
        public int? Area { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public HashSet<Order> Orders { get; set; }

        public HashSet<Comment> Comments { get; set; }

        public DateTime? Update { get; set; } = DateTime.Now;
    }
}
