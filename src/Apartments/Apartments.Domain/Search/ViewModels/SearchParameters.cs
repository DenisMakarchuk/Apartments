using System;
using System.Collections.Generic;
using System.Text;

namespace Apartments.Domain.Search.ViewModels
{
    public class SearchParameters
    {
        public string CountryId { get; set; }
        public string CityName { get; set; }
        public int RoomsFrom { get; set; }
        public int RoomsTill { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTill { get; set; }
        public IEnumerable<DateTime> NeedDates { get; set; }
    }
}