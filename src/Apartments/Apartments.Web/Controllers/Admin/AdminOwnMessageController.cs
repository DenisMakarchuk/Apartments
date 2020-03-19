using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Apartments.Domain.Logic.Admin.AdminServiceIntersaces;

namespace Apartments.Web.Controllers.Admin
{
    /// <summary>
    /// The controller for the administrator to work with own messages
    /// </summary>
    [Route("api/[controller]")]
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
        /// <param name="ownAccountId"></param>
        /// <param name="destinationId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{ownAccountId}")]
        public async Task<IActionResult> SendMessageAsync(string ownAccountId, [FromBody] Message message)
        {
            if (string.IsNullOrEmpty(ownAccountId) || !Guid.TryParse(ownAccountId, out var _)) // todo: validate message
            {
                return BadRequest();
            }

            try
            {
                var result = await _adminOwnMessageService.SendMessage(ownAccountId, message);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet] 
        [Route("{accountId}")]
        public async Task<IActionResult> GetAllMessagesByAccountIdAsync(string accountId)
        {
            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _adminOwnMessageService.GetAllMessagesByAccountId(accountId);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{accountId}/{messageId}")]
        public async Task<IActionResult> GetMessageByAccountIdAndMessageIdAsync(string accountId, string messageId)
        {
            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(messageId) ||
                !Guid.TryParse(accountId, out var _) || !Guid.TryParse(messageId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _adminOwnMessageService.GetMessageByAccountIdAndMessageId(accountId, messageId);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{ownAccountId}/{messageId}")]
        public async Task<IActionResult> UpdateMessageByOwnAccountIdAndMessageIdAsync(string ownAccountId, string messageId)
        {
            if (string.IsNullOrEmpty(ownAccountId) || string.IsNullOrEmpty(messageId) ||
                !Guid.TryParse(ownAccountId, out var _) || !Guid.TryParse(messageId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _adminOwnMessageService.UpdateMessageByOwnAccountIdAndMessageId(ownAccountId, messageId);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{ownAccountId}/{messageId}")]
        public async Task<IActionResult> DeleteMessageByOwnAccountIdAndMessageIdAsync(string ownAccountId, string messageId)
        {
            if (string.IsNullOrEmpty(ownAccountId) || string.IsNullOrEmpty(messageId) ||
                !Guid.TryParse(ownAccountId, out var _) || !Guid.TryParse(messageId, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _adminOwnMessageService.DeleteMessageByOwnAccountIdAndMessageId(ownAccountId, messageId);

                return result is null ? NotFound() : (IActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}