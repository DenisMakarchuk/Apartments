using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceInterfaces
{
    /// <summary>
    /// The service for the administrator to work with own messages
    /// </summary>
    public interface IAdminOwnMessageService
    {
        /// <summary>
        /// Send new message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<Result<Message>> SendMessageAsync(Message message);

        /// <summary>
        /// Get all messages that are in the own account by account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<IEnumerable<Message>> GetAllOwnMessagesByAccountIdAsync(string accountId);

        /// <summary>
        /// Get message that is in the own account by message Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<Message> GetOwnMessageByMessageIdAsync(string messageId);

        /// <summary>
        /// Update the message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<Result<Message>> UpdateOwnMessageAsync(Message message);

        /// <summary>
        /// Delete own message by message Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<Result> DeleteOwnMessageByMessageIdAsync(string messageId);
    }
}
