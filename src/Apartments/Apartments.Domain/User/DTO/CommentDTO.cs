using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.User.DTO
{
    /// <summary>
    /// Model for working with an existing comment
    /// </summary>
    public class CommentDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public DateTime Update { get; set; }
    }
}
