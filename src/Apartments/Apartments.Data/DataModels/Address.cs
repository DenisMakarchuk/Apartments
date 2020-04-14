using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Data.DataModels
{
    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? CountryId { get; set; }
        public Country Country { get; set; }

        public Guid? ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public int? NumberOfApartment { get; set; }
    }
}