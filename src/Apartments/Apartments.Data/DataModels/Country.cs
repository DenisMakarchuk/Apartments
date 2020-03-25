using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class Country
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public HashSet<Address> Addresses { get; set; }
    }
}
