using Apartments.Common;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    public interface IUserService
    {
        Task<Result<UserDTO>> CreateUserAsync(AddUser user);
        Task<Result<UserDTO>> GetUserByIdAsync(string id);
        Task<Result<UserDTO>> UpdateUserAsync(UserDTO user);
        Task<Result> DeleteUserByIdAsync(string id);
    }
}
