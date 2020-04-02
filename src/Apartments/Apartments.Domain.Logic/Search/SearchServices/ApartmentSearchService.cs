﻿using Apartments.Common;
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
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Search.SearchServices
{
    public class ApartmentSearchService : IApartmentSearchService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public ApartmentSearchService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ApartmentSearchDTO>>> GetAllApartmentsAsync(
            string countryId,
            string cityName,
            int roomsFrom,
            int roomsTill,
            decimal priceFrom,
            decimal priceTill,
            IEnumerable<DateTime> needDates)
        {
            IQueryable<Apartment> apartments = _db.Apartments.Where(_=>_.IsOpen == true);


            if (countryId != null)
            {
                Guid id = Guid.Parse(countryId);

                apartments = apartments.Where(_ => _.Address.CountryId == id);
            }

            if (cityName != null)
            {
                apartments = apartments.Where(_ => _.Address.City.Contains(cityName));
            }

            if (roomsFrom > 0)
            {
                apartments = apartments.Where(_ => _.NumberOfRooms >= roomsFrom);
            }

            if (roomsTill > 0 && roomsTill >= roomsFrom)
            {
                apartments = apartments.Where(_ => _.NumberOfRooms <= roomsTill);
            }

            if (priceFrom > 0)
            {
                apartments = apartments.Where(_ => _.Price >= priceFrom);
            }

            if (priceTill > 0 && priceTill >= priceFrom)
            {
                apartments = apartments.Where(_ => _.Price <= priceTill);
            }

            if (needDates!=null && needDates.Any())
            {
                foreach (var item in needDates)
                {
                    apartments = apartments.Where(_ => _.Dates.Where(_ => _.Date.Date == item.Date).First() == null);
                }
            }

            try
            {
                var result = _mapper.Map<IEnumerable<ApartmentSearchDTO>>(apartments.ToList());

                if (!result.Any())
                {
                    return (Result<IEnumerable<ApartmentSearchDTO>>)Result<IEnumerable<ApartmentSearchDTO>>
                        .Fail<IEnumerable<ApartmentSearchDTO>>("No Apartments with this parameters");
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

        public async Task<Result<ApartmentSearchView>> GetApartmentByIdAsync(string apartmentId)
        {
            Guid id = Guid.Parse(apartmentId);

            try
            {
                var apartment = await _db.Apartments.Where(_ => _.Id == id)
                    .Include(_ => _.Address.Country).Include(_ => _.Address)
                    .AsNoTracking().FirstOrDefaultAsync();

                if (apartment is null)
                {
                    return (Result<ApartmentSearchView>)Result<ApartmentSearchView>
                        .Fail<ApartmentSearchView>($"Apartment was not found");
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

        public async Task<Result<IEnumerable<CountrySearchDTO>>> GetAllCountriesAsync()
        {
            var countries = await _db.Countries.AsNoTracking().ToListAsync();

            if (!countries.Any())
            {
                return (Result<IEnumerable<CountrySearchDTO>>)Result<IEnumerable<CountrySearchDTO>>
                    .Fail<IEnumerable<CountrySearchDTO>>("No Countries found");
            }

            return (Result<IEnumerable<CountrySearchDTO>>)Result<IEnumerable<CountrySearchDTO>>
                .Ok(_mapper.Map<IEnumerable<CountrySearchDTO>>(countries));
        }

        public async Task<Result<CountrySearchDTO>> GetCountryByIdAsync(string countryId)
        {
            Guid id = Guid.Parse(countryId);

            try
            {
                var country = await _db.Countries.Where(_ => _.Id == id).AsNoTracking().FirstOrDefaultAsync();

                if (country is null)
                {
                    return (Result<CountrySearchDTO>)Result<CountrySearchDTO>
                        .Fail<CountrySearchDTO>($"Country was not found");

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