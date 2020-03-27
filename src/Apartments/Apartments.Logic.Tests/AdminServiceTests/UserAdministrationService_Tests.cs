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
        public void GetAllUsersAsync_Positive_Test()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetAllUsersAsync_Positive_Test")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                context.SaveChanges();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new UserAdministrationService(context, _mapper);

                var result = service.GetAllUsersAsync();

                foreach (var item in _users)
                {
                    var itemFromResult = result.Result.Where(_ => _.Name.Equals(item.Name)).Select(_ => _).FirstOrDefault();

                    itemFromResult.Should().NotBeNull();
                    Guid.TryParse(itemFromResult.Id.ToString(), out var _).Should().BeTrue();
                }
            }
        }

        [Fact]
        public void GetUserByIdAsync_PositiveAndNegative_Test()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetUserByIdAsync_PositiveAndNegative_Test")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                context.SaveChanges();
            }

            using (var context = new ApartmentContext(options))
            {
                var user = _users.FirstOrDefault();

                var service = new UserAdministrationService(context, _mapper);

                var resultPositive = service.GetUserByIdAsync(user.Id.ToString());
                var resultNegative = service.GetUserByIdAsync(new Guid().ToString());

                resultPositive.Result.IsSuccess.Should().BeTrue();
                resultPositive.Result.Data.Name.Should().BeEquivalentTo(user.Name);

                resultNegative.Result.IsSuccess.Should().BeFalse();
                resultNegative.Result.Data.Should().BeNull();
            }
        }

        [Fact]
        public void DeleteUserByIdAsync_PositiveAndNegative_Test()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "DeleteUserByIdAsync_PositiveAndNegative_Test")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                context.SaveChanges();
            }

            using (var context = new ApartmentContext(options))
            {
                var user = _users.FirstOrDefault();

                var service = new UserAdministrationService(context, _mapper);

                var resultPositive = service.DeleteUserByIdAsync(user.Id.ToString());
                var resultNegative = service.DeleteUserByIdAsync(new Guid().ToString());

                resultPositive.Result.IsSuccess.Should().BeTrue();
                resultPositive.Result.Message.Should().BeNull();

                resultNegative.Result.IsSuccess.Should().BeFalse();
                resultNegative.Result.Message.Should().Contain("User was not found");
            }
        }
    }
}
