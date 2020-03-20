using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Apartments.Domain.Logic.Admin.AdminServiceIntersaces;
using Apartments.Domain;

namespace Apartments.Web.Controllers.Admin
{
    /// <summary>
    /// The controller for the administrator to work with own messages
    /// </summary>
    [Route("api/[controller]")] // todo: change Rote
    [ApiController]
    public class AdminOwnMessageController : ControllerBase
    {
        private readonly IAdminOwnMessageService _adminOwnMessageService;

        public AdminOwnMessageController(IAdminOwnMessageService adminOwnMessageService)
        {
            _adminOwnMessageService = adminOwnMessageService;
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

            var result = await _adminOwnMessageService.SendMessageAsync(message); //will return Result<Message>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Get all messages that are in the own account by account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet] 
        [Route("{accountId}")]
        public async Task<IActionResult> GetAllOwnMessagesByAccountIdAsync(string accountId)
        {
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _adminOwnMessageService.GetAllOwnMessagesByAccountIdAsync(accountId);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get message that is in the own account by message Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{messageId}")]
        public async Task<IActionResult> GetOwnMessageByMessageIdAsync(string messageId)
        {
            if (string.IsNullOrEmpty(messageId) || !Guid.TryParse(messageId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _adminOwnMessageService.GetOwnMessageByMessageIdAsync(messageId);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update own message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateOwnMessageAsync([FromBody] Message message)
        {
            if (message is null || ModelState.IsValid) // todo: validate message
            {
                return BadRequest(ModelState);
            }

            var result = await _adminOwnMessageService.UpdateOwnMessageAsync(message); //will return Result<Message>

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Delete own message by message Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{messageId}")]
        public async Task<IActionResult> DeleteOwnMessageByMessageIdAsync(string messageId)
        {
            if (string.IsNullOrEmpty(messageId) || !Guid.TryParse(messageId, out var _))
            {
                return BadRequest();
            }

            var result = await _adminOwnMessageService.DeleteOwnMessageByMessageIdAsync(messageId); //will return Result

            return result.IsError ? BadRequest(result.Message) : (IActionResult)Ok(result.IsSuccess);
        }
    }
}