using Apartments.Common;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// Methods of User work with own Comments
    /// </summary>
    public interface ICommentUserService
    {
        /// <summary>
        /// Add Comment to the DataBase
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Result<CommentDTO>> 
            CreateCommentAsync(AddComment comment, string authorId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get all User Comments by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<CommentDTO>>> 
            GetAllCommentsByAuthorIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get all Comments by Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<CommentDTO>>> 
            GetAllCommentsByApartmentIdAsync(string apartmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get Comment by Comment Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<Result<CommentDTO>> 
            GetCommentByIdAsync(string commentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update Comment in DataBase
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Result<CommentDTO>> 
            UpdateCommentAsync(CommentDTO comment, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete Comment by Comment Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> 
            DeleteCommentByIdAsync(string commentId, string authorId, CancellationToken cancellationToken = default(CancellationToken));
    }
}