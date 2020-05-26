using Apartments.Common;
using Apartments.Domain.Admin.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceInterfaces
{
    /// <summary>
    /// Methods of Administrator work with User profile
    /// </summary>
    public interface IUserAdministrationService
    {
        /// <summary>
        /// Get User profile by User Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<UserDTOAdministration>> 
            GetUserProfileByIdentityIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Delete User profile by User Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> 
            DeleteUserProfileByIdentityIdAsync(string id, CancellationToken cancellationToken);
    }
}