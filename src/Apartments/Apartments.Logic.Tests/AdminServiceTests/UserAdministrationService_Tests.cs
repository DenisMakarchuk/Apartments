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
        static Faker<User> _fakeUser = new Faker<User>().RuleFor(x => x.Name, y => y.Person.FullName.ToString());
        IMapper _mapper;

        public UserAdministrationService_Tests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTOAdministration, User>().ReverseMap();
            });

            _mapper = new Mapper(mapperConfig);
        }

        [Fact]
        public void GetAllUsersTest()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetAllUsersTest")
                .Options;

            // Run the test against one instance of the context
            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_fakeUser.Generate(3));
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApartmentContext(options))
            {
                var service = new UserAdministrationService(context, _mapper);
                var result = service.GetAll();

                result.Result.ToList().Should().NotBeNull();
            }
        }
    }
}
