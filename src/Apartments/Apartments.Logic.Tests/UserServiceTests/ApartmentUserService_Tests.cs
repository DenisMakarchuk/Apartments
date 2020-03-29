using Apartments.Data.Context;
using Apartments.Data.DataModels;
using Apartments.Domain.Logic.Users.UserService;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Apartments.Logic.Tests.UserServiceTests
{
    public class ApartmentUserService_Tests
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

        public ApartmentUserService_Tests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddApartment, Apartment>();
                cfg.CreateMap<ApartmentDTO, Apartment>().ReverseMap();

                cfg.CreateMap<AddAddress, Address>();
                cfg.CreateMap<AddressDTO, Address>().ReverseMap();

                cfg.CreateMap<CountryDTO, Country>();
            });

            _mapper = new Mapper(mapperConfig);

            _users = _fakeUser.Generate(2);
            _apartments = _fakeApartment.Generate(2);
            _addresses = _fakeAddress.Generate(2);
        }

        [Fact]
        public async void CreateApartmentAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "CreateApartmentAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new ApartmentUserService(context, _mapper);

                User user = await context.Users.FirstOrDefaultAsync();

                AddAddress address = new AddAddress()
                {
                    CountryId = context.Countries.FirstOrDefault().Id.ToString(),
                    City = "MyCity",
                    Street = "Street",
                    Home = "Home",
                    NumberOfApartment = 1
                };

                AddApartment apartment = new AddApartment()
                {
                    Address = address,
                    Area = 54,
                    IsOpen = true,
                    OwnerId = context.Users.FirstOrDefault().Id.ToString(),
                    Price = 15M,
                    Title = "Apartment",
                    Text = "AddedApartment",
                    NumberOfRooms = 2
                };

                var resultPositive = await service.CreateApartmentAsync(apartment);
                var resultNegative = await service.CreateApartmentAsync(new AddApartment());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Country.Id.Should().BeEquivalentTo(address.CountryId);
                resultPositive.Data.Address.Street.Should().BeEquivalentTo(address.Street);
                resultPositive.Data.Apartment.Title.Should().BeEquivalentTo(apartment.Title);

                resultNegative.IsSuccess.Should().BeFalse();
            }
        }
    }
}
