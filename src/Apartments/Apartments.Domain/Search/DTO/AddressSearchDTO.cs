using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Search.DTO
{
    public class AddressSearchDTO
    {
        public string Id { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public int NumberOfApartment { get; set; }
    }
}
