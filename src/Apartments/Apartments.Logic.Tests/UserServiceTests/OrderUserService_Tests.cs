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
    public class OrderUserService_Tests
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

        public OrderUserService_Tests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddApartment, Apartment>();
                cfg.CreateMap<ApartmentDTO, Apartment>().ReverseMap();

                cfg.CreateMap<AddAddress, Address>();
                cfg.CreateMap<AddressDTO, Address>().ReverseMap();

                cfg.CreateMap<CountryDTO, Country>().ReverseMap();

                cfg.CreateMap<AddOrder, Order>()
                .ForMember(_=>_.Dates, _=>_.Ignore());
                cfg.CreateMap<OrderDTO, Order>()
                .ForMember(_ => _.Dates, _ => _.Ignore())
                .ReverseMap()
                .ForMember(_ => _.Dates, _ => _.Ignore());
            });

            _mapper = new Mapper(mapperConfig);

            _users = _fakeUser.Generate(2);
            _apartments = _fakeApartment.Generate(2);
            _addresses = _fakeAddress.Generate(2);
        }

        [Fact]
        public async void CreateOrderAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "CreateOrderAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                await context.SaveChangesAsync();

                User userWithApartments = context.Users.AsNoTracking().FirstOrDefault();

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
                var service = new OrderUserService(context, _mapper);

                User user = await context.Users.FirstOrDefaultAsync();

                Apartment apartment = await context.Apartments
                    .Include(_ => _.Address.Country).FirstOrDefaultAsync();

                IEnumerable<DateTime> dateTimes = new List<DateTime>()
                {
                    DateTime.Now.Date
                };

                AddOrder order = new AddOrder()
                {
                    ApartmentId = apartment.Id.ToString(),
                    CustomerId = user.Id.ToString(),
                    Dates = dateTimes
                };

                AddOrder failOrder = new AddOrder()
                {
                    ApartmentId = apartment.Id.ToString(),
                    CustomerId = user.Id.ToString(),
                    Dates = dateTimes
                };

                var resultPositive = await service.CreateOrderAsync(order);
                //var resultNegative = await service.CreateOrderAsync(failOrder);

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Country.Name.Should().BeEquivalentTo(apartment.Address.Country.Name);
                resultPositive.Data.Apartment.Title.Should().BeEquivalentTo(apartment.Title);
                resultPositive.Data.Order.Dates.FirstOrDefault().Should().BeSameDateAs(dateTimes.First());

                context.BusyDates.FirstOrDefault().Date.Should().BeSameDateAs(dateTimes.First());

                //resultNegative.IsSuccess.Should().BeFalse();
            }
        }

        [Fact]
        public async void GetAllOrdersByUserIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetAllOrdersByUserIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            User userWithOrders;

            using (var context = new ApartmentContext(options))
            {
                Country country = new Country()
                {
                    Name = "Litva"
                };

                context.Add(country);

                context.AddRange(_users);
                await context.SaveChangesAsync();

                userWithOrders = context.Users.AsNoTracking().FirstOrDefault();

                foreach (var item in _addresses)
                {
                    item.CountryId = context.Countries.FirstOrDefault().Id;
                }

                for (int i = 0; i < 2; i++)
                {
                    _apartments[i].OwnerId = userWithOrders.Id;
                    _apartments[i].Address = _addresses[i];
                }

                context.AddRange(_apartments);
                await context.SaveChangesAsync();

                Order order = new Order()
                {
                    ApartmentId = context.Apartments.FirstOrDefault().Id,
                    CustomerId = userWithOrders.Id,
                };

                List<BusyDate> busyDates = new List<BusyDate>();

                    BusyDate date = new BusyDate()
                    {
                        ApartmentId = context.Apartments.FirstOrDefault().Id,
                        Date = DateTime.Now.Date
                    };

                    busyDates.Add(date);

                order.Dates = _mapper.Map<HashSet<BusyDate>>(busyDates);

                context.AddRange(order);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new OrderUserService(context, _mapper);

                var ordersInBase = await context.Orders.AsNoTracking().ToListAsync();
                var userWithoutOrders = await context.Users.Where(_ => _.Id != userWithOrders.Id).FirstOrDefaultAsync();

                var resultPositive = await service.GetAllOrdersByUserIdAsync(userWithOrders.Id.ToString());
                var resultNegative = await service.GetAllOrdersByUserIdAsync(userWithoutOrders.Id.ToString());

                foreach (var item in ordersInBase)
                {
                    resultPositive.Data.FirstOrDefault()
                        .Order.CustomerId
                        .Should().BeEquivalentTo(item.CustomerId.ToString());
                }

                resultNegative.IsSuccess.Should().BeFalse();
            }
        }

        [Fact]
        public async void GetAllOrdersByApartmentIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetAllOrdersByApartmentIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            Apartment apartmentWithOrders;

            using (var context = new ApartmentContext(options))
            {
                Country country = new Country()
                {
                    Name = "Litva"
                };

                context.Add(country);

                context.AddRange(_users);
                await context.SaveChangesAsync();

                User userWithOrders = context.Users.AsNoTracking().FirstOrDefault();

                foreach (var item in _addresses)
                {
                    item.CountryId = context.Countries.FirstOrDefault().Id;
                }

                for (int i = 0; i < 2; i++)
                {
                    _apartments[i].OwnerId = userWithOrders.Id;
                    _apartments[i].Address = _addresses[i];
                }

                context.AddRange(_apartments);
                await context.SaveChangesAsync();

                apartmentWithOrders = context.Apartments.AsNoTracking().FirstOrDefault();

                Order order = new Order()
                {
                    ApartmentId = apartmentWithOrders.Id,
                    CustomerId = userWithOrders.Id,
                };

                List<BusyDate> busyDates = new List<BusyDate>();

                BusyDate date = new BusyDate()
                {
                    ApartmentId = apartmentWithOrders.Id,
                    Date = DateTime.Now.Date
                };

                busyDates.Add(date);

                order.Dates = _mapper.Map<HashSet<BusyDate>>(busyDates);

                context.AddRange(order);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new OrderUserService(context, _mapper);

                var ordersInBase = await context.Orders.AsNoTracking().ToListAsync();
                var apartmentWithoutOrders = await context.Users.Where(_ => _.Id != apartmentWithOrders.Id).FirstOrDefaultAsync();

                var resultPositive = await service.GetAllOrdersByApartmentIdAsync(apartmentWithOrders.Id.ToString());
                var resultNegative = await service.GetAllOrdersByApartmentIdAsync(apartmentWithoutOrders.Id.ToString());

                foreach (var item in ordersInBase)
                {
                    resultPositive.Data.FirstOrDefault()
                        .ApartmentId
                        .Should().BeEquivalentTo(item.ApartmentId.ToString());
                }

                resultNegative.IsSuccess.Should().BeFalse();
            }
        }

        [Fact]
        public async void GetOrderByIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetOrderByIdAsync_PositiveAndNegative_TestAsync")
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

                User userWithOrders = context.Users.AsNoTracking().FirstOrDefault();

                foreach (var item in _addresses)
                {
                    item.CountryId = context.Countries.FirstOrDefault().Id;
                }

                for (int i = 0; i < 2; i++)
                {
                    _apartments[i].OwnerId = userWithOrders.Id;
                    _apartments[i].Address = _addresses[i];
                }

                context.AddRange(_apartments);
                await context.SaveChangesAsync();

                Apartment apartmentWithOrders = context.Apartments.AsNoTracking().FirstOrDefault();

                Order order = new Order()
                {
                    ApartmentId = apartmentWithOrders.Id,
                    CustomerId = userWithOrders.Id,
                };

                List<BusyDate> busyDates = new List<BusyDate>();

                BusyDate date = new BusyDate()
                {
                    ApartmentId = apartmentWithOrders.Id,
                    Date = DateTime.Now.Date
                };

                busyDates.Add(date);

                order.Dates = _mapper.Map<HashSet<BusyDate>>(busyDates);

                context.AddRange(order);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new OrderUserService(context, _mapper);

                var orderInBase = await context.Orders.AsNoTracking().FirstOrDefaultAsync();

                var resultPositive = await service.GetOrderByIdAsync(orderInBase.Id.ToString());
                var resultNegative = await service.GetOrderByIdAsync(new Guid().ToString());

                resultPositive.Data.Order.Dates.FirstOrDefault().Should().BeSameDateAs(DateTime.Now.Date);

                resultNegative.IsSuccess.Should().BeFalse();
            }
        }

        [Fact]
        public async void UpdateOrderAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "UpdateOrderAsync_PositiveAndNegative_TestAsync")
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

                User userWithOrders = context.Users.AsNoTracking().FirstOrDefault();

                foreach (var item in _addresses)
                {
                    item.CountryId = context.Countries.FirstOrDefault().Id;
                }

                for (int i = 0; i < 2; i++)
                {
                    _apartments[i].OwnerId = userWithOrders.Id;
                    _apartments[i].Address = _addresses[i];
                }

                context.AddRange(_apartments);
                await context.SaveChangesAsync();

                Apartment apartmentWithOrders = context.Apartments.AsNoTracking().FirstOrDefault();

                Order order = new Order()
                {
                    ApartmentId = apartmentWithOrders.Id,
                    CustomerId = userWithOrders.Id,
                };

                List<BusyDate> busyDates = new List<BusyDate>();

                BusyDate date = new BusyDate()
                {
                    ApartmentId = apartmentWithOrders.Id,
                    Date = DateTime.Now.Date
                };

                busyDates.Add(date);

                order.Dates = _mapper.Map<HashSet<BusyDate>>(busyDates);

                context.AddRange(order);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new OrderUserService(context, _mapper);

                var orderInBase = await context.Orders.AsNoTracking().FirstOrDefaultAsync();
                var orderForUpdate = _mapper.Map<OrderDTO>(orderInBase);

                DateTime newDate = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);

                IEnumerable<DateTime> dates = new List<DateTime>() 
                {
                    newDate
                };

                orderForUpdate.Dates = dates;

                //OrderDTO failOrder = new OrderDTO()
                //{
                //    Id = new Guid().ToString(),
                //    ApartmentId = orderForUpdate.ApartmentId
                //};

                var resultPositive = await service.UpdateOrderAsync(orderForUpdate);
                //var resultNegative = await service.UpdateOrderAsync(failOrder);

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Order.Dates.FirstOrDefault().Should().BeSameDateAs(newDate);

                //resultNegative.IsSuccess.Should().BeFalse();
            }
        }

        [Fact]
        public async void DeleteOrderByIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "DeleteOrderByIdAsync_PositiveAndNegative_TestAsync")
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

                User userWithOrders = context.Users.AsNoTracking().FirstOrDefault();

                foreach (var item in _addresses)
                {
                    item.CountryId = context.Countries.FirstOrDefault().Id;
                }

                for (int i = 0; i < 2; i++)
                {
                    _apartments[i].OwnerId = userWithOrders.Id;
                    _apartments[i].Address = _addresses[i];
                }

                context.AddRange(_apartments);
                await context.SaveChangesAsync();

                Apartment apartmentWithOrders = context.Apartments.AsNoTracking().FirstOrDefault();

                Order order = new Order()
                {
                    ApartmentId = apartmentWithOrders.Id,
                    CustomerId = userWithOrders.Id,
                };

                List<BusyDate> busyDates = new List<BusyDate>();

                BusyDate date = new BusyDate()
                {
                    ApartmentId = apartmentWithOrders.Id,
                    Date = DateTime.Now.Date
                };

                busyDates.Add(date);

                order.Dates = _mapper.Map<HashSet<BusyDate>>(busyDates);

                context.AddRange(order);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var orderInBase = await context.Orders.AsNoTracking().FirstOrDefaultAsync();

                var service = new OrderUserService(context, _mapper);

                var resultPositive = await service.DeleteOrderByIdAsync(orderInBase.Id.ToString());
                var resultNegative = await service.DeleteOrderByIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Message.Should().BeNull();

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Message.Should().Contain("Order was not found");
            }
        }
    }
}
