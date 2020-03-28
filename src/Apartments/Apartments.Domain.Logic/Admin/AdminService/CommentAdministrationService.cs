﻿using Apartments.Common;
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
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminService
{
    public class CommentAdministrationService : ICommentAdministrationService
    {
        private readonly ApartmentContext _db;
        private readonly IMapper _mapper;

        public CommentAdministrationService(ApartmentContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<CommentDTOAdministration>>> GetAllCommentsByUserIdAsync(string userId)
        {
            Guid athorId = Guid.Parse(userId);

            try
            {
                var comments = await _db.Comments.Where(_ => _.AuthorId == athorId).Select(_ => _)
                    .AsNoTracking().ToListAsync();

                if (!comments.Any())
                {
                    return (Result<IEnumerable<CommentDTOAdministration>>)Result<IEnumerable<CommentDTOAdministration>>
                        .Fail("This User haven't Comments");
                }

                return (Result<IEnumerable<CommentDTOAdministration>>)Result<IEnumerable<CommentDTOAdministration>>
                    .Ok(_mapper.Map<IEnumerable<CommentDTOAdministration>>(comments));
            }
            catch (NullReferenceException ex)
            {
                return (Result<IEnumerable<CommentDTOAdministration>>)Result<IEnumerable<CommentDTOAdministration>>
                    .Fail($"Source is null. {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<CommentDTOAdministration>>> GetAllCommentsByApartmentIdAsync(string apartmentId)
        {
            Guid id = Guid.Parse(apartmentId);

            try
            {
                var comments = await _db.Comments.Where(_ => _.ApartmentId == id).Select(_ => _)
                    .AsNoTracking().ToListAsync();

                if (!comments.Any())
                {
                    return (Result<IEnumerable<CommentDTOAdministration>>)Result<IEnumerable<CommentDTOAdministration>>
                        .Fail("This Apartment haven't Comments");
                }

                return (Result<IEnumerable<CommentDTOAdministration>>)Result<IEnumerable<CommentDTOAdministration>>
                    .Ok(_mapper.Map<IEnumerable<CommentDTOAdministration>>(comments));
            }
            catch (NullReferenceException ex)
            {
                return (Result<IEnumerable<CommentDTOAdministration>>)Result<IEnumerable<CommentDTOAdministration>>
                    .Fail($"Source is null. {ex.Message}");
            }
        }

        public async Task<Result<CommentDTOAdministration>> GetCommentByIdAsync(string commentId)
        {
            Guid id = Guid.Parse(commentId);

            try
            {
                var user = await _db.Comments.Where(_ => _.Id == id).AsNoTracking().FirstOrDefaultAsync();

                if (user is null)
                {
                    return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                        .Fail($"Comment was not found");
                }

                return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                    .Ok(_mapper.Map<CommentDTOAdministration>(user));
            }
            catch (NullReferenceException ex)
            {
                return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                    .Fail($"Source is null. {ex.Message}");
            }
        }

        public async Task<Result<CommentDTOAdministration>> UpdateCommentAsync(CommentDTOAdministration comment)
        {
            comment.Update = DateTime.Now;
            Comment commentForUpdate = _mapper.Map<Comment>(comment);

            _db.Comments.Attach(commentForUpdate);

            _db.Entry(commentForUpdate).Property(c => c.Title).IsModified = true;
            _db.Entry(commentForUpdate).Property(c => c.Text).IsModified = true;

            _db.Entry(commentForUpdate).Property(c => c.Update).IsModified = true;

            try
            {
                await _db.SaveChangesAsync();

                return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                    .Ok(comment);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                    .Fail($"Cannot delete model. {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return (Result<CommentDTOAdministration>)Result<CommentDTOAdministration>
                    .Fail($"Cannot delete model. {ex.Message}");
            }
        }

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