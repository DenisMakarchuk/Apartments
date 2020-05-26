using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Logic
{
    public class MyIdentityDataInitializer
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {

            if (userManager.FindByNameAsync("user1").Result == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "admin";
                user.Email = "hiedmant@mail.ru";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync
                (user, "test1234").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
