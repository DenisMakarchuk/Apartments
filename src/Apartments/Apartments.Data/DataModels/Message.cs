using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    class Message
    {
        public string Id { get; set; }

        public string DestinationId { get; set; }
        public string AthorId { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public DateTime? Update { get; set; } = DateTime.Now;
    }
}
