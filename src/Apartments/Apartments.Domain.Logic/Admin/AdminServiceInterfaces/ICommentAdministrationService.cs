using Apartments.Common;
using Apartments.Domain.Admin.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceInterfaces
{
    public interface ICommentAdministrationService
    {
        Task<Result<IEnumerable<CommentDTOAdministration>>> GetAllCommentsByUserId(string userId);
        Task<Result<IEnumerable<CommentDTOAdministration>>> GetAllCommentsByApartmentId(string apartmentId);
        Task<Result<CommentDTOAdministration>> GetCommentById(string commentId);
        Task<Result<CommentDTOAdministration>> UpdateComment(CommentDTOAdministration comment);
        Task<Result> DeleteCommentById(string id);
    }
}
