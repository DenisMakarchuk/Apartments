using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.User.AddDTO
{
    /// <summary>
    /// Model for adding a comment
    /// </summary>
    public class AddComment
    {
        public string ApartmentId { get; set; }

        public string AuthorId { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
    }
}
