using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceIntersaces
{
    /// <summary>
    /// The service for the administrator to work with User comments
    /// </summary>
    public interface IUserCommentAdministrationService
    {
        /// <summary>
        /// Post new comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Result<Comment>> PostCommentAsync(Comment comment);

        /// <summary>
        /// Get all comments that are in the User account by account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<IEnumerable<Comment>> GetAllUserCommentsByAccountIdAsync(string accountId);

        /// <summary>
        /// Get comment that is in the User account by comments Id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task<Comment> GetUserCommentByCommentIdAsync(string commentId);

        /// <summary>
        /// Update User comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Result<Comment>> UpdateUserCommentAsync(Comment comment);

        /// <summary>
        /// Delete User comment by comment Id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task<Result> DeleteUserCommentByCommentIdAsync(string commentId);
    }
}
