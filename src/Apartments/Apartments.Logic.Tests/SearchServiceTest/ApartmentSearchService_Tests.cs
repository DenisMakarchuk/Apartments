using Apartments.Data.Context;
using Apartments.Data.DataModels;
using Apartments.Domain.Logic.Search.SearchServices;
using Apartments.Domain.Logic.Users.UserService;
using Apartments.Domain.Search.DTO;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using Apartments.Domain.Users.ViewModels;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Apartments.Logic.Tests.SearchServiceTest
{
    public class ApartmentSearchService_Tests
    {
        private Faker<User> _fakeUser = new Faker<User>()
            .RuleFor(x => x.Name, y => y.Person.FullName.ToString());

        private Faker<Apartment> _fakeApartment = new Faker<Apartment>()
            .RuleFor(x => x.IsOpen, true)
            .RuleFor(x => x.Price, y => y.Random.Decimal(5M, 15M))
            .RuleFor(x => x.Title, y => y.Name.JobTitle())
            .RuleFor(x => x.Text, y => y.Name.JobDescriptor());

        private Faker<Address> _fakeAddress = new Faker<Address>()
            .RuleFor(x => x.City, y => y.Address.City())
            .RuleFor(x => x.Street, y => y.Address.StreetName())
            .RuleFor(x => x.Home, y => y.Random.Int(1, 10).ToString())
            .RuleFor(x => x.NumberOfApartment, y => y.Random.Int(1, 10));

        List<User> _users;
        List<Apartment> _apartments;
        List<Address> _addresses;

        IMapper _mapper;

        public ApartmentSearchService_Tests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Apartment, ApartmentSearchDTO>().ReverseMap();

                cfg.CreateMap<Address, AddressSearchDTO>().ReverseMap();

                cfg.CreateMap<Country, CountrySearchDTO>().ReverseMap();
            });

            _mapper = new Mapper(mapperConfig);

            _users = _fakeUser.Generate(2);
            _apartments = _fakeApartment.Generate(2);
            _addresses = _fakeAddress.Generate(2);
        }

        [Fact]
        public async void GetAllApartmentsAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetAllApartmentByUserIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            User userWithApartments;

            using (var context = new ApartmentContext(options))
            {
                Country[] countries = new Country[]
                {
                    new Country()
                    {
                        Name = "Litva"
                    },

                    new Country()
                    {
                        Name = "Poland"
                    }
                };

                context.AddRange(countries);

                context.AddRange(_users);
                await context.SaveChangesAsync();

                userWithApartments = context.Users.AsNoTracking().FirstOrDefault();

                Address[] addresses = new Address[]
                {
                    new Address()
                    {
                        CountryId = context.Countries.FirstOrDefault().Id,
                        City = "Slonim",
                        Home = "40",
                        Street = "S",
                        NumberOfApartment = 7
                    },

                    new Address()
                    {
                        CountryId = context.Countries.LastOrDefault().Id,
                        City = "Minsk",
                        Home = "8",
                        Street = "S",
                        NumberOfApartment = 32
                    }
                };

                Apartment[] apartments = new Apartment[]
                {
                    new Apartment()
                    {
                        Address = addresses.FirstOrDefault(),
                        IsOpen = true,
                        Area = 45,
                        Price = 35m,
                        Text ="Text",
                        Title = "Open",
                        OwnerId = userWithApartments.Id,
                        NumberOfRooms = 3
                    },

                    new Apartment()
                    {
                        Address = addresses.LastOrDefault(),
                        IsOpen = false,
                        Area = 45,
                        Price = 20m,
                        Text ="Text",
                        Title = "Close",
                        OwnerId = userWithApartments.Id,
                        NumberOfRooms = 5
                    }
                };

                context.AddRange(apartments);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new ApartmentSearchService(context, _mapper);
                var countryFirst = await context.Countries.FirstOrDefaultAsync();
                var countryLast = await context.Countries.LastOrDefaultAsync();

                var apartmentsInBase = await context.Apartments.AsNoTracking().ToListAsync();

                var resultPositive = await service.GetAllApartmentsAsync(
                    null,
                    null,
                    3,
                    0,
                    0,
                    0,
                    null
                    );


                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.FirstOrDefault().Title.Should().BeEquivalentTo("Open");

                var resultNegative = await service.GetAllApartmentsAsync(
                    null,
                    null,
                    0,
                    2,
                    0,
                    0,
                    null
                    );

                resultNegative.IsSuccess.Should().BeFalse();
            }
        }

        [Fact]
        public async void GetApartmentByIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetApartmentByIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                Country country = new Country()
                {
                    Name = "Litva"
                };

                context.Add(country);

                context.AddRange(_users);
                await context.SaveChangesAsync();

                var userWithApartments = context.Users.AsNoTracking().FirstOrDefault();

                foreach (var item in _addresses)
                {
                    item.CountryId = context.Countries.FirstOrDefault().Id;
                }

                for (int i = 0; i < 2; i++)
                {
                    _apartments[i].OwnerId = userWithApartments.Id;
                    _apartments[i].Address = _addresses[i];
                }

                context.AddRange(_apartments);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var apartment = await context.Apartments.AsNoTracking().FirstOrDefaultAsync();

                var service = new ApartmentSearchService(context, _mapper);

                var resultPositive = await service.GetApartmentByIdAsync(apartment.Id.ToString());
                var resultNegative = await service.GetApartmentByIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Apartment.Title.Should().BeEquivalentTo(apartment.Title);
                resultPositive.Data.Country.Name.Should().BeEquivalentTo("Litva");

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Data.Should().BeNull();
            }
        }
    }
}
