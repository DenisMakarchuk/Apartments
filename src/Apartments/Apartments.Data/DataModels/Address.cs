using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class Address
    {
        public string Id { get; set; }

        public string Country { get; set; }

        public HashSet<AddressPart> AddressParts { get; set; }
    }
}
