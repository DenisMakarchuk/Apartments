using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceIntersaces
{
    /// <summary>
    /// The service for the administrator to work with User messages
    /// </summary>
    public interface IUserMessageAdministrationService
    {
        /// <summary>
        /// Send new message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<Result<Message>> SendMessageAsync(Message message);

        /// <summary>
        /// Get all messages that are in the User account by account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<IEnumerable<Message>> GetAllUserMessagesByAccountIdAsync(string accountId);

        /// <summary>
        /// Get message that is in the User account by message Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<Message> GetUserMessageByMessageIdAsync(string messageId);

        /// <summary>
        /// Update User message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<Result<Message>> UpdateUserMessageAsync(Message message);

        /// <summary>
        ///  Delete User message by message Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<Result> DeleteUserMessageByMessageIdAsync(string messageId);
    }
}
