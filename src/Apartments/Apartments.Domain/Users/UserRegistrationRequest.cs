using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Domain.Users.ViewModels
{
    public class UserRegistrationRequest
    {
        [MinLength(1)]
        public string UserName { get; set; }

        [MinLength(1)]
        public string NickName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}