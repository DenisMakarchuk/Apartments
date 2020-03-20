using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminService
{
    public class GlobalAdminProfileService : IGlobalAdminProfileService
    {
        public Task<GlobalAdmin> GetGlobalAdminAccountByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Message>> UpdateGlobalAdminAsync(GlobalAdmin globalAdmin)
        {
            throw new NotImplementedException();
        }
    }
}
