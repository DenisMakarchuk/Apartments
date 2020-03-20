using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceInterfaces
{
    /// <summary>
    /// The service for the administrator to work with User accounts
    /// </summary>
    public interface IUserAccountAdministrationService
    {
        /// <summary>
        /// Create new User account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<User>> CreateUserAsync(User user);

        /// <summary>
        /// Get all User accounts
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetAllUserAsync();

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
