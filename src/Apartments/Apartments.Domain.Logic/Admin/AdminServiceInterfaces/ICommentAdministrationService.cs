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
        /// <summary>
        /// Get all User Comments by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<CommentDTOAdministration>>> GetAllCommentsByUserIdAsync(string userId);

        /// <summary>
        /// Get all Apartment Comments by Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<CommentDTOAdministration>>> GetAllCommentsByApartmentIdAsync(string apartmentId);

        /// <summary>
        /// Get Comment by Comment Id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task<Result<CommentDTOAdministration>> GetCommentByIdAsync(string commentId);

        /// <summary>
        /// Update Comment in DataBase
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Result<CommentDTOAdministration>> UpdateCommentAsync(CommentDTOAdministration comment);

        /// <summary>
        /// Delete Comment by Comment Id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task<Result> DeleteCommentByIdAsync(string commentId);
    }
}
