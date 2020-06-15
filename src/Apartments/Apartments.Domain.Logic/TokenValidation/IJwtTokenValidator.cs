using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Apartments.Domain.Logic.TokenValidation
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
