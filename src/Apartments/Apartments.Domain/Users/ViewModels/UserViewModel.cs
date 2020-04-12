using Apartments.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.ViewModels
{
    public class UserViewModel
    {
        public UserDTO Profile { get; set; }

        public string Token { get; set; }
    }
}
