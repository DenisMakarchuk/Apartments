﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class User
    {
        public Guid Id { get; set; }

        public string NickName { get; set; }

        public HashSet<Apartment> Apartments { get; set; }

        public HashSet<Order> Orders { get; set; }

        public HashSet<Comment> Comments { get; set; }

        public HashSet<RefreshToken> RefreshTokens { get; set; }

        public DateTime? Update { get; set; } = DateTime.UtcNow;
    }
}