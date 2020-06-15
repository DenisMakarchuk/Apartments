using Apartments.Common;
using Apartments.Domain.Users.AddDTO;
using Apartments.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// Methods of User work with own profile
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Add User to the DataBase
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="nickName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<UserDTO>> 
            CreateUserProfileAsync(string identityId, 
                                   string nickName,
                                   CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get User by User Id
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<UserDTO>> 
            GetUserProfileByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete User by User Id
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result>
            DeleteUserProfileByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default(CancellationToken));

        Task<Result> AddRefrashTokenAsync(string refrashToken, Guid id, CancellationToken cancellationToken = default(CancellationToken));

        Task<Result> DeleteRefreshTokenAsync(string refrashToken, Guid id, CancellationToken cancellationToken = default(CancellationToken));
    }
}