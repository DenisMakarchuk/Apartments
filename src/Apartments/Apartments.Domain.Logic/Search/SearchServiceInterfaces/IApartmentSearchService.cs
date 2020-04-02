using Apartments.Common;
using Apartments.Domain.Search.DTO;
using Apartments.Domain.Search.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Search.SearchServiceInterfaces
{
    public interface IApartmentSearchService
    {
        Task<Result<IEnumerable<ApartmentSearchDTO>>> GetAllApartmentsAsync(
            string countryId,
            string cityName,
            int roomsFrom,
            int roomsTill,
            decimal priceFrom,
            decimal priceTill,
            IEnumerable<DateTime> needDates);

        Task<Result<ApartmentSearchView>> GetApartmentByIdAsync(string apartmentId);

        Task<Result<IEnumerable<CountrySearchDTO>>> GetAllCountriesAsync();

        Task<Result<CountrySearchDTO>> GetCountryByIdAsync(string countryId);
    }
}
