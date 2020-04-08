using Apartments.Domain.Admin.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Admin.ViewModel
{
    public class UserAdministrationView
    {
        public UserDTOAdministration Profile { get; set; }

        public IdentityUser UserIdentity { get; set; }
    }
}
