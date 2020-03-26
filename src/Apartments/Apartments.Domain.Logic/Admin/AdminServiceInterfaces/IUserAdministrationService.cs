using Apartments.Common;
using Apartments.Domain.Admin.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceInterfaces
{
    /// <summary>
    /// Methods of Administrator work with Users
    /// </summary>
    public interface IUserAdministrationService
    {
        /// <summary>
        /// Get all Users from the database
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserDTOAdministration>> GetAll();

        /// <summary>
        /// Get User by User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<UserDTOAdministration>> GetUserById(string id);

        /// <summary>
        /// Delete User by User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteUserById(string id);
    }
}
