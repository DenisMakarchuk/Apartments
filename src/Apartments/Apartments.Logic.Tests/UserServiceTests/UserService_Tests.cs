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
        private Faker<User> _fakeUser = new Faker<User>().RuleFor(x => x.Name, y => y.Person.FullName.ToString());
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

                var user = _mapper.Map<AddUser>(_users.FirstOrDefault());

                AddUser failAddUser = new AddUser()
                {
                    Name = user.Name
                };

                var resultPositive = await service.CreateUserAsync(user);
                //var resultNegative = await service.CreateUserAsync(failAddUser);

                resultPositive.Data.Name.Should().BeEquivalentTo(user.Name);

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

                var resultPositive = await service.GetUserByIdAsync(user.Id.ToString());
                var resultNegative = await service.GetUserByIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Name.Should().BeEquivalentTo(user.Name);

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Data.Should().BeNull();
            }
        }

        [Fact]
        public async void UpdateUserAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "UpdateUserAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var user = await context.Users.AsNoTracking().FirstOrDefaultAsync();
                var userForUpdate = _mapper.Map<UserDTO>(user);

                userForUpdate.Name = "Updated";
                UserDTO failUserForUpdate = new UserDTO()
                {
                    Id = new Guid().ToString(),
                    Name = "Fail"
                };

                var service = new UserService(context, _mapper);

                var resultPositive = await service.UpdateUserAsync(userForUpdate);
                var resultNegative = await service.UpdateUserAsync(failUserForUpdate);

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Name.Should().BeEquivalentTo(userForUpdate.Name);

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
