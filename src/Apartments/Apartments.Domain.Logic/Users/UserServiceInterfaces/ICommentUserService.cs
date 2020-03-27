using Apartments.Common;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    public interface ICommentUserService
    {
        Task<Result<CommentDTO>> CreateCommentAsync(AddComment comment);
        Task<Result<IEnumerable<CommentDTO>>> GetAllCommentsByUserIdAsync(string userId);
        Task<Result<IEnumerable<CommentDTO>>> GetAllCommentsByApartmentIdAsync(string apartmentId);
        Task<Result<CommentDTO>> GetCommentByIdAsync(string orderId);
        Task<Result<CommentDTO>> UpdateCommentAsync(CommentDTO comment);
        Task<Result> DeleteCommentByIdAsync(string id);
    }
}
