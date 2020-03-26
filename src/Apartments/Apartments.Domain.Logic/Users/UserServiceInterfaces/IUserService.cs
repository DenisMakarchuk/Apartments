using Apartments.Common;
using Apartments.Domain.User.AddDTO;
using Apartments.Domain.User.DTO;
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
