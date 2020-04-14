using Apartments.Common;
using Apartments.Domain.Admin.DTO;
using Apartments.Domain.Admin.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceInterfaces
{
    /// <summary>
    /// Administrator work with Identity Users & User Profoles
    /// </summary>
    public interface IIdentityUserAdministrationService
    {
        /// <summary>
        /// Get all Identity Users in role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<IdentityUserAdministrationDTO>>> 
            GetAllUsersInRoleAsync(string role);

        /// <summary>
        /// Get IdentityUser with User Profile by IdentityId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<UserAdministrationView>> 
            GetUserByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Delete IdentityUser & User Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> 
            DeleteByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Add User to Admin role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<UserAdministrationView>> 
            AddToAdminAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Remove User from Admin role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<UserAdministrationView>> 
            AddToUserAsync(string id, CancellationToken cancellationToken);
    }
}