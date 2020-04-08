using Apartments.Common;
using Apartments.Domain.Admin.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceInterfaces
{
    public interface IIdentityUserAdministrationService
    {
        Task<Result<IEnumerable<IdentityUser>>> GetAllUsersInRoleAsync(string role);

        Task<Result<UserAdministrationView>> GetUserByIdAsync(string id);

        Task<Result> DeleteByIdAsync(string id);

        Task<Result<UserAdministrationView>> MakeAdminAsync(string id);

        Task<Result<UserAdministrationView>> MakeUserAsync(string id);
    }
}
