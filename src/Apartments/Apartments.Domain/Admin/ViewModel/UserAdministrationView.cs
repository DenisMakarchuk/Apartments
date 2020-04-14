using Apartments.Domain.Admin.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Admin.ViewModel
{
    /// <summary>
    /// View model for User administration
    /// </summary>
    public class UserAdministrationView
    {
        public UserDTOAdministration Profile { get; set; }

        public IdentityUserAdministrationDTO IdentityUser { get; set; }
    }
}