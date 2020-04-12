using Apartments.Common;
using Apartments.Domain.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// Methods of User work with own profile & Identity
    /// </summary>
    public interface IIdentityUserService
    {
        /// <summary>
        /// Create User profile & Identity User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Result<UserViewModel>> RegisterAsync(string email, string password);

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Result<UserViewModel>> LoginAsync(string email, string password);

        /// <summary>
        /// Delete User own profile & Identity User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Result> DeleteAsync(string email, string password);
    }
}
