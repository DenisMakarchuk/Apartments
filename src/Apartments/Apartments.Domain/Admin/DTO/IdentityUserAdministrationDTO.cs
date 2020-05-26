using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Admin.DTO
{
    /// <summary>
    /// Model for IdentityUser administration
    /// </summary>
    public class IdentityUserAdministrationDTO
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}