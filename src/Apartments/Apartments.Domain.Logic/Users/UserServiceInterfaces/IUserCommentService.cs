using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// The servuce for the User to work with own Comments
    /// </summary>
    public interface IUserCommentService
    {
        /// <summary>
        /// Create new User Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Result<Comment>> CreateCommentAsync(AddComment comment);

        /// <summary>
        /// Get all User Comments
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Comment>> GetAllCommentsAsync();

        /// <summary>
        /// Get User Comment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Comment> GetCommentByIdAsync(string id);

        /// <summary>
        /// Update the User Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Result<Comment>> UpdateCommentAsync(Comment comment);

        /// <summary>
        /// Delete User Comment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteCommentByIdAsync(string id);
    }
}
