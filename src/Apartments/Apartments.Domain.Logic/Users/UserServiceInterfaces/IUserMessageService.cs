using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Users.UserServiceInterfaces
{
    /// <summary>
    /// The servuce for the User to work with own Messages
    /// </summary>
    public interface IUserMessageService
    {
        /// <summary>
        /// Create new User Message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<Result<Message>> CreateMessageAsync(AddMessage message);

        /// <summary>
        /// Get all User Messages
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Message>> GetAllMessagesAsync();

        /// <summary>
        /// Get User Message by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Message> GetMessageByIdAsync(string id);

        /// <summary>
        /// Update the User Message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<Result<Message>> UpdateMessageAsync(Message message);

        /// <summary>
        /// Delete User Message by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteMessageByIdAsync(string id);
    }
}
