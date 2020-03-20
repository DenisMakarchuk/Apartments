using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Apartments.Domain.Logic.Admin.AdminServiceInterfaces;
using Apartments.Domain;

namespace Apartments.Web.Controllers.Admin
{
    /// <summary>
    /// The controller for the administrator to work with User messages
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserMessageAdministrationController : ControllerBase
    {
        private readonly IUserMessageAdministrationService _userMessageAdministrationService;

        public UserMessageAdministrationController(IUserMessageAdministrationService userMessageAdministrationService)
        {
            _userMessageAdministrationService = userMessageAdministrationService;
        }

        /// <summary>
        /// Send new message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SendMessageAsync([FromBody] Message message)
        {
            if (message is null || ModelState.IsValid) // todo: validate message
            {
                return BadRequest(ModelState);
            }

            var result = await _userMessageAdministrationService.SendMessageAsync(message); //will return Result<Message>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Get all messages that are in the User account by account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{ownAccountId}")]
        public async Task<IActionResult> GetAllUserMessagesByAccountIdAsync(string accountId)
        {
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _userMessageAdministrationService.GetAllUserMessagesByAccountIdAsync(accountId);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get message that is in the User account by message Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{messageId}")]
        public async Task<IActionResult> GetUserMessageByMessageIdAsync(string messageId)
        {
            if (string.IsNullOrEmpty(messageId) || !Guid.TryParse(messageId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _userMessageAdministrationService.GetUserMessageByMessageIdAsync(messageId);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update User message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateUserMessageAsync([FromBody] Message message)
        {
            if (message is null || ModelState.IsValid) // todo: validate message
            {
                return BadRequest(ModelState);
            }

            var result = await _userMessageAdministrationService.UpdateUserMessageAsync(message); //will return Result<Message>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Delete User message by message Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{messageId}")]
        public async Task<IActionResult> DeleteUserMessageByMessageIdAsync(string messageId)
        {
            if (string.IsNullOrEmpty(messageId) || !Guid.TryParse(messageId, out var _))
            {
                return BadRequest();
            }

            var result = await _userMessageAdministrationService.DeleteUserMessageByMessageIdAsync(messageId); //will return Result

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}