using Apartments.Domain.Search.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Search.ViewModels
{
    public class ApartmentSearchView
    {
        public ApartmentSearchDTO Apartment { get; set; }

        public AddressSearchDTO Address { get; set; }

        public CountrySearchDTO Country { get; set; }
    }
}
