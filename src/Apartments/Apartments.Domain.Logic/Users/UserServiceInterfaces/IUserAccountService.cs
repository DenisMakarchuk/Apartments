using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// The service for the User to work with own account
    /// </summary>
    public interface IUserAccountService
    {
        /// <summary>
        /// Create new User account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<User>> CreateUserAsync(AddUser user);

        /// <summary>
        /// Get User account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetUserByIdAsync(string id);

        /// <summary>
        /// Update the User account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<User>> UpdateUserAsync(User user);

        /// <summary>
        /// Delete User account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteUserByIdAsync(string id);
    }
}
