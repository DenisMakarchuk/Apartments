using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    class Address
    {
        public string Id { get; set; }

        public string Country { get; set; }
        public string Sity { get; set; }
        public string Street { get; set; }
        public int? HomeNumber { get; set; }
        public int? NumberOfApartment { get; set; }
    }
}
