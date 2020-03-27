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
        Task<Result<UserDTO>> CreateUser(AddUser user);
        Task<Result<UserDTO>> GetUserById(string id);
        Task<Result<UserDTO>> UpdateUser(UserDTO user);
        Task<Result> DeleteUserById(string id);
    }
}
