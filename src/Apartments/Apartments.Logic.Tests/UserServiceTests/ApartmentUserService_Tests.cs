using Apartments.Data.Context;
using Apartments.Data.DataModels;
using Apartments.Domain.Logic.Users.UserService;
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

namespace Apartments.Logic.Tests.UserServiceTests
{
    public class ApartmentUserService_Tests
    {
        private Faker<User> _fakeUser = new Faker<User>()
            .RuleFor(x => x.Id, new Guid());

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

                cfg.CreateMap<CountryDTO, Country>().ReverseMap();
            });

            _mapper = new Mapper(mapperConfig);

            _users = _fakeUser.Generate(2);
            _apartments = _fakeApartment.Generate(2);
            _addresses = _fakeAddress.Generate(2);
        }

        [Fact]
        public async void CreateApartmentAsync_Positive_TestAsync()
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

                AddApartment apartmentOk = new AddApartment()
                {
                    Address = address,
                    Area = 54,
                    IsOpen = true,
                    Price = 15M,
                    Title = "Apartment",
                    Text = "AddedApartment",
                    NumberOfRooms = 2
                };

                var resultPositive = await service.CreateApartmentAsync(apartmentOk, user.Id.ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Country.Id.Should().BeEquivalentTo(address.CountryId);
                resultPositive.Data.Address.Street.Should().BeEquivalentTo(address.Street);
                resultPositive.Data.Apartment.Title.Should().BeEquivalentTo(apartmentOk.Title);
            }
        }

        [Fact]
        public async void GetAllApartmentByUserIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetAllApartmentByUserIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            User userWithApartments;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                await context.SaveChangesAsync();

                userWithApartments = context.Users.AsNoTracking().FirstOrDefault();

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
                var service = new ApartmentUserService(context, _mapper);

                var apartmentsInBase = await context.Apartments.AsNoTracking().ToListAsync();
                var userWithoutApartments = await context.Users.Where(_ => _.Id != userWithApartments.Id).FirstOrDefaultAsync();

                var resultPositive = await service.GetAllApartmentByOwnerIdAsync(userWithApartments.Id.ToString());
                var resultNegative = await service.GetAllApartmentByOwnerIdAsync(userWithoutApartments.Id.ToString());

                foreach (var item in apartmentsInBase)
                {
                    resultPositive.Data
                        .Where(_ => _.Apartment.Id == item.Id.ToString())
                        .FirstOrDefault()
                        .Should().NotBeNull();
                }

                resultNegative.Data.Should().BeEmpty();
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
                var apartment = await context.Apartments.AsNoTracking()
                    .Include(_ => _.Address.Country).FirstOrDefaultAsync();

                var service = new ApartmentUserService(context, _mapper);

                var resultPositive = await service.GetApartmentByIdAsync(apartment.Id.ToString());
                var resultNegative = await service.GetApartmentByIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Apartment.Title.Should().BeEquivalentTo(apartment.Title);
                resultPositive.Data.Country.Name.Should().BeEquivalentTo(apartment.Address.Country.Name);

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Data.Should().BeNull();
            }
        }

        [Fact]
        public async void UpdateApartmentAsync_Positive_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "UpdateApartmentAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                await context.AddRangeAsync(_users);
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

                await context.AddRangeAsync(_apartments);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var apartment = await context.Apartments
                    .Include(_ => _.Address.Country).Include(_ => _.Address)
                    .AsNoTracking().FirstOrDefaultAsync();

                var newCountry = await context.Countries.Where(_ => _.Id != apartment.Address.CountryId).FirstOrDefaultAsync();

                ApartmentView view = new ApartmentView()
                {

                    Apartment = _mapper.Map<ApartmentDTO>(apartment),

                    Address = _mapper.Map<AddressDTO>(apartment.Address),

                    Country = _mapper.Map<CountryDTO>(apartment.Address.Country)
                };

                view.Address.City = "Updated";
                view.Apartment.Title = "Updated";
                view.Country = _mapper.Map<CountryDTO>(newCountry);

                var service = new ApartmentUserService(context, _mapper);

                var resultPositive = await service.UpdateApartmentAsync(view);

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Apartment.Title.Should().BeEquivalentTo("Updated");
                resultPositive.Data.Address.City.Should().BeEquivalentTo("Updated");
                resultPositive.Data.Country.Name.Should().BeEquivalentTo(newCountry.Name);
            }
        }

        [Fact]
        public async void DeleteApartmentByIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "DeleteApartmentByIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                await context.AddRangeAsync(_users);
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

                await context.AddRangeAsync(_apartments);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var apartmenr = await context.Apartments.AsNoTracking().FirstOrDefaultAsync();

                var service = new ApartmentUserService(context, _mapper);

                var resultPositive = await service.DeleteApartmentByIdAsync(apartmenr.Id.ToString(), apartmenr.OwnerId.ToString());
                var resultNegative = await service.DeleteApartmentByIdAsync(new Guid().ToString(), new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Message.Should().BeNull();

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Message.Should().Contain("Apartment was not found");
            }
        }
    }
}