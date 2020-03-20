using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminService
{
    public class SubAdminAccountService : ISubAdminAccountService
    {
        public Task<Result<SubAdmin>> CreateSubAdminAccountAsync(SubAdmin subAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteSubAdminAccountByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteSubAdminMessageByMessageIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SubAdmin>> GetAllSubAdminAccountsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetAllSubAdminMessagesByAccountIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SubAdmin> GetSubAdminAccountByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetSubAdminMessageByMessageIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<SubAdmin>> UpdateSubAdminAccountAsync(SubAdmin subAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Message>> UpdateSubAdminMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
