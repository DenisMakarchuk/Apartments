﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain
{
    public class GlobalAdmin
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
