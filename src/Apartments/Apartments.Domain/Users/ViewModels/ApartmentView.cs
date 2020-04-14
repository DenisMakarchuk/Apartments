using Apartments.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Users.ViewModels
{
    /// <summary>
    /// View model for working with an existing Apsrtment
    /// </summary>
    public class ApartmentView
    {
        public ApartmentDTO Apartment { get; set; }

        public AddressDTO Address { get; set; }

        public CountryDTO Country { get; set; }
    }
}