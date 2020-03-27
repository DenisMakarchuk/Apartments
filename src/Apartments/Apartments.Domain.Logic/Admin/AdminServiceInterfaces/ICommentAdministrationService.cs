using Apartments.Common;
using Apartments.Domain.Admin.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceInterfaces
{
    /// <summary>
    /// Methods of Administrator work with User Comments
    /// </summary>
    public interface ICommentAdministrationService
    {
        Task<Result<IEnumerable<CommentDTOAdministration>>> GetAllCommentsByUserIdAsync(string userId);
        Task<Result<IEnumerable<CommentDTOAdministration>>> GetAllCommentsByApartmentIdAsync(string apartmentId);
        Task<Result<CommentDTOAdministration>> GetCommentByIdAsync(string commentId);
        Task<Result<CommentDTOAdministration>> UpdateCommentAsync(CommentDTOAdministration comment);
        Task<Result> DeleteCommentByIdAsync(string id);
    }
}
