using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceIntersaces
{
    /// <summary>
    /// The service for the GlobalAdmin to work with own account
    /// </summary>
    public interface IGlobalAdminProfileService
    {
        /// <summary>
        /// Get GlobalAdmin account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GlobalAdmin> GetGlobalAdminAccountByIdAsync(string id);

        /// <summary>
        /// Update the GlobalAdmin account
        /// </summary>
        /// <param name="globalAdmin"></param>
        /// <returns></returns>
        Task<Result<Message>> UpdateGlobalAdminAsync(GlobalAdmin globalAdmin);
    }
}
