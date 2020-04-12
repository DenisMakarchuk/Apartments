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
    public class UserService_Tests
    {
        private Faker<User> _fakeUser = new Faker<User>().RuleFor(x => x.Id, new Guid());
        List<User> _users;
        IMapper _mapper;

        public UserService_Tests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>().ReverseMap();
                cfg.CreateMap<AddUser, User>().ReverseMap();
            });

            _mapper = new Mapper(mapperConfig);

            _users = _fakeUser.Generate(3);
        }

        [Fact]
        public async void CreateUserAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "CreateUserAsync_Positive_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                var service = new UserService(context, _mapper);

                AddUser user = new AddUser()
                {
                    Id = Guid.NewGuid().ToString()
                };

                AddUser failAddUser = new AddUser()
                {
                    Id = user.Id
                };

                var resultPositive = await service.CreateUserProfileAsync(user.Id);
                //var resultNegative = await service.CreateUserProfileAsync(failAddUser.Id);

                resultPositive.Data.Id.Should().BeEquivalentTo(user.Id);

                //resultNegative.IsSuccess.Should().BeFalse();
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

                var service = new UserService(context, _mapper);

                var resultPositive = await service.GetUserProfileByIdentityIdAsync(user.Id.ToString());
                var resultNegative = await service.GetUserProfileByIdentityIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Id.Should().BeEquivalentTo(user.Id.ToString());

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Data.Should().BeNull();
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

                var service = new UserService(context, _mapper);

                var resultPositive = await service.DeleteUserProfileByIdentityIdAsync(user.Id.ToString());
                var resultNegative = await service.DeleteUserProfileByIdentityIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Message.Should().BeNull();

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Message.Should().BeNull();
            }
        }
    }
}
