using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.User.DTO
{
    /// <summary>
    /// Model for working with an existing address
    /// </summary>
    public class AddressDTO
    {
        public string Id { get; set; }

        public string CountryId { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public int NumberOfApartment { get; set; }

        public DateTime Update { get; set; }
    }
}
