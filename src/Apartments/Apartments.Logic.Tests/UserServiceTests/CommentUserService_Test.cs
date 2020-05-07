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
    public class CommentUserService_Test
    {
        private Faker<User> _fakeUser = new Faker<User>().RuleFor(x => x.Id, new Guid());
        private Faker<Apartment> _fakeApartment = new Faker<Apartment>().RuleFor(x => x.IsOpen, true)
            .RuleFor(x => x.Price, y => y.Random.Decimal(5M, 15M))
            .RuleFor(x => x.Title, y => y.Name.JobTitle())
            .RuleFor(x => x.Text, y => y.Name.JobDescriptor());
        private Faker<Comment> _fakeComment = new Faker<Comment>()
            .RuleFor(x => x.Title, y => y.Name.JobTitle())
            .RuleFor(x => x.Text, y => y.Name.JobDescriptor());

        List<User> _users;
        List<Apartment> _apartments;
        List<Comment> _comments;

        IMapper _mapper;

        public CommentUserService_Test()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommentDTO, Comment>().ReverseMap();
                cfg.CreateMap<AddComment, Comment>();
            });

            _mapper = new Mapper(mapperConfig);

            _users = _fakeUser.Generate(2);
            _apartments = _fakeApartment.Generate(2);
            _comments = _fakeComment.Generate(2);
        }

        [Fact]
        public async void CreateCommentAsync_Positive_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "CreateCommentAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                context.AddRange(_apartments);

                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var user = context.Users.AsNoTracking().FirstOrDefault(); ;
                var apartment = context.Apartments.AsNoTracking().FirstOrDefault(); ;

                AddComment comment = new AddComment()
                {
                    ApartmentId = apartment.Id.ToString(),
                    Title = "Title",
                    Text = "Text"
                };

                var service = new CommentUserService(context, _mapper);

                var resultPositive = await service.CreateCommentAsync(comment, user.Id.ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Title.Should().BeEquivalentTo(comment.Title);
            }
        }

        [Fact]
        public async void GetAllCommentsByUserIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetAllCommentsByUserIdAsync_PositiveAndNegative_Test")
                .Options;

            User userWithComments;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_users);
                await context.SaveChangesAsync();

                userWithComments = context.Users.AsNoTracking().FirstOrDefault();

                foreach (var item in _comments)
                {
                    item.AuthorId = userWithComments.Id;
                }

                context.AddRange(_comments);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new CommentUserService(context, _mapper);

                var commentsInBase = await context.Comments.AsNoTracking().ToListAsync();
                var userWithoutComments = await context.Users.Where(_ => _.Id != userWithComments.Id).FirstOrDefaultAsync();

                var resultPositive = await service.GetAllCommentsByAuthorIdAsync(userWithComments.Id.ToString());
                var resultNegative = await service.GetAllCommentsByAuthorIdAsync(userWithoutComments.Id.ToString());

                foreach (var item in commentsInBase)
                {
                    resultPositive.Data
                        .Where(_ => _.Id == item.Id.ToString())
                        .FirstOrDefault()
                        .Should().NotBeNull();
                }

                resultNegative.Data.Should().BeEmpty();
            }
        }

        [Fact]
        public async void GetAllCommentsByApartmentIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetAllCommentsByApartmentIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            Apartment apartmentWithComments;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_apartments);
                await context.SaveChangesAsync();

                apartmentWithComments = context.Apartments.AsNoTracking().FirstOrDefault();

                foreach (var item in _comments)
                {
                    item.ApartmentId = apartmentWithComments.Id;
                }

                context.AddRange(_comments);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var service = new CommentUserService(context, _mapper);

                var commentsInBase = await context.Comments.AsNoTracking().ToListAsync();
                var apartmentWithoutComments = context.Apartments.Where(_ => _.Id != apartmentWithComments.Id).FirstOrDefault();

                var resultPositive = await service.GetAllCommentsByApartmentIdAsync(apartmentWithComments.Id.ToString());
                var resultNegative = await service.GetAllCommentsByApartmentIdAsync(apartmentWithoutComments.Id.ToString());

                foreach (var item in commentsInBase)
                {
                    resultPositive.Data
                        .Where(_ => _.Id == item.Id.ToString())
                        .FirstOrDefault()
                        .Should().NotBeNull();
                }

                resultNegative.Data.Should().BeEmpty();
            }
        }

        [Fact]
        public async void GetCommentByIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "GetCommentByIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_comments);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var comment = await context.Comments.AsNoTracking().FirstOrDefaultAsync();

                var service = new CommentUserService(context, _mapper);

                var resultPositive = await service.GetCommentByIdAsync(comment.Id.ToString());
                var resultNegative = await service.GetCommentByIdAsync(new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Title.Should().BeEquivalentTo(comment.Title);

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Data.Should().BeNull();
            }
        }

        [Fact]
        public async void UpdateCommentAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "UpdateCommentAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                context.AddRange(_comments);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var comment = await context.Comments.AsNoTracking().FirstOrDefaultAsync();

                var service = new CommentUserService(context, _mapper);

                CommentDTO updateComment = new CommentDTO()
                {
                    Id = comment.Id.ToString(),
                    Title = "Title",
                    Text = "newText"
                };

                CommentDTO failComment = new CommentDTO()
                {
                    Id = new Guid().ToString(),
                    Title = "newTitle",
                    Text = "newText"
                };

                var resultPositive = await service.UpdateCommentAsync(updateComment);
                var resultNegative = await service.UpdateCommentAsync(failComment);

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Data.Title.Should().BeEquivalentTo(updateComment.Title);
                resultPositive.Data.Title.Should().NotBeEquivalentTo(comment.Title);

                resultNegative.IsSuccess.Should().BeFalse();
            }
        }

        [Fact]
        public async void DeleteCommentByIdAsync_PositiveAndNegative_TestAsync()
        {
            var options = new DbContextOptionsBuilder<ApartmentContext>()
                .UseInMemoryDatabase(databaseName: "DeleteCommentByIdAsync_PositiveAndNegative_TestAsync")
                .Options;

            using (var context = new ApartmentContext(options))
            {
                foreach (var item in _comments)
                {
                    item.AuthorId = Guid.NewGuid();
                }

                context.AddRange(_comments);
                await context.SaveChangesAsync();
            }

            using (var context = new ApartmentContext(options))
            {
                var comment = await context.Comments.AsNoTracking().FirstOrDefaultAsync();

                var service = new CommentUserService(context, _mapper);

                var resultPositive = await service.DeleteCommentByIdAsync(comment.Id.ToString(), comment.AuthorId.ToString());
                var resultNegative = await service.DeleteCommentByIdAsync(new Guid().ToString(), new Guid().ToString());

                resultPositive.IsSuccess.Should().BeTrue();
                resultPositive.Message.Should().BeNull();

                resultNegative.IsSuccess.Should().BeFalse();
                resultNegative.Message.Should().Contain("Comment was not found");
            }
        }
    }
}
