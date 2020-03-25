using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class Country
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public HashSet<Address> Addresses { get; set; }
    }
}
