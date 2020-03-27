using Apartments.Common;
using Apartments.Data.Context;
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
                    .AsNoTracking().ToListAsync().ConfigureAwait(false);

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

        public Task<Result<IEnumerable<CommentDTOAdministration>>> GetAllCommentsByApartmentIdAsync(string apartmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<CommentDTOAdministration>> GetCommentByIdAsync(string commentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<CommentDTOAdministration>> UpdateCommentAsync(CommentDTOAdministration comment)
        {
            throw new NotImplementedException();
        }       
        
        public Task<Result> DeleteCommentByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
