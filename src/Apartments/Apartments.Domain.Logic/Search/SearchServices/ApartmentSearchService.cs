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
        /// <param name="search"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<ApartmentSearchDTO>>> 
            GetAllApartmentsAsync(SearchParameters search, CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<Apartment> apartments = _db.Apartments.Where(_=>_.IsOpen == true);

            if (!string.IsNullOrEmpty(search?.CountryId))
            {
                Guid id = Guid.Parse(search.CountryId);

                apartments = apartments.Where(_ => _.Address.CountryId == id);
            }

            if (!string.IsNullOrEmpty(search?.CityName))
            {
                apartments = apartments.Where(_ => _.Address.City.Contains(search.CityName));
            }

            if (search?.RoomsFrom > 0)
            {
                apartments = apartments.Where(_ => _.NumberOfRooms >= search.RoomsFrom);
            }

            if (search?.RoomsTill > 0 && search.RoomsTill >= search.RoomsFrom)
            {
                apartments = apartments.Where(_ => _.NumberOfRooms <= search.RoomsTill);
            }

            if (search?.PriceFrom > 0)
            {
                apartments = apartments.Where(_ => _.Price >= search.PriceFrom);
            }

            if (search?.PriceTill > 0 && search?.PriceTill >= search.PriceFrom)
            {
                apartments = apartments.Where(_ => _.Price <= search.PriceTill);
            }

            if (search?.NeedDates != null && search.NeedDates.Any())
            {
                foreach (var item in search.NeedDates)
                {
                    apartments = apartments.Where(_ => _.Dates.Where(_ => _.Date == item.Date).FirstOrDefault() == null);
                }
            }

            try
            {
                var result = _mapper.Map<IEnumerable<ApartmentSearchDTO>>(await apartments.ToListAsync(cancellationToken));

                if (!result.Any())
                {
                    return (Result<IEnumerable<ApartmentSearchDTO>>)Result<IEnumerable<ApartmentSearchDTO>>
                        .NoContent<IEnumerable<ApartmentSearchDTO>>();
                }

                return (Result<IEnumerable<ApartmentSearchDTO>>)Result<IEnumerable<ApartmentSearchDTO>>
                    .Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return (Result<IEnumerable<ApartmentSearchDTO>>)Result<IEnumerable<ApartmentSearchDTO>>
                    .Fail<IEnumerable<ApartmentSearchDTO>>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get Apartment by Id
        /// </summary>
        /// <param name="apartmentId"></param>
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
                        .NoContent<ApartmentSearchView>();
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
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<CountrySearchDTO>>> 
            GetAllCountriesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var countries = await _db.Countries.AsNoTracking().ToListAsync(cancellationToken);

                if (!countries.Any())
                {
                    return (Result<IEnumerable<CountrySearchDTO>>)Result<IEnumerable<CountrySearchDTO>>
                        .NoContent<IEnumerable<CountrySearchDTO>>();
                }

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
        /// Get Country by Id
        /// </summary>
        /// <param name="countryId"></param>
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
                        .NoContent<CountrySearchDTO>();

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
