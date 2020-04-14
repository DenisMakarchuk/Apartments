using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Admin.DTO
{
    /// <summary>
    /// Model for user administration
    /// </summary>
    public class UserDTOAdministration
    {
        public string Id { get; set; }

        public DateTime Update { get; set; }
    }
}