using Apartments.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Web.Identities
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        public IdentityService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<string>> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if(existingUser != null)
            {
                return (Result<string>)Result<string>.Fail<string>("User with this Emai already exista");
            }

            var newUser = new I
        }
    }
}
