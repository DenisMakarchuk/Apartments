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
    public class CommentAdministrationService_Test
    {
        private Faker<User> _fakeUser = new Faker<User>().RuleFor(x => x.Name, y => y.Person.FullName.ToString());
        private Faker<Apartment> _fakeApartment = new Faker<Apartment>().RuleFor(x => x.IsOpen, true)
            .RuleFor(x=>x.Price, y=>y.Random.Decimal(5M,15M))
            .RuleFor(x => x.Title, y => y.Name.JobTitle())
            .RuleFor(x => x.Text, y => y.Name.JobDescriptor());
        private Faker<Comment> _fakeComment = new Faker<Comment>()
            .RuleFor(x => x.Title, y => y.Name.JobTitle())
            .RuleFor(x => x.Text, y => y.Name.JobDescriptor());


        List<User> _users;
        List<Apartment> _apartments;
        List<Comment> _comments;

        IMapper _mapper;

        public CommentAdministrationService_Test()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommentDTOAdministration, Comment>().ReverseMap();
            });

            _mapper = new Mapper(mapperConfig);

            _users = _fakeUser.Generate(2);
            _apartments = _fakeApartment.Generate(2);
            _comments = _fakeComment.Generate(2);
        }

        [Fact]
        public void GetAllCommentsByUserIdAsync_PositiveAndNegative_Test()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetAllCommentsByUserIdAsync_PositiveAndNegative_Test")
                .Options;

            User userWithComments;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                context.SaveChanges();

                userWithComments = context.Users.AsNoTracking().FirstOrDefault();

                foreach (var item in _comments)
                {
                    item.AuthorId = userWithComments.Id;
                }

                context.AddRange(_comments);
                context.SaveChanges();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new CommentAdministrationService(context, _mapper);

                var commentsInBase = context.Comments.AsNoTracking().ToList();
                var userWithoutComments = context.Users.Where(_ => _.Id != userWithComments.Id).FirstOrDefault();

                var resultPositive = service.GetAllCommentsByUserIdAsync(userWithComments.Id.ToString());
                var resultNegative = service.GetAllCommentsByUserIdAsync(userWithoutComments.Id.ToString());

                foreach (var item in commentsInBase)
                {
                    resultPositive.Result.Data
                        .Where(_=>_.Id == item.Id.ToString())
                        .FirstOrDefault()
                        .Should().NotBeNull();
                }

                resultNegative.Result.IsSuccess.Should().BeFalse();
            }
        }
    }
}
