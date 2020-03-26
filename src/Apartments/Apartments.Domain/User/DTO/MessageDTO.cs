using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.User.DTO
{
    /// <summary>
    /// Model for working with an existing message
    /// </summary>
    public class MessageDTO
    {
        public string Id { get; set; }

        public string DestinationId { get; set; }

        public string AuthorId { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public DateTime Update { get; set; }
    }
}
