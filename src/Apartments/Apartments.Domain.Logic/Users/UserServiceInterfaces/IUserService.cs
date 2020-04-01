using Apartments.Common;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// Methods of User work with own profile
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Put User to the DataBase
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<UserDTO>> CreateUserAsync(AddUser user);

        /// <summary>
        /// Get User by User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<UserDTO>> GetUserByIdAsync(string id);

        /// <summary>
        /// Update User in DataBase
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<UserDTO>> UpdateUserAsync(UserDTO user);

        /// <summary>
        /// Delete User by User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteUserByIdAsync(string id);
    }
}
