using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Admin.DTO
{
    /// <summary>
    /// Model for comment administration
    /// </summary>
    public class CommentDTOAdministration
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime Update { get; set; }
    }
}