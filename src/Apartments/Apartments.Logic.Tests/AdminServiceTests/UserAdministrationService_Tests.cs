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
        private Faker<User> _fakeUser = new Faker<User>().RuleFor(x => x.IdentityId, Guid.NewGuid().ToString());
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
        public async void GetUserProfileByIdentityIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetUserProfileByIdentityIdAsync_PositiveAndNegative_TestAsync")
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

                var resultPositive = await service.GetUserProfileByIdentityIdAsync(user.IdentityId.ToString());
                var resultNegative = await service.GetUserProfileByIdentityIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.IdentityId.Should().BeEquivalentTo(user.IdentityId);

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Data.Should().BeNull();
            }
        }

        [Fact]
        public async void DeleteUserProfileByIdentityIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "DeleteUserProfileByIdentityIdAsync_PositiveAndNegative_TestAsync")
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

                var resultPositive = await service.DeleteUserProfileByIdentityIdAsync(user.IdentityId.ToString());
                var resultNegative = await service.DeleteUserProfileByIdentityIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Message.Should().BeNull();

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Message.Should().BeNull();
            }
        }
    }
}
