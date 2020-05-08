using Apartments.Common;
using Apartments.Data.Context;
using Apartments.Data.DataModels;
using Apartments.Domain.Admin.DTO;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminService
{
    /// <summary>
    /// Methods of Administrator work with Comments
    /// </summary>
    public class CommentAdministrationService : ICommentAdministrationService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public CommentAdministrationService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all User Comments by User Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<PagedResponse<CommentDTOAdministration>>> 
            GetAllCommentsByUserIdAsync(PagedRequest<string> request, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid athorId = Guid.Parse(request.Data);

            try
            {
                var count = await _db.Comments.Where(_ => _.AuthorId == athorId).CountAsync();

                var comments = await _db.Comments.Where(_ => _.AuthorId == athorId)
                                                 .Skip((request.PageNumber - 1) * request.PageSize)
                                                 .Take(request.PageSize)
                                                 .AsNoTracking().ToListAsync(cancellationToken);

                PagedResponse<CommentDTOAdministration> response
                    = new PagedResponse<CommentDTOAdministration>(_mapper.Map<IEnumerable<CommentDTOAdministration>>(comments),
                                                                  count,
                                                                  request.PageNumber,
                                                                  request.PageSize);

                return (Result<PagedResponse<CommentDTOAdministration>>)Result<PagedResponse<CommentDTOAdministration>>
                    .Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                return (Result<PagedResponse<CommentDTOAdministration>>)Result<PagedResponse<CommentDTOAdministration>>
                    .Fail<PagedResponse<CommentDTOAdministration>>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get all Apartment Comments by Apartment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<PagedResponse<CommentDTOAdministration>>> 
            GetAllCommentsByApartmentIdAsync(PagedRequest<string> request, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(request.Data);

            try
            {
                var count = await _db.Comments.Where(_ => _.ApartmentId == id).CountAsync();

                var comments = await _db.Comments.Where(_ => _.ApartmentId == id)
                                                 .Skip((request.PageNumber - 1) * request.PageSize)
                                                 .Take(request.PageSize)
                                                 .AsNoTracking().ToListAsync(cancellationToken);

                PagedResponse<CommentDTOAdministration> response
                = new PagedResponse<CommentDTOAdministration>(_mapper.Map<IEnumerable<CommentDTOAdministration>>(comments),
                                                              count,
                                                              request.PageNumber,
                                                              request.PageSize);

                return (Result<PagedResponse<CommentDTOAdministration>>)Result<PagedResponse<CommentDTOAdministration>>
                    .Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                return (Result<PagedResponse<CommentDTOAdministration>>)Result<PagedResponse<CommentDTOAdministration>>
                    .Fail<PagedResponse<CommentDTOAdministration>>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Get Comment by Comment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<CommentDTOAdministration>> 
            GetCommentByIdAsync(string commentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(commentId);

            try
            {
                var comment = await _db.Comments.Where(_ => _.Id == id).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (comment is null)
                {
                    return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                        .NotOk<CommentDTOAdministration>(null, "Comment is not exist");
                }

                return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                    .Ok(_mapper.Map<CommentDTOAdministration>(comment));
            }
            catch (ArgumentNullException ex)
            {
                return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                    .Fail<CommentDTOAdministration>($"Source is null. {ex.Message}");
            }
        }

        /// <summary>
        /// Update Comment in DataBase
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result<CommentDTOAdministration>> 
            UpdateCommentAsync(CommentDTOAdministration comment, CancellationToken cancellationToken = default(CancellationToken))
        {
            comment.Update = DateTime.UtcNow;
            Comment commentForUpdate = _mapper.Map<Comment>(comment);

            _db.Entry(commentForUpdate).Property(c => c.Title).IsModified = true;
            _db.Entry(commentForUpdate).Property(c => c.Text).IsModified = true;

            _db.Entry(commentForUpdate).Property(c => c.Update).IsModified = true;

            try
            {
                await _db.SaveChangesAsync(cancellationToken);

                return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                    .Ok(comment);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                    .Fail<CommentDTOAdministration>($"Cannot update model. {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                    .Fail<CommentDTOAdministration>($"Cannot update model. {ex.Message}");
            }
        }

        /// <summary>
        /// Delete Comment by Comment Id. Id must be verified to convert to Guid at the web level
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAttribute]
        public async Task<Result> 
            DeleteCommentByIdAsync(string commentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guid id = Guid.Parse(commentId);

            var comment = await _db.Comments.IgnoreQueryFilters().FirstOrDefaultAsync(_ => _.Id == id, cancellationToken);

            if (comment is null)
            {
                return await Task.FromResult(Result.NotOk("Comment is not exist"));
            }

            try
            {
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync(cancellationToken);

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