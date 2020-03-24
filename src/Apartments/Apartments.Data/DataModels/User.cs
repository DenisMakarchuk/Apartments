﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public HashSet<Apartment> Apartments { get; set; }

        public HashSet<Order> Orders { get; set; }

        public HashSet<Message> Messages { get; set; }

        public HashSet<Comment> Comments { get; set; }

        public DateTime? Update { get; set; } = DateTime.Now;
    }
}
