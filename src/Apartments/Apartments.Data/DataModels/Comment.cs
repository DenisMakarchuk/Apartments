using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Guid ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public Guid AuthorId { get; set; }
        public User Author { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public DateTime? Update { get; set; } = DateTime.UtcNow;
    }
}
