using Apartments.Common;
using Apartments.Data.Context;
using Apartments.Data.DataModels;
using Apartments.Domain.Logic.Users.UserServiceInterfaces;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserService
{
    /// <summary>
    /// Methods of User work with own Comments
    /// </summary>
    public class CommentUserService : ICommentUserService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public CommentUserService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Put Comment to the DataBase
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<CommentDTO>> CreateCommentAsync(AddComment comment)
        {
            var addedComment = _mapper.Map<Comment>(comment);

            _db.Comments.Add(addedComment);

            try
            {
                await _db.SaveChangesAsync();

                Comment commentAfterAdding = await _db.Comments.Where(_ => _.AuthorId == addedComment.AuthorId)
                    .Select(_ => _)
                    .AsNoTracking().FirstOrDefaultAsync();

                return (Result<CommentDTO>)Result<CommentDTO>
                    .Ok(_mapper.Map<CommentDTO>(commentAfterAdding));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return (Result<CommentDTO>)Result<CommentDTO>
                    .Fail<CommentDTO>($"Cannot save model. {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return (Result<CommentDTO>)Result<CommentDTO>
                    .Fail<CommentDTO>($"Cannot save model. {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                return (Result<CommentDTO>)Result<CommentDTO>
                    .Fail<CommentDTO>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get all User Comments by User Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<CommentDTO>>> GetAllCommentsByUserIdAsync(string userId)
        {
            Guid athorId = Guid.Parse(userId);

            try
            {
                var comments = await _db.Comments.Where(_ => _.AuthorId == athorId).Select(_ => _)
                    .AsNoTracking().ToListAsync();

                if (!comments.Any())
                {
                    return (Result<IEnumerable<CommentDTO>>)Result<IEnumerable<CommentDTO>>
                        .Fail<IEnumerable<CommentDTO>>("This User haven't Comments");
                }

                return (Result<IEnumerable<CommentDTO>>)Result<IEnumerable<CommentDTO>>
                    .Ok(_mapper.Map<IEnumerable<CommentDTO>>(comments));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<IEnumerable<CommentDTO>>)Result<IEnumerable<CommentDTO>>
                    .Fail<IEnumerable<CommentDTO>>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get all Comments by Apartment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<IEnumerable<CommentDTO>>> GetAllCommentsByApartmentIdAsync(string apartmentId)
        {
            Guid id = Guid.Parse(apartmentId);

            try
            {
                var comments = await _db.Comments.Where(_ => _.ApartmentId == id).Select(_ => _)
                    .AsNoTracking().ToListAsync();

                if (!comments.Any())
                {
                    return (Result<IEnumerable<CommentDTO>>)Result<IEnumerable<CommentDTO>>
                        .Fail<IEnumerable<CommentDTO>>("This Apartment haven't Comments");
                }

                return (Result<IEnumerable<CommentDTO>>)Result<IEnumerable<CommentDTO>>
                    .Ok(_mapper.Map<IEnumerable<CommentDTO>>(comments));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<IEnumerable<CommentDTO>>)Result<IEnumerable<CommentDTO>>
                    .Fail<IEnumerable<CommentDTO>>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get Comment by Comment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<CommentDTO>> GetCommentByIdAsync(string commentId)
        {
            Guid id = Guid.Parse(commentId);

            try
            {
                var user = await _db.Comments.Where(_ => _.Id == id).AsNoTracking().FirstOrDefaultAsync();

                if (user is null)
                {
                    return (Result<CommentDTO>)Result<CommentDTO>
                        .Fail<CommentDTO>($"Comment was not found");
                }

                return (Result<CommentDTO>)Result<CommentDTO>
                    .Ok(_mapper.Map<CommentDTO>(user));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<CommentDTO>)Result<CommentDTO>
                    .Fail<CommentDTO>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Update Comment in DataBase
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<CommentDTO>> UpdateCommentAsync(CommentDTO comment)
        {
            comment.Update = DateTime.Now;
            Comment commentForUpdate = _mapper.Map<Comment>(comment);

            _db.Entry(commentForUpdate).Property(c => c.Title).IsModified = true;
            _db.Entry(commentForUpdate).Property(c => c.Text).IsModified = true;

            _db.Entry(commentForUpdate).Property(c => c.Update).IsModified = true;

            try
            {
                await _db.SaveChangesAsync();

                return (Result<CommentDTO>)Result<CommentDTO>
                    .Ok(comment);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return (Result<CommentDTO>)Result<CommentDTO>
                    .Fail<CommentDTO>($"Cannot update model. {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return (Result<CommentDTO>)Result<CommentDTO>
                    .Fail<CommentDTO>($"Cannot update model. {ex.Message}");
            }
        }

        /// <summary>
        /// Delete Comment by Comment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> DeleteCommentByIdAsync(string commentId)
        {
            Guid id = Guid.Parse(commentId);

            var comment = await _db.Comments.IgnoreQueryFilters().FirstOrDefaultAsync(_ => _.Id == id);

            if (comment is null)
            {
                return await Task.FromResult(Result.Fail("Comment was not found"));
            }

            try
            {
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();

                return await Task.FromResult(Result.Ok());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete Comment. {ex.Message}"));
            }
            catch (DbUpdateException ex)
            {
                return await Task.FromResult(Result.Fail($"Cannot delete Comment. {ex.Message}"));
            }
        }
    }
}
