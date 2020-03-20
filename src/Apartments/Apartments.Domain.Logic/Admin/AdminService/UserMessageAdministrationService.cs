using Apartments.Domain.Logic.Admin.AdminServiceIntersaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminService
{
    class UserMessageAdministrationService : IUserMessageAdministrationService
    {
        public Task<Result> DeleteUserMessageByMessageIdAsync(string messageId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetAllUserMessagesByAccountIdAsync(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetUserMessageByMessageIdAsync(string messageId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Message>> SendMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Message>> UpdateUserMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
