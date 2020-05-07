using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.AddDTO
{
    /// <summary>
    /// Model for user profile adding
    /// </summary>
    public class AddUser
    {
        public string Id { get; set; }

        public string NickName { get; set; }
    }
}