using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminService
{
    public class UserAccountAdministrationService : IUserAccountAdministrationService
    {
        public Task<Result<User>> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<User>> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
