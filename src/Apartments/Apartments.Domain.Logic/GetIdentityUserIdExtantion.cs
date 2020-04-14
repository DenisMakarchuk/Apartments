using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic
{
    /// <summary>
    /// Extantion for get IdentityUser own ID
    /// </summary>
    public static class GetIdentityUserIdExtantion
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(_ => _.Type == "id").Value;
        }
    }
}