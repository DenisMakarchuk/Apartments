using Apartments.Domain.Logic.Admin.AdminServiceIntersaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminService
{
    class UserCommentAdministrationService : IUserCommentAdministrationService
    {
        public Task<Result> DeleteUserCommentByCommentIdAsync(string commentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetAllUserCommentsByAccountIdAsync(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetUserCommentByCommentIdAsync(string commentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Comment>> PostCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Comment>> UpdateUserCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
