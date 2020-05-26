using Apartments.Common;
using Apartments.Data.Context;
using Apartments.Data.DataModels;
using Apartments.Domain.Logic.Search.SearchServiceInterfaces;
using Apartments.Domain.Search.DTO;
using Apartments.Domain.Search.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Search.SearchServices
{
    /// <summary>
    /// Apartment Search methods
    /// </summary>
    public class ApartmentSearchService : IApartmentSearchService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public ApartmentSearchService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Apartments by Parameters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<PagedResponse<ApartmentSearchView>>> 
            GetAllApartmentsAsync(PagedRequest<SearchParameters> request, CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<Apartment> apartments = _db.Apartments.Where(_=>_.IsOpen == true);

            if (!string.IsNullOrEmpty(request.Data.CountryId) && Guid.TryParse(request.Data.CountryId, out var _))
            {
                Guid id = Guid.Parse(request.Data.CountryId);

                apartments = apartments.Where(_ => _.Address.CountryId == id);
            }

            if (!string.IsNullOrEmpty(request.Data.CityName))
            {
                apartments = apartments.Where(_ => _.Address.City.Contains(request.Data.CityName));
            }

            if (request.Data.RoomsFrom > 0)
            {
                apartments = apartments.Where(_ => _.NumberOfRooms >= request.Data.RoomsFrom);
            }

            if (request.Data.RoomsTill > 0 && request.Data.RoomsTill >= request.Data.RoomsFrom)
            {
                apartments = apartments.Where(_ => _.NumberOfRooms <= request.Data.RoomsTill);
            }

            if (request.Data.PriceFrom > 0)
            {
                apartments = apartments.Where(_ => _.Price >= request.Data.PriceFrom);
            }

            if (request.Data.PriceTill > 0 && request.Data.PriceTill >= request.Data.PriceFrom)
            {
                apartments = apartments.Where(_ => _.Price <= request.Data.PriceTill);
            }

            if (request.Data.NeedDates != null && request.Data.NeedDates.Any())
            {
                foreach (var item in request.Data.NeedDates)
                {
                    apartments = apartments.Where(_ => _.Dates.Where(_ => _.Date == item.Date).FirstOrDefault() == null);
                }
            }

            try
            {
                var count = await apartments.CountAsync();

                var searchResult = await apartments.Include(_ => _.Address.Country)
                                               .Include(_ => _.Address)
                                               .Skip((request.PageNumber - 1) * request.PageSize)
                                               .Take(request.PageSize)
                                               .AsNoTracking().ToListAsync(cancellationToken);

                List<ApartmentSearchView> result = new List<ApartmentSearchView>();

                foreach (var apartment in searchResult)
                {
                    ApartmentSearchView view = new ApartmentSearchView()
                    {

                        Apartment = _mapper.Map<ApartmentSearchDTO>(apartment),

                        Address = _mapper.Map<AddressSearchDTO>(apartment.Address),

                        Country = _mapper.Map<CountrySearchDTO>(apartment.Address.Country)
                    };

                    result.Add(view);
                }

                PagedResponse<ApartmentSearchView> response
                = new PagedResponse<ApartmentSearchView>(_mapper.Map<IEnumerable<ApartmentSearchView>>(result),
                                                          count,
                                                          request.PageNumber,
                                                          request.PageSize);

                return (Result<PagedResponse<ApartmentSearchView>>)Result<PagedResponse<ApartmentSearchView>>
                    .Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                return (Result<PagedResponse<ApartmentSearchView>>)Result<PagedResponse<ApartmentSearchView>>
                    .Fail<PagedResponse<ApartmentSearchView>>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get Apartment by Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<ApartmentSearchView>> 
            GetApartmentByIdAsync(string apartmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(apartmentId);

            try
            {
                var apartment = await _db.Apartments.Where(_ => _.Id == id)
                    .Include(_ => _.Address.Country).Include(_ => _.Address)
                    .AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (apartment is null)
                {
                    return (Result<ApartmentSearchView>)Result<ApartmentSearchView>
                        .NotOk<ApartmentSearchView>(null, "Apartment is not exist");
                }

                ApartmentSearchView view = new ApartmentSearchView()
                {

                    Apartment = _mapper.Map<ApartmentSearchDTO>(apartment),

                    Address = _mapper.Map<AddressSearchDTO>(apartment.Address),

                    Country = _mapper.Map<CountrySearchDTO>(apartment.Address.Country)
                };

                return (Result<ApartmentSearchView>)Result<ApartmentSearchView>
                    .Ok(view);
            }
            catch (ArgumentNullException ex)
            {
                return (Result<ApartmentSearchView>)Result<ApartmentSearchView>
                    .Fail<ApartmentSearchView>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get all countries from DB
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<CountrySearchDTO>>> 
            GetAllCountriesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var countries = await _db.Countries.OrderBy(s=>s.Name).AsNoTracking().ToListAsync(cancellationToken);

                return (Result<IEnumerable<CountrySearchDTO>>)Result<IEnumerable<CountrySearchDTO>>
                    .Ok(_mapper.Map<IEnumerable<CountrySearchDTO>>(countries));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<IEnumerable<CountrySearchDTO>>)Result<IEnumerable<CountrySearchDTO>>
                    .Fail<IEnumerable<CountrySearchDTO>>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get Country by Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<CountrySearchDTO>> 
            GetCountryByIdAsync(string countryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(countryId);

            try
            {
                var country = await _db.Countries.Where(_ => _.Id == id)
                    .AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (country is null)
                {
                    return (Result<CountrySearchDTO>)Result<CountrySearchDTO>
                        .NotOk<CountrySearchDTO>(null, "Country is not exist");

                }

                return (Result<CountrySearchDTO>)Result<CountrySearchDTO>
                    .Ok(_mapper.Map<CountrySearchDTO>(country));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<CountrySearchDTO>)Result<CountrySearchDTO>
                    .Fail<CountrySearchDTO>($"Source is null. {ex.Message}");
            }
        }
    }
}