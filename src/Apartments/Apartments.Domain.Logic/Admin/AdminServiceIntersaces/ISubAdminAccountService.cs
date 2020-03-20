﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Admin.AdminServiceIntersaces
{
    /// <summary>
    /// The service for the administrator to work with SubAdmin accounts
    /// </summary>
    public interface ISubAdminAccountService
    {
        #region SubAdminAccount

        /// <summary>
        /// Create new SubAdmin account
        /// </summary>
        /// <param name="subAdminAccount"></param>
        /// <returns></returns>
        Task<Result<SubAdminAccount>> CreateSubAdminAccountAsync(SubAdminAccount subAdminAccount);

        /// <summary>
        /// Get all SubAdmin accounts
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SubAdminAccount>> GetAllSubAdminAccountsAsync();

        /// <summary>
        /// Get SubAdmin account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SubAdminAccount> GetSubAdminAccountByIdAsync(string id);

        /// <summary>
        /// Update the SubAdmin account
        /// </summary>
        /// <param name="subAdminAccount"></param>
        /// <returns></returns>
        Task<Result<SubAdminAccount>> UpdateSubAdminAccountAsync(SubAdminAccount subAdminAccount);

        /// <summary>
        /// Delete SubAdmin account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteSubAdminAccountByIdAsync(string id);

        #endregion

        #region SubAdminMessage

        /// <summary>
        /// Get all messages that are in the SubAdmin account by account Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Message>> GetAllSubAdminMessagesByAccountIdAsync(string id);

        /// <summary>
        /// Get message that is in the SubAdmin account by message Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Message> GetSubAdminMessageByMessageIdAsync(string id);

        /// <summary>
        /// Update SubAdmin message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<Result<Message>> UpdateSubAdminMessageAsync(Message message);

        /// <summary>
        /// Delete SubAdmin message by message Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteSubAdminMessageByMessageIdAsync(string id);

        #endregion
    }
}
