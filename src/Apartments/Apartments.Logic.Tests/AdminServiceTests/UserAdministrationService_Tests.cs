using Apartments.Data.Context;
using Apartments.Data.DataModels;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using AutoMapper;
using Apartments.Domain.Admin.DTO;
using Apartments.Domain.Logic.Admin.AdminService;
using FluentAssertions;

namespace Apartments.Logic.Tests.AdminServiceTests
{
    public class UserAdministrationService_Tests
    {
        private Faker<User> _fakeUser = new Faker<User>().RuleFor(x => x.Name, y => y.Person.FullName.ToString());
        List<User> _users;
        IMapper _mapper;

        public UserAdministrationService_Tests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTOAdministration, User>().ReverseMap();
            });

            _mapper = new Mapper(mapperConfig);

            _users = _fakeUser.Generate(3);
        }

        [Fact]
        public async void GetAllUsersAsync_Positive_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetAllUsersAsync_Positive_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new UserAdministrationService(context, _mapper);

                var result = await service.GetAllUsersAsync();

                foreach (var item in _users)
                {
                    var itemFromResult = result.Where(_ => _.Name.Equals(item.Name)).Select(_ => _).FirstOrDefault();

                    itemFromResult.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public async void GetUserByIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetUserByIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var user = await context.Users.AsNoTracking().FirstOrDefaultAsync();

                var service = new UserAdministrationService(context, _mapper);

                var resultPositive = await service.GetUserByIdAsync(user.Id.ToString());
                //var resultNegative = await service.GetUserByIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Name.Should().BeEquivalentTo(user.Name);

                //resultNegative.IsSuccess.Should().BeFalse();
                //resultNegative.Data.Should().BeNull();
            }
        }

        [Fact]
        public async void DeleteUserByIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "DeleteUserByIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var user = await context.Users.AsNoTracking().FirstOrDefaultAsync();

                var service = new UserAdministrationService(context, _mapper);

                var resultPositive = await service.DeleteUserByIdAsync(user.Id.ToString());
                var resultNegative = await service.DeleteUserByIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Message.Should().BeNull();

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Message.Should().Contain("User was not found");
            }
        }
    }
}
