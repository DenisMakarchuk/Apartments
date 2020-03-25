using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class Address
    {
        public string Id { get; set; }

        public string CountryId { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public int? HomeNumber { get; set; }
        public int? NumberOfApartment { get; set; }
    }
}
