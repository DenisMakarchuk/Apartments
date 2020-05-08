using Apartments.Common;
using Apartments.Domain.Search.DTO;
using Apartments.Domain.Search.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Search.SearchServiceInterfaces
{
    /// <summary>
    /// Apartment Search methods
    /// </summary>
    public interface IApartmentSearchService
    {
        /// <summary>
        /// Get all Apartments by Parameters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedResponse<ApartmentSearchView>>> 
            GetAllApartmentsAsync(PagedRequest<SearchParameters> request, CancellationToken cancellationToken);

        /// <summary>
        /// Get Apartment by Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ApartmentSearchView>> 
            GetApartmentByIdAsync(string apartmentId, CancellationToken cancellationToken);

        /// <summary>
        /// Get all countries from DB
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<CountrySearchDTO>>> 
            GetAllCountriesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get Country by Id
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<CountrySearchDTO>> 
            GetCountryByIdAsync(string countryId, CancellationToken cancellationToken);
    }
}