using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminService
{
    public class AdminOwnMessageService : IAdminOwnMessageService
    {
        public Task<Result<Message>> SendMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetAllOwnMessagesByAccountIdAsync(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetOwnMessageByMessageIdAsync(string messageId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Message>> UpdateOwnMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }        
        
        public Task<Result> DeleteOwnMessageByMessageIdAsync(string messageId)
        {
            throw new NotImplementedException();
        }
    }
}
