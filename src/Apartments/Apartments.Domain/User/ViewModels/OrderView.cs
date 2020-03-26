using Apartments.Domain.User.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.User.ViewModels
{
    public class OrderView
    {
        public OrderDTO Order { get; set; }

        public ApartmentDTO Apartment { get; set; }

        public AddressDTO Address { get; set; }

        public CountryDTO Country { get; set; }
    }
}
