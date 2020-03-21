using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain
{
    public class Apartment
    {
        public string Id { get; set; }

        public string OwnerId { get; set; }
        public bool IsOpen { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public int NumberOfRooms { get; set; }
        public int Area { get; set; }

        public string Country { get; set; }
        public string Sity { get; set; }
        public string Street { get; set; }
        public int HomeNumber { get; set; }
        public int NumberOfApartment { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
