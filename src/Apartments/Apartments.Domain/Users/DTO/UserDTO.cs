using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.DTO
{
    /// <summary>
    /// Model for working with an existing user
    /// </summary>
    public class UserDTO
    {
        public string Id { get; set; }

        public DateTime Update { get; set; }
    }
}