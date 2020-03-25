using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class Message
    {
        public Guid Id { get; set; }

        public Guid DestinationId { get; set; }
        public User Destination { get; set; }

        public Guid AuthorId { get; set; }
        public User Author { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public DateTime? Update { get; set; } = DateTime.Now;
    }
}
