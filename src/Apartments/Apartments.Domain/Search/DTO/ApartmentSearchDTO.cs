using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Search.DTO
{
    public class ApartmentSearchDTO
    {
        public string Id { get; set; }

        public bool IsOpen { get; set; }
        public decimal Price { get; set; }

        public int NumberOfRooms { get; set; }
        public int Area { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public DateTime Update { get; set; }
    }
}
