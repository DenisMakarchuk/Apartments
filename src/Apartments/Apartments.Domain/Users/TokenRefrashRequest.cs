using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users
{
    public class TokenRefrashRequest
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
