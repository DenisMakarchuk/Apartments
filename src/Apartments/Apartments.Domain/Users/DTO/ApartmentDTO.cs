using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.DTO
{
    /// <summary>
    /// Model for working with an existing apartment
    /// </summary>
    public class ApartmentDTO
    {
        public string Id { get; set; }

        public string OwnerId { get; set; }

        public bool IsOpen { get; set; }
        public decimal Price { get; set; }

        public int NumberOfRooms { get; set; }
        public int Area { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public DateTime Update { get; set; }
    }
}