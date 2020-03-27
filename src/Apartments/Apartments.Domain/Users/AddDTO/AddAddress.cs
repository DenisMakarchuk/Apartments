using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.AddDTO
{
    /// <summary>
    /// Model for adding an address
    /// </summary>
    public class AddAddress
    {
        public string CountryId { get; set; }

        public string ApartmentId { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public int NumberOfApartment { get; set; }
    }
}
