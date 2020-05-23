using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Apartments.Domain.Users
{
    public class ForgotPasswordModel
    {
        [Required]
        public string CallBackUrl { get; set; }

        [Required]
        public string LogInNameOrEmail { get; set; }
    }
}
