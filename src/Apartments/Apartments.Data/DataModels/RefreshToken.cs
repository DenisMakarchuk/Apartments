using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; private set; }
        public DateTime Expires { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; set; }
        public bool Active => DateTime.UtcNow <= Expires;

        public RefreshToken(string token, DateTime expires, Guid userId)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
        }
    }
}
