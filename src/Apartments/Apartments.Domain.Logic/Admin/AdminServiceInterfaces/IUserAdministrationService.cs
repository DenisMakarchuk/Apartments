using Apartments.Common;
using Apartments.Domain.Admin.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceInterfaces
{
    public interface IUserAdministrationService
    {
        Task<Result<IEnumerable<UserDTOAdministration>>> GetAll();
        Task<Result<UserDTOAdministration>> GetUserById(string id);
        Task<Result> DeleteUserById(string id);
    }
}
