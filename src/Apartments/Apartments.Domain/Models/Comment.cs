using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain
{
    public class Comment
    {
        public string Id { get; set; }

        public string ApartmentId { get; set; }
        public string AthorId { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
    }
}
