using Apartments.Common;
using Apartments.Domain.User.AddDTO;
using Apartments.Domain.User.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    public interface ICommentUserService
    {
        Task<Result<CommentDTO>> CreateComment(AddComment comment);
        Task<Result<IEnumerable<CommentDTO>>> GetAllCommentsByUserId(string userId);
        Task<Result<IEnumerable<CommentDTO>>> GetAllCommentsByApartmentId(string apartmentId);
        Task<Result<CommentDTO>> GetCommentById(string orderId);
        Task<Result<CommentDTO>> UpdateComment(CommentDTO comment);
        Task<Result> DeleteCommentById(string id);
    }
}
